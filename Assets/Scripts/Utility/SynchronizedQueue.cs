using System;
using System.Collections;
using System.Collections.Generic;

namespace Utility
{
	class SynchronizedQueue<T>
	{
		public SynchronizedQueue()
		{
			m_queue = new Queue<T>();
		}
		public SynchronizedQueue( IEnumerable<T> _collection )
		{
			m_queue = new Queue<T>( _collection );
		}
		public SynchronizedQueue( Int32 _count )
		{
			m_queue = new Queue<T>( _count );
		}

		public Int32 Count { get { lock( m_lock ) { return m_queue.Count; } } }
		public void Clear()
		{
			lock( m_lock )
			{
				m_queue.Clear();
			}
		}
		public bool Contains( T _item )
		{
			lock( m_lock )
			{
				return m_queue.Contains( _item );
			}
		}
		public void CopyTo( T[] _array, int _index )
		{
			lock( m_lock )
			{
				m_queue.CopyTo( _array, _index );
			}
		}
		public bool Dequeue( out T _value )
		{
			_value = default( T );
			lock( m_lock )
			{
				if( 0 == m_queue.Count )
					return false;
				_value = m_queue.Dequeue();
				return true;
			}
		}
		public T Dequeue()
		{
			lock( m_lock )
			{
				return m_queue.Dequeue();
			}
		}
		public void Enqueue( T _item )
		{
			lock( m_lock )
			{
				m_queue.Enqueue( _item );
			}
		}

		private Object m_lock = new Object();
		private Queue<T> m_queue = null;
	}
}
