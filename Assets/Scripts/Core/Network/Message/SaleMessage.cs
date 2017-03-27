using System;
using System.Collections.Generic;
using System.Text;
using Groot.Network;
using Utility;

namespace Groot.Network
{
	class CG_UserSaleToSystem : MessageBase
	{
		[MessageFiled( 0 )]
		public UInt64 PlayerID;

		[MessageFiled( 1 )]
		public UInt32 SaleItemCount;

		[MessageFiled( 2, 1 )]
		public List<PropItem> Items;

		public CG_UserSaleToSystem() : base( EMsgDirection.MSG_CG, EMsgType.TYPE_SALE, (ushort)ESaleMsgId.SALE_USERSALETOSYSTEM_CG )
		{
			
		}
	};
}