using System;
using System.Collections.Generic;
using System.Text;

using Utility;

namespace Groot
{
	public class StateMachine<T_Type, T_Entity>
	{
		public State<T_Type, T_Entity> CurrentState { get { return m_current_state; } }
		public State<T_Type, T_Entity> PrevState { get { return m_prev_state; } }
		public bool IsActive { get; private set; }

		private State<T_Type, T_Entity>  m_current_state;

		/// <summary>
		/// use to decide something
		/// </summary>
		private State<T_Type, T_Entity>  m_prev_state;

		//private T_Entity m_entity;


		/// <summary>
		/// all state available here
		/// </summary>
		private Dictionary<T_Type, State<T_Type, T_Entity> > m_states = new Dictionary<T_Type, State<T_Type, T_Entity>>();


		public void Initialize( T_Entity _entity, IEnumerable<State<T_Type, T_Entity>> _states )
		{
			//m_entity = _entity;
			foreach( State<T_Type, T_Entity>  state in _states )
			{
				m_states.Add( state.StateType, state );
			}
		}
		public void ChangeStateTo( T_Type _state )
		{
			Log.LogInfo( eLogLevel.Info, eLogType.Normal, "before change state: [{0}] to [{1}]", m_current_state == null ? "null" : m_current_state.StateType.ToString(), _state.ToString() );

			State<T_Type, T_Entity>  entry_state;
			if( !m_states.TryGetValue( _state, out entry_state ) )
				throw new Exception( String.Format( "StateMachine, state: '{0}' not exist!", _state.ToString() ) );

			if( m_current_state != null )
			{
				Log.LogInfo( eLogLevel.Info, eLogType.Normal, "before [{0}] exit!", m_current_state.StateType.ToString() );
				m_current_state.Exit();
			}

			m_prev_state = m_current_state;
			m_current_state = entry_state;

			Log.LogInfo( eLogLevel.Info, eLogType.Normal, "before [{0}] enter!", _state.ToString() );
			m_current_state.Enter();
		}

		public void Activate( T_Type _entry_state )
		{
			State<T_Type, T_Entity>  entry_state;
			if( !m_states.TryGetValue( _entry_state, out entry_state ) )
				throw new Exception( String.Format( "[StateMachine(Activate)] entry state: '{0}' not exist!", entry_state.ToString() ) );
			ChangeStateTo( _entry_state );
		}

		public void InActivate()
		{
			Log.LogInfo( eLogLevel.Info, eLogType.Normal, "[StateMachine] inactivate. current state: [{0}]", m_current_state == null ? "null" : m_current_state.StateType.ToString() );
			if( m_current_state != null )
			{
				Log.LogInfo( eLogLevel.Info, eLogType.Normal, "before [{0}] exit!", m_current_state.StateType.ToString() );
				m_current_state.Exit();
			}
			m_prev_state = m_current_state;
			m_current_state = null;
		}

		public void FixedUpdate( float _dt )
		{
			m_current_state.FixedUpdate( _dt );
		}
		public void Update( float _dt )
		{
			m_current_state.Update( _dt );
		}
	}
}
