﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages.JSONRealmList
{
	[System.Runtime.Serialization.DataContract]
	[FlexJamStruct(Name = "JamJSONRealmListServerIPFamily", Version = 28333852u)]
	public class JamJSONRealmListServerIPFamily
	{
		[System.Runtime.Serialization.DataMember(Name = "family")]
		[FlexJamMember(Name = "family", Type = FlexJamType.Int8)]
		public sbyte Family { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "addresses")]
		[FlexJamMember(ArrayDimensions = 1, Name = "addresses", Type = FlexJamType.Struct)]
		public JamJSONRealmListServerIPAddress[] Addresses { get; set; }
	}
}
