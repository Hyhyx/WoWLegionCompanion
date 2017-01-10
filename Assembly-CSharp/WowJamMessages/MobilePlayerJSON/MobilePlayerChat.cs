﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages.MobilePlayerJSON
{
	[System.Runtime.Serialization.DataContract]
	[FlexJamMessage(Id = 4789, Name = "MobilePlayerChat", Version = 33577221u)]
	public class MobilePlayerChat
	{
		public MobilePlayerChat()
		{
			this.TargetName = string.Empty;
			this.ChatText = string.Empty;
		}

		[System.Runtime.Serialization.DataMember(Name = "slashCmd")]
		[FlexJamMember(Name = "slashCmd", Type = FlexJamType.UInt8)]
		public byte SlashCmd { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "chatText")]
		[FlexJamMember(Name = "chatText", Type = FlexJamType.String)]
		public string ChatText { get; set; }

		[FlexJamMember(Name = "targetName", Type = FlexJamType.String)]
		[System.Runtime.Serialization.DataMember(Name = "targetName")]
		public string TargetName { get; set; }
	}
}
