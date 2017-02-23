using System;
using UnityEngine;
using System.Collections.Generic;
namespace Utility
{
	class Helper
	{
		/// <summary>
		/// 加速移动到目标
		/// </summary>
		/// <param name="transform">物体</param>
		/// <param name="target">目标点</param>
		/// <param name="time">当前时间</param>
		/// <param name="acceleration">加速度</param>
		/// <returns>是否到达目标</returns>
		public static bool AccelerateMoveToTarget( Transform transform, Vector3 target_position, float time, float acceleration )
		{
			Vector3 positon = ( target_position - transform.position ).normalized * time * time * acceleration / 2;
			//距离判断
			float distance = Vector3.Distance( transform.position, target_position );
			float change_distance = positon.magnitude;
			if( change_distance < distance )
			{
				transform.position += positon;
			}
			else
			{
				transform.position = target_position;
				return true;
			}
			return false;
		}
		/// <summary>
		/// 创建Sprite
		/// </summary>
		/// <param name="_name">图片名</param>
		/// <param name="_pack_name">图集路径</param>
		/// <returns></returns>
		public static Sprite CreateSprite( string _name, string _pack_url )
		{
			Sprite[] sprites = Resources.LoadAll<Sprite>( _pack_url );
			for( int i = 0; i < sprites.Length; i++ )
			{
				if( 0 == _name.CompareTo( sprites[i].name ) )
				{
					return sprites[i];
				}
			}
			return null;
		}
		/// <summary>
		/// 合并两个Byte
		/// </summary>
		/// <returns></returns>
		public static Int16 GenerateInt16( Byte _one, Byte _two )
		{
			Int16 id = (Int16)( _one << 8 );
			id += _two;
			return id;
		}

		public static Int32 GenerateInt32( Byte _type, Byte _dir, UInt16 _id )
		{
			Int32 id = (Int32) ( _type << 24 ) + (Int32)( _dir << 16 ) + _id;
			return id;
		}

		/// <summary>
		/// 重置list大小
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public static List<T> ResizeList<T>( List<T> _old_list, int _length )
		{
			if( _old_list.Count == _length )
			{
				return _old_list;
			}
			List < T > new_list = new List<T>( _length );
			for( int i = 0; i < _length; i++ )
			{
				if( _old_list.Count > i )
				{
					new_list.Add( _old_list[i] );
				}
				else
				{
					new_list.Add( default( T ) );
				}
			}
			return new_list;
		}
		/// <summary>
		/// 计算夹角的角度 0~360
		/// </summary>
		/// <param name="_from"></param>
		/// <param name="_to"></param>
		/// <returns></returns>
		public static float Angle360( Vector3 _from, Vector3 _to )
		{
			Vector3 v3 = Vector3.Cross( _from, _to );
			if( v3.z > 0 )
				return Vector3.Angle( _from, _to );
			else
				return 360 - Vector3.Angle( _from, _to );
		}
		/// <summary>
		/// 把角度转化成0~360角度
		/// </summary>
		/// <param name="_angle"></param>
		/// <returns></returns>
		public static float AngleTo360( float _angle )
		{
			int lengh = Mathf.FloorToInt( _angle / 360f );
			float angle = _angle - lengh * 360f;
			return angle;
		}
		/// <summary>
		/// 把向量转化成0~360角度
		/// </summary>
		/// <param name="_vector"></param>
		/// <returns></returns>
		public static float VectorTo360( Vector3 _vector)
		{
			float angle = Mathf.Atan2( _vector.y, _vector.x ) * Mathf.Rad2Deg;
			angle = Helper.AngleTo360( angle );
			return angle;
		}
		/// <summary>
		/// 向量与X轴的夹角的sin
		/// </summary>
		/// <param name="_angle"></param>
		/// <returns></returns>
		public static float VectorXSin( Vector3 _vector )
		{
			float angle = Vector3.Angle( _vector, Vector3.right );
			angle = Mathf.Sin( angle * Mathf.Deg2Rad );
			return angle;
		}
	}
}
