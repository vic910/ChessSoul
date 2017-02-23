using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Utility
{
	/// <summary>
	/// 给定目录下给定类型的文件, 并按照文件名排序
	/// </summary>
	public static class FileHelper
	{
		/// <summary>
		/// 获得相对路径
		/// </summary>
		/// <param name="_full"></param>
		/// <param name="_root"></param>
		/// <returns></returns>
		public static String GetRelativePath( String _full, String _root )
		{
			String path = _full.Substring( _root.Length + 1, _full.Length - _root.Length - 1 );
			path = path.Replace( '\\', '/' );
			return path;
		}

		/// <summary>
		/// 获取文件路径
		/// </summary>
		/// <param name="_file_path"></param>
		/// <returns></returns>
		public static String GetDirectoryPathFromFilePath( String _file_path )
		{
			String path = _file_path.Substring( 0, _file_path.LastIndexOfAny( new []{ '/', '\\' } ) );
			return path;
		}

		/// <summary>
		/// 根据文件路径获取文件名
		/// </summary>
		/// <param name="_file_path"></param>
		/// <param name="_with_extension"></param>
		/// <returns></returns>
		public static String GetFileNameByPath( String _file_path, Boolean _with_extension )
		{
			Int32 tmp = _file_path.LastIndexOfAny( new []{ '/', '\\' } );
			String name =  _file_path.Substring( tmp + 1, _file_path.Length - tmp - 1 );

			if( _with_extension )
				return name;

			Int32 tmp2 = name.LastIndexOf( '.' );
			return tmp2 == -1 ? name : name.Substring( 0, tmp2 );
		}

		/// <summary>
		/// 读取文件的二进制
		/// </summary>
		/// <param name="_file_path"></param>
		/// <returns></returns>
		public static Byte[] ReadAllBytesFromFile( String _file_path )
		{
			if( !File.Exists( _file_path ) )
				throw new FileNotFoundException( String.Format( "FileHelper.ReadAllBytesFromFile: file {0} not exist!", _file_path ) );
			Byte[] bytes;
			using( FileStream file_stream = new FileStream( _file_path, FileMode.Open, FileAccess.Read ) )
			{
				bytes = new Byte[file_stream.Length];
				// 假定文件不会超过 2G... 事实上也不可能超过... 
				Int32 num_bytes_to_read = (Int32)file_stream.Length;
				Int32 num_bytes_read = 0;
				while( num_bytes_to_read > 0 )
				{
					// Read may return anything from 0 to numBytesToRead. 
					Int32 n = file_stream.Read(bytes, num_bytes_read, num_bytes_to_read);

					// Break when the end of the file is reached. 
					if( n == 0 )
						break;

					num_bytes_read += n;
					num_bytes_to_read -= n;
				}
			}
			return bytes;
		}

		public static List<FileInfo> GetAllFileInfo( String _path, String _filter = "*" )
		{
			if( !Directory.Exists( _path ) )
				return null;

			return GetAllFileInfo( new DirectoryInfo( _path ), _filter );
		}
		public static List<DirectoryInfo> GetAllDirectories( String _path )
		{
			if( !Directory.Exists( _path ) )
				return null;
			var dir = new DirectoryInfo( _path );
			var dir_list = new List<DirectoryInfo>();
			dir_list.AddRange( dir.GetDirectories() );
			if( dir_list.Count == 0 )
				return null;
			return dir_list;
		}

		public static List<FileInfo> GetAllFileInfo( DirectoryInfo _directory, String _filter )
		{
			List<FileInfo> file_list = new List<FileInfo>();
			DirectoryInfo[] sub_directories = _directory.GetDirectories();
			foreach( DirectoryInfo sub_directory in sub_directories )
			{
				var sub_dirfiles = GetAllFileInfo( sub_directory, _filter );
				if( sub_dirfiles != null )
					file_list.AddRange( sub_dirfiles );
			}

			FileInfo[] files = _directory.GetFiles( _filter );
			file_list.AddRange( files );

			if( file_list.Count != 0 )
				return file_list;
			return null;
		}

		/// <summary>
		/// 获取给定目录下所有的制定类型文件
		/// </summary>
		/// <param name="_str_path"></param>
		/// <param name="_search_pattern"></param>
		public static List<FileInfo> GetFiles( String _str_path, String _search_pattern )
		{
			DirectoryInfo dir = new DirectoryInfo( _str_path );
			//fileNames = new List<String>();
			FileInfo[] file_arrs = dir.GetFiles( _search_pattern );
			List<FileInfo> files = new List<FileInfo>();
			files.AddRange( file_arrs );

			DirectoryInfo[] subdir = dir.GetDirectories();
			foreach( DirectoryInfo t in subdir )
			{
				files.AddRange( GetFiles( t.FullName, _search_pattern ) );
			}
			return files;
		}
		/// <summary>
		/// 文件存在性检测
		/// </summary>
		/// <param name="_file_names">文件名集合</param>
		/// <returns></returns>
		public static Boolean FileExist( List<String> _file_names )
		{
			if( _file_names == null )
				return false;

			foreach( String file_name in _file_names )
			{
				if( !File.Exists( file_name ) )
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// 文件存在性检测
		/// </summary>
		/// <param name="_file_name">文件名</param>
		/// <returns></returns>
		public static Boolean FileExist( String _file_name )
		{
			if( !File.Exists( _file_name ) )
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// 删除文件
		/// </summary>
		/// <param name="_file_names">文件名集合</param>
		public static void DeleteFiles( IEnumerable<String> _file_names )
		{
			foreach( String file_name in _file_names )
			{
				if( File.Exists( file_name ) )
					File.Delete( file_name );
			}
		}

		/// <summary>
		/// 删除文件
		/// </summary>
		/// <param name="_file_name"></param>
		public static void DeleteFile( String _file_name )
		{
			if( File.Exists( _file_name ) )
				File.Delete( _file_name );
		}

		public static Boolean DirectoryExsits( String _dir_path )
		{
			return Directory.Exists( _dir_path );
		}

		/// <summary>
		/// 删除空文件夹
		/// </summary>
		/// <param name="_directory_name"></param>
		public static void DeleteEmptyDirectory( String _directory_name )
		{
			if( !Directory.Exists( _directory_name ) )
				return;

			String[] sub_directoies =  Directory.GetDirectories( _directory_name );

			foreach( String sub_directoy in sub_directoies )
			{
				DeleteEmptyDirectory( sub_directoy );
			}

			if( Directory.GetDirectories( _directory_name ).Length == 0
				&& Directory.GetFiles( _directory_name ).Length == 0 )
			{
				Directory.Delete( _directory_name );
			}
		}

		/// <summary>
		/// 删除文件夹
		/// </summary>
		/// <param name="_directory_names">文件夹名</param>
		/// <param name="_recursive">是否删除子文件夹</param>
		public static void DeleteDirectory( String _directory_names, Boolean _recursive )
		{
			if( Directory.Exists( _directory_names ) )
				Directory.Delete( _directory_names, _recursive );
		}

		/// <summary>
		/// 删除文件夹
		/// </summary>
		/// <param name="_directory_names">文件夹集合</param>
		/// <param name="_recursive"></param>
		public static void DeleteDirectories( IList<String> _directory_names, Boolean _recursive )
		{
			if( _directory_names == null )
				return;
			foreach( String dir in _directory_names )
			{
				if( Directory.Exists( dir ) )
					Directory.Delete( dir, _recursive );
			}
		}

		/// <summary>
		/// 根据文件名创建目录
		/// </summary>
		/// <param name="_file_path"></param>
		public static void CreateDirectoryByFilePath( String _file_path )
		{
			CreateDirectory( GetDirectoryPathFromFilePath( _file_path ) );
		}

		public static void DirectoryCopy( String _source_dir_name, String _dest_dir_name, Boolean _copy_sub_dirs, Boolean _overwrite )
		{
			// Get the subdirectories for the specified directory.
			DirectoryInfo dir = new DirectoryInfo( _source_dir_name );

			if( !dir.Exists )
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ _source_dir_name );
			}

			DirectoryInfo[] dirs = dir.GetDirectories();
			// If the destination directory doesn't exist, create it.
			if( !Directory.Exists( _dest_dir_name ) )
			{
				Directory.CreateDirectory( _dest_dir_name );
			}

			// Get the files in the directory and copy them to the new location.
			FileInfo[] files = dir.GetFiles();
			foreach( FileInfo file in files )
			{
				String temppath = Path.Combine( _dest_dir_name, file.Name );
				file.CopyTo( temppath, _overwrite );
			}

			// If copying subdirectories, copy them and their contents to new location.
			if( !_copy_sub_dirs )
				return;
			foreach( DirectoryInfo subdir in dirs )
			{
				String temppath = Path.Combine( _dest_dir_name, subdir.Name );
				DirectoryCopy( subdir.FullName, temppath, true, _overwrite );
			}
		}

		/// <summary>
		/// 创建文件夹
		/// </summary>
		/// <param name="_path"></param>
		public static void CreateDirectory( String _path )
		{
			if( Directory.Exists( _path ) )
				return;

			//创建所有父级文件夹
			List<String> parents = new List<String>( _path.Split( '\\', '/' ) );
			StringBuilder string_buider = new StringBuilder( 512 );

			foreach( String parent in parents )
			{
				string_buider.AppendFormat( "{0}/", parent );
				if( !Directory.Exists( string_buider.ToString() ) )
				{
					Directory.CreateDirectory( string_buider.ToString() );
				}
			}
		}

		/*/// <summary>
		/// 复制文件夹下所有文件到目标位置, 创建目标文件夹(如果需要)
		/// </summary>
		/// <param name="source"></param>
		/// <param name="dest"></param>
		/// <param name="overwrite"></param>
		public static void CopyDirectory( String source, String dest, Boolean overwrite )
		{
			CreateDirectory( dest );
			//Console.WriteLine( "directory: " + dest );
			// 复制文件
			try
			{
				String[] fileNames = Directory.GetFiles( source, "*.*" );
				foreach ( String fileName in fileNames )
				{
					String[] name = fileName.Split( '\\' );
					String destf = String.Format( "{0}\\{1}", dest, name[name.Length - 1] );
					File.Copy( fileName, destf, overwrite );
					//updateState(destf, System.Drawing.Color.Green);
					//Console.WriteLine( "file: " + name[name.Length - 1] );
				}
			}
			catch ( System.Exception ex )
			{
				Console.WriteLine( ex.Message );
				if ( updateState != null )
					updateState( ex.Message + "\n" + source, System.Drawing.Color.Red );
			}
			// 复制文件夹
			try
			{
				String[] dirNames = Directory.GetDirectories( source );
				foreach ( String dir in dirNames )
				{
					DirectoryInfo dirInfo = new DirectoryInfo( dir );
					String sorceDir = String.Format( "{0}\\{1}", source, dirInfo.Name );
					String destDir = String.Format( "{0}\\{1}", dest, dirInfo.Name );
					CopyDirectory( sorceDir, destDir, true );
				}
			}
			catch ( System.Exception ex )
			{
				Console.WriteLine( ex.Message );
				if ( updateState != null )
					updateState( ex.Message + "\n" + source, System.Drawing.Color.Red );
			}
		}

		/// <summary>
		/// 拷贝文件到目标位置
		/// </summary>
		/// <param name="source"></param>
		/// <param name="dest"></param>
		/// <param name="overWrite"></param>
		/*public static void CopyFile( String source, String dest, Boolean overWrite )
		{
			StringBuilder sb = new StringBuilder();
			if ( !File.Exists( source ) )
			{
				sb.AppendFormat( "File: {0} Not Exist!", source );
				if ( updateState != null )
					updateState( sb.ToString(), System.Drawing.Color.Red );
				Console.WriteLine( sb.ToString() );
				return;
			}
			FileInfo fi = new FileInfo( source );

			FileHelper.CreateDirectory( fi.DirectoryName );
			try
			{
				File.Copy( source, dest, overWrite );
				//updateState("copyFile Done\n" + source + "\n" + dest, System.Drawing.Color.Green);
			}
			catch ( Exception e )
			{
				if ( updateState != null )
					updateState( e.Message + "\n" + source + "\n" + dest, System.Drawing.Color.Red );
				Console.WriteLine( "FileHelper.CopyFile: error! {0}", e.Message );
			}

		}

		/// <summary>
		/// 复制某类文件(循环参数)
		/// </summary>
		/// <param name="srcPath"></param>
		/// <param name="destPath"></param>
		/// <param name="type"></param>
		/// <param name="recursive"></param>
		public static void CopyFileByType( String srcPath, String destPath, String type, Boolean recursive, Boolean overwrite )
		{
			CreateDirectory( destPath );
			try
			{
				String[] files = Directory.GetFiles( srcPath, type );
				foreach ( String file in files )
				{
					FileInfo fileInfo = new FileInfo( file );
					String destFile = String.Format( "{0}\\{1}", destPath, fileInfo.Name );
					File.Copy( file, destFile, overwrite );
#if DEBUG
					Console.WriteLine( destFile + " done!" );
#endif
				}
			}
			catch ( System.Exception e )
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat( "CopyFileByType: {0}\n", e.Message );
				sb.AppendFormat( "srcPath: {0}\n", srcPath );
				sb.AppendFormat( "destPath: {0}\n", destPath );
				sb.AppendFormat( "type: {0}\n", type );
				if ( updateState != null )
					updateState( sb.ToString(), System.Drawing.Color.Red );
				Console.WriteLine( sb.ToString() );
			}
			if ( !recursive )
				return;
			String[] dirs = Directory.GetDirectories( srcPath );
			foreach ( String dir in dirs )
			{
				String[] subDirs = dir.Split( '\\' );
				String newDestPath = String.Format( "{0}\\{1}", destPath, subDirs[subDirs.Length - 1] );

				CopyFileByType( dir, newDestPath, type, recursive, true );
			}

		}*/

	}

}
