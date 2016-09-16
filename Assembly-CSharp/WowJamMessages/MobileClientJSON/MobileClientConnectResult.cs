﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages.MobileClientJSON
{
	[System.Runtime.Serialization.DataContract]
	[FlexJamMessage(Id = 4832, Name = "MobileClientConnectResult", Version = 28333852u)]
	public class MobileClientConnectResult
	{
		public MobileClientConnectResult()
		{
			this.Version = 0;
		}

		[System.Runtime.Serialization.DataMember(Name = "result")]
		[FlexJamMember(Name = "result", Type = FlexJamType.Enum)]
		public MOBILE_CONNECT_RESULT Result { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "version")]
		[FlexJamMember(Name = "version", Type = FlexJamType.Int32)]
		public int Version { get; set; }
	}
}
