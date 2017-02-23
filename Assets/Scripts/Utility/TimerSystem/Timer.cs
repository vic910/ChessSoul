using System;

namespace Utility
{
	public class Timer
	{
		public enum DispatcherType
		{
			Game,
			Localhost,
		}
		public event Action<Timer, DateTime, Int64> eventTimesUp;
		private Timer( UInt64 _timer_id, DispatcherType _type )
		{
			m_timer_id = _timer_id;
			m_dispatcher_type = _type;
		}
		public void Start()
		{
			TimerSystem.Instance.ScheduleTimer( this );
		}
		public void Stop()
		{
			TimerSystem.Instance.UnscheduleTimer( this );
		}
		public void _riseTimesUp( DateTime _fire_time, Int64 _interval )
		{
			eventTimesUp( this, _fire_time, _interval );
		}

		public static Timer CreateTimer( Int64 _interval, DispatcherType _type = DispatcherType.Game )
		{
			return _createTimerImpl( _interval, false, _type );
		}
		public static Timer CreateAutoResetTimer( Int64 _interval, DispatcherType _type = DispatcherType.Game )
		{
			return _createTimerImpl( _interval, true, _type );
		}

		private static Timer _createTimerImpl( Int64 _interval, bool _auto_reset, DispatcherType _type )
		{
			++s_max_timer_id;
			Timer timer = new Timer( s_max_timer_id, _type );
			timer.Interval = _interval;
			timer.AutoReset = _auto_reset;
			return timer;
		}

		// properties
		public UInt64 TimerId { get { return m_timer_id; } }
		// how many seconds the timer will be triggered in?
		public Int64 Interval
		{
			get { lock( m_lock ) { return m_interval; } }
			internal set { lock( m_lock ) { m_interval = value; } }
		}
		// the timer should reactivate or not after the time is up
		public bool AutoReset
		{
			get { lock( m_lock ) { return m_auto_reset; } }
			set { lock( m_lock ) { m_auto_reset = value; } }
		}
		// if disabled, the timer wouldn't raise up any event even if the trigger has issued.
		public bool Enabled
		{
			get { lock( m_lock ) { return m_enabled; } }
			set { lock( m_lock ) { m_enabled = value; } }
		}
		// whether the timer has been scheduled by timer system
		public bool InSchedule
		{
			get { lock( m_lock ) { return m_activated; } }
			set { lock( m_lock ) { m_activated = value; } }
		}

		public DispatcherType Type
		{
			get { return m_dispatcher_type; }
		}

		public static UInt64 MaxTimerId { get { return s_max_timer_id; } }

		private DispatcherType m_dispatcher_type;
		private UInt64 m_timer_id = 0;
		private Int64 m_interval = 0;
		private bool m_auto_reset = false;
		private bool m_enabled = true;
		private bool m_activated = false;
		private Object m_lock = new Object();
		private static UInt64 s_max_timer_id = 0;
	}
}
