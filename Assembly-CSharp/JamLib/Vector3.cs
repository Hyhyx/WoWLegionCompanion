﻿using System;
using System.Runtime.Serialization;

namespace JamLib
{
	[FlexJamStruct(Name = "vector3")]
	[System.Runtime.Serialization.DataContract]
	public struct Vector3
	{
		[FlexJamMember(Name = "x", Type = FlexJamType.Float)]
		[System.Runtime.Serialization.DataMember(Name = "x")]
		public float X { get; set; }

		[FlexJamMember(Name = "y", Type = FlexJamType.Float)]
		[System.Runtime.Serialization.DataMember(Name = "y")]
		public float Y { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "z")]
		[FlexJamMember(Name = "z", Type = FlexJamType.Float)]
		public float Z { get; set; }
	}
}
