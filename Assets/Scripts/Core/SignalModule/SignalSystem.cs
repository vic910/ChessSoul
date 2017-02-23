using System;
using System.Collections.Generic;

namespace Groot
{
	delegate void SignalCallback( SignalId _signal_id, SignalParameters _parameters );
	class SignalParameters
	{
		public SignalParameters()
		{
		}
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="_count">预先分配指定个数的对象</param>
		public SignalParameters( Int32 _count )
		{
			m_parameters.Capacity = _count;
		}
		/// <summary>
		/// 添加一个参数到参数列表
		/// </summary>
		/// <param name="_obj">添加一个参数</param>
		public void AddParameter( Object _obj )
		{
			m_parameters.Add( _obj );
		}
		/// <summary>
		/// 返回指定索引位置的对象
		/// </summary>
		/// <param name="_index">索引位置，索引取值范围为0-(个数-1)</param>
		/// <returns>返回索引位置所指向的对象，若索引值超出取值范围返回null</returns>
		public Object this[Int32 _index]
		{
			get
			{
				if( _index < 0 || _index >= m_parameters.Count )
					return null;
				return m_parameters[_index];
			}
		}
		/// <summary>
		/// 返回参数个数
		/// </summary>
		public Int32 Count { get { return m_parameters.Count; } }
		/// <summary>
		/// 清空所有参数
		/// </summary>
		public void Clear()
		{
			m_parameters.Clear();
		}
		/// <summary>
		/// 参数列表
		/// </summary>
		private List<Object> m_parameters = new List<object>();
	}

