using System;

namespace Utility
{
	//public delegate void Action();
	//public delegate void Action<T1>( T1 _arg1 );
	//public delegate void Action<T1, T2>( T1 _arg1, T2 _arg2 );
	//public delegate void Action<T1, T2, T3>( T1 _arg1, T2 _arg2, T3 _arg3 );
	//public delegate void Action<T1, T2, T3, T4>( T1 _arg1, T2 _arg2, T3 _arg3, T4 _arg4 );
	public delegate void Action<in T1, in T2, in T3, in T4, in T5>( T1 _arg1, T2 _arg2, T3 _arg3, T4 _arg4, T5 _arg5 );
	public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6>( T1 _arg1, T2 _arg2, T3 _arg3, T4 _arg4, T5 _arg5, T6 _arg6 );


	public delegate TResult MyFunc<T, TResult>( out T _arg );
	public delegate TResult MyFunc<in T1, T2, TResult>( T1 _arg1, out T2 _arg2 );
	public delegate TResult MyFunc<in T1, in T2, T3, TResult>( T1 _arg1, T2 _arg2, out T3 _arg3 );

	public delegate Boolean Predicate<in T1, in T2>( T1 _arg1, T2 _arg2 );
	public delegate Boolean Predicate<in T1, in T2, in T3>( T1 _arg1, T2 _arg2, T3 _arg3 );
	public delegate Boolean Predicate<in T1, in T2, in T3, in T4>( T1 _arg1, T2 _arg2, T3 _arg3, T4 _arg4 );

	// TODO 不够再加...
}
