using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Groot.Res
{
	class ExceptionAssetBundleDependenciesStateError : Exception
	{
		public ResourceState State { get; set; } 
		public ExceptionAssetBundleDependenciesStateError( ResourceState _state )
		{
			State = _state;
		}
	}
}
