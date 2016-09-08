﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages.MobilePlayerJSON
{
	[FlexJamMessage(Id = 4809, Name = "MobilePlayerUtilityMsgForBI", Version = 28333852u)]
	[System.Runtime.Serialization.DataContract]
	public class MobilePlayerUtilityMsgForBI
	{
		[System.Runtime.Serialization.DataMember(Name = "msgType")]
		[FlexJamMember(Name = "msgType", Type = FlexJamType.Int32)]
		public int MsgType { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "data4")]
		[FlexJamMember(Name = "data4", Type = FlexJamType.Int32)]
		public int Data4 { get; set; }

		[FlexJamMember(Name = "data3", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "data3")]
		public int Data3 { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "data2")]
		[FlexJamMember(Name = "data2", Type = FlexJamType.Int32)]
		public int Data2 { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "data1")]
		[FlexJamMember(Name = "data1", Type = FlexJamType.Int32)]
		public int Data1 { get; set; }
	}
}