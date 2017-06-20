﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamStruct(Name = "MobileEmissaryFaction", Version = 39869590u)]
	[System.Runtime.Serialization.DataContract]
	public class MobileEmissaryFaction
	{
		[FlexJamMember(Name = "factionID", Type = FlexJamType.UInt16)]
		[System.Runtime.Serialization.DataMember(Name = "factionID")]
		public ushort FactionID { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "factionAmount")]
		[FlexJamMember(Name = "factionAmount", Type = FlexJamType.Int32)]
		public int FactionAmount { get; set; }
	}
}
