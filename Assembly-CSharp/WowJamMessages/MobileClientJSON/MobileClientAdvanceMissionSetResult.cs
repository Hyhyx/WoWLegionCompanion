﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4843, Name = "MobileClientAdvanceMissionSetResult", Version = 33577221u)]
	[System.Runtime.Serialization.DataContract]
	public class MobileClientAdvanceMissionSetResult
	{
		[FlexJamMember(Name = "missionSetID", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "missionSetID")]
		public int MissionSetID { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "success")]
		[FlexJamMember(Name = "success", Type = FlexJamType.Bool)]
		public bool Success { get; set; }
	}
}
