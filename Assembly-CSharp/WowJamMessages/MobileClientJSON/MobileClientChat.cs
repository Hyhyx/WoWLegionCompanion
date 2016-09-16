﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages.MobileClientJSON
{
	[FlexJamMessage(Id = 4844, Name = "MobileClientChat", Version = 28333852u)]
	[System.Runtime.Serialization.DataContract]
	public class MobileClientChat
	{
		public MobileClientChat()
		{
			this.SenderGUID = "0000000000000000";
			this.SenderName = string.Empty;
			this.Prefix = string.Empty;
			this.Channel = string.Empty;
			this.ChatText = string.Empty;
			this.ChatFlags = 0;
		}

		[FlexJamMember(Name = "senderName", Type = FlexJamType.String)]
		[System.Runtime.Serialization.DataMember(Name = "senderName")]
		public string SenderName { get; set; }

		[FlexJamMember(Name = "senderGUID", Type = FlexJamType.WowGuid)]
		[System.Runtime.Serialization.DataMember(Name = "senderGUID")]
		public string SenderGUID { get; set; }

		[FlexJamMember(Name = "chatText", Type = FlexJamType.String)]
		[System.Runtime.Serialization.DataMember(Name = "chatText")]
		public string ChatText { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "prefix")]
		[FlexJamMember(Name = "prefix", Type = FlexJamType.String)]
		public string Prefix { get; set; }

		[FlexJamMember(Name = "channel", Type = FlexJamType.String)]
		[System.Runtime.Serialization.DataMember(Name = "channel")]
		public string Channel { get; set; }

		[FlexJamMember(Name = "slashCmd", Type = FlexJamType.UInt8)]
		[System.Runtime.Serialization.DataMember(Name = "slashCmd")]
		public byte SlashCmd { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "chatFlags")]
		[FlexJamMember(Name = "chatFlags", Type = FlexJamType.UInt16)]
		public ushort ChatFlags { get; set; }
	}
}
