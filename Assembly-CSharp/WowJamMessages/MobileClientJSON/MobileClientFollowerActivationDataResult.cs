﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages.MobileClientJSON
{
	[System.Runtime.Serialization.DataContract]
	[FlexJamMessage(Id = 4865, Name = "MobileClientFollowerActivationDataResult", Version = 28333852u)]
	public class MobileClientFollowerActivationDataResult
	{
		[FlexJamMember(Name = "goldCost", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "goldCost")]
		public int GoldCost { get; set; }

		[FlexJamMember(Name = "activationsRemaining", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "activationsRemaining")]
		public int ActivationsRemaining { get; set; }
	}
}