	static class SignalSystem
	{
		/// <summary>
		/// 注册一个回调函数，该回调函数将在指定信号触发时被调用
		/// </summary>
		/// <param name="_signal_id">信号编号，详细列表请参见SingalId的枚举定义</param>
		/// <param name="_callback">信号被触发时将被调用的函数</param>
		/// <returns></returns>
		public static bool Register( SignalId _signal_id, SignalCallback _callback )
		{
			if( m_signal_callbacks.ContainsKey( _signal_id ) )
				m_signal_callbacks[_signal_id] += _callback;
			else
				m_signal_callbacks.Add( _signal_id, _callback );
			return true;
		}
		/// <summary>
		/// 反注册一个回调函数，该回调函数在指定信号触发时将不再被调用
		/// </summary>
		/// <param name="_signal_id">信号编号，详细列表请参见SingalId的枚举定义</param>
		/// <param name="_callback">信号被触发时将被调用的函数</param>
		public static void Unregister( SignalId _signal_id, SignalCallback _callback )
		{
			if( m_signal_callbacks.ContainsKey( _signal_id ) )
			{
				m_signal_callbacks[_signal_id] -= _callback;
				if( null == m_signal_callbacks[_signal_id] )
					m_signal_callbacks.Remove( _signal_id );
			}
		}
		/// <summary>
		/// 触发信号，注册了该信号的回调函数将会被调用
		/// </summary>
		/// <param name="_signal_id">信号编号，详细列表请参见SingalId的枚举定义</param>
		public static void FireSignal( SignalId _signal_id )
		{
			SignalParameters parameters = new SignalParameters();
			FireSignal( _signal_id, parameters );
		}
		/// <summary>
		/// 触发信号，注册了该信号的回调函数将会被调用
		/// </summary>
		/// <param name="_signal_id">信号编号，详细列表请参见SingalId的枚举定义</param>
		/// <param name="_a1">第一个参数</param>
		public static void FireSignal( SignalId _signal_id, Object _a1 )
		{
			SignalParameters parameters = new SignalParameters( 1 );
			parameters.AddParameter( _a1 );
			FireSignal( _signal_id, parameters );
		}
		/// <summary>
		/// 触发信号，注册了该信号的回调函数将会被调用
		/// </summary>
		/// <param name="_signal_id">信号编号，详细列表请参见SingalId的枚举定义</param>
		/// <param name="_a1">第一个参数</param>
		/// <param name="_a2">第二个参数</param>
		public static void FireSignal( SignalId _signal_id, Object _a1, Object _a2 )
		{
			SignalParameters parameters = new SignalParameters( 2 );
			parameters.AddParameter( _a1 );
			parameters.AddParameter( _a2 );
			FireSignal( _signal_id, parameters );
		}
		/// <summary>
		/// 触发信号，注册了该信号的回调函数将会被调用
		/// </summary>
		/// <param name="_signal_id">信号编号，详细列表请参见SingalId的枚举定义</param>
		/// <param name="_a1">第一个参数</param>
		/// <param name="_a2">第二个参数</param>
		/// <param name="_a3">第三个参数</param>
		public static void FireSignal( SignalId _signal_id, Object _a1, Object _a2, Object _a3 )
		{
			SignalParameters parameters = new SignalParameters( 3 );
			parameters.AddParameter( _a1 );
			parameters.AddParameter( _a2 );
			parameters.AddParameter( _a3 );
			FireSignal( _signal_id, parameters );
		}
		/// <summary>
		/// 触发信号，注册了该信号的回调函数将会被调用
		/// </summary>
		/// <param name="_signal_id">信号编号，详细列表请参见SingalId的枚举定义</param>
		/// <param name="_a1">第一个参数</param>
		/// <param name="_a2">第二个参数</param>
		/// <param name="_a3">第三个参数</param>
		/// <param name="_a4">第四个参数</param>
		public static void FireSignal( SignalId _signal_id
			, Object _a1
			, Object _a2
			, Object _a3
			, Object _a4 )
		{
			SignalParameters parameters = new SignalParameters( 4 );
			parameters.AddParameter( _a1 );
			parameters.AddParameter( _a2 );
			parameters.AddParameter( _a3 );
			parameters.AddParameter( _a4 );
			FireSignal( _signal_id, parameters );
		}
		/// <summary>
		/// 触发信号，注册了该信号的回调函数将会被调用
		/// </summary>
		/// <param name="_signal_id">信号编号，详细列表请参见SingalId的枚举定义</param>
		/// <param name="_a1">第一个参数</param>
		/// <param name="_a2">第二个参数</param>
		/// <param name="_a3">第三个参数</param>
		/// <param name="_a4">第四个参数</param>
		/// <param name="_a5">第五个参数</param>
		/// <param name="_a6">第六个参数</param>
		public static void FireSignal( SignalId _signal_id
			, Object _a1
			, Object _a2
			, Object _a3
			, Object _a4
			, Object _a5
			, Object _a6 )
		{
			SignalParameters parameters = new SignalParameters( 6 );
			parameters.AddParameter( _a1 );
			parameters.AddParameter( _a2 );
			parameters.AddParameter( _a3 );
			parameters.AddParameter( _a4 );
			parameters.AddParameter( _a5 );
			parameters.AddParameter( _a6 );
			FireSignal( _signal_id, parameters );
		}
		/// <summary>
		/// 触发信号，注册了该信号的回调函数将会被调用
		/// </summary>
		/// <param name="_signal_id">信号编号，详细列表请参见SingalId的枚举定义</param>
		/// <param name="_parameters">参数列表</param>
		public static void FireSignal( SignalId _signal_id, SignalParameters _parameters )
		{
			//if( m_is_firing )
			//{
			//	Utility.Log.Error( "信号[{0}]触发正在处理中不能再触发信号[{1}]"
			//		, m_present_signal.ToString()
			//		, _signal_id.ToString() );
			//	return;
			//}
			//try
			//{
				//m_present_signal = _signal_id;
				//m_is_firing = true;
				SignalCallback callback;
				if( m_signal_callbacks.TryGetValue( _signal_id, out callback ) )
				{
					callback( _signal_id, _parameters );
				}
			//}
			/*catch( Exception e )
			{
				//m_is_firing = false;
				Utility.Log.Error( "Something goes wrong on fire signal: {0}", e.Message );
			}
			finally
			{
				//m_is_firing = false;
			}*/
		}
		/// <summary>
		/// 清理
		/// </summary>
		public static void Clear()
		{
			m_signal_callbacks.Clear();
		}
		/// <summary>
		/// 回调函数列表
		/// </summary>
		private static Dictionary<SignalId, SignalCallback> m_signal_callbacks = new Dictionary<SignalId, SignalCallback>();
		/// <summary>
		/// 当前是否正在处理某信号的触发事件
		/// </summary>
		//private static bool m_is_firing = false;
		//private static SignalId m_present_signal;
	}
}
