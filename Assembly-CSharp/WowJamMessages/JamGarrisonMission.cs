﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages
{
	[System.Runtime.Serialization.DataContract]
	[FlexJamStruct(Name = "JamGarrisonMission", Version = 28333852u)]
	public class JamGarrisonMission
	{
		public JamGarrisonMission()
		{
			this.Flags = 0u;
		}

		[FlexJamMember(Name = "offerTime", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "offerTime")]
		public int OfferTime { get; set; }

		[FlexJamMember(Name = "successChance", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "successChance")]
		public int SuccessChance { get; set; }

		[FlexJamMember(Name = "travelDuration", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "travelDuration")]
		public int TravelDuration { get; set; }

		[FlexJamMember(Name = "missionRecID", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "missionRecID")]
		public int MissionRecID { get; set; }

		[FlexJamMember(Name = "missionState", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "missionState")]
		public int MissionState { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "flags")]
		[FlexJamMember(Name = "flags", Type = FlexJamType.UInt32)]
		public uint Flags { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "startTime")]
		[FlexJamMember(Name = "startTime", Type = FlexJamType.Int32)]
		public int StartTime { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "dbID")]
		[FlexJamMember(Name = "dbID", Type = FlexJamType.UInt64)]
		public ulong DbID { get; set; }

		[FlexJamMember(Name = "offerDuration", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "offerDuration")]
		public int OfferDuration { get; set; }

		[FlexJamMember(Name = "missionDuration", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "missionDuration")]
		public int MissionDuration { get; set; }
	}
}