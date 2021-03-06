﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using WowStatConstants;
using WowStaticData;

namespace WoWCompanionApp
{
	public class GeneralHelpers : MonoBehaviour
	{
		public static Color GetQualityColor(int quality)
		{
			Color white = Color.white;
			Color green = Color.green;
			Color result;
			result..ctor(0.34f, 0.49f, 1f, 1f);
			Color result2;
			result2..ctor(0.58f, 0f, 1f, 1f);
			Color result3;
			result3..ctor(1f, 0.56f, 0f, 1f);
			switch (quality)
			{
			case 1:
				return white;
			case 2:
				return green;
			case 3:
				return result;
			case 4:
				return result2;
			case 5:
				return result3;
			case 6:
				return result2;
			default:
				return Color.gray;
			}
		}

		private static int GetMobileAtlasMemberOverride(int iconFileDataID)
		{
			int result = 0;
			switch (iconFileDataID)
			{
			case 1383681:
				result = 6150;
				break;
			case 1383682:
				result = 6149;
				break;
			case 1383683:
				result = 6148;
				break;
			default:
				if (iconFileDataID != 1380306)
				{
					if (iconFileDataID == 1390116)
					{
						result = 0;
					}
				}
				else
				{
					result = 6147;
				}
				break;
			}
			return result;
		}

		public static Sprite LoadIconAsset(AssetBundleType assetBundleType, int iconFileDataID)
		{
			if (iconFileDataID == 894556)
			{
				string locale = Main.instance.GetLocale();
				return Resources.Load<Sprite>("MiscIcons/LocalizedIcons/" + locale + "/XP_Icon");
			}
			if (iconFileDataID == 1380306)
			{
				return Resources.Load<Sprite>("MissionMechanicEffects/MechanicIcon-Curse-Shadow");
			}
			if (iconFileDataID == 1383683)
			{
				return Resources.Load<Sprite>("MissionMechanicEffects/MechanicIcon-Disorienting-Shadow");
			}
			if (iconFileDataID == 1383682)
			{
				return Resources.Load<Sprite>("MissionMechanicEffects/MechanicIcon-Lethal-Shadow");
			}
			if (iconFileDataID == 1390116)
			{
				return Resources.Load<Sprite>("MissionMechanicEffects/MechanicIcon-Powerful-Shadow");
			}
			if (iconFileDataID == 1383681)
			{
				return Resources.Load<Sprite>("MissionMechanicEffects/MechanicIcon-Slowing-Shadow");
			}
			int mobileAtlasMemberOverride = GeneralHelpers.GetMobileAtlasMemberOverride(iconFileDataID);
			if (mobileAtlasMemberOverride > 0)
			{
				return TextureAtlas.instance.GetAtlasSprite(mobileAtlasMemberOverride);
			}
			AssetBundle assetBundle = null;
			string arg = string.Empty;
			if (assetBundleType == AssetBundleType.Icons)
			{
				arg = "Assets/BundleAssets/Icons/";
				assetBundle = AssetBundleManager.Icons;
			}
			if (assetBundleType == AssetBundleType.PortraitIcons)
			{
				arg = "Assets/BundleAssets/PortraitIcons/";
				assetBundle = AssetBundleManager.PortraitIcons;
			}
			string text = arg + iconFileDataID + ".tga";
			Sprite sprite = assetBundle.LoadAsset<Sprite>(text);
			if (sprite == null)
			{
				text = arg + iconFileDataID + ".png";
				sprite = assetBundle.LoadAsset<Sprite>(text);
			}
			return sprite;
		}

		public static Sprite LoadCurrencyIcon(int currencyID)
		{
			return AssetBundleManager.Icons.LoadAsset<Sprite>("Currency_" + currencyID.ToString("00000000"));
		}

