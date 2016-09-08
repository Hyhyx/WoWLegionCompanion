﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages.JSONRealmList
{
	[System.Runtime.Serialization.DataContract]
	[FlexJamStruct(Name = "JamJSONRealmListTicketClientInformation", Version = 28333852u)]
	public class JamJSONRealmListTicketClientInformation
	{
		public JamJSONRealmListTicketClientInformation()
		{
			this.Secret = new byte[32];
		}

		[System.Runtime.Serialization.DataMember(Name = "platform")]
		[FlexJamMember(Name = "platform", Type = FlexJamType.UInt32)]
		public uint Platform { get; set; }

		[FlexJamMember(Name = "currentTime", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "currentTime")]
		public int CurrentTime { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "buildVariant")]
		[FlexJamMember(Name = "buildVariant", Type = FlexJamType.String)]
		public string BuildVariant { get; set; }

		[FlexJamMember(Name = "timeZone", Type = FlexJamType.String)]
		[System.Runtime.Serialization.DataMember(Name = "timeZone")]
		public string TimeZone { get; set; }

		[FlexJamMember(Name = "versionDataBuild", Type = FlexJamType.UInt32)]
		[System.Runtime.Serialization.DataMember(Name = "versionDataBuild")]
		public uint VersionDataBuild { get; set; }

		[FlexJamMember(Name = "audioLocale", Type = FlexJamType.UInt32)]
		[System.Runtime.Serialization.DataMember(Name = "audioLocale")]
		public uint AudioLocale { get; set; }

		[FlexJamMember(Name = "version", Type = FlexJamType.Struct)]
		[System.Runtime.Serialization.DataMember(Name = "version")]
		public JamJSONGameVersion Version { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "secret")]
		[FlexJamMember(ArrayDimensions = 1, Name = "secret", Type = FlexJamType.UInt8)]
		public byte[] Secret { get; set; }

		[FlexJamMember(Name = "type", Type = FlexJamType.UInt32)]
		[System.Runtime.Serialization.DataMember(Name = "type")]
		public uint Type { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "textLocale")]
		[FlexJamMember(Name = "textLocale", Type = FlexJamType.UInt32)]
		public uint TextLocale { get; set; }
	}
}