﻿using System;
using System.Runtime.Serialization;
using JamLib;

namespace WowJamMessages.MobilePlayerJSON
{
	[System.Runtime.Serialization.DataContract]
	[FlexJamMessage(Id = 4791, Name = "MobilePlayerCreateShipment", Version = 28333852u)]
	public class MobilePlayerCreateShipment
	{
		[FlexJamMember(Name = "charShipmentID", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "charShipmentID")]
		public int CharShipmentID { get; set; }

		[FlexJamMember(Name = "numShipments", Type = FlexJamType.Int32)]
		[System.Runtime.Serialization.DataMember(Name = "numShipments")]
		public int NumShipments { get; set; }
	}
}
