﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages
{
	[System.Runtime.Serialization.DataContract]
	[FlexJamStruct(Name = "JamGarrisonTalent", Version = 28333852u)]
	public class JamGarrisonTalent
	{
		[System.Runtime.Serialization.DataMember(Name = "flags")]
		[FlexJamMember(Name = "flags", Type = FlexJamType.Int32)]
		public int Flags { get; set; }

		[FlexJamMember(Name = "garrTalentID", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "garrTalentID")]
		public int GarrTalentID { get; set; }

		[FlexJamMember(Name = "startTime", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "startTime")]
		public int StartTime { get; set; }
	}
}
