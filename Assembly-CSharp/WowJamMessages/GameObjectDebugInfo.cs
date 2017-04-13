﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages
{
	[System.Runtime.Serialization.DataContract]
	[FlexJamStruct(Name = "GameObjectDebugInfo", Version = 28333852u)]
	public class GameObjectDebugInfo
	{
		[FlexJamMember(Name = "health", Type = FlexJamType.Float)]
		[System.Runtime.Serialization.DataMember(Name = "health")]
		public float Health { get; set; }

		[FlexJamMember(Name = "state", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "state")]
		public int State { get; set; }

		[FlexJamMember(Name = "flags", Type = FlexJamType.UInt32)]
		[System.Runtime.Serialization.DataMember(Name = "flags")]
		public uint Flags { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "gameObjectType")]
		[FlexJamMember(Name = "gameObjectType", Type = FlexJamType.Int32)]
		public int GameObjectType { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "debugName")]
		[FlexJamMember(Name = "debugName", Type = FlexJamType.String)]
		public string DebugName { get; set; }
	}
}
