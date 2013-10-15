using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Microsoft.Win32;

namespace DataObjects
{
	/// <remarks>
	/// Encapsulates Registry management functions.
	/// </remarks>
	public static class RegistryManager
	{
		#region Member variables

		private static Microsoft.Win32.RegistryKey _root = Application.UserAppDataRegistry;	// application data key
		private static int _maxRecentFiles = 10;	// number of recent files to store

		#endregion

		#region Properties

		public static int MaxRecentFiles
		{
			get { return _maxRecentFiles; }
			set { _maxRecentFiles = value; }
		}

		public static StringList RecentFiles
		{
			get
			{
				return GetRecentFiles();
			}
		}

		#endregion

		#region Application settings

		/// <summary>
		/// Load a integer from the specified registry key. 
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value, or int.MinValue if not found.</returns>
		public static int LoadValue(string key)
		{
			object regObject = _root.GetValue(key);

			int value = int.MinValue;
			if (regObject != null)
				value = (int)regObject;
			return value;
		}

		/// <summary>
		/// Store key-value pair in application registry.  Value should be integer
		/// </summary>
		public static void StoreValue(string key, object value)
		{
			_root.SetValue(key, value);
		}

		/// <summary>
		/// Get the list of recent files.
		/// </summary>
		/// <returns>A list of recent paths, in order from most to least recent.</returns>
		public static StringList GetRecentFiles()
		{
			string[] arrFiles = _root.GetValue("RecentFiles", null) as string[];

			StringList listFiles = new StringList();

			if (arrFiles == null)
				return listFiles;	// empty list

			foreach (string file in arrFiles)
			{
				listFiles.Add(file);	// append in order from most to least recent
			}

			return listFiles;
		}

		/// <summary>
		/// Add another file to the list of recent files.
		/// </summary>
		/// <param name="path">The path to the file.</param>
		public static void StoreRecentFile(string path)
		{
			if (path == string.Empty)
				return;

			StringList listFiles = GetRecentFiles();

			// If the file is already in the list, remove it (so it will be added only once)
			if (listFiles.Contains(path))
			{
				listFiles.Remove(path);
			}

			// Pre-pend the file to the list
			listFiles.Insert(0, path);

			// Limit the number of files to MaxRecentFiles setting (default = 10)
			if (listFiles.Count > _maxRecentFiles)
				listFiles.RemoveRange(_maxRecentFiles, listFiles.Count - _maxRecentFiles);

			// Store to registry
			_root.SetValue("RecentFiles", listFiles.ToArray(), RegistryValueKind.MultiString);
		}

		#endregion

		/// <summary>
		/// Development only.
		/// </summary>
		public static void Main()
		{
			StoreRecentFile("c:\\temp\\file0.jpg");
			StoreRecentFile("c:\\temp\\file1.jpg");
			StoreRecentFile("c:\\temp\file2.jpg");
			StoreRecentFile("c:\\temp\\file3.jpg");
			StoreRecentFile("c:\\temp\\file4.jpg");
			StoreRecentFile("c:\\temp\\file5.jpg");
			StoreRecentFile("c:\\temp\\file6.jpg");
			StoreRecentFile("c:\\temp\\file7.jpg");
			StoreRecentFile("c:\\temp\\file8.jpg");
			StoreRecentFile("c:\\temp\\file9.jpg");
			StoreRecentFile("c:\\temp\\file10.jpg");
			StoreRecentFile("c:\\temp\\file11.jpg");
			StoreRecentFile("c:\\temp\\file1.jpg");	// this should be moved to the front

			StringList listFiles = RecentFiles;
		}

	}	// class

}	// namespace