		public static void GetXpCapInfo(int followerLevel, int followerQuality, out uint xpToNextLevelOrQuality, out bool isQuality, out bool isMaxLevelAndMaxQuality)
		{
			isMaxLevelAndMaxQuality = false;
			isQuality = false;
			GarrFollowerLevelXPRec garrFollowerLevelXPRec = StaticDB.garrFollowerLevelXPDB.GetRecordsByParentID(followerLevel).First((GarrFollowerLevelXPRec rec) => (GARR_TYPE)StaticDB.garrFollowerTypeDB.GetRecord((int)rec.GarrFollowerTypeID).GarrTypeID == GarrisonStatus.GarrisonType);
			if (garrFollowerLevelXPRec.XpToNextLevel > 0)
			{
				xpToNextLevelOrQuality = (uint)garrFollowerLevelXPRec.XpToNextLevel;
				return;
			}
			isQuality = true;
			GarrFollowerQualityRec garrFollowerQualityRec = StaticDB.garrFollowerQualityDB.GetRecordsByParentID(followerQuality).First((GarrFollowerQualityRec rec) => (GARR_FOLLOWER_TYPE)rec.GarrFollowerTypeID == GarrisonStatus.GarrisonFollowerType);
			xpToNextLevelOrQuality = garrFollowerQualityRec.XpToNextQuality;
			if (garrFollowerQualityRec.XpToNextQuality == 0u)
			{
				isMaxLevelAndMaxQuality = true;
			}
		}

		public static uint GetMaxFollowerItemLevel()
		{
			GarrFollowerTypeRec record = StaticDB.garrFollowerTypeDB.GetRecord((int)GarrisonStatus.GarrisonFollowerType);
			return (uint)record.MaxItemLevel;
		}

		public static FollowerStatus GetFollowerStatus(WrapperGarrisonFollower follower)
		{
			bool flag = (follower.Flags & 4) != 0;
			if (flag)
			{
				return FollowerStatus.inactive;
			}
			bool flag2 = (follower.Flags & 2) != 0;
			if (flag2)
			{
				return FollowerStatus.fatigued;
			}
			bool flag3 = follower.CurrentBuildingID != 0;
			if (flag3)
			{
				return FollowerStatus.inBuilding;
			}
			bool flag4 = follower.CurrentMissionID != 0;
			if (flag4)
			{
				return FollowerStatus.onMission;
			}
			return FollowerStatus.available;
		}

		public static string GetColorFromInt(int color)
		{
			int num = color >> 16 & 255;
			int num2 = color >> 8 & 255;
			int num3 = color & 255;
			return string.Format("{0:X2}{1:X2}{2:X2}", num, num2, num3);
		}

		public static string GetMobileStatColorString(WrapperStatColor color)
		{
			switch (color)
			{
			case 0:
				return "808080ff";
			case 2:
				return "00ff00ff";
			case 3:
				return "ff0000ff";
			case 4:
				return "808080ff";
			case 5:
				return "ff2020ff";
			}
			return "ffffffff";
		}

		public static string GetItemQualityColorTag(int qualityID)
		{
			return "<color=#" + GeneralHelpers.GetItemQualityColor(qualityID) + ">";
		}

		public static string GetItemQualityColor(int qualityID)
		{
			switch (qualityID)
			{
			case 0:
				return "9d9d9dff";
			case 2:
				return "1eff00ff";
			case 3:
				return "0070ddff";
			case 4:
				return "a335eeff";
			case 5:
				return "ff8000ff";
			case 6:
				return "e6cc80ff";
			case 7:
				return "00ccffff";
			case 8:
				return "00ccffff";
			}
			return "ffffffff";
		}

