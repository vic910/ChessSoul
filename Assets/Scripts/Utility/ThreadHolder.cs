using System;
using System.Threading;

namespace Utility
{
	sealed class ThreadHolder
	{
		public ThreadHolder( ThreadStart _func )
		{
			m_thread_func = _func;
		}
		public bool CreateThread()
		{
			if( null == m_thread_func )
				return false;
			m_thread = new Thread( m_thread_func );
			m_thread.Start();
			return true;
		}
		public void StopThread()
		{
			IsStopped = true;
		}
		public void WaitForThread()
		{
			try
			{
				m_thread.Join();
			}
			catch( ThreadStateException )
			{
				Log.Error( "Try to wait an unstarted thread." );
			}
		}
		public bool IsStopped
		{
			get { lock( m_token_locker ) { return m_stop_token; } }
			private set { lock( m_token_locker ) { m_stop_token = value; } }
		}
		private ThreadStart m_thread_func = null;
		private Thread m_thread = null;
		private Object m_token_locker = new Object();
		private bool m_stop_token = false;		// should the thread exit?
	}
}
