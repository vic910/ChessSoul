using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.EqualityComparer
{
	class StringCompare : IEqualityComparer<String>
	{
		public static readonly StringCompare DefaultInstance = new StringCompare();

		public Boolean Equals( String x, String y )
		{
			return x.Equals( y, StringComparison.Ordinal );
		}

		public Int32 GetHashCode( String obj )
		{
			return obj.GetHashCode();
		}
	}
}
