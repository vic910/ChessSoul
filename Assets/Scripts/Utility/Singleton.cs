using UnityEngine;

namespace Weiqi
{
	public class UnitySingleton<T> : MonoBehaviour
		where T : UnityEngine.MonoBehaviour
	{
		public static T Instance { get; private set; }
		void Awake()
		{
			if( Instance != null )
			{
				Utility.Log.Critical( "UnitySingleton: 多个实例存在，后一个被删除! Name: {0} GameObject: {1}", typeof( T ).Name, gameObject.name );
				this.enabled = false;
				Destroy( this );
				return;
			}
			Instance = this as T;
			OnAwake();
		}

		void Start() { OnStart(); }

		/// <summary>
		/// 销毁时销毁单例
		/// </summary>
		private void Destory() { Instance = null; OnDestory(); }

		protected virtual void OnAwake()
		{

		}

		protected virtual void OnStart()
		{

		}

		protected virtual void OnDestory()
		{
			Instance = null;
		}

		/*public virtual void Initialize()
		{
		}

		public virtual void Uninitialize()
		{
		}*/
	}


	public abstract class GrootSingleton<T> where T : class, new()
	{
		public static readonly T Instance = new T();

		public abstract void Initialize();

		public abstract void Uninitialize();
	}
}