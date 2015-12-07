using DbSpace.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DbSpace.ViewModel
{

	/// <summary>
	/// This class contains properties that the main View can data bind to.
	/// <para>
	/// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
	/// </para>
	/// <para>
	/// You can also use Blend to data bind with the tool's support.
	/// </para>
	/// <para>
	/// See http://www.galasoft.ch/mvvm
	/// </para>
	/// </summary>
	public class MainViewModel : ViewModelBase
	{
		private bool _IsSimpleMode;
		public bool IsSimpleMode
		{
			get { return _IsSimpleMode; }
			set { Set(ref _IsSimpleMode, value); }
		}
		private bool _IsDetailedMode;
		public bool IsDetailedMode
		{
			get { return _IsDetailedMode; }
			set { Set(ref _IsDetailedMode, value); }
		}
		private string _SqlConnectionString;
		public string SqlConnectionString
		{
			get { return _SqlConnectionString; }
			set { Set(ref _SqlConnectionString, value); }
		}
		private List<DbInfo> _SimpleInfo;
		public List<DbInfo> SimpleInfo
		{
			get { return _SimpleInfo; }
			set { Set(ref _SimpleInfo, value); }
		}
		private List<DbFile> _DetailedInfo;
		public List<DbFile> DetailedInfo
		{
			get { return _DetailedInfo; }
			set { Set(ref _DetailedInfo, value); }
		}
		private ICommand _GetInfoCommand;
		public ICommand GetInfoCommand
		{
			get
			{
				if (_GetInfoCommand == null)
					_GetInfoCommand = new RelayCommand(OnGetInfo);
				return _GetInfoCommand;
			}
		}
		private ICommand _GetDetailedInfoCommand;
		public ICommand GetDetailedInfoCommand
		{
			get
			{
				if (_GetDetailedInfoCommand == null)
					_GetDetailedInfoCommand = new RelayCommand(OnGetDetailedInfo);
				return _GetDetailedInfoCommand;
			}
		}
		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		public MainViewModel()
		{
			IsSimpleMode = true;
		}
		private void OnGetInfo()
		{
			IsDetailedMode = false;
			IsSimpleMode = true;
			try
			{
				Helpers.WaitableOperation(() =>
				{
					DbInfoReader dbReader = new DbInfoReader(SqlConnectionString);
					SimpleInfo = dbReader.GetDbInfo();
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Couldn't get database information", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		private void OnGetDetailedInfo()
		{
			IsDetailedMode = true;
			IsSimpleMode = false;
			try
			{
				Helpers.WaitableOperation(() =>
				{
					DbInfoReader dbReader = new DbInfoReader(SqlConnectionString);
					var temp = dbReader.GetDbInfo();
					List<DbFile> result = new List<DbFile>();
					temp.ForEach(x => result.AddRange(x.Files));
					DetailedInfo = result;
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Couldn't get database information", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}