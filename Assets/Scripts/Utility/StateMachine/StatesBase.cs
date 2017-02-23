using System;

using Utility;

namespace Groot
{
	public class State<T_State, T_Entity>
	{
		public State() { }

		public virtual void Enter() { }
		public virtual void Exit() { }
		public virtual void FixedUpdate( float _dt ) { }
		public virtual void Update( float _dt ) { }

		public T_State StateType { get; private set; }
		protected T_Entity Entity { get; private set; }
		public StateMachine<T_State, T_Entity> FiniteStateMachine { get; private set; }

		protected virtual void Initialze( T_State _type, T_Entity _entity, StateMachine<T_State, T_Entity> _fsm )
		{
			StateType = _type;
			Entity = _entity;
			FiniteStateMachine = _fsm;
		}
	}
}