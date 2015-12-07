using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSpace.Models
{
	class DbInfoReader
	{
		private readonly string _cs;

		public DbInfoReader(string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
				throw new ArgumentException("is null or empty", "connectionString");
			_cs = connectionString;
		}
		public List<DbInfo> GetDbInfo()
		{
			Stopwatch sw = Stopwatch.StartNew();
			try
			{
				List<DbInfo> result = new List<DbInfo>();

				using (SqlConnection sqlConnection = new SqlConnection(_cs))
				{
					try { sqlConnection.Open(); }
					catch (Exception ex)
					{
						throw new InvalidOperationException(string.Format("Coudln't open connection with MSSQL server with connection string '{0}': {1}", _cs, ex.Message), ex);
					}
					List<string> allDb = GetAllDatabases(sqlConnection);
					foreach (var db in allDb)
					{
						DbInfo dbInfo = null;
						if (!GetDbInfo(sqlConnection, db, out dbInfo))
							continue;
						result.Add(dbInfo);
					}
				}
				return result;
			}
			finally
			{
				sw.Stop();
				Debug.WriteLine("GetDbInfo() executed in {0} ms", sw.ElapsedMilliseconds);
			}
		}
		private bool GetDbInfo(SqlConnection con, string dbName, out DbInfo result)
		{
			result = null;
			SqlCommand cmd = new SqlCommand("sp_helpdb", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@dbname", dbName));
			SqlDataAdapter a = new SqlDataAdapter(cmd);
			DataSet ds = new DataSet();
			a.Fill(ds);
			if (ds.Tables.Count != 2)
				return false;

			var databaseInfo = ds.Tables[0];
			if (databaseInfo.Rows.Count == 0)
				return false;
			var row = databaseInfo.Rows[0];
			string db_size = Convert.ToString(row[databaseInfo.Columns["db_size"].Ordinal]).Trim();
			string owner = Convert.ToString(row[databaseInfo.Columns["owner"].Ordinal]);
			int dbid = Convert.ToInt32(row[databaseInfo.Columns["dbid"].Ordinal]);
			string created = Convert.ToString(row[databaseInfo.Columns["created"].Ordinal]);
			string status = Convert.ToString(row[databaseInfo.Columns["status"].Ordinal]);
			int compatibility_level = Convert.ToInt32(row[databaseInfo.Columns["compatibility_level"].Ordinal]);

			result = new DbInfo(dbName, db_size, owner, dbid, created, status, compatibility_level);
			var filesInfo = ds.Tables[1];
			foreach (DataRow fiRow in filesInfo.Rows)
			{
				string name = Convert.ToString(fiRow[filesInfo.Columns["name"].Ordinal]);
				int fileid = Convert.ToInt32(fiRow[filesInfo.Columns["fileid"].Ordinal]);
				string filename = Convert.ToString(fiRow[filesInfo.Columns["filename"].Ordinal]);
				string filegroup = Convert.ToString(fiRow[filesInfo.Columns["filegroup"].Ordinal]);
				string size = Convert.ToString(fiRow[filesInfo.Columns["size"].Ordinal]);
				string maxsize = Convert.ToString(fiRow[filesInfo.Columns["maxsize"].Ordinal]);
				string growth = Convert.ToString(fiRow[filesInfo.Columns["growth"].Ordinal]);
				string usage = Convert.ToString(fiRow[filesInfo.Columns["usage"].Ordinal]);
				DbFile dbFile = new DbFile(name, fileid, filename, filegroup, size, maxsize, growth, usage, dbName);
				result.AddFile(dbFile);
			}
			return true;
		}
		private List<string> GetAllDatabases(SqlConnection con)
		{
			List<string> result = new List<string>();
			DataTable databases = con.GetSchema("Databases");
			foreach (DataRow database in databases.Rows)
			{
				result.Add(database.Field<string>("database_name"));
			}
			return result;
		}
	}
}