		public static string GetItemDescription(ItemRec itemRec)
		{
			string text = string.Empty;
			int num = (from effectRec in StaticDB.itemEffectDB.GetRecordsByParentID(itemRec.ID)
			select effectRec.SpellID).FirstOrDefault((int id) => id != 0);
			if (num > 0)
			{
				SpellTooltipRec record = StaticDB.spellTooltipDB.GetRecord(num);
				if (record != null)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"<color=#",
						GeneralHelpers.s_friendlyColor,
						">",
						WowTextParser.parser.Parse(record.Description, num),
						"</color>"
					});
				}
				else
				{
					Debug.Log("GetItemDescription: spellID " + num + " not found in spellTooltipDB.");
				}
			}
			if (itemRec.Description != null && itemRec.Description != string.Empty)
			{
				if (text != string.Empty)
				{
					text += "\n\n";
				}
				if (itemRec.ID != 141028 && itemRec.ID != 137565 && itemRec.ID != 137560 && itemRec.ID != 137561)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"<color=#",
						GeneralHelpers.s_defaultColor,
						">\"",
						WowTextParser.parser.Parse(itemRec.Description, 0),
						"\"</color>"
					});
				}
			}
			return text;
		}

		public static bool SpellGrantsArtifactXP(int spellID)
		{
			if (spellID <= 0)
			{
				return false;
			}
			return StaticDB.spellEffectDB.GetRecordsByParentID(spellID).Any((SpellEffectRec effectRec) => effectRec.Effect == 240);
		}

		public static int GetNumActiveChampions()
		{
			return PersistentFollowerData.followerDictionary.Count((KeyValuePair<int, WrapperGarrisonFollower> pair) => !GeneralHelpers.IsFollowerInactive(pair.Value) && !GeneralHelpers.IsFollowerTroop(pair.Value));
		}

		public static int GetNumInactiveChampions()
		{
			return PersistentFollowerData.followerDictionary.Count((KeyValuePair<int, WrapperGarrisonFollower> pair) => GeneralHelpers.IsFollowerInactive(pair.Value) && !GeneralHelpers.IsFollowerTroop(pair.Value));
		}

		public static int GetNumTroops()
		{
			return PersistentFollowerData.followerDictionary.Count((KeyValuePair<int, WrapperGarrisonFollower> pair) => !GeneralHelpers.IsFollowerInactive(pair.Value) && GeneralHelpers.IsFollowerTroop(pair.Value) && pair.Value.Durability > 0);
		}

		private static bool IsFollowerInactive(WrapperGarrisonFollower follower)
		{
			return (follower.Flags & 4) != 0;
		}

		private static bool IsFollowerTroop(WrapperGarrisonFollower follower)
		{
			return (follower.Flags & 8) != 0;
		}

		public static string GetInventoryTypeString(INVENTORY_TYPE invType)
		{
			switch (invType)
			{
			case INVENTORY_TYPE.HEAD:
				return StaticDB.GetString("INVTYPE_HEAD", null);
			case INVENTORY_TYPE.NECK:
				return StaticDB.GetString("INVTYPE_NECK", null);
			case INVENTORY_TYPE.SHOULDER:
				return StaticDB.GetString("INVTYPE_SHOULDER", null);
			case INVENTORY_TYPE.BODY:
				return StaticDB.GetString("INVTYPE_BODY", null);
			case INVENTORY_TYPE.CHEST:
				return StaticDB.GetString("INVTYPE_CHEST", null);
			case INVENTORY_TYPE.WAIST:
				return StaticDB.GetString("INVTYPE_WAIST", null);
			case INVENTORY_TYPE.LEGS:
				return StaticDB.GetString("INVTYPE_LEGS", null);
			case INVENTORY_TYPE.FEET:
				return StaticDB.GetString("INVTYPE_FEET", null);
			case INVENTORY_TYPE.WRIST:
				return StaticDB.GetString("INVTYPE_WRIST", null);
			case INVENTORY_TYPE.HAND:
				return StaticDB.GetString("INVTYPE_HAND", null);
			case INVENTORY_TYPE.FINGER:
				return StaticDB.GetString("INVTYPE_FINGER", null);
			case INVENTORY_TYPE.TRINKET:
				return StaticDB.GetString("INVTYPE_TRINKET", null);
			case INVENTORY_TYPE.WEAPON:
				return StaticDB.GetString("INVTYPE_WEAPON", null);
			case INVENTORY_TYPE.SHIELD:
				return StaticDB.GetString("INVTYPE_SHIELD", null);
			case INVENTORY_TYPE.RANGED:
				return StaticDB.GetString("INVTYPE_RANGED", null);
			case INVENTORY_TYPE.CLOAK:
				return StaticDB.GetString("INVTYPE_CLOAK", null);
			case INVENTORY_TYPE.TWO_H_WEAPON:
				return StaticDB.GetString("INVTYPE_2HWEAPON", null);
			case INVENTORY_TYPE.BAG:
				return StaticDB.GetString("INVTYPE_BAG", null);
			case INVENTORY_TYPE.TABARD:
				return StaticDB.GetString("INVTYPE_TABARD", null);
			case INVENTORY_TYPE.ROBE:
				return StaticDB.GetString("INVTYPE_ROBE", null);
			case INVENTORY_TYPE.WEAPONMAINHAND:
				return StaticDB.GetString("INVTYPE_WEAPONMAINHAND", null);
			case INVENTORY_TYPE.WEAPONOFFHAND:
				return StaticDB.GetString("INVTYPE_WEAPONOFFHAND", null);
			case INVENTORY_TYPE.HOLDABLE:
				return StaticDB.GetString("INVTYPE_HOLDABLE", null);
			case INVENTORY_TYPE.AMMO:
				return StaticDB.GetString("INVTYPE_AMMO", null);
			case INVENTORY_TYPE.THROWN:
				return StaticDB.GetString("INVTYPE_THROWN", null);
			case INVENTORY_TYPE.RANGEDRIGHT:
				return StaticDB.GetString("INVTYPE_RANGEDRIGHT", null);
			case INVENTORY_TYPE.QUIVER:
				return StaticDB.GetString("INVTYPE_QUIVER", null);
			case INVENTORY_TYPE.RELIC:
				return StaticDB.GetString("INVTYPE_RELIC", null);
			default:
				return string.Empty;
			}
		}

		public static string GetBonusStatString(BonusStatIndex statIndex)
		{
			switch (statIndex)
			{
			case BonusStatIndex.MANA:
				return StaticDB.GetString("ITEM_MOD_MANA_SHORT", null);
			case BonusStatIndex.HEALTH:
				return StaticDB.GetString("ITEM_MOD_HEALTH_SHORT", null);
			default:
				if (statIndex != BonusStatIndex.MASTERY_RATING)
				{
					return string.Empty;
				}
				return StaticDB.GetString("ITEM_MOD_MASTERY_RATING", null);
			case BonusStatIndex.AGILITY:
				return StaticDB.GetString("ITEM_MOD_AGILITY_SHORT", null);
			case BonusStatIndex.STRENGTH:
				return StaticDB.GetString("ITEM_MOD_STRENGTH_SHORT", null);
			case BonusStatIndex.INTELLECT:
				return StaticDB.GetString("ITEM_MOD_INTELLECT_SHORT", null);
			case BonusStatIndex.STAMINA:
				return StaticDB.GetString("ITEM_MOD_STAMINA_SHORT", null);
			case BonusStatIndex.DEFENSE_SKILL_RATING:
				return StaticDB.GetString("ITEM_MOD_DEFENSE_SKILL_RATING_SHORT", null);
			case BonusStatIndex.DODGE_RATING:
				return StaticDB.GetString("ITEM_MOD_DODGE_RATING_SHORT", null);
			case BonusStatIndex.PARRY_RATING:
				return StaticDB.GetString("ITEM_MOD_PARRY_RATING_SHORT", null);
			case BonusStatIndex.BLOCK_RATING:
				return StaticDB.GetString("ITEM_MOD_BLOCK_RATING_SHORT", null);
			case BonusStatIndex.HIT_MELEE_RATING:
				return StaticDB.GetString("ITEM_MOD_HIT_MELEE_RATING_SHORT", null);
			case BonusStatIndex.HIT_RANGED_RATING:
				return StaticDB.GetString("ITEM_MOD_HIT_RANGED_RATING_SHORT", null);
			case BonusStatIndex.HIT_SPELL_RATING:
				return StaticDB.GetString("ITEM_MOD_HIT_SPELL_RATING_SHORT", null);
			case BonusStatIndex.CRIT_MELEE_RATING:
				return StaticDB.GetString("ITEM_MOD_CRIT_MELEE_RATING_SHORT", null);
			case BonusStatIndex.CRIT_RANGED_RATING:
				return StaticDB.GetString("ITEM_MOD_CRIT_RANGED_RATING_SHORT", null);
			case BonusStatIndex.CRIT_SPELL_RATING:
				return StaticDB.GetString("ITEM_MOD_CRIT_SPELL_RATING_SHORT", null);
			case BonusStatIndex.HIT_RATING:
				return StaticDB.GetString("ITEM_MOD_HIT_RATING_SHORT", null);
			case BonusStatIndex.CRIT_RATING:
				return StaticDB.GetString("ITEM_MOD_CRIT_RATING_SHORT", null);
			case BonusStatIndex.RESILIENCE_RATING:
				return StaticDB.GetString("ITEM_MOD_RESILIENCE_RATING_SHORT", null);
			case BonusStatIndex.HASTE_RATING:
				return StaticDB.GetString("ITEM_MOD_HASTE_RATING_SHORT", null);
			case BonusStatIndex.EXPERTISE_RATING:
				return StaticDB.GetString("ITEM_MOD_EXPERTISE_RATING_SHORT", null);
			case BonusStatIndex.ATTACK_POWER:
				return StaticDB.GetString("ITEM_MOD_ATTACK_POWER_SHORT", null);
			case BonusStatIndex.VERSATILITY:
				return StaticDB.GetString("ITEM_MOD_VERSATILITY", null);
			}
		}

		public static FollowerCanCounterMechanic HasFollowerWhoCanCounter(int garrMechanicTypeID)
		{
			bool flag = false;
			using (Dictionary<int, WrapperGarrisonFollower>.ValueCollection.Enumerator enumerator = PersistentFollowerData.followerDictionary.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WrapperGarrisonFollower follower = enumerator.Current;
					for (int i = 0; i < follower.AbilityIDs.Count; i++)
					{
						GarrAbilityRec record = StaticDB.garrAbilityDB.GetRecord(follower.AbilityIDs[i]);
						if (record == null)
						{
							Debug.Log(string.Concat(new object[]
							{
								"Invalid Ability ID ",
								follower.AbilityIDs[i],
								" from follower ",
								follower.GarrFollowerID
							}));
						}
						else
						{
							IEnumerable<GarrAbilityEffectRec> source = from garrAbilityEffectRec in StaticDB.garrAbilityEffectDB.GetRecordsByParentID(record.ID)
							where garrAbilityEffectRec.GarrMechanicTypeID != 0 && garrAbilityEffectRec.AbilityAction == 0 && garrMechanicTypeID == (int)garrAbilityEffectRec.GarrMechanicTypeID
							select garrAbilityEffectRec;
							if (source.Any(delegate(GarrAbilityEffectRec garrAbilityEffectRec)
							{
								bool flag2 = (follower.Flags & 4) != 0;
								bool flag3 = (follower.Flags & 2) != 0;
								bool flag4 = follower.CurrentMissionID != 0;
								bool flag5 = follower.CurrentBuildingID != 0;
								return !flag2 && !flag3 && !flag4 && !flag5;
							}))
							{
								return FollowerCanCounterMechanic.yesAndAvailable;
							}
							flag |= (source.Count<GarrAbilityEffectRec>() > 0);
						}
					}
				}
			}
			if (flag)
			{
				return FollowerCanCounterMechanic.yesButBusy;
			}
			return FollowerCanCounterMechanic.no;
		}

		public static Sprite LoadClassIcon(uint? classID)
		{
			if (classID != null)
			{
				switch (classID.Value)
				{
				case 1u:
					return Resources.Load<Sprite>("NewLoginPanel/Character-Selection-Warrior");
				case 2u:
					return Resources.Load<Sprite>("NewLoginPanel/Character-Selection-Paladin");
				case 3u:
					return Resources.Load<Sprite>("NewLoginPanel/Character-Selection-Hunter");
				case 4u:
					return Resources.Load<Sprite>("NewLoginPanel/Character-Selection-Rogue");
				case 5u:
					return Resources.Load<Sprite>("NewLoginPanel/Character-Selection-Priest");
				case 6u:
					return Resources.Load<Sprite>("NewLoginPanel/Character-Selection-Death Knight");
				case 7u:
					return Resources.Load<Sprite>("NewLoginPanel/Character-Selection-Shaman");
				case 8u:
					return Resources.Load<Sprite>("NewLoginPanel/Character-Selection-Mage");
				case 9u:
					return Resources.Load<Sprite>("NewLoginPanel/Character-Selection-Warlock");
				case 10u:
					return Resources.Load<Sprite>("NewLoginPanel/Character-Selection-Monk");
				case 11u:
					return Resources.Load<Sprite>("NewLoginPanel/Character-Selection-Druid");
				case 12u:
					return Resources.Load<Sprite>("NewLoginPanel/Character-Selection-Demon Hunter");
				}
			}
			return Resources.Load<Sprite>("NewLoginPanel/Character-Selection-Unknown");
		}

		public static Font LoadFancyFont()
		{
			return FontLoader.LoadFancyFont();
		}

		public static Font LoadStandardFont()
		{
			return FontLoader.LoadStandardFont();
		}

		public static int CurrentUnixTime()
		{
			return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
		}

		public static int[] GetBuffsForCurrentMission(int garrFollowerID, int garrMissionID, GameObject missionFollowerSlotGroup, int missionDuration)
		{
			HashSet<int> hashSet = new HashSet<int>();
			if (!PersistentFollowerData.followerDictionary.ContainsKey(garrFollowerID))
			{
				return hashSet.ToArray<int>();
			}
			WrapperGarrisonFollower wrapperGarrisonFollower = PersistentFollowerData.followerDictionary[garrFollowerID];
			for (int i = 0; i < wrapperGarrisonFollower.AbilityIDs.Count; i++)
			{
				GarrAbilityRec record = StaticDB.garrAbilityDB.GetRecord(wrapperGarrisonFollower.AbilityIDs[i]);
				if (record == null)
				{
					Debug.Log(string.Concat(new object[]
					{
						"Invalid Ability ID ",
						wrapperGarrisonFollower.AbilityIDs[i],
						" from follower ",
						wrapperGarrisonFollower.GarrFollowerID
					}));
				}
				else
				{
					foreach (GarrAbilityEffectRec garrAbilityEffectRec in StaticDB.garrAbilityEffectDB.GetRecordsByParentID(record.ID))
					{
						if ((garrAbilityEffectRec.Flags & 1) == 0)
						{
							bool flag = false;
							byte abilityAction = garrAbilityEffectRec.AbilityAction;
							switch (abilityAction)
							{
							case 0:
								break;
							case 1:
							{
								MissionFollowerSlot[] componentsInChildren = missionFollowerSlotGroup.GetComponentsInChildren<MissionFollowerSlot>(true);
								int num = 0;
								foreach (MissionFollowerSlot missionFollowerSlot in componentsInChildren)
								{
									if (missionFollowerSlot.GetCurrentGarrFollowerID() > 0)
									{
										num++;
									}
								}
								flag = (num == 1);
								break;
							}
							case 2:
							case 17:
							case 21:
								flag = true;
								break;
							default:
								if (abilityAction != 37)
								{
								}
								break;
							case 5:
							{
								MissionFollowerSlot[] componentsInChildren2 = missionFollowerSlotGroup.GetComponentsInChildren<MissionFollowerSlot>(true);
								bool flag2 = false;
								foreach (MissionFollowerSlot missionFollowerSlot2 in componentsInChildren2)
								{
									int currentGarrFollowerID = missionFollowerSlot2.GetCurrentGarrFollowerID();
									if (currentGarrFollowerID > 0 && currentGarrFollowerID != wrapperGarrisonFollower.GarrFollowerID)
									{
										GarrFollowerRec record2 = StaticDB.garrFollowerDB.GetRecord(currentGarrFollowerID);
										if (record2 != null)
										{
											uint num2 = (uint)((GarrisonStatus.Faction() != PVP_FACTION.ALLIANCE) ? record2.HordeGarrFollRaceID : record2.AllianceGarrFollRaceID);
											if (num2 == (uint)garrAbilityEffectRec.ActionRace)
											{
												flag2 = true;
												break;
											}
										}
									}
								}
								flag = flag2;
								break;
							}
							case 6:
								flag = ((float)missionDuration >= (float)garrAbilityEffectRec.ActionHours * 3600f);
								break;
							case 7:
								flag = ((float)missionDuration <= (float)garrAbilityEffectRec.ActionHours * 3600f);
								break;
							case 9:
							{
								GarrMissionRec record3 = StaticDB.garrMissionDB.GetRecord(garrMissionID);
								flag = (record3 != null && (float)record3.TravelDuration >= (float)garrAbilityEffectRec.ActionHours * 3600f);
								break;
							}
							case 10:
							{
								GarrMissionRec record4 = StaticDB.garrMissionDB.GetRecord(garrMissionID);
								flag = (record4 != null && (float)record4.TravelDuration <= (float)garrAbilityEffectRec.ActionHours * 3600f);
								break;
							}
							case 12:
								break;
							case 22:
							{
								MissionFollowerSlot[] componentsInChildren3 = missionFollowerSlotGroup.GetComponentsInChildren<MissionFollowerSlot>(true);
								bool flag3 = false;
								foreach (MissionFollowerSlot missionFollowerSlot3 in componentsInChildren3)
								{
									int currentGarrFollowerID2 = missionFollowerSlot3.GetCurrentGarrFollowerID();
									if (currentGarrFollowerID2 > 0 && currentGarrFollowerID2 != wrapperGarrisonFollower.GarrFollowerID)
									{
										GarrFollowerRec record5 = StaticDB.garrFollowerDB.GetRecord(currentGarrFollowerID2);
										if (record5 != null)
										{
											uint num3 = (uint)((GarrisonStatus.Faction() != PVP_FACTION.ALLIANCE) ? record5.HordeGarrClassSpecID : record5.AllianceGarrClassSpecID);
											if ((ulong)num3 == (ulong)((long)garrAbilityEffectRec.ActionRecordID))
											{
												flag3 = true;
												break;
											}
										}
									}
								}
								flag = flag3;
								break;
							}
							case 23:
							{
								bool flag4 = false;
								if (PersistentMissionData.missionDictionary.ContainsKey(garrMissionID))
								{
									WrapperGarrisonMission wrapperGarrisonMission = PersistentMissionData.missionDictionary[garrMissionID];
									for (int m = 0; m < wrapperGarrisonMission.Encounters.Count; m++)
									{
										for (int n = 0; n < wrapperGarrisonMission.Encounters[m].MechanicIDs.Count; n++)
										{
											GarrMechanicRec record6 = StaticDB.garrMechanicDB.GetRecord(wrapperGarrisonMission.Encounters[m].MechanicIDs[n]);
											if (record6 != null && garrAbilityEffectRec.GarrMechanicTypeID == record6.GarrMechanicTypeID)
											{
												flag4 = true;
												break;
											}
										}
									}
								}
								flag = flag4;
								break;
							}
							case 26:
							{
								MissionFollowerSlot[] componentsInChildren4 = missionFollowerSlotGroup.GetComponentsInChildren<MissionFollowerSlot>(true);
								bool flag5 = false;
								foreach (MissionFollowerSlot missionFollowerSlot4 in componentsInChildren4)
								{
									int currentGarrFollowerID3 = missionFollowerSlot4.GetCurrentGarrFollowerID();
									if (currentGarrFollowerID3 > 0 && currentGarrFollowerID3 != wrapperGarrisonFollower.GarrFollowerID && garrAbilityEffectRec.ActionRecordID == currentGarrFollowerID3)
									{
										flag5 = true;
										break;
									}
								}
								flag = flag5;
								break;
							}
							}
							if (flag)
							{
								hashSet.Add(record.ID);
							}
						}
					}
				}
			}
			return hashSet.ToArray<int>();
		}

		public static int GetAdjustedMissionDuration(GarrMissionRec garrMissionRec, List<WrapperGarrisonFollower> followerList, GameObject enemyPortraits)
		{
			float num = (float)garrMissionRec.MissionDuration;
			IEnumerable<GarrAbilityEffectRec> enumerable;
			if (enemyPortraits != null)
			{
				MissionMechanic[] componentsInChildren = enemyPortraits.GetComponentsInChildren<MissionMechanic>(true);
				enumerable = from garrAbilityEffectRec in (from mechanic in componentsInChildren
				where !mechanic.IsCountered() && mechanic.AbilityID() != 0
				select mechanic).SelectMany((MissionMechanic mechanic) => StaticDB.garrAbilityEffectDB.GetRecordsByParentID(mechanic.AbilityID()))
				where garrAbilityEffectRec.AbilityAction == 17
				select garrAbilityEffectRec;
				foreach (GarrAbilityEffectRec garrAbilityEffectRec3 in enumerable)
				{
					num *= garrAbilityEffectRec3.ActionValueFlat;
				}
			}
			enumerable = from garrAbilityEffectRec in followerList.SelectMany((WrapperGarrisonFollower follower) => follower.AbilityIDs.SelectMany((int abilityID) => StaticDB.garrAbilityEffectDB.GetRecordsByParentID(abilityID)))
			where garrAbilityEffectRec.AbilityAction == 17
			select garrAbilityEffectRec;
			foreach (GarrAbilityEffectRec garrAbilityEffectRec2 in enumerable)
			{
				num *= garrAbilityEffectRec2.ActionValueFlat;
			}
			num *= GeneralHelpers.GetMissionDurationTalentMultiplier();
			return (int)num;
		}

		public static long ApplyArtifactXPMultiplier(long inputAmount, double multiplier)
		{
			if (multiplier > 1.0)
			{
				double num = (double)inputAmount * multiplier;
				long num2;
				if (num < 50.0)
				{
					num2 = 1L;
				}
				else if (num < 1000.0)
				{
					num2 = 5L;
				}
				else if (num < 5000.0)
				{
					num2 = 25L;
				}
				else
				{
					num2 = 50L;
				}
				return num2 * (long)Math.Round(num / (double)num2);
			}
			return inputAmount;
		}

		public static bool MissionHasUncounteredDeadly(GameObject enemyPortraitsGroup)
		{
			if (enemyPortraitsGroup != null)
			{
				MissionMechanic[] componentsInChildren = enemyPortraitsGroup.GetComponentsInChildren<MissionMechanic>(true);
				return (from mechanic in componentsInChildren
				where !mechanic.IsCountered() && mechanic.AbilityID() != 0
				select mechanic).SelectMany((MissionMechanic mechanic) => StaticDB.garrAbilityEffectDB.GetRecordsByParentID(mechanic.AbilityID())).Any((GarrAbilityEffectRec garrAbilityEffectRec) => garrAbilityEffectRec.AbilityAction == 27);
			}
			return false;
		}

		public static string TextOrderString(string text1, string text2)
		{
			if (Main.instance.GetLocale() == "koKR")
			{
				return string.Format("{0} {1}", text2, text1);
			}
			return string.Format("{0} {1}", text1, text2);
		}

		public static Sprite GetLocalizedFollowerXpIcon()
		{
			string locale = Main.instance.GetLocale();
			return Resources.Load<Sprite>("MiscIcons/LocalizedIcons/" + locale + "/XPBonus_Icon");
		}

		public static string LimitZhLineLength(string inText, int length)
		{
			if (Main.instance.GetLocale() != "zhCN" && Main.instance.GetLocale() != "zhTW")
			{
				return inText;
			}
			bool flag = false;
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < inText.Length; i++)
			{
				string text = inText.Substring(i, 1);
				stringBuilder.Append(text);
				if (text == "<")
				{
					flag = true;
				}
				else if (text == ">")
				{
					flag = false;
				}
				else if (!flag)
				{
					num++;
					if (num > length)
					{
						stringBuilder.Append(" ");
						num = 0;
					}
				}
			}
			return stringBuilder.ToString();
		}

		public static float GetMissionDurationTalentMultiplier()
		{
			float num = 1f;
			foreach (WrapperGarrisonTalent wrapperGarrisonTalent in PersistentTalentData.talentDictionary.Values)
			{
				if ((wrapperGarrisonTalent.Flags & 1) != 0)
				{
					GarrTalentRec record = StaticDB.garrTalentDB.GetRecord(wrapperGarrisonTalent.GarrTalentID);
					if (record != null)
					{
						if (record.GarrAbilityID > 0u)
						{
							foreach (GarrAbilityEffectRec garrAbilityEffectRec in from rec in StaticDB.garrAbilityEffectDB.GetRecordsByParentID((int)record.GarrAbilityID)
							where rec.AbilityAction == 17
							select rec)
							{
								num *= garrAbilityEffectRec.ActionValueFlat;
							}
						}
					}
				}
			}
			return num;
		}

		public static string QuantityRule(string formatString, int quantity)
		{
			formatString = formatString.Replace("%d", quantity.ToString());
			Regex regex = new Regex("\\|4(?<singular>[\\p{L}\\d\\s]+):(?<plural1>[\\p{L}\\d\\s]+)(:(?<plural2>[\\p{L}\\d\\s]+))?;");
			Match match = regex.Match(formatString);
			if (match.Success)
			{
				return regex.Replace(formatString, delegate(Match m)
				{
					switch (GeneralHelpers.GetPluralIndex(quantity))
					{
					case 1:
						return m.Groups["plural1"].Value;
					case 2:
						return m.Groups["plural2"].Value;
					}
					return m.Groups["singular"].Value;
				});
			}
			if (formatString.Contains("|4"))
			{
				Debug.LogError("Error parsing string for quantity rule: " + formatString);
			}
			return formatString;
		}

		public static int GetPluralIndex(int quantity)
		{
			if (!MobileDeviceLocale.GetLanguageCode().StartsWith("ru", StringComparison.OrdinalIgnoreCase))
			{
				return (quantity <= 1) ? 0 : 1;
			}
			switch (quantity % 100)
			{
			case 11:
			case 12:
			case 13:
			case 14:
				return 2;
			default:
				switch (quantity % 10)
				{
				case 1:
					return 0;
				case 2:
				case 3:
				case 4:
					return 1;
				default:
					return 2;
				}
				break;
			}
		}

		public static string s_defaultColor = "ffd200ff";

		public static string s_normalColor = "ffffffff";

		public static string s_friendlyColor = "00ff00ff";
	}
}
