using System;
using System.Runtime.InteropServices;

namespace Utility.SheetLite
{
	/// <summary>
	/// Sheet: 类型Module解析错误
	/// </summary>
	public class ExceptionSheetParseError : ExceptionEx
	{
		public ExceptionSheetParseError( String _file, Int32 _line, String _msg )
			: base( "Sheet解析错误 filename: {0} line: {1}\t {2}", _file, _line, _msg )
		{
			Log.Exception( "Sheet解析错误 filename: {0} line: {1}\t {2}", _file, _line, _msg );
		}
	}
	public class ExceptionSheetGenerate : ExceptionEx
	{
		public ExceptionSheetGenerate( String _file, Int32 _line, String _msg )
			: base( "Sheet生成错误 filename: {0} line: {1}\t {2}", _file, _line, _msg )
		{
			Log.Exception( "Sheet生成错误 filename: {0} line: {1}\t {2}", _file, _line, _msg );
		}
	}
}