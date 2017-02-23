using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

public class GVersion : ICloneable, IComparable<GVersion>, IEquatable<GVersion>
{
	/// <summary>
	/// 功能主版本号
	/// </summary>
	public Int32 Major { get { return m_major; } }
	private Int32 m_major = -1;

	/// <summary>
	/// 功能次版本号
	/// </summary>
	public Int32 Minor { get { return m_minor; } }
	private Int32 m_minor = -1;

	/// <summary>
	/// 修订版本每次正式上线的修正更新该版本号
	/// </summary>
	public Int32 Revision { get { return m_revision; } }
	private Int32 m_revision = -1;

	/// <summary>
	/// 资源版本号或者是程序的build版本
	/// </summary>
	public Int32 Build { get { return m_build; } }
	private Int32 m_build = -1;



	/// <summary>
	/// Bundle版本号字符串
	/// </summary>
	private string m_bundle_version_str = String.Empty;

	public string BundleVersionStr { get { return m_bundle_version_str; } }

	/// <summary>
	/// 版本号字符串
	/// </summary>
	private string m_version_str = string.Empty;
	public string VersionStr { get { return m_version_str; } }

	public GVersion( String _version_str )
	{
		var versions = _version_str.Split( '.' );
		if( versions.Length != 3 && versions.Length != 4 )
			goto Error;

		if( !Int32.TryParse( versions[0], out m_major ) )
			goto Error;

		if( !Int32.TryParse( versions[1], out m_minor ) )
			goto Error;

		if( !Int32.TryParse( versions[2], out m_revision ) )
			goto Error;

		if( versions.Length == 4 && !Int32.TryParse( versions[3], out m_build ) )
			goto Error;

		_rebuild_version_str();
		return;

		Error:
		throw new VersionException( _version_str );
	}

	public GVersion( Int32 _major, Int32 _minor, Int32 _revision )
	{
		m_major = _major;
		m_minor = _minor;
		m_revision = _revision;
		_rebuild_version_str();
	}

	public GVersion( Int32 _major, Int32 _minor, Int32 _revision, Int32 _build )
	{
		m_major = _major;
		m_minor = _minor;
		m_revision = _revision;
		m_build = _build;
		_rebuild_version_str();
	}

	public void IncreaseMajor()
	{
		if( m_major == Int32.MaxValue )
			throw new System.OperationCanceledException( "GVersion, major overflow!" );
		m_major++;
		_rebuild_version_str();
	}
	public void IncreaseMinor()
	{
		if( m_minor == Int32.MaxValue )
			throw new System.OperationCanceledException( "GVersion, minor overflow!" );
		m_minor++;
		_rebuild_version_str();
	}
	public void IncreaseRevision()
	{
		if( m_revision == Int32.MaxValue )
			throw new System.OperationCanceledException( "GVersion, revision overflow!" );
		m_revision++;
		_rebuild_version_str();
	}

	public void IncreaseBuild()
	{
		if( m_build == Int32.MaxValue )
			throw new System.OperationCanceledException( "GVersion, build overflow!" );
		m_build++;
		_rebuild_version_str();
	}

	private void _rebuild_version_str()
	{
		m_version_str = string.Format( "{0}.{1}.{2}.{3}", m_major, m_minor, m_revision, m_build );
		m_bundle_version_str = String.Format( "{0}.{1}.{2}", m_major, m_minor, m_revision );
	}

	public int CompareTo( GVersion _v2 )
	{
		if( _v2 == null )
			return 1;

		if( this.m_major != _v2.m_major )
		{
			if( this.m_major > _v2.m_major )
				return 1;
			return -1;
		}
		if( this.m_minor != _v2.m_minor )
		{
			if( this.m_minor > _v2.m_minor )
				return 1;
			return -1;
		}
		if( this.m_revision != _v2.m_revision )
		{
			if( this.m_revision > _v2.m_revision )
				return 1;
			return -1;
		}

		if( this.m_build == _v2.m_build )
			return 0;
		if( this.m_build > _v2.m_build )
			return 1;
		return -1;
	}
	public bool Equals( GVersion _v2 )
	{
		if( _v2 == null )
			return false;
		return ( this.m_major == _v2.m_major ) && ( this.m_minor == _v2.m_minor ) && ( this.m_revision == _v2.m_revision ) && ( this.m_build == _v2.m_build );
	}

	public override bool Equals( object _v2 )
	{
		GVersion v2 = _v2 as GVersion;
		if( _v2 == null )
			return false;
		return ( this.m_major == v2.m_major ) && ( this.m_minor == v2.m_minor ) && ( this.m_revision == v2.m_revision ) && ( this.m_build == v2.m_build );
	}

	public override int GetHashCode()
	{
		// just copy from System.Version. 
		// we don't use GVersion to HashTable key.
		// so this function seems no use -^-~
		throw new System.NotSupportedException( "'GVersion' not designer to support use as hash table key!" );

		/*int num = 0;
		num |= ( this.m_major & 15 ) << 0x1c;
		num |= ( this.m_minor & 0xff ) << 20;
		num |= ( this.m_revision & 0xff ) << 12;
		return ( num | ( this.m_revision & 0xfff ) );*/
		//return Int32.MaxValue;
	}

	public static bool operator ==( GVersion v1, GVersion v2 )
	{
		if( ReferenceEquals( v1, null ) )
		{
			return ReferenceEquals( v2, null );
		}
		return v1.Equals( v2 );
	}

	public static bool operator >( GVersion v1, GVersion v2 )
	{
		return ( v2 < v1 );
	}

	public static bool operator >=( GVersion v1, GVersion v2 )
	{
		return ( v2 <= v1 );
	}

	public static bool operator !=( GVersion v1, GVersion v2 )
	{
		return !( v1 == v2 );
	}

	public static bool operator <( GVersion v1, GVersion v2 )
	{
		if( v1 == null )
		{
			throw new ArgumentNullException( "v1" );
		}
		return ( v1.CompareTo( v2 ) < 0 );
	}

	public static bool operator <=( GVersion v1, GVersion v2 )
	{
		if( v1 == null )
		{
			throw new ArgumentNullException( "v1" );
		}
		return ( v1.CompareTo( v2 ) <= 0 );
	}

	public object Clone()
	{
		return new GVersion( m_major, m_minor, m_revision, m_build );
	}

	public override string ToString()
	{
		return VersionStr;
	}

}