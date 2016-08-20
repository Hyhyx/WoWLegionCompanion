﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamAuctionableTokenInfo", Version = 28333852u)]
	[System.Runtime.Serialization.DataContract]
	public class JamAuctionableTokenInfo
	{
		[FlexJamMember(Name = "price", Type = FlexJamType.UInt64)]
		[System.Runtime.Serialization.DataMember(Name = "price")]
		public ulong Price { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "status")]
		[FlexJamMember(Name = "status", Type = FlexJamType.Int32)]
		public int Status { get; set; }

		[FlexJamMember(Name = "expectedSecondsUntilSold", Type = FlexJamType.UInt32)]
		[System.Runtime.Serialization.DataMember(Name = "expectedSecondsUntilSold")]
		public uint ExpectedSecondsUntilSold { get; set; }

		[FlexJamMember(Name = "id", Type = FlexJamType.UInt64)]
		[System.Runtime.Serialization.DataMember(Name = "id")]
		public ulong Id { get; set; }

		[FlexJamMember(Name = "lastUpdate", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "lastUpdate")]
		public int LastUpdate { get; set; }
	}
}
