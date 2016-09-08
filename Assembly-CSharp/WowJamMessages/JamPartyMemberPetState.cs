﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamPartyMemberPetState", Version = 28333852u)]
	[System.Runtime.Serialization.DataContract]
	public class JamPartyMemberPetState
	{
		public JamPartyMemberPetState()
		{
			this.Guid = "0000000000000000";
		}

		[System.Runtime.Serialization.DataMember(Name = "guid")]
		[FlexJamMember(Name = "guid", Type = FlexJamType.WowGuid)]
		public string Guid { get; set; }

		[FlexJamMember(Name = "health", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "health")]
		public int Health { get; set; }

		[FlexJamMember(Name = "maxHealth", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "maxHealth")]
		public int MaxHealth { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "name")]
		[FlexJamMember(Name = "name", Type = FlexJamType.String)]
		public string Name { get; set; }

		[FlexJamMember(ArrayDimensions = 1, Name = "auras", Type = FlexJamType.Struct)]
		[System.Runtime.Serialization.DataMember(Name = "auras")]
		public JamPartyMemberAuraState[] Auras { get; set; }

		[FlexJamMember(Name = "displayID", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "displayID")]
		public int DisplayID { get; set; }
	}
}