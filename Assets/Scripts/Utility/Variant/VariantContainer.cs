using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
namespace Utility.Variant
{
	class VariantContainer
	{
		private readonly Dictionary<String, Variant> m_variants = new Dictionary<string, Variant>();

		public void Add( String _key, Variant _value )
		{
			m_variants.Add( _key, _value );
		}

		public Boolean Contain( String _key )
		{
			return m_variants.ContainsKey( _key );
		}

		public Boolean TryGetValue( String _key, out Variant _value )
		{
			return m_variants.TryGetValue( _key, out _value );
		}

		public Boolean TryGetValue<T>( String _key, out T _value )
		{
			Variant v;
			if( !TryGetValue( _key, out v ) )
			{
				_value = default( T );
				return false;
			}
			_value = v.GetValue<T>();
			return true;
		}

		public Variant GetValue( String _key )
		{
			return m_variants[_key];
		}

		public T GetValue<T>( String _key )
		{
			return m_variants[_key].GetValue<T>();
		}

		public void Clear()
		{
			m_variants.Clear();
		}
	}
}
