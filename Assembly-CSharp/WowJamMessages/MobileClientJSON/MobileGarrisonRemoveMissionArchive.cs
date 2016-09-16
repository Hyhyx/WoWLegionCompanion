﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4839, Name = "MobileGarrisonRemoveMissionArchive", Version = 28333852u)]
	[System.Runtime.Serialization.DataContract]
	public class MobileGarrisonRemoveMissionArchive
	{
		[System.Runtime.Serialization.DataMember(Name = "result")]
		[FlexJamMember(Name = "result", Type = FlexJamType.Int32)]
		public int Result { get; set; }

		[FlexJamMember(Name = "garrTypeID", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "garrTypeID")]
		public int GarrTypeID { get; set; }

		[FlexJamMember(Name = "missionID", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "missionID")]
		public int MissionID { get; set; }
	}
}
