﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4867, Name = "MobileClientWorldQuestInactiveBountiesResult", Version = 33577221u)]
	[System.Runtime.Serialization.DataContract]
	public class MobileClientWorldQuestInactiveBountiesResult
	{
		[FlexJamMember(ArrayDimensions = 1, Name = "bounty", Type = FlexJamType.Struct)]
		[System.Runtime.Serialization.DataMember(Name = "bounty")]
		public MobileWorldQuestBounty[] Bounty { get; set; }
	}
}