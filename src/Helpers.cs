using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DbSpace
{
	public static class Helpers
	{
		public static void WaitableOperation(Action action, Window window = null)
		{
			if (window == null)
				window = App.Current.MainWindow;
			Cursor currentCursor = window.Cursor;
			window.Cursor = Cursors.Wait;
			try { action(); }
			finally
			{
				window.Cursor = currentCursor;
			}
		}
	}
}
