﻿using System;
using System.Collections;
using UnityEngine;

namespace WowStaticData
{
	public class GarrEncounterDB
	{
		public GarrEncounterRec GetRecord(int id)
		{
			return (GarrEncounterRec)this.m_records[id];
		}

		public void EnumRecords(Predicate<GarrEncounterRec> callback)
		{
			foreach (object obj in this.m_records.Values)
			{
				GarrEncounterRec obj2 = (GarrEncounterRec)obj;
				if (!callback(obj2))
				{
					break;
				}
			}
		}

		public bool Load(string path, AssetBundle nonLocalizedBundle, AssetBundle localizedBundle, string locale)
		{
			string text = string.Concat(new string[]
			{
				path,
				locale,
				"/GarrEncounter_",
				locale,
				".txt"
			});
			if (this.m_records != null)
			{
				Debug.Log("Already loaded static db " + text);
				return false;
			}
			TextAsset textAsset = localizedBundle.LoadAsset<TextAsset>(text);
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
					GarrEncounterRec garrEncounterRec = new GarrEncounterRec();
					garrEncounterRec.Deserialize(valueLine);
					this.m_records.Add(garrEncounterRec.ID, garrEncounterRec);
					num = num2 + 1;
				}
			}
			while (num2 > 0);
			return true;
		}

		private Hashtable m_records;
	}
}
