﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamGarrisonMissionSet", Version = 28333852u)]
	[System.Runtime.Serialization.DataContract]
	public class JamGarrisonMissionSet
	{
		[System.Runtime.Serialization.DataMember(Name = "garrMissionSetID")]
		[FlexJamMember(Name = "garrMissionSetID", Type = FlexJamType.Int32)]
		public int GarrMissionSetID { get; set; }

		[FlexJamMember(Name = "lastUpdateTime", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "lastUpdateTime")]
		public int LastUpdateTime { get; set; }
	}
}
