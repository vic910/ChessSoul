using System;
using System.Security.Cryptography;
using System.Text;

namespace Utility
{
	public class MD5Helper
	{
		public static String GetMD5Hash( Byte [] _bytes )
		{
			byte[] hash_data = null;
			using( MD5 md5_hash = MD5.Create() )
			{
				// Convert the input String to a byte array and compute the hash. 
				hash_data = md5_hash.ComputeHash( _bytes );
			}

			// Create a new Stringbuilder to collect the bytes 
			// and create a String.
			StringBuilder string_buider = new StringBuilder();

			// Loop through each byte of the hashed data  
			// and format each one as a hexadecimal String. 
			foreach( Byte b in hash_data )
			{
				string_buider.Append( b.ToString( "x2" ) );
			}
			// Return the hexadecimal String. 
			return string_buider.ToString();
		}

		public static Boolean VerifyMD5Hash( String _hash1, String _hash2 )
		{
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;

			return 0 == comparer.Compare( _hash1, _hash2 );
		}
	}
}
