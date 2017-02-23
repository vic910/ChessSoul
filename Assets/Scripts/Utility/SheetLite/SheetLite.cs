﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Utility.SheetLite
{
	public class SheetLite
	{
		class InvalidFormatException : ExceptionEx
		{
			public InvalidFormatException( String _msg )
				: base( _msg )
			{ }
		};

		public bool Open( String _sheet_name, Byte[] _bytes )
		{
			using( MemoryStream memory_stream = new MemoryStream( _bytes ) )
			{
				using( StreamReader stream = new StreamReader( memory_stream ) )
				{
					bool key_loaded = false;
					while( !stream.EndOfStream )
					{
						String line = stream.ReadLine();
						if( 0 == line[0] || '#' == line[0] || ( '\r' == line[0] && 0 == line[1] ) )
							continue;
						if( '&' == line[0] )
						{
							if( !key_loaded )
							{
								int key_count = 0;
								try
								{
									_parseLine( line, 1, delegate ( String _key ) {
										if( String.Empty == _key )
											throw new InvalidFormatException( String.Format( "配置表[{0}]表头中存在空键值。", _sheet_name ) );
										if( m_key_to_index.ContainsKey( _key ) )
											throw new InvalidFormatException( String.Format( "配置表[{0}]表头中存在多个同样的键值[{1}]。", _sheet_name, _key ) );
										m_key_to_index[_key] = key_count;
										++key_count;
									} );
								}
								catch( InvalidFormatException e )
								{
									Log.Error( e.Message );
									return false;
								}

								key_loaded = true;
							}
							else
							{
								Log.Error( "[SheetLite]: Duplicated key is found in {0}", _sheet_name );
							}
						}
						else
						{
							SheetRow row = new SheetRow( this );
							_parseLine( line, 0, delegate ( String _value ) {
								row.Append( _value );
							} );
							if( row.Count != m_key_to_index.Count )
							{
								Log.Error( "[SheetLite]: The data of {0}th row is not incomplete or overload in {1}", row.Count, _sheet_name );
								continue;
							}
							m_rows.Add( row );
						}
					}
					SheetName = _sheet_name;
					return true;
				}
			}
		}

		public Int32 Count { get { return m_rows.Count; } }
		public SheetRow this[Int32 _index]
		{
			get
			{
				if( 0 > _index || m_rows.Count <= _index )
					return null;
				return m_rows[_index];
			}
		}
		public Int32 GetKeyIndex( String _key )
		{
			Int32 index = 0;
			if( !m_key_to_index.TryGetValue( _key, out index ) )
				return -1;
			return index;
		}
		// Private methods
		private delegate void ParseLineFunctor( String _value );
		private static void _parseLine( String _line, Int32 _offset, ParseLineFunctor _functor )
		{
			Func<String, Int32, bool> is_tail_char = delegate( String _value, Int32 _cursor )
			{
				if( _cursor >= _value.Length )
					return true;
				Char c = _value[_cursor];
				return ( 0 == c || '\n' == c || '\r' == c );
			};
			while( true )
			{
				StringBuilder buff = new StringBuilder( 1024 );
				bool mark = false;
				if( _offset >= _line.Length )
				{
					_functor( "" );
					break;
				}
				if( '"' == _line[_offset] )
				{
					++_offset;
					mark = true;
				}
				while( true )
				{
					if( is_tail_char( _line, _offset ) )
					{
						//AUX_ASSERT( !mark && "Invalid CSV Format!" );
						break;
					}
					if( '\t' == _line[_offset] && !mark )
						break;
					if( '"' == _line[_offset] )
					{
						//AUX_ASSERT( mark && "Invalid CSV Format!" );
						if( is_tail_char( _line, _offset + 1 ) )
							break;
						Char next = _line[_offset + 1];
						if( '"' == next )
						{
							++_offset;
						}
						else if( '\t' == next )
						{
							break;
						}
						else
						{
							//AUX_ASSERT( !"Invalid CSV Format!" );
						}
					}
					buff.Append( _line[_offset] );
					++_offset;
				}
				String value = buff.ToString();
				_functor( value );
				if( mark )
					++_offset;
				if( is_tail_char( _line, _offset ) )
					break;
				++_offset;      // skip delimiter
			}
		}
		// Data member
		private Dictionary<String, Int32> m_key_to_index = new Dictionary<String, Int32>();
		private List<SheetRow> m_rows = new List<SheetRow>();
		public String SheetName { get; private set; }
	}
}