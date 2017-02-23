using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Utility.Factory
{
	class SimpleFactory<TKey, TBaseClass> where TBaseClass : class, new()
	{
		/// <summary>
		/// 实例创建基类
		/// </summary>
		private abstract class Creator
		{
			/// <summary>
			/// 创建实例
			/// </summary>
			/// <returns></returns>
			public abstract TBaseClass CreateInstance();
		}

		/// <summary>
		/// 实例创建派生类
		/// </summary>
		/// <typeparam name="TSubclass">要创建的类实例类型</typeparam>
		private class TCreator<TSubclass> : Creator where TSubclass : class, TBaseClass, new()
		{
			public override TBaseClass CreateInstance()
			{
				return new TSubclass();
				//return Activator.CreateInstance<TSubclass>();
			}
		}

		/// <summary>
		/// 注册类型创建函数
		/// </summary>
		/// <typeparam name="T">要创建的实例类型</typeparam>
		/// <param name="_type_id">要创建的实例类型所对应的编号</param>
		public void Register<T>( TKey _type_id ) where T : class, TBaseClass, new()
		{
			m_dictCreatableType[_type_id] = new TCreator<T>();
		}

		public void Unregister( TKey _type_id )
		{
			m_dictCreatableType.Remove( _type_id );
		}
		/// <summary>
		/// 根据指定类型编号创建对应的类型实例
		/// </summary>
		/// <param name="_type_id">类型编号</param>
		/// <returns>创建的实例</returns>
		public TBaseClass CreateInstance( TKey _type_id )
		{
			Creator creator;
			if( !m_dictCreatableType.TryGetValue( _type_id, out creator ) )
				return default( TBaseClass );
			return creator.CreateInstance();
		}
		/// <summary>
		/// 是否可以创建
		/// </summary>
		/// <param name="_type_id"></param>
		/// <returns></returns>
		public bool CanCreate( TKey _type_id )
		{
			return m_dictCreatableType.ContainsKey( _type_id );
		}

		private Dictionary<TKey, Creator> m_dictCreatableType = new Dictionary<TKey, Creator>();
	}
	class GameObjectFactory<TKey, TBaseClass> where TBaseClass : MonoBehaviour
	{
		/// <summary>
		/// 实例创建基类
		/// </summary>
		private abstract class Creator
		{
			/// <summary>
			/// 创建实例
			/// </summary>
			/// <returns></returns>
			public abstract TBaseClass CreateInstance( GameObject _object );
		}

		/// <summary>
		/// 实例创建派生类
		/// </summary>
		/// <typeparam name="TSubclass">要创建的类实例类型</typeparam>
		private class TCreator<TSubclass> : Creator where TSubclass : MonoBehaviour, TBaseClass
		{
			public override TBaseClass CreateInstance( GameObject _object )
			{
				return _object.AddComponent<TSubclass>();
			}
		}

		/// <summary>
		/// 注册类型创建函数
		/// </summary>
		/// <typeparam name="T">要创建的实例类型</typeparam>
		/// <param name="_type_id">要创建的实例类型所对应的编号</param>
		public void Register<T>( TKey _type_id ) where T : MonoBehaviour, TBaseClass
		{
			m_dictCreatableType[_type_id] = new TCreator<T>();
		}
		/// <summary>
		/// 根据指定类型编号创建对应的类型实例
		/// </summary>
		/// <param name="_type_id">类型编号</param>
		/// <returns>创建的实例</returns>
		public TBaseClass CreateInstance( TKey _type_id, GameObject _object )
		{
			Creator creator;
			if( !m_dictCreatableType.TryGetValue( _type_id, out creator ) )
				return default( TBaseClass );
			return creator.CreateInstance( _object );
		}
		private Dictionary<TKey, Creator> m_dictCreatableType = new Dictionary<TKey, Creator>();
	}
}
