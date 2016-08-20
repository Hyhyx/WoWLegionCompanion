﻿using System;

namespace WowStatConstants
{
	public enum GARR_ABILITY_FLAGS
	{
		IS_TRAIT = 1,
		NO_RANDOM_SELECTION,
		HORDE_ONLY = 4,
		ALLIANCE_ONLY = 8,
		NO_REROLL = 16,
		FIRST_SLOT_ONLY = 32,
		SINGLE_MISSION_DURATION = 64,
		ACTIVE_ONLY_ON_ZONE_SUPPORT = 128,
		APPLY_TO_FIRST_MISSION = 256,
		IS_SPECIALIZATION = 512,
		IS_EMPTY_SLOT = 1024
	}
}
