﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamBattlePayPurchase", Version = 28333852u)]
	[System.Runtime.Serialization.DataContract]
	public class JamBattlePayPurchase
	{
		[System.Runtime.Serialization.DataMember(Name = "purchaseID")]
		[FlexJamMember(Name = "purchaseID", Type = FlexJamType.UInt64)]
		public ulong PurchaseID { get; set; }

		[FlexJamMember(Name = "status", Type = FlexJamType.UInt32)]
		[System.Runtime.Serialization.DataMember(Name = "status")]
		public uint Status { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "resultCode")]
		[FlexJamMember(Name = "resultCode", Type = FlexJamType.UInt32)]
		public uint ResultCode { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "productID")]
		[FlexJamMember(Name = "productID", Type = FlexJamType.UInt32)]
		public uint ProductID { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "walletName")]
		[FlexJamMember(Name = "walletName", Type = FlexJamType.String)]
		public string WalletName { get; set; }
	}
}