using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Utility
{
	/// <summary>
	/// A system for dispatching code to execute on the main thread.
	/// </summary>
	public class Dispatcher : MonoBehaviour
	{
		private static Dispatcher s_instance;

		// We can't use the behaviour reference from other threads, so we use a separate bool
		// to track the instance so we can use that on the other threads.
		private static bool s_instance_exists;

		private static Thread s_main_thread;
		private static object s_lock_object = new object();
		private static readonly Queue<Action> s_actions = new Queue<Action>();

		/// <summary>
		/// Gets a value indicating whether or not the current thread is the game's main thread.
		/// </summary>
		public static bool IsMainThread
		{
			get
			{
				return Thread.CurrentThread == s_main_thread;
			}
		}

		/// <summary>
		/// Queues an action to be invoked on the main game thread.
		/// </summary>
		/// <param name="action">The action to be queued.</param>
		public static void InvokeAsync( Action action )
		{
			if( !s_instance_exists )
			{
				Debug.LogError( "No Dispatcher exists in the scene. Actions will not be invoked!" );
				return;
			}

			if( IsMainThread )
			{
				// Don't bother queuing work on the main thread; just execute it.
				action();
			}
			else
			{
				lock( s_lock_object )
				{
					s_actions.Enqueue( action );
				}
			}
		}

		/// <summary>
		/// Queues an action to be invoked on the main game thread and blocks the
		/// current thread until the action has been executed.
		/// </summary>
		/// <param name="action">The action to be queued.</param>
		public static void Invoke( Action action )
		{
			if( !s_instance_exists )
			{
				Debug.LogError( "No Dispatcher exists in the scene. Actions will not be invoked!" );
				return;
			}

			bool hasRun = false;

			InvokeAsync( () => {
				action();
				hasRun = true;
			} );

			// Lock until the action has run
			while( !hasRun )
			{
				Thread.Sleep( 5 );
			}
		}

		void Awake()
		{
			if( s_instance )
			{
				DestroyImmediate( this );
			}
			else
			{
				s_instance = this;
				s_instance_exists = true;
				s_main_thread = Thread.CurrentThread;
			}
		}

		void OnDestroy()
		{
			if( s_instance == this )
			{
				s_instance = null;
				s_instance_exists = false;
			}
		}

		void Update()
		{
			lock( s_lock_object )
			{
				while( s_actions.Count > 0 )
				{
					s_actions.Dequeue()();
				}
			}
		}
	}
}