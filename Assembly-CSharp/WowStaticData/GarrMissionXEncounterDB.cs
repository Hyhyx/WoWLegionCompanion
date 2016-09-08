﻿using System;
using System.Collections;
using UnityEngine;

namespace WowStaticData
{
	public class GarrMissionXEncounterDB
	{
		public GarrMissionXEncounterRec GetRecord(int id)
		{
			return (GarrMissionXEncounterRec)this.m_records[id];
		}

		public void EnumRecords(Predicate<GarrMissionXEncounterRec> callback)
		{
			foreach (object obj in this.m_records.Values)
			{
				GarrMissionXEncounterRec obj2 = (GarrMissionXEncounterRec)obj;
				if (!callback(obj2))
				{
					break;
				}
			}
		}

		public void EnumRecordsByParentID(int parentID, Predicate<GarrMissionXEncounterRec> callback)
		{
			foreach (object obj in this.m_records.Values)
			{
				GarrMissionXEncounterRec garrMissionXEncounterRec = (GarrMissionXEncounterRec)obj;
				if ((ulong)garrMissionXEncounterRec.GarrMissionID == (ulong)((long)parentID) && !callback(garrMissionXEncounterRec))
				{
					break;
				}
			}
		}

		public bool Load(string path, AssetBundle nonLocalizedBundle, AssetBundle localizedBundle, string locale)
		{
			string text = path + "NonLocalized/GarrMissionXEncounter.txt";
			if (this.m_records != null)
			{
				Debug.Log("Already loaded static db " + text);
				return false;
			}
			TextAsset textAsset = nonLocalizedBundle.LoadAsset<TextAsset>(text);
			if (textAsset == null)
			{
				Debug.Log("Unable to load static db " + text);
				return false;
			}
			string text2 = textAsset.ToString();
			this.m_records = new Hashtable();
			int num = 0;
			int num2;
			do
			{
				num2 = text2.IndexOf('\n', num);
				if (num2 >= 0)
				{
					string valueLine = text2.Substring(num, num2 - num + 1).Trim();
					GarrMissionXEncounterRec garrMissionXEncounterRec = new GarrMissionXEncounterRec();
					garrMissionXEncounterRec.Deserialize(valueLine);
					this.m_records.Add(garrMissionXEncounterRec.ID, garrMissionXEncounterRec);
					num = num2 + 1;
				}
			}
			while (num2 > 0);
			return true;
		}

		private Hashtable m_records;
	}
}