using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
	static class CSharpExtensionMethod
	{
		public static void RemoveNull<T>(this List<T> _list )
		{
			// 找出第一个空元素 O(n)
			int count = _list.Count;
			for( Int32 i = 0; i < count; i++ )
			{
				if( _list[i] == null )
				{
					// 记录当前位置
					int new_count = i++;

					// 对每个非空元素，复制至当前位置 O(n)
					for( ; i < count; i++ )
						if( _list[i] != null )
							_list[new_count++] = _list[i];

					// 移除多余的元素 O(n)
					_list.RemoveRange( new_count, count - new_count );
					break;
				}
			}
		}
	}
}
