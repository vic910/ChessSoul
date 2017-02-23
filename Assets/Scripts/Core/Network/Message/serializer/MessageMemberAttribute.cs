using System;
using System.Text;
using System.Reflection;

namespace Groot.Network
{
	[AttributeUsage( AttributeTargets.Field, AllowMultiple = false, Inherited = true )]
	class MessageFiled : Attribute
	{
		public Int32 Index;
		public Int32 Length;
		public Int32 Length2;
		public MessageFiled( Int32 _index )
		{
			Index = _index;
		}

		public MessageFiled( Int32 _index, Int32 _length )
		{
			Index = _index;
			Length = _length;
		}

		public MessageFiled( Int32 _index, Int32 _length, Int32 _length2 )
		{
			Index = _index;
			Length = _length;
			Length2 = _length2;
		}
	}
	class MessageFieldInfo
	{
		public MessageFieldInfo( FieldInfo _info, MessageFiled _atribute )
		{
			Info = _info;
			Attribute = _atribute;
		}
		public FieldInfo Info;
		public MessageFiled Attribute;
		public Int32 NewOrder;
	}
}
