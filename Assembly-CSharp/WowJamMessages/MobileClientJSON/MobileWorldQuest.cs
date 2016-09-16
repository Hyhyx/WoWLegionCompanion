﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages.MobileClientJSON
{
	[System.Runtime.Serialization.DataContract]
	[FlexJamStruct(Name = "MobileWorldQuest", Version = 28333852u)]
	public class MobileWorldQuest
	{
		[System.Runtime.Serialization.DataMember(Name = "item")]
		[FlexJamMember(ArrayDimensions = 1, Name = "item", Type = FlexJamType.Struct)]
		public MobileWorldQuestReward[] Item { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "endTime")]
		[FlexJamMember(Name = "endTime", Type = FlexJamType.Int32)]
		public int EndTime { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "worldMapAreaID")]
		[FlexJamMember(Name = "worldMapAreaID", Type = FlexJamType.Int32)]
		public int WorldMapAreaID { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "experience")]
		[FlexJamMember(Name = "experience", Type = FlexJamType.Int32)]
		public int Experience { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "startLocationMapID")]
		[FlexJamMember(Name = "startLocationMapID", Type = FlexJamType.Int32)]
		public int StartLocationMapID { get; set; }

		[FlexJamMember(Name = "questID", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "questID")]
		public int QuestID { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "questInfoID")]
		[FlexJamMember(Name = "questInfoID", Type = FlexJamType.Int32)]
		public int QuestInfoID { get; set; }

		[FlexJamMember(Name = "money", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "money")]
		public int Money { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "startTime")]
		[FlexJamMember(Name = "startTime", Type = FlexJamType.Int32)]
		public int StartTime { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "startLocationY")]
		[FlexJamMember(Name = "startLocationY", Type = FlexJamType.Int32)]
		public int StartLocationY { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "currency")]
		[FlexJamMember(ArrayDimensions = 1, Name = "currency", Type = FlexJamType.Struct)]
		public MobileWorldQuestReward[] Currency { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "startLocationX")]
		[FlexJamMember(Name = "startLocationX", Type = FlexJamType.Int32)]
		public int StartLocationX { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "faction")]
		[FlexJamMember(ArrayDimensions = 1, Name = "faction", Type = FlexJamType.Struct)]
		public MobileWorldQuestReward[] Faction { get; set; }

		[FlexJamMember(ArrayDimensions = 1, Name = "objective", Type = FlexJamType.Struct)]
		[System.Runtime.Serialization.DataMember(Name = "objective")]
		public MobileWorldQuestObjective[] Objective { get; set; }

		[FlexJamMember(Name = "questTitle", Type = FlexJamType.String)]
		[System.Runtime.Serialization.DataMember(Name = "questTitle")]
		public string QuestTitle { get; set; }
	}
}
