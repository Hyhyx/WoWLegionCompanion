﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;
using WowJamMessages.MobileClientJSON;
using WowJamMessages.MobilePlayerJSON;
using WowStatConstants;
using WowStaticData;

public class Main : MonoBehaviour
{
	private void Awake()
	{
		Main.instance = this;
		this.GenerateUniqueIdentifier();
		this.canvasAnimator = this.mainCanvas.GetComponent<Animator>();
		this.allPanels.ShowConnectingPanel();
		AllPanels.instance.ShowConnectingPanelCancelButton(false);
		AllPanels.instance.SetConnectingPanelStatus(string.Empty);
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		Application.targetFrameRate = 30;
		GarrisonStatus.ArtifactKnowledgeLevel = 0;
		GarrisonStatus.ArtifactXpMultiplier = 1f;
	}

	private void Update()
	{
		this.ProcessMessages();
		this.UpdateDebugText();
		this.UpdateCanvasOrientation();
	}

	private void UpdateCanvasOrientation()
	{
		if (Screen.width > Screen.height)
		{
			this.canvasAnimator.SetBool("isLandscape", true);
		}
		else
		{
			this.canvasAnimator.SetBool("isLandscape", false);
		}
	}

	private void PrecacheMissionChances()
	{
		foreach (object obj in PersistentMissionData.missionDictionary.Values)
		{
			JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)obj;
			GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(jamGarrisonMobileMission.MissionRecID);
			if (record != null && record.GarrFollowerTypeID == 4u)
			{
				if (jamGarrisonMobileMission.MissionState == 1)
				{
					List<JamGarrisonFollower> list = new List<JamGarrisonFollower>();
					foreach (JamGarrisonFollower jamGarrisonFollower in PersistentFollowerData.followerDictionary.Values)
					{
						if (jamGarrisonFollower.CurrentMissionID == jamGarrisonMobileMission.MissionRecID)
						{
							list.Add(jamGarrisonFollower);
						}
					}
					MobilePlayerEvaluateMission mobilePlayerEvaluateMission = new MobilePlayerEvaluateMission();
					mobilePlayerEvaluateMission.GarrMissionID = jamGarrisonMobileMission.MissionRecID;
					mobilePlayerEvaluateMission.GarrFollowerID = new int[list.Count];
					int num = 0;
					foreach (JamGarrisonFollower jamGarrisonFollower2 in list)
					{
						mobilePlayerEvaluateMission.GarrFollowerID[num++] = jamGarrisonFollower2.GarrFollowerID;
					}
					Login.instance.SendToMobileServer(mobilePlayerEvaluateMission);
				}
			}
		}
	}

	private void HandleEnterWorld()
	{
		Main main = Main.instance;
		main.GarrisonDataResetFinishedAction = (Action)Delegate.Remove(main.GarrisonDataResetFinishedAction, new Action(this.HandleEnterWorld));
		AllPanels.instance.ShowAdventureMap();
		this.PrecacheMissionChances();
	}

	public void MobileLoggedIn()
	{
		Main main = Main.instance;
		main.GarrisonDataResetFinishedAction = (Action)Delegate.Combine(main.GarrisonDataResetFinishedAction, new Action(this.HandleEnterWorld));
		this.MobileRequestData();
	}

	public void OnMessageReceivedCB(object msg, MobileNetwork.MobileNetworkEventArgs args)
	{
		Queue<Main.MobileMessage> messageQueue = this.m_messageQueue;
		lock (messageQueue)
		{
			Main.MobileMessage mobileMessage = new Main.MobileMessage();
			mobileMessage.msg = msg;
			mobileMessage.args = args;
			this.m_messageQueue.Enqueue(mobileMessage);
		}
	}

	public void OnUnknownMessageReceivedCB(object msg, EventArgs e)
	{
		this.m_unknownMsg = (string)msg;
		Debug.Log("Received unknown message " + this.m_unknownMsg.ToString());
	}

	private void ProcessMessages()
	{
		Main.MobileMessage mobileMessage;
		do
		{
			Queue<Main.MobileMessage> messageQueue = this.m_messageQueue;
			lock (messageQueue)
			{
				if (this.m_messageQueue.Count > 0)
				{
					mobileMessage = this.m_messageQueue.Dequeue();
				}
				else
				{
					mobileMessage = null;
				}
			}
			if (mobileMessage != null)
			{
				this.ProcessMessage(mobileMessage);
			}
		}
		while (mobileMessage != null);
	}

	private void ProcessMessage(Main.MobileMessage mobileMsg)
	{
		object msg = mobileMsg.msg;
		if (msg is MobileClientLoginResult)
		{
			this.MobileClientLoginResultHandler((MobileClientLoginResult)msg);
		}
		else if (msg is MobileClientConnectResult)
		{
			this.MobileClientConnectResultHandler((MobileClientConnectResult)msg);
		}
		else if (msg is MobileClientGarrisonDataRequestResult)
		{
			this.MobileClientGarrisonDataRequestResultHandler((MobileClientGarrisonDataRequestResult)msg);
		}
		else if (msg is MobileClientStartMissionResult)
		{
			this.MobileClientStartMissionResultHandler((MobileClientStartMissionResult)msg);
		}
		else if (msg is MobileClientCompleteMissionResult)
		{
			this.MobileClientCompleteMissionResultHandler((MobileClientCompleteMissionResult)msg);
		}
		else if (msg is MobileClientClaimMissionBonusResult)
		{
			this.MobileClientClaimMissionBonusResultHandler((MobileClientClaimMissionBonusResult)msg);
		}
		else if (msg is MobileClientMissionAdded)
		{
			this.MobileClientMissionAddedHandler((MobileClientMissionAdded)msg);
		}
		else if (msg is MobileClientPong)
		{
			this.MobileClientPongHandler((MobileClientPong)msg);
		}
		else if (msg is MobileClientFollowerChangedXP)
		{
			this.MobileClientFollowerChangedXPHandler((MobileClientFollowerChangedXP)msg);
		}
		else if (msg is MobileClientExpediteMissionCheatResult)
		{
			this.MobileClientExpediteMissionCheatResultHandler((MobileClientExpediteMissionCheatResult)msg);
		}
		else if (msg is MobileClientAdvanceMissionSetResult)
		{
			this.MobileClientAdvanceMissionSetResultHandler((MobileClientAdvanceMissionSetResult)msg);
		}
		else if (msg is MobileClientChat)
		{
			this.MobileClientChatHandler((MobileClientChat)msg, mobileMsg.args.Count);
		}
		else if (msg is MobileClientGuildMembersOnline)
		{
			this.MobileClientGuildMembersOnlineHandler((MobileClientGuildMembersOnline)msg);
		}
		else if (msg is MobileClientGuildMemberLoggedIn)
		{
			this.MobileClientGuildMemberLoggedInHandler((MobileClientGuildMemberLoggedIn)msg);
		}
		else if (msg is MobileClientGuildMemberLoggedOut)
		{
			this.MobileClientGuildMemberLoggedOutHandler((MobileClientGuildMemberLoggedOut)msg);
		}
		else if (msg is MobileClientEmissaryFactionUpdate)
		{
			this.MobileClientEmissaryFactionUpdateHandler((MobileClientEmissaryFactionUpdate)msg);
		}
		else if (msg is MobileClientCreateShipmentResult)
		{
			this.MobileClientCreateShipmentResultHandler((MobileClientCreateShipmentResult)msg);
		}
		else if (msg is MobileClientShipmentsUpdate)
		{
			this.MobileClientShipmentsUpdateHandler((MobileClientShipmentsUpdate)msg);
		}
		else if (msg is MobileClientWorldQuestUpdate)
		{
			this.MobileClientWorldQuestUpdateHandler((MobileClientWorldQuestUpdate)msg);
		}
		else if (msg is MobileClientEvaluateMissionResult)
		{
			this.MobileClientEvaluateMissionResultHandler((MobileClientEvaluateMissionResult)msg);
		}
		else if (msg is MobileClientShipmentTypes)
		{
			this.MobileClientShipmentTypesHandler((MobileClientShipmentTypes)msg);
		}
		else if (msg is MobileClientCompleteShipmentResult)
		{
			this.MobileClientCompleteShipmentResultHandler((MobileClientCompleteShipmentResult)msg);
		}
		else if (msg is MobileClientSetShipmentDurationCheatResult)
		{
			this.MobileClientSetShipmentDurationCheatResultHandler((MobileClientSetShipmentDurationCheatResult)msg);
		}
		else if (msg is MobileClientSetMissionDurationCheatResult)
		{
			this.MobileClientSetMissionDurationCheatResultHandler((MobileClientSetMissionDurationCheatResult)msg);
		}
		else if (msg is MobileClientCanResearchGarrisonTalentResult)
		{
			this.MobileClientCanResearchGarrisonTalentResultHandler((MobileClientCanResearchGarrisonTalentResult)msg);
		}
		else if (msg is MobileClientResearchGarrisonTalentResult)
		{
			this.MobileClientResearchGarrisonTalentResultHandler((MobileClientResearchGarrisonTalentResult)msg);
		}
		else if (msg is MobileClientFollowerEquipmentResult)
		{
			this.MobileClientFollowerEquipmentResultHandler((MobileClientFollowerEquipmentResult)msg);
		}
		else if (msg is MobileClientFollowerChangedQuality)
		{
			this.MobileClientFollowerChangedQualityHandler((MobileClientFollowerChangedQuality)msg);
		}
		else if (msg is MobileClientFollowerArmamentsResult)
		{
			this.MobileClientFollowerArmamentsResultHandler((MobileClientFollowerArmamentsResult)msg);
		}
		else if (msg is MobileClientUseFollowerArmamentResult)
		{
			this.MobileClientUseFollowerArmamentResultHandler((MobileClientUseFollowerArmamentResult)msg);
		}
		else if (msg is MobileClientWorldQuestBountiesResult)
		{
			this.MobileClientWorldQuestBountiesResultHandler((MobileClientWorldQuestBountiesResult)msg);
		}
		else if (msg is MobileClientFollowerActivationDataResult)
		{
			this.MobileClientFollowerActivationDataResultHandler((MobileClientFollowerActivationDataResult)msg);
		}
		else if (msg is MobileClientChangeFollowerActiveResult)
		{
			this.MobileClientChangeFollowerActiveResultHandler((MobileClientChangeFollowerActiveResult)msg);
		}
		else if (msg is MobileClientGetItemTooltipInfoResult)
		{
			this.MobileClientGetItemTooltipInfoResultHandler((MobileClientGetItemTooltipInfoResult)msg);
		}
		else if (msg is MobileClientAuthChallenge)
		{
			this.MobileClientAuthChallengeHandler((MobileClientAuthChallenge)msg);
		}
		else if (msg is MobileClientArtifactInfoResult)
		{
			this.MobileClientArtifactInfoResultHandler((MobileClientArtifactInfoResult)msg);
		}
		else
		{
			Debug.Log("Unknown message received: " + msg.ToString());
		}
	}

	private void MobileClientLoginResultHandler(MobileClientLoginResult msg)
	{
		Login.instance.MobileLoginResult(msg.Success, msg.Version);
	}

	private void MobileClientConnectResultHandler(MobileClientConnectResult msg)
	{
		Login.instance.MobileConnectResult(msg);
	}

	private void MobileClientGarrisonDataRequestResultHandler(MobileClientGarrisonDataRequestResult msg)
	{
		PersistentFollowerData.ClearData();
		PersistentMissionData.ClearData();
		PersistentTalentData.ClearData();
		if (this.GarrisonDataResetStartedAction != null)
		{
			this.GarrisonDataResetStartedAction();
		}
		GarrisonStatus.SetFaction(msg.PvpFaction);
		GarrisonStatus.SetGarrisonServerConnectTime(msg.ServerTime);
		GarrisonStatus.SetCurrencies(msg.GoldCurrency, msg.OilCurrency, msg.OrderhallResourcesCurrency);
		GarrisonStatus.SetCharacterName(msg.CharacterName);
		GarrisonStatus.SetCharacterLevel(msg.CharacterLevel);
		GarrisonStatus.SetCharacterClass(msg.CharacterClassID);
		uint num = 0u;
		while ((ulong)num < (ulong)((long)msg.Follower.GetLength(0)))
		{
			JamGarrisonFollower jamGarrisonFollower = msg.Follower[(int)((UIntPtr)num)];
			PersistentFollowerData.AddOrUpdateFollower(jamGarrisonFollower);
			bool flag = (jamGarrisonFollower.Flags & 8) != 0;
			if (flag && jamGarrisonFollower.Durability <= 0)
			{
				Debug.Log("Follower " + jamGarrisonFollower.GarrFollowerID + " has expired.");
				if (this.TroopExpiredAction != null)
				{
					this.TroopExpiredAction(jamGarrisonFollower);
				}
			}
			num += 1u;
		}
		uint num2 = 0u;
		while ((ulong)num2 < (ulong)((long)msg.Mission.GetLength(0)))
		{
			PersistentMissionData.AddMission(msg.Mission[(int)((UIntPtr)num2)]);
			num2 += 1u;
		}
		for (int i = 0; i < msg.Talent.GetLength(0); i++)
		{
			PersistentTalentData.AddOrUpdateTalent(msg.Talent[i]);
		}
		if (this.GarrisonDataResetFinishedAction != null)
		{
			this.GarrisonDataResetFinishedAction();
		}
		if (this.FollowerDataChangedAction != null)
		{
			this.FollowerDataChangedAction();
		}
	}

	private void MobileClientStartMissionResultHandler(MobileClientStartMissionResult msg)
	{
		Debug.Log(string.Concat(new object[]
		{
			"StartMissionResult: ID=",
			msg.GarrMissionID,
			", result=",
			msg.Result
		}));
		if (msg.Result != 0)
		{
			GARRISON_RESULT result = (GARRISON_RESULT)msg.Result;
			string text = result.ToString();
			if (result == GARRISON_RESULT.FOLLOWER_SOFT_CAP_EXCEEDED)
			{
				text = StaticDB.GetString("TOO_MANY_ACTIVE_CHAMPIONS", null);
				AllPopups.instance.ShowGenericPopup(StaticDB.GetString("MISSION_START_FAILED", null), text);
			}
			else if (result == GARRISON_RESULT.MISSION_MISSING_REQUIRED_FOLLOWER)
			{
				text = StaticDB.GetString("MISSING_REQUIRED_FOLLOWER", null);
				AllPopups.instance.ShowGenericPopup(StaticDB.GetString("MISSION_START_FAILED", null), text);
			}
			else
			{
				AllPopups.instance.ShowGenericPopupFull(StaticDB.GetString("MISSION_START_FAILED", null));
				Debug.Log("Mission start result: " + text);
			}
		}
		this.MobileRequestData();
	}

	private void MobileClientCompleteMissionResultHandler(MobileClientCompleteMissionResult msg)
	{
		Debug.Log(string.Concat(new object[]
		{
			"CompleteMissionResult: ID=",
			msg.GarrMissionID,
			", result=",
			msg.Result,
			" success chance was ",
			msg.MissionSuccessChance
		}));
		PersistentMissionData.UpdateMission(msg.Mission);
		if (this.CompleteMissionResultAction != null)
		{
			this.CompleteMissionResultAction(msg.GarrMissionID, msg.Result, (int)msg.MissionSuccessChance);
		}
		MobilePlayerGarrisonDataRequest mobilePlayerGarrisonDataRequest = new MobilePlayerGarrisonDataRequest();
		mobilePlayerGarrisonDataRequest.GarrTypeID = 3;
		Login.instance.SendToMobileServer(mobilePlayerGarrisonDataRequest);
		MobilePlayerRequestShipmentTypes obj = new MobilePlayerRequestShipmentTypes();
		Login.instance.SendToMobileServer(obj);
		MobilePlayerRequestShipments obj2 = new MobilePlayerRequestShipments();
		Login.instance.SendToMobileServer(obj2);
		MobilePlayerFollowerEquipmentRequest mobilePlayerFollowerEquipmentRequest = new MobilePlayerFollowerEquipmentRequest();
		mobilePlayerFollowerEquipmentRequest.GarrFollowerTypeID = 4;
		Login.instance.SendToMobileServer(mobilePlayerFollowerEquipmentRequest);
	}

	private void MobileClientClaimMissionBonusResultHandler(MobileClientClaimMissionBonusResult msg)
	{
		PersistentMissionData.UpdateMission(msg.Mission);
		if (this.ClaimMissionBonusResultAction != null)
		{
			this.ClaimMissionBonusResultAction(msg.GarrMissionID, msg.AwardOvermax, msg.Result);
		}
	}

	private void MobileClientMissionAddedHandler(MobileClientMissionAdded msg)
	{
		if (msg.Result == 0 && msg.Mission.MissionRecID != 0)
		{
			PersistentMissionData.AddMission(msg.Mission);
		}
		else
		{
			GARRISON_RESULT result = (GARRISON_RESULT)msg.Result;
			Debug.Log(string.Concat(new object[]
			{
				"Error adding mission: ",
				result.ToString(),
				" Mission ID:",
				msg.Mission.MissionRecID
			}));
		}
		if (this.MissionAddedAction != null)
		{
			this.MissionAddedAction(msg.Mission.MissionRecID, msg.Result);
		}
	}

	private void MobileClientPongHandler(MobileClientPong msg)
	{
		Login.instance.PongReceived();
	}

	private void MobileClientFollowerChangedXPHandler(MobileClientFollowerChangedXP msg)
	{
		Debug.Log(string.Concat(new object[]
		{
			"MobileClientFollowerChangedXPHandler: follower ",
			msg.Follower.GarrFollowerID,
			" xp changed by ",
			msg.XpChange
		}));
		if (this.FollowerChangedXPAction != null)
		{
			this.FollowerChangedXPAction(msg.OldFollower, msg.Follower);
		}
	}

	private void MobileClientExpediteMissionCheatResultHandler(MobileClientExpediteMissionCheatResult msg)
	{
		if (msg.Result == 0)
		{
			Debug.Log("Expedited completion of mission " + msg.MissionRecID);
			MobilePlayerGarrisonDataRequest mobilePlayerGarrisonDataRequest = new MobilePlayerGarrisonDataRequest();
			mobilePlayerGarrisonDataRequest.GarrTypeID = 3;
			Login.instance.SendToMobileServer(mobilePlayerGarrisonDataRequest);
		}
		else
		{
			Debug.Log(string.Concat(new object[]
			{
				"MobileClientExpediteMissionCheatResult: Mission ID ",
				msg.MissionRecID,
				" failed with error ",
				msg.Result
			}));
		}
	}

	private void MobileClientAdvanceMissionSetResultHandler(MobileClientAdvanceMissionSetResult msg)
	{
		Debug.Log(string.Concat(new object[]
		{
			"Advance mission set ",
			msg.MissionSetID,
			" success: ",
			msg.Success
		}));
	}

	private void MobileClientChatHandler(MobileClientChat msg, int count)
	{
		if (msg.SlashCmd == 4)
		{
			this.m_chatPopup.OnReceiveText(msg.SenderName, msg.ChatText);
		}
		Debug.Log(string.Concat(new object[]
		{
			"Count: ",
			count,
			", Chat type ",
			(GuildChatSlider.SLASH_CMD)msg.SlashCmd,
			" from ",
			msg.SenderName,
			": ",
			msg.ChatText
		}));
	}

	private void MobileClientGuildMembersOnlineHandler(MobileClientGuildMembersOnline msg)
	{
		foreach (MobileGuildMember mobileGuildMember in msg.Members.AsEnumerable<MobileGuildMember>())
		{
			Debug.Log(string.Concat(new string[]
			{
				"Guild member ",
				mobileGuildMember.Name,
				" (",
				mobileGuildMember.Guid,
				") is online."
			}));
			GuildData.AddGuildMember(mobileGuildMember);
		}
		this.allPanels.adventureMapPanel.m_guildChatSlider_Bottom.UpdateGuildMateRoster();
	}

	private void MobileClientGuildMemberLoggedInHandler(MobileClientGuildMemberLoggedIn msg)
	{
		Debug.Log(string.Concat(new string[]
		{
			"Guild member ",
			msg.Member.Name,
			" (",
			msg.Member.Guid,
			") logged in."
		}));
		GuildData.AddGuildMember(msg.Member);
		this.allPanels.adventureMapPanel.m_guildChatSlider_Bottom.UpdateGuildMateRoster();
	}

	private void MobileClientGuildMemberLoggedOutHandler(MobileClientGuildMemberLoggedOut msg)
	{
		Debug.Log(string.Concat(new string[]
		{
			"Guild member ",
			msg.Member.Name,
			" (",
			msg.Member.Guid,
			") logged out."
		}));
		GuildData.RemoveGuildMember(msg.Member.Guid);
		this.allPanels.adventureMapPanel.m_guildChatSlider_Bottom.UpdateGuildMateRoster();
	}

	public void SetChatScript(GuildChatSlider script)
	{
		this.m_chatPopup = script;
	}

	public void SendGuildChat(string chatText)
	{
		MobilePlayerChat mobilePlayerChat = new MobilePlayerChat();
		mobilePlayerChat.SlashCmd = 4;
		mobilePlayerChat.ChatText = chatText;
		Login.instance.SendToMobileServer(mobilePlayerChat);
	}

	public void MobileRequestData()
	{
		MobilePlayerGarrisonDataRequest mobilePlayerGarrisonDataRequest = new MobilePlayerGarrisonDataRequest();
		mobilePlayerGarrisonDataRequest.GarrTypeID = 3;
		Login.instance.SendToMobileServer(mobilePlayerGarrisonDataRequest);
		MobilePlayerRequestShipmentTypes obj = new MobilePlayerRequestShipmentTypes();
		Login.instance.SendToMobileServer(obj);
		MobilePlayerRequestShipments obj2 = new MobilePlayerRequestShipments();
		Login.instance.SendToMobileServer(obj2);
		this.RequestWorldQuests();
		MobilePlayerFollowerEquipmentRequest mobilePlayerFollowerEquipmentRequest = new MobilePlayerFollowerEquipmentRequest();
		mobilePlayerFollowerEquipmentRequest.GarrFollowerTypeID = 4;
		Login.instance.SendToMobileServer(mobilePlayerFollowerEquipmentRequest);
		MobilePlayerFollowerArmamentsRequest mobilePlayerFollowerArmamentsRequest = new MobilePlayerFollowerArmamentsRequest();
		mobilePlayerFollowerArmamentsRequest.GarrFollowerTypeID = 4;
		Login.instance.SendToMobileServer(mobilePlayerFollowerArmamentsRequest);
		MobilePlayerWorldQuestBountiesRequest obj3 = new MobilePlayerWorldQuestBountiesRequest();
		Login.instance.SendToMobileServer(obj3);
		MobilePlayerFollowerActivationDataRequest mobilePlayerFollowerActivationDataRequest = new MobilePlayerFollowerActivationDataRequest();
		mobilePlayerFollowerActivationDataRequest.GarrTypeID = 3;
		Login.instance.SendToMobileServer(mobilePlayerFollowerActivationDataRequest);
		MobilePlayerGetArtifactInfo obj4 = new MobilePlayerGetArtifactInfo();
		Login.instance.SendToMobileServer(obj4);
	}

	private void UpdateDebugText()
	{
		this.m_frameCount++;
	}

	public void OnQuitButton()
	{
		Login.instance.BnQuit();
		Application.Quit();
	}

	private void OnDestroy()
	{
		Login.instance.MobileConnectDestroy();
	}

	private void ClearPendingNotifications()
	{
	}

	private void ScheduleNotifications()
	{
	}

	public void StartMission(int garrMissionID, ulong[] followerDBIDs)
	{
		Debug.Log("Main.StartMission() " + garrMissionID);
		MobilePlayerGarrisonStartMission mobilePlayerGarrisonStartMission = new MobilePlayerGarrisonStartMission();
		mobilePlayerGarrisonStartMission.GarrMissionID = garrMissionID;
		mobilePlayerGarrisonStartMission.FollowerDBIDs = followerDBIDs;
		Login.instance.SendToMobileServer(mobilePlayerGarrisonStartMission);
	}

	public void CompleteMission(int garrMissionID)
	{
		Debug.Log("Main.CompleteMission() " + garrMissionID);
		MobilePlayerGarrisonCompleteMission mobilePlayerGarrisonCompleteMission = new MobilePlayerGarrisonCompleteMission();
		mobilePlayerGarrisonCompleteMission.GarrMissionID = garrMissionID;
		Login.instance.SendToMobileServer(mobilePlayerGarrisonCompleteMission);
	}

	public void ClaimMissionBonus(int garrMissionID)
	{
		Debug.Log(string.Concat(new object[]
		{
			"Main.ClaimMissionBonus() ",
			garrMissionID,
			" State is: ",
			((JamGarrisonMobileMission)PersistentMissionData.missionDictionary[garrMissionID]).MissionState
		}));
		MobilePlayerClaimMissionBonus mobilePlayerClaimMissionBonus = new MobilePlayerClaimMissionBonus();
		mobilePlayerClaimMissionBonus.GarrMissionID = garrMissionID;
		Login.instance.SendToMobileServer(mobilePlayerClaimMissionBonus);
	}

	public void CompleteAllMissions()
	{
		Debug.Log("Main.CompleteAllMissions()");
		foreach (object obj in PersistentMissionData.missionDictionary.Values)
		{
			JamGarrisonMobileMission jamGarrisonMobileMission = (JamGarrisonMobileMission)obj;
			GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(jamGarrisonMobileMission.MissionRecID);
			if (record != null && record.GarrFollowerTypeID == 4u)
			{
				if (jamGarrisonMobileMission.MissionState == 1)
				{
					long num = GarrisonStatus.CurrentTime() - jamGarrisonMobileMission.StartTime;
					long num2 = jamGarrisonMobileMission.MissionDuration - num;
					if (num2 <= 0L)
					{
						this.CompleteMission(jamGarrisonMobileMission.MissionRecID);
					}
				}
			}
		}
	}

	public void ExpediteMissionCheat(int garrMissionID)
	{
		MobilePlayerExpediteMissionCheat mobilePlayerExpediteMissionCheat = new MobilePlayerExpediteMissionCheat();
		mobilePlayerExpediteMissionCheat.GarrMissionID = garrMissionID;
		Login.instance.SendToMobileServer(mobilePlayerExpediteMissionCheat);
	}

	public void AddMissionCheat(int garrMissionID)
	{
		MobilePlayerAddMissionCheat mobilePlayerAddMissionCheat = new MobilePlayerAddMissionCheat();
		mobilePlayerAddMissionCheat.GarrMissionID = garrMissionID;
		Login.instance.SendToMobileServer(mobilePlayerAddMissionCheat);
	}

	public void AdvanceMissionSetCheat()
	{
		MobilePlayerGarrisonAdvanceMissionSet mobilePlayerGarrisonAdvanceMissionSet = new MobilePlayerGarrisonAdvanceMissionSet();
		mobilePlayerGarrisonAdvanceMissionSet.GarrTypeID = 3;
		mobilePlayerGarrisonAdvanceMissionSet.MissionSetID = 0;
		Login.instance.SendToMobileServer(mobilePlayerGarrisonAdvanceMissionSet);
	}

	public void AllTheMissions()
	{
		StaticDB.garrMissionDB.EnumRecords(delegate(GarrMissionRec missionRec)
		{
			if (missionRec.GarrTypeID == 3u)
			{
				this.AddMissionCheat(missionRec.ID);
			}
			return true;
		});
	}

	public string GetLocale()
	{
		if (this.m_locale == null || this.m_locale == string.Empty)
		{
			this.m_locale = MobileDeviceLocale.GetBestGuessForLocale();
		}
		return this.m_locale;
	}

	public void RequestEmissaryFactions()
	{
		MobilePlayerRequestEmissaryFactions obj = new MobilePlayerRequestEmissaryFactions();
		Login.instance.SendToMobileServer(obj);
	}

	public void RequestWorldQuests()
	{
		if (AdventureMapPanel.instance == null)
		{
			return;
		}
		MobilePlayerRequestWorldQuests obj = new MobilePlayerRequestWorldQuests();
		Login.instance.SendToMobileServer(obj);
	}

	private void MobileClientEmissaryFactionUpdateHandler(MobileClientEmissaryFactionUpdate msg)
	{
		this.allPopups.EmissaryFactionUpdate(msg);
	}

	private void MobileClientCreateShipmentResultHandler(MobileClientCreateShipmentResult msg)
	{
		GARRISON_RESULT result = (GARRISON_RESULT)msg.Result;
		if (result != GARRISON_RESULT.SUCCESS)
		{
			AllPopups.instance.ShowGenericPopup(StaticDB.GetString("SHIPMENT_CREATION_FAILED", null), result.ToString());
		}
		if (this.CreateShipmentResultAction != null)
		{
			this.CreateShipmentResultAction(msg.Result);
		}
		if (result == GARRISON_RESULT.SUCCESS)
		{
			MobilePlayerGarrisonDataRequest mobilePlayerGarrisonDataRequest = new MobilePlayerGarrisonDataRequest();
			mobilePlayerGarrisonDataRequest.GarrTypeID = 3;
			Login.instance.SendToMobileServer(mobilePlayerGarrisonDataRequest);
		}
	}

	private void MobileClientShipmentTypesHandler(MobileClientShipmentTypes msg)
	{
		PersistentShipmentData.SetAvailableShipmentTypes(msg.Shipment);
		if (this.ShipmentTypesUpdatedAction != null)
		{
			this.ShipmentTypesUpdatedAction();
		}
	}

	private void MobileClientCompleteShipmentResultHandler(MobileClientCompleteShipmentResult msg)
	{
		SHIPMENT_RESULT result = (SHIPMENT_RESULT)msg.Result;
		if (this.CompleteShipmentResultAction != null)
		{
			this.CompleteShipmentResultAction(result, msg.ShipmentID);
		}
	}

	private void MobileClientShipmentsUpdateHandler(MobileClientShipmentsUpdate msg)
	{
		PersistentShipmentData.ClearData();
		foreach (JamCharacterShipment jamCharacterShipment in msg.Shipment)
		{
			PersistentShipmentData.AddOrUpdateShipment(jamCharacterShipment);
			if (this.ShipmentAddedAction != null)
			{
				this.ShipmentAddedAction(jamCharacterShipment.ShipmentRecID, jamCharacterShipment.ShipmentID);
			}
		}
	}

	private void MobileClientWorldQuestUpdateHandler(MobileClientWorldQuestUpdate msg)
	{
		WorldQuestData.ClearData();
		foreach (MobileWorldQuest mobileWorldQuest in msg.Quest)
		{
			if (mobileWorldQuest.StartLocationMapID == 1220)
			{
				WorldQuestData.AddWorldQuest(mobileWorldQuest);
			}
		}
		this.allPanels.adventureMapPanel.UpdateWorldQuests();
	}

	private void MobileClientEvaluateMissionResultHandler(MobileClientEvaluateMissionResult msg)
	{
		if (msg.Result == 0)
		{
			MissionDataCache.AddOrUpdateMissionData(msg.GarrMissionID, msg.SuccessChance);
			if (this.MissionSuccessChanceChangedAction != null)
			{
				this.MissionSuccessChanceChangedAction(msg.SuccessChance);
			}
		}
		else
		{
			GARRISON_RESULT result = (GARRISON_RESULT)msg.Result;
			Debug.Log("MobileClientEvaluateMissionResult failed with error " + result.ToString());
		}
	}

	public void FastMissionsCheat()
	{
		MobilePlayerSetMissionDurationCheat mobilePlayerSetMissionDurationCheat = new MobilePlayerSetMissionDurationCheat();
		mobilePlayerSetMissionDurationCheat.Seconds = 10;
		Login.instance.SendToMobileServer(mobilePlayerSetMissionDurationCheat);
	}

	public void FastShipmentsCheat()
	{
		MobilePlayerSetShipmentDurationCheat mobilePlayerSetShipmentDurationCheat = new MobilePlayerSetShipmentDurationCheat();
		mobilePlayerSetShipmentDurationCheat.Seconds = 10;
		Login.instance.SendToMobileServer(mobilePlayerSetShipmentDurationCheat);
	}

	private void MobileClientSetShipmentDurationCheatResultHandler(MobileClientSetShipmentDurationCheatResult msg)
	{
		AllPopups.instance.HideAllPopups();
	}

	private void MobileClientSetMissionDurationCheatResultHandler(MobileClientSetMissionDurationCheatResult msg)
	{
		AllPopups.instance.HideAllPopups();
	}

	private void MobileClientCanResearchGarrisonTalentResultHandler(MobileClientCanResearchGarrisonTalentResult msg)
	{
		if (this.CanResearchGarrisonTalentResultAction != null)
		{
			this.CanResearchGarrisonTalentResultAction(msg.GarrTalentID, msg.Result, msg.ConditionText);
		}
	}

	private void MobileClientResearchGarrisonTalentResultHandler(MobileClientResearchGarrisonTalentResult msg)
	{
		GARRISON_RESULT result = (GARRISON_RESULT)msg.Result;
		if (result != GARRISON_RESULT.SUCCESS)
		{
			AllPopups.instance.ShowGenericPopup(StaticDB.GetString("TALENT_RESEARCH_FAILED", null), result.ToString());
		}
		if (this.ResearchGarrisonTalentResultAction != null)
		{
			this.ResearchGarrisonTalentResultAction(msg.GarrTalentID, msg.Result);
		}
	}

	private void MobileClientFollowerEquipmentResultHandler(MobileClientFollowerEquipmentResult msg)
	{
		PersistentEquipmentData.ClearData();
		uint num = 0u;
		while ((ulong)num < (ulong)((long)msg.Equipment.Length))
		{
			PersistentEquipmentData.AddOrUpdateEquipment(msg.Equipment[(int)((UIntPtr)num)]);
			num += 1u;
		}
		if (this.EquipmentInventoryChangedAction != null)
		{
			this.EquipmentInventoryChangedAction();
		}
	}

	private void MobileClientFollowerChangedQualityHandler(MobileClientFollowerChangedQuality msg)
	{
		PersistentFollowerData.AddOrUpdateFollower(msg.Follower);
		if (this.UseEquipmentResultAction != null)
		{
			this.UseEquipmentResultAction(msg.OldFollower, msg.Follower);
		}
		MobilePlayerFollowerEquipmentRequest mobilePlayerFollowerEquipmentRequest = new MobilePlayerFollowerEquipmentRequest();
		mobilePlayerFollowerEquipmentRequest.GarrFollowerTypeID = 4;
		Login.instance.SendToMobileServer(mobilePlayerFollowerEquipmentRequest);
	}

	private void MobileClientFollowerArmamentsResultHandler(MobileClientFollowerArmamentsResult msg)
	{
		PersistentArmamentData.ClearData();
		uint num = 0u;
		while ((ulong)num < (ulong)((long)msg.Armament.Length))
		{
			PersistentArmamentData.AddOrUpdateArmament(msg.Armament[(int)((UIntPtr)num)]);
			num += 1u;
		}
		if (this.ArmamentInventoryChangedAction != null)
		{
			this.ArmamentInventoryChangedAction();
		}
	}

	private void MobileClientUseFollowerArmamentResultHandler(MobileClientUseFollowerArmamentResult msg)
	{
		GARRISON_RESULT result = (GARRISON_RESULT)msg.Result;
		if (result == GARRISON_RESULT.SUCCESS)
		{
			PersistentFollowerData.AddOrUpdateFollower(msg.Follower);
			MobilePlayerFollowerArmamentsRequest mobilePlayerFollowerArmamentsRequest = new MobilePlayerFollowerArmamentsRequest();
			mobilePlayerFollowerArmamentsRequest.GarrFollowerTypeID = 4;
			Login.instance.SendToMobileServer(mobilePlayerFollowerArmamentsRequest);
		}
		else
		{
			AllPopups.instance.ShowGenericPopup(StaticDB.GetString("USE_ARMAMENT_FAILED", null), result.ToString());
		}
		if (this.UseArmamentResultAction != null)
		{
			this.UseArmamentResultAction(msg.Result, msg.OldFollower, msg.Follower);
		}
	}

	private void MobileClientWorldQuestBountiesResultHandler(MobileClientWorldQuestBountiesResult msg)
	{
		PersistentBountyData.ClearData();
		PersistentBountyData.SetBountiesVisible(msg.Visible);
		if (msg.Visible)
		{
		}
		if (msg.LockedQuestID != 0)
		{
		}
		uint num = 0u;
		while ((ulong)num < (ulong)((long)msg.Bounty.Length))
		{
			PersistentBountyData.AddOrUpdateBounty(msg.Bounty[(int)((UIntPtr)num)]);
			num += 1u;
		}
		if (this.BountyInfoUpdatedAction != null)
		{
			this.BountyInfoUpdatedAction();
		}
	}

	private void MobileClientFollowerActivationDataResultHandler(MobileClientFollowerActivationDataResult msg)
	{
		GarrisonStatus.SetFollowerActivationInfo(msg.ActivationsRemaining, msg.GoldCost);
	}

	private void MobileClientChangeFollowerActiveResultHandler(MobileClientChangeFollowerActiveResult msg)
	{
		GARRISON_RESULT result = (GARRISON_RESULT)msg.Result;
		if (result == GARRISON_RESULT.SUCCESS)
		{
			PersistentFollowerData.AddOrUpdateFollower(msg.Follower);
			FollowerStatus followerStatus = GeneralHelpers.GetFollowerStatus(msg.Follower);
			if (followerStatus == FollowerStatus.inactive)
			{
				Debug.Log("Follower is now inactive. " + msg.ActivationsRemaining + " activations remain for the day.");
			}
			else
			{
				Debug.Log("Follower is now active. " + msg.ActivationsRemaining + " activations remain for the day.");
			}
			if (this.FollowerDataChangedAction != null)
			{
				this.FollowerDataChangedAction();
			}
			MobilePlayerFollowerActivationDataRequest mobilePlayerFollowerActivationDataRequest = new MobilePlayerFollowerActivationDataRequest();
			mobilePlayerFollowerActivationDataRequest.GarrTypeID = 3;
			Login.instance.SendToMobileServer(mobilePlayerFollowerActivationDataRequest);
		}
		else
		{
			Debug.Log("Follower activation/deactivation failed for reason " + result.ToString());
		}
	}

	private void MobileClientGetItemTooltipInfoResultHandler(MobileClientGetItemTooltipInfoResult msg)
	{
		ItemStatCache.instance.AddMobileItemStats(msg.ItemID, msg.ItemContext, msg.Stats);
	}

	private void MobileClientAuthChallengeHandler(MobileClientAuthChallenge msg)
	{
		Login.instance.MobileAuthChallengeReceived(msg.ServerChallenge);
	}

	private void MobileClientArtifactInfoResultHandler(MobileClientArtifactInfoResult msg)
	{
		GarrisonStatus.ArtifactKnowledgeLevel = msg.KnowledgeLevel;
		GarrisonStatus.ArtifactXpMultiplier = msg.XpMultiplier;
	}

	public void UseEquipment(int garrFollowerID, int itemID, int replaceThisAbilityID)
	{
		if (this.UseEquipmentStartAction != null)
		{
			this.UseEquipmentStartAction(replaceThisAbilityID);
		}
		GarrFollowerRec record = StaticDB.garrFollowerDB.GetRecord(garrFollowerID);
		MobilePlayerUseFollowerEquipment mobilePlayerUseFollowerEquipment = new MobilePlayerUseFollowerEquipment();
		mobilePlayerUseFollowerEquipment.GarrFollowerID = garrFollowerID;
		mobilePlayerUseFollowerEquipment.GarrFollowerTypeID = (int)record.GarrFollowerTypeID;
		mobilePlayerUseFollowerEquipment.ItemID = itemID;
		mobilePlayerUseFollowerEquipment.ReplaceAbilityID = replaceThisAbilityID;
		Login.instance.SendToMobileServer(mobilePlayerUseFollowerEquipment);
	}

	public void UseArmament(int garrFollowerID, int itemID)
	{
		if (this.UseArmamentStartAction != null)
		{
			this.UseArmamentStartAction(garrFollowerID);
		}
		GarrFollowerRec record = StaticDB.garrFollowerDB.GetRecord(garrFollowerID);
		MobilePlayerUseFollowerArmament mobilePlayerUseFollowerArmament = new MobilePlayerUseFollowerArmament();
		mobilePlayerUseFollowerArmament.GarrFollowerID = garrFollowerID;
		mobilePlayerUseFollowerArmament.GarrFollowerTypeID = (int)record.GarrFollowerTypeID;
		mobilePlayerUseFollowerArmament.ItemID = itemID;
		Login.instance.SendToMobileServer(mobilePlayerUseFollowerArmament);
	}

	public void SetDebugText(string newText)
	{
		this.m_debugText.text = newText;
		this.m_debugButton.SetActive(true);
	}

	public void HideDebugText()
	{
		this.m_debugButton.SetActive(false);
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			this.ScheduleNotifications();
		}
		else
		{
			this.ClearPendingNotifications();
		}
	}

	public void Logout()
	{
		MissionDataCache.ClearData();
		if (this.StartLogOutAction != null)
		{
			this.StartLogOutAction();
		}
		AllPopups.instance.HideAllPopups();
		AllPanels.instance.ShowOrderHallMultiPanel(false);
		Login.instance.ReconnectToMobileServerCharacterSelect();
	}

	public void SelectOrderHallNavButton(OrderHallNavButton navButton)
	{
		if (this.OrderHallNavButtonSelectedAction != null)
		{
			this.OrderHallNavButtonSelectedAction(navButton);
		}
	}

	public void SelectOrderHallFilterOptionsButton(OrderHallFilterOptionsButton filterOptionsButton)
	{
		if (this.OrderHallfilterOptionsButtonSelectedAction != null)
		{
			this.OrderHallfilterOptionsButtonSelectedAction(filterOptionsButton);
		}
	}

	private static string getMd5Hash(string input)
	{
		if (input == string.Empty)
		{
			return string.Empty;
		}
		MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] array = md5CryptoServiceProvider.ComputeHash(Encoding.Default.GetBytes(input));
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	private void GenerateUniqueIdentifier()
	{
		bool flag = false;
		string text = string.Empty;
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.content.Context");
		string static2 = androidJavaClass2.GetStatic<string>("TELEPHONY_SERVICE");
		AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getSystemService", new object[]
		{
			static2
		});
		bool flag2 = false;
		try
		{
			text = androidJavaObject.Call<string>("getDeviceId", new object[0]);
		}
		catch (Exception ex)
		{
			Debug.Log(ex.ToString());
			flag2 = true;
		}
		if (text == null)
		{
			text = string.Empty;
		}
		if ((flag2 && !flag) || (!flag2 && text == string.Empty && flag))
		{
			AndroidJavaClass androidJavaClass3 = new AndroidJavaClass("android.provider.Settings$Secure");
			string static3 = androidJavaClass3.GetStatic<string>("ANDROID_ID");
			AndroidJavaObject androidJavaObject2 = @static.Call<AndroidJavaObject>("getContentResolver", new object[0]);
			text = androidJavaClass3.CallStatic<string>("getString", new object[]
			{
				androidJavaObject2,
				static3
			});
			if (text == null)
			{
				text = string.Empty;
			}
		}
		if (text == string.Empty)
		{
			string text2 = "00000000000000000000000000000000";
			try
			{
				StreamReader streamReader = new StreamReader("/sys/class/net/wlan0/address");
				text2 = streamReader.ReadLine();
				streamReader.Close();
			}
			catch (Exception ex2)
			{
				Debug.Log(ex2.ToString());
			}
			text = text2.Replace(":", string.Empty);
		}
		Main.uniqueIdentifier = Main.getMd5Hash(text);
	}

	public void CheatFastForwardOneHour()
	{
		GarrisonStatus.CheatFastForwardOneHour();
	}

	private int m_frameCount;

	private Queue<Main.MobileMessage> m_messageQueue = new Queue<Main.MobileMessage>();

	public Canvas mainCanvas;

	private Animator canvasAnimator;

	public AllPanels allPanels;

	public AllPopups allPopups;

	private string m_unknownMsg;

	public UISound m_UISound;

	public static Main instance;

	private GuildChatSlider m_chatPopup;

	public Action FollowerDataChangedAction;

	public Action<int> MissionSuccessChanceChangedAction;

	public Action GarrisonDataResetStartedAction;

	public Action GarrisonDataResetFinishedAction;

	public Action ShipmentTypesUpdatedAction;

	public Action<int> CreateShipmentResultAction;

	public Action<int, ulong> ShipmentAddedAction;

	public Action<SHIPMENT_RESULT, ulong> CompleteShipmentResultAction;

	public Action<int, int, int> CompleteMissionResultAction;

	public Action<int, bool, int> ClaimMissionBonusResultAction;

	public Action<int, int> MissionAddedAction;

	public Action<JamGarrisonFollower, JamGarrisonFollower> FollowerChangedXPAction;

	public Action<JamGarrisonFollower> TroopExpiredAction;

	public Action<int, int, string> CanResearchGarrisonTalentResultAction;

	public Action<int, int> ResearchGarrisonTalentResultAction;

	public Action BountyInfoUpdatedAction;

	public Action<int> UseEquipmentStartAction;

	public Action<JamGarrisonFollower, JamGarrisonFollower> UseEquipmentResultAction;

	public Action EquipmentInventoryChangedAction;

	public Action<int> UseArmamentStartAction;

	public Action<int, JamGarrisonFollower, JamGarrisonFollower> UseArmamentResultAction;

	public Action ArmamentInventoryChangedAction;

	public Action<OrderHallNavButton> OrderHallNavButtonSelectedAction;

	public Action<OrderHallFilterOptionsButton> OrderHallfilterOptionsButtonSelectedAction;

	public Action StartLogOutAction;

	public GameObject m_debugButton;

	public Text m_debugText;

	private string m_locale;

	public GameClient m_gameClientScript;

	public static string uniqueIdentifier;

	public CanvasBlurManager m_canvasBlurManager;

	public BackButtonManager m_backButtonManager;

	private class MobileMessage
	{
		public object msg;

		public MobileNetwork.MobileNetworkEventArgs args;
	}

	public interface SetCache
	{
		void SetCachePath(string cachePath);
	}
}