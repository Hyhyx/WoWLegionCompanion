﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages
{
	[System.Runtime.Serialization.DataContract]
	[FlexJamStruct(Name = "JamBattlenetRequestHeader", Version = 28333852u)]
	public class JamBattlenetRequestHeader
	{
		public JamBattlenetRequestHeader()
		{
			this.ObjectID = 0UL;
		}

		[FlexJamMember(Name = "methodType", Type = FlexJamType.UInt64)]
		[System.Runtime.Serialization.DataMember(Name = "methodType")]
		public ulong MethodType { get; set; }

		[FlexJamMember(Name = "token", Type = FlexJamType.UInt32)]
		[System.Runtime.Serialization.DataMember(Name = "token")]
		public uint Token { get; set; }

		[FlexJamMember(Name = "objectID", Type = FlexJamType.UInt64)]
		[System.Runtime.Serialization.DataMember(Name = "objectID")]
		public ulong ObjectID { get; set; }
	}
}
