﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages
{
	[System.Runtime.Serialization.DataContract]
	[FlexJamStruct(Name = "JamGarrisonMobileMission", Version = 28333852u)]
	public class JamGarrisonMobileMission
	{
		[System.Runtime.Serialization.DataMember(Name = "offerTime")]
		[FlexJamMember(Name = "offerTime", Type = FlexJamType.Int64)]
		public long OfferTime { get; set; }

		[FlexJamMember(Name = "travelDuration", Type = FlexJamType.Int64)]
		[System.Runtime.Serialization.DataMember(Name = "travelDuration")]
		public long TravelDuration { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "missionRecID")]
		[FlexJamMember(Name = "missionRecID", Type = FlexJamType.Int32)]
		public int MissionRecID { get; set; }

		[FlexJamMember(Name = "missionState", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "missionState")]
		public int MissionState { get; set; }

		[FlexJamMember(ArrayDimensions = 1, Name = "encounter", Type = FlexJamType.Struct)]
		[System.Runtime.Serialization.DataMember(Name = "encounter")]
		public JamGarrisonEncounter[] Encounter { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "reward")]
		[FlexJamMember(ArrayDimensions = 1, Name = "reward", Type = FlexJamType.Struct)]
		public JamGarrisonMissionReward[] Reward { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "startTime")]
		[FlexJamMember(Name = "startTime", Type = FlexJamType.Int64)]
		public long StartTime { get; set; }

		[FlexJamMember(Name = "dbID", Type = FlexJamType.UInt64)]
		[System.Runtime.Serialization.DataMember(Name = "dbID")]
		public ulong DbID { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "offerDuration")]
		[FlexJamMember(Name = "offerDuration", Type = FlexJamType.Int64)]
		public long OfferDuration { get; set; }

		[FlexJamMember(ArrayDimensions = 1, Name = "overmaxReward", Type = FlexJamType.Struct)]
		[System.Runtime.Serialization.DataMember(Name = "overmaxReward")]
		public JamGarrisonMissionReward[] OvermaxReward { get; set; }

		[FlexJamMember(Name = "missionDuration", Type = FlexJamType.Int64)]
		[System.Runtime.Serialization.DataMember(Name = "missionDuration")]
		public long MissionDuration { get; set; }
	}
}