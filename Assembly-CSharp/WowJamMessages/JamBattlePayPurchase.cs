﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamBattlePayPurchase", Version = 28333852u)]
	[System.Runtime.Serialization.DataContract]
	public class JamBattlePayPurchase
	{
		[FlexJamMember(Name = "purchaseID", Type = FlexJamType.UInt64)]
		[System.Runtime.Serialization.DataMember(Name = "purchaseID")]
		public ulong PurchaseID { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "status")]
		[FlexJamMember(Name = "status", Type = FlexJamType.UInt32)]
		public uint Status { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "resultCode")]
		[FlexJamMember(Name = "resultCode", Type = FlexJamType.UInt32)]
		public uint ResultCode { get; set; }

		[FlexJamMember(Name = "productID", Type = FlexJamType.UInt32)]
		[System.Runtime.Serialization.DataMember(Name = "productID")]
		public uint ProductID { get; set; }

		[FlexJamMember(Name = "walletName", Type = FlexJamType.String)]
		[System.Runtime.Serialization.DataMember(Name = "walletName")]
		public string WalletName { get; set; }
	}
}
