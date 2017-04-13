﻿using System;
using System.Runtime.Serialization;

namespace JamLib
{
	[FlexJamStruct(Name = "vector2")]
	[System.Runtime.Serialization.DataContract]
	public struct Vector2
	{
		[FlexJamMember(Name = "x", Type = FlexJamType.Float)]
		[System.Runtime.Serialization.DataMember(Name = "x")]
		public float X { get; set; }

		[System.Runtime.Serialization.DataMember(Name = "y")]
		[FlexJamMember(Name = "y", Type = FlexJamType.Float)]
		public float Y { get; set; }
	}
}
