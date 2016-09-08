﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages
{
	[FlexJamStruct(Name = "JamServerBuckDataList", Version = 28333852u)]
	[System.Runtime.Serialization.DataContract]
	public class JamServerBuckDataList
	{
		[FlexJamMember(Name = "mpID", Type = FlexJamType.UInt32)]
		[System.Runtime.Serialization.DataMember(Name = "mpID")]
		public uint MpID { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "entries")]
		[FlexJamMember(ArrayDimensions = 1, Name = "entries", Type = FlexJamType.Struct)]
		public JamServerBuckDataEntry[] Entries { get; set; }
	}
}