using System;
using System.Threading;
using System.Collections.Generic;

namespace Utility
{
	class TimerSystem
	{
		public static readonly TimerSystem Instance = new TimerSystem();
		class FireEvent : Groot.GameEvent
		{
			public FireEvent( Timer _timer, Int64 _interval )
			{
				m_timer = _timer;
				m_interval = _interval;
			}
			public override void ExecuteEvent()
			{
				m_timer._riseTimesUp( m_fire_time, m_interval );
			}
			private Timer m_timer = null;
			private DateTime m_fire_time = DateTime.Now;
			private Int64 m_interval = 0;
		}
		public TimerSystem()
		{
		}
		public bool Initialize()
		{
			// create thread
			m_thread = new ThreadHolder( Run );
			if( !m_thread.CreateThread() )
			{
				m_thread = null;
				return false;
			}
			return true;
		}
		public void Uninitialize()
		{
			m_thread.StopThread();
			m_event.Set();
			m_thread.WaitForThread();
		}
		// put the timer into the schedule table
		public void ScheduleTimer( Timer _timer )
		{
			_timer.Enabled = false;
			Request action = new Request( RequestType.Schedule, _timer );
			m_requests.Enqueue( action );
			m_event.Set();
		}
		// remove the timer from the schedule table
		public void UnscheduleTimer( Timer _timer )
		{
			_timer.Enabled = false;
			Request action = new Request( RequestType.Unschedule, _timer );
			m_requests.Enqueue( action );
			m_event.Set();
		}

		private void Run()
		{
			//Log.Info( "TimerSystem has started." );
			m_watch.Start();
			while( !m_thread.IsStopped )
			{
				_updateCurrentTime();
				// Handle all schedules which time is already up
				_handleSchedules();
				// Handle all requests
				_handleRequests();
				// Wait for the next timer...
				Int32 wait_time = _getLatestTime();
				//Log.Info( "I'll wait for {0} milliseconds.", wait_time );
				if( m_event.WaitOne( wait_time ) )
				{
					//Log.Info( "TimerSystem got a signal." );
				}
				else
				{
					//Log.Info( "TimerSystem will wake up some timer now." );
				}
			}
			m_watch.Stop();
			//Log.Info( "TimerSystem has terminated." );
		}

		private void _handleRequests()
		{
			Request request = null;
			while( m_requests.Dequeue( out request ) )
			{
				switch( request.Type )
				{
				case RequestType.Schedule:
					_scheduleTimer( request.Timer );
					break;
				case RequestType.Unschedule:
					_unscheduleTimer( request.Timer );
					break;
				}
			}
			m_schedules.Sort( ( s1, s2 ) => { return (Int32)( s2.DetonateTime - s1.DetonateTime ); } );
		}
		private void _handleSchedules()
		{
			Int32 count = m_schedules.Count;
			for( Int32 i = count - 1; i >= 0; --i )
			{
				if( m_schedules[i].DetonateTime > m_current_time )
					break;
				_riseTimesUp( m_schedules[i] );
				Timer timer = m_schedules[i].Timer;
				timer.InSchedule = false;
				if( timer.Enabled && timer.AutoReset )
				{
					Request action = new Request( RequestType.Schedule, timer );
					m_requests.Enqueue( action );
				}
				m_schedules.RemoveAt( i );
			}
		}

		private Int32 _getLatestTime()
		{
			Int32 count = m_schedules.Count;
			if( 0 == count )
				return -1;
			_updateCurrentTime();
			Int64 interval = m_schedules[count - 1].DetonateTime - m_current_time;
			return (Int32)( interval < 0 ? 0 : interval );
		}

		private void _riseTimesUp( Schedule _schedule )
		{
			//Utility.Log.Warning( "PushEvent of TimeUp" );
			//GameWorld状态机重调 暂时不推送定时器事件 2016 -10-20
			FireEvent fire_event = new FireEvent( _schedule.Timer, _schedule.Interval );
			GameApp.Instance.PushEvent( fire_event );
		}

		private void _scheduleTimer( Timer _timer )
		{
			_unscheduleTimer( _timer );
			Schedule schedule = new Schedule( _timer, m_current_time + _timer.Interval, _timer.Interval );
			m_schedules.Add( schedule );
			_timer.InSchedule = true;
			_timer.Enabled = true;
		}
		private void _unscheduleTimer( Timer _timer )
		{
			if( !_timer.InSchedule )
				return;
			m_schedules.RemoveAll( schedule => { return schedule.Timer.TimerId == _timer.TimerId; } );
			_timer.InSchedule = false;
		}

		private void _updateCurrentTime()
		{
			m_current_time = m_watch.ElapsedMilliseconds;
		}

		enum RequestType
		{
			Schedule,
			Unschedule,
		}
		class Request
		{
			public Request( RequestType _type, Timer _timer )
			{
				Type = _type;
				Timer = _timer;
			}
			public RequestType Type { get; private set; }
			public Timer Timer { get; private set; }
		}
		class Schedule
		{
			public Schedule( Timer _timer, Int64 _time, Int64 _interval )
			{
				Timer = _timer;
				DetonateTime = _time;
				Interval = _interval;
			}
			public Timer Timer { get; private set; }
			public Int64 DetonateTime { get; private set; }
			public Int64 Interval { get; private set; }
		}


		private ThreadHolder m_thread = null;
		private System.Threading.AutoResetEvent m_event = new System.Threading.AutoResetEvent( false );
		private Utility.SynchronizedQueue<Request> m_requests = new Utility.SynchronizedQueue<Request>();
		private List<Schedule> m_schedules = new List<Schedule>();
		private Int64 m_current_time = 0;
		private System.Diagnostics.Stopwatch m_watch = new System.Diagnostics.Stopwatch();
	}
}
