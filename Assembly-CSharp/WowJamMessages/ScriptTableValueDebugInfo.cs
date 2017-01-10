﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "ScriptTableValueDebugInfo", Version = 28333852u)]
	[System.Runtime.Serialization.DataContract]
	public class ScriptTableValueDebugInfo
	{
		[FlexJamMember(Name = "keyName", Type = FlexJamType.String)]
		[System.Runtime.Serialization.DataMember(Name = "keyName")]
		public string KeyName { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "valueName")]
		[FlexJamMember(Name = "valueName", Type = FlexJamType.String)]
		public string ValueName { get; set; }
	}
}
