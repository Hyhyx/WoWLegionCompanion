﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages.JSONRealmList
{
	[FlexJamStruct(Name = "JamJSONRealmListTicketClientInformation", Version = 28333852u)]
	[System.Runtime.Serialization.DataContract]
	public class JamJSONRealmListTicketClientInformation
	{
		public JamJSONRealmListTicketClientInformation()
		{
			this.Secret = new byte[32];
		}

		[System.Runtime.Serialization.DataMember(Name = "platform")]
		[FlexJamMember(Name = "platform", Type = FlexJamType.UInt32)]
		public uint Platform { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "currentTime")]
		[FlexJamMember(Name = "currentTime", Type = FlexJamType.Int32)]
		public int CurrentTime { get; set; }

		[FlexJamMember(Name = "buildVariant", Type = FlexJamType.String)]
		[System.Runtime.Serialization.DataMember(Name = "buildVariant")]
		public string BuildVariant { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "timeZone")]
		[FlexJamMember(Name = "timeZone", Type = FlexJamType.String)]
		public string TimeZone { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "versionDataBuild")]
		[FlexJamMember(Name = "versionDataBuild", Type = FlexJamType.UInt32)]
		public uint VersionDataBuild { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "audioLocale")]
		[FlexJamMember(Name = "audioLocale", Type = FlexJamType.UInt32)]
		public uint AudioLocale { get; set; }

		[FlexJamMember(Name = "version", Type = FlexJamType.Struct)]
		[System.Runtime.Serialization.DataMember(Name = "version")]
		public JamJSONGameVersion Version { get; set; }

		[FlexJamMember(ArrayDimensions = 1, Name = "secret", Type = FlexJamType.UInt8)]
		[System.Runtime.Serialization.DataMember(Name = "secret")]
		public byte[] Secret { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "type")]
		[FlexJamMember(Name = "type", Type = FlexJamType.UInt32)]
		public uint Type { get; set; }

		[FlexJamMember(Name = "textLocale", Type = FlexJamType.UInt32)]
		[System.Runtime.Serialization.DataMember(Name = "textLocale")]
		public uint TextLocale { get; set; }
	}
}
