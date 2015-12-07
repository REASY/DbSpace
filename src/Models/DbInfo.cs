using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSpace.Models
{
	public class DbInfo
	{
		public string Name { get; private set; }
		public decimal Size { get; private set; }
		public string Owner { get; private set; }
		public int DbId { get; private set; }
		public string Created { get; private set; }
		public string Status { get; private set; }
		public int CompatibilityLevel { get; private set; }
		public List<DbFile> Files { get; private set; }

		public DbInfo(string name, string size, string owner, int dbId, string created, string status, int cl)
		{
			Name = name;
			size = size.Replace("MB", "");
			decimal sz;
			decimal.TryParse(size, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.InvariantCulture, out sz);
			Size = sz;
			Owner = owner;
			DbId = dbId;
			Created = created;
			Status = status;
			CompatibilityLevel = cl;
			Files = new List<DbFile>(2);
		}
		public void AddFile(DbFile dbFile)
		{
			Files.Add(dbFile);
		}
	}
	public class DbFile
	{
		public string Name { get; private set; }
		public int Id { get; private set; }
		public string FileName { get; private set; }
		public string FileGroup { get; private set; }
		public int Size { get; private set; }
		public string MaxSize { get; private set; }
		public string Growth { get; private set; }
		public string Usage { get; private set; }
		public string DbName { get; private set; }

		public DbFile(string name, int id, string fileName, string fileGroup, string size, string maxSize, string growth, string usage, string dbName)
		{
			Name = name;
			Id = id;
			size = size.Replace("KB", "");
			int sz = 0;
			int.TryParse(size, out sz);
			FileName = fileName;
			FileGroup = fileGroup;
			Size = sz;
			MaxSize = maxSize;
			Growth = growth;
			Usage = usage;
			DbName = dbName;
		}
	}
}
