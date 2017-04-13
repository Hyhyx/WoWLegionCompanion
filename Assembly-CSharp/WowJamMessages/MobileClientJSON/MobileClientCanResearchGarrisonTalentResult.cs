﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4870, Name = "MobileClientCanResearchGarrisonTalentResult", Version = 39869590u)]
	[System.Runtime.Serialization.DataContract]
	public class MobileClientCanResearchGarrisonTalentResult
	{
		[FlexJamMember(Name = "conditionText", Type = FlexJamType.String)]
		[System.Runtime.Serialization.DataMember(Name = "conditionText")]
		public string ConditionText { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "result")]
		[FlexJamMember(Name = "result", Type = FlexJamType.Int32)]
		public int Result { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "garrTalentID")]
		[FlexJamMember(Name = "garrTalentID", Type = FlexJamType.Int32)]
		public int GarrTalentID { get; set; }
	}
}
