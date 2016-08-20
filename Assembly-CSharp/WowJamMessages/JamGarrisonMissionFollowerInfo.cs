﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages
{
	[System.Runtime.Serialization.DataContract]
	[FlexJamStruct(Name = "JamGarrisonMissionFollowerInfo", Version = 28333852u)]
	public class JamGarrisonMissionFollowerInfo
	{
		[FlexJamMember(Name = "followerDBID", Type = FlexJamType.UInt64)]
		[System.Runtime.Serialization.DataMember(Name = "followerDBID")]
		public ulong FollowerDBID { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "missionCompleteState")]
		[FlexJamMember(Name = "missionCompleteState", Type = FlexJamType.UInt32)]
		public uint MissionCompleteState { get; set; }
	}
}
