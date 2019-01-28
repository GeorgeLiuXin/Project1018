using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace XWorld
{
    public class ActorManager : XWorldGameManagerBase
    {
  //      private Dictionary<int, GalaxyActor> ServerIDActorManager;
  //      private Dictionary<int, GalaxyActor> ClientIDActorManager;
  //      private Dictionary<int, LocalPlayer> LocalPlayerList;
  //      private Dictionary<int, float> DelayRemoveList;

  //      // Add by Gavon : 在迭代器中删除元素导致奔溃，先记录，退出迭代器后再删除
  //      private List<int> RemoveAvatarList = new List<int>();
  //      public int LocalPlayerServerID{get;set;}
  //      private LocalPlayer m_LocalPlayer;
  //      private GPacketDefineManager m_packetDefineMgr;

  //      //private HitUIParams hitUIParams;

  //      private UIManager m_uiMgr;
  //      protected UIManager UIMgr
  //      {
  //          get
  //          {
  //              if (m_uiMgr == null)
  //              {
  //                  m_uiMgr = GalaxyGameModule.GetGameManager<UIManager>();
  //              }
  //              return m_uiMgr;
  //          }
  //      }

  //      public override void InitManager()
  //      {
  //          ServerIDActorManager = new Dictionary<int, GalaxyActor>();
  //          ClientIDActorManager = new Dictionary<int, GalaxyActor>();
  //          LocalPlayerList = new Dictionary<int, LocalPlayer>();
  //          DelayRemoveList = new Dictionary<int, float>();
  //          m_packetDefineMgr = GalaxyGameModule.GetGameManager<GPacketDefineManager>();

  //          //hitUIParams = new HitUIParamsBase();

  //          RegisterEvents();
  //      }

  //      public override void Update(float fElapseTimes)
  //      {
  //          List<GalaxyActor> actorList = new List<GalaxyActor>(ServerIDActorManager.Values);

  //          for (int i = 0; i < actorList.Count; i++)
  //          {
  //              actorList[i].Tick(fElapseTimes);
  //          }

  //          foreach (var key in new List<int>(DelayRemoveList.Keys))
  //          {
  //              DelayRemoveList[key] -= fElapseTimes;
  //              if (DelayRemoveList[key] <= 0.0f)
  //              {
  //                  GalaxyActor act = GetByServerID(key);
  //                  if (act != null)
  //                  {
  //                      RemoveActor(act);
  //                      DelayRemoveList.Remove(key);
  //                  }
  //              }
  //          }
  //      }

  //      public override void ShutDown()
  //      {
  //          //hitUIParams = null;
  //          ServerIDActorManager.Clear();
  //          ClientIDActorManager.Clear();
  //          LocalPlayerList.Clear();
  //          UnRegisterEvents();
  //      }

  //      private void RegisterEvents()
  //      {
  //          EventListener.Instance.AddListener(CltEvent.NetWork_LOGIN.CONNECT_SUCC, OnLoginConnectSuc);
  //          EventListener.Instance.AddListener(CltEvent.NetWork_LOGIN.INIT_AVATAR_RES, OnInitAvatarRes);
  //          EventListener.Instance.AddListener(CltEvent.NetWork_LOGIN.PLAYER_USER_DATA, OnPlayerUserData);
  //          EventListener.Instance.AddListener(CltEvent.NetWork_LOGIN.RECONNECT_INIT_AVATAR, OnReconnectInitAvatar);
            
  //          EventListener.Instance.AddListener(CltEvent.NetWork_LOGIN.UPDATE_AVATAR_RES, OnUpdateAvatarRes);
  //      }

  //      private void UnRegisterEvents()
  //      {
  //          EventListener.Instance.RemoveListener(CltEvent.NetWork_LOGIN.CONNECT_SUCC, OnLoginConnectSuc);
  //          EventListener.Instance.RemoveListener(CltEvent.NetWork_LOGIN.INIT_AVATAR_RES, OnInitAvatarRes);
  //          EventListener.Instance.RemoveListener(CltEvent.NetWork_LOGIN.PLAYER_USER_DATA, OnPlayerUserData);
  //          EventListener.Instance.RemoveListener(CltEvent.NetWork_LOGIN.RECONNECT_INIT_AVATAR, OnReconnectInitAvatar);
  //          EventListener.Instance.RemoveListener(CltEvent.NetWork_LOGIN.UPDATE_AVATAR_RES, OnUpdateAvatarRes);
  //      }

  //      private void OnLoginConnectSuc(object[] values)
  //      {
  //          GPacketBase pkt = values[0] as GPacketBase;
  //          if (pkt == null)
  //              return;

  //          this.LocalPlayerServerID = pkt.mAvatarID;
  //      }

  //      private void OnInitAvatarRes(object[] values)
  //      {
  //          GPacketBase pkt = values[0] as GPacketBase;
  //          if (pkt == null)
  //              return;

  //          InitAvatarData(pkt);
  //      }

  //      private void OnUpdateAvatarRes(object[] values)
  //      {
  //          GPacketBase pkt = values[0] as GPacketBase;
  //          if (pkt == null)
  //              return;

  //          UpdateAvatarData(pkt);
  //      }

  //      private void OnPlayerUserData(object[] values)
  //      {
  //          GPacketBase pkt = values[0] as GPacketBase;
  //          if (pkt == null)
  //              return;

  //          PlayerUserData(pkt);
  //      }

  //      private void OnReconnectInitAvatar(object[] values)
  //      {
  //          GPacketBase pkt = values[0] as GPacketBase;
  //          if (pkt == null)
  //              return;

  //          ReconnectInitAvatar(pkt);
  //      }

  //      public void OnDeleteRoleFinish(object[] values)
  //      {
  //          GPacketBase pkt = values[0] as GPacketBase;
  //          if (pkt == null)
  //              return;
  //          int flag = pkt.GetInt32("nflag");
  //          if (flag != 0)//0==Delete_Avatar_Sucess
  //              return;
  //          Int64 avatarDID = pkt.GetInt64("avatarDID");
  //          foreach (var player in LocalPlayerList)
  //          {
  //              if (player.Value.Pool.GetInt64("avatardid") == avatarDID)
  //              {
  //                  LocalPlayerList.Remove(player.Key);
  //                  break;
  //              }
  //          }
  //      }

  //      public void OnFSMState(GPacketBase pkt)
  //      {
  //          if (pkt == null)
  //              return;

  //          ActorObj act = GetByServerID(pkt.GetInt32("StateAvatarID")) as ActorObj;

  //          if (act == null)
  //              return;

  //          eState StateID = (eState)pkt.GetByte("StateID");
  //          if (act.fsmCom == null)
  //          {
  //              if (StateID == eState.State_Born && act.m_bIsControllByLocal)
  //              {
  //                  StateParamBase paramTmp = FSMManager.Instance.MakeParam((int)StateID);
  //                  if (paramTmp != null)
  //                  {
  //                      byte[] bufferTmp = new byte[pkt.mBuffSize];
  //                      pkt.ReadBuffer(bufferTmp, pkt.mBuffSize);
  //                      int offsetTmp = 0;
  //                      paramTmp.SerialParam(bufferTmp, ref offsetTmp, false);

  //                      FSMParam_Born bornParam = (FSMParam_Born)paramTmp;
  //                      act.SetBornPos(bornParam.vCurPos);
  //                      act.SetBornDir(bornParam.vCurDir);

  //                  }
  //              }
  //              return;
  //          }
  //          bool bForce = true;
  //          if (act.m_bIsControllByLocal)
  //          {
  //              bForce = false;
  //              if (act.fsmCom.IsActiveState(StateID))
  //              {
  //                  return;
  //              }
  //          }

  //          StateParamBase param = act.fsmCom.MakeParam(StateID);

  //          if (param == null)
  //              return;

  //          byte[] buffer = new byte[pkt.mBuffSize];
  //          pkt.ReadBuffer(buffer, pkt.mBuffSize);
  //          int offset = 0;
  //          param.SerialParam(buffer, ref offset, false);

  //          eStateMach eMach = (eStateMach)pkt.GetInt32("StateMach");
  //          act.SetTargetState(param, eMach, bForce);
  //      }

  //      public void OnFSMValue(GPacketBase pkt)
  //      {
  //          if (pkt == null)
  //              return;

  //          ActorObj act = GetByServerID(pkt.GetInt32("StateAvatarID")) as ActorObj;
  //          if (null == act || act.m_bIsControllByLocal)
  //              return;

  //          if (act.fsmCom)
  //          {
  //              act.fsmCom.UpdateFSMValue(pkt);
  //          }
  //      }

  //      public void OnFSMSkillMove(GPacketBase pkt)
  //      {
  //          if (pkt == null)
  //              return;

  //          ActorObj act = GetByServerID(pkt.GetInt32("StateAvatarID")) as ActorObj;
  //          if (null == act || act.m_bIsControllByLocal)
  //              return;

  //          if (act.fsmCom)
  //          {
  //              act.fsmCom.UpdateFSMSkillMove(pkt);
  //          }
  //      }

  //      public void OnSkillSpell(GPacketBase pkt)
  //      {
  //          if (pkt == null)
  //              return;

  //          ActorObj pAvatar = GetByServerID(pkt.mAvatarID) as ActorObj;
  //          if (pAvatar == null || pAvatar.skillCom == null)
  //              return;

  //          GTargetInfo sTarInfo = new GTargetInfo();
  //          sTarInfo.m_nTargetID = pkt.GetInt32("SkillTarget");
  //          sTarInfo.m_vSrcPos = new Vector3(pkt.GetInt16("x")/10.0f, pkt.GetInt16("z") / 10.0f, pkt.GetInt16("y") / 10.0f);
  //          sTarInfo.m_vTarPos = new Vector3(pkt.GetInt16("tarPosX") / 10.0f, pkt.GetInt16("tarPosZ") / 10.0f, pkt.GetInt16("tarPosY") / 10.0f);
  //          sTarInfo.m_vAimDir = new Vector3(pkt.GetInt16("aimDirX") / 10000.0f, pkt.GetInt16("aimDirZ") / 10000.0f, pkt.GetInt16("aimDirY") / 10000.0f);
  //          sTarInfo.m_vMoveTarPos = new Vector3(pkt.GetInt16("moveTarPosX") / 10.0f, pkt.GetInt16("moveTarPosZ") / 10.0f, pkt.GetInt16("moveTarPosY") / 10.0f);
  //          pAvatar.skillCom.OnSpellSkill(pkt.GetInt32("SkillID"), pkt.GetInt32("SkillSlots"), ref sTarInfo);
  //      }

  //      public void OnServerSkillSpell(GPacketBase pkt)
  //      {
  //          if (pkt == null)
  //              return;

  //          ActorObj pAvatar = GetByServerID(pkt.mAvatarID) as ActorObj;
  //          if (pAvatar == null)
  //              return;

  //          GTargetInfo sTarInfo = new GTargetInfo();
  //          sTarInfo.m_nTargetID = pkt.GetInt32("SkillTarget");
  //          sTarInfo.m_vSrcPos = new Vector3(pkt.GetInt16("x") / 10.0f, pkt.GetInt16("z") / 10.0f, pkt.GetInt16("y") / 10.0f);
  //          sTarInfo.m_vTarPos = new Vector3(pkt.GetInt16("tarPosX") / 10.0f, pkt.GetInt16("tarPosZ") / 10.0f, pkt.GetInt16("tarPosY") / 10.0f);
  //          sTarInfo.m_vAimDir = new Vector3(pkt.GetInt16("aimDirX") / 10000.0f, pkt.GetInt16("aimDirZ") / 10000.0f, pkt.GetInt16("aimDirY") / 10000.0f);
  //          pAvatar.skillCom.OnServerSpellSkill(pkt.GetInt32("SkillID"), pkt.GetInt32("SkillSlots"), ref sTarInfo);
  //      }

  //      public void OnSkillCast(GPacketBase pkt)
  //      {
  //          if (pkt == null)
  //              return;

  //          ActorObj pAvatar = GetByServerID(pkt.mAvatarID) as ActorObj;
  //          if (pAvatar == null)
  //              return;

  //          if (pAvatar == GetLocalPlayer())
  //              return;

  //          pAvatar.skillCom.CastSkill(pkt.GetInt32("SkillID"));
  //      }

  //      private void DoShowHit(ActorObj pAttackAvatar, ActorObj pTarAvatar, int nSkillID, GPacketBase pkt)
  //      {
  //          if (null == pAttackAvatar || null == pTarAvatar)
  //              return;
  //          //特效
  //          if (pTarAvatar.GetEngineObj() != null && pTarAvatar.CombatCom != null && pAttackAvatar.GetEngineObj() != null)
  //          {
  //              GSkillData skilldata = GalaxyGameModule.GetGameManager<GModifyDataManager>().GetSkillData(nSkillID);
  //              if (skilldata == null)
  //                  return;
  //              if (pkt.GetInt32("effectValue") == 0)
  //                  return;

  //              pTarAvatar.CombatCom.HitEffectLogic(skilldata, pAttackAvatar.ServerID, pTarAvatar.ServerID, false);
  //          }
  //      }

  //      private void DoUpdateUIBlood(ActorObj pAttackAvatar, ActorObj pTarAvatar, GPacketBase pkt)
  //      {
  //          if (pkt == null)
  //              return;
  //          if (pTarAvatar == null || pAttackAvatar == null)
  //              return;
  //          if (pTarAvatar != GetLocalPlayer() && pAttackAvatar != GetLocalPlayer())
  //              return;
  //          int skillID = pkt.GetInt32("skillID");
  //          int notifyType = pkt.GetInt32("notifyType");
  //          int effectType = pkt.GetInt32("effectType");
            
  //          //假伤害帧
  //          if ((effectType & (int)eTriggerNotify.TriggerNotify_Damage) != 0)
  //          {
  //              int damageValue = pkt.GetInt32("effectValue");
  //              if (damageValue != 0 && pTarAvatar.GetEngineObj() != null && pAttackAvatar.GetEngineObj() != null)
  //              {
  //                  HitUIParamsBase hitEvent = GalaxyGameModule.GetGameManager<HitUIParamManager>()
  //                      .GetHitUIParamsByType((eTriggerNotifyType)notifyType); //new HitUIParams();
  //                  int direction = 1;
  //                  Vector3 posTar = Camera.main.WorldToViewportPoint(pTarAvatar.GetEngineObj().transform.position);
  //                  Vector3 posAttack = Camera.main.WorldToViewportPoint(pAttackAvatar.GetEngineObj().transform.position);

  //                  direction = (posTar.x > posAttack.x ? 1 : -1);
  //                  hitEvent.SetDir(direction);
  //                  hitEvent.SetTarget(pTarAvatar);
  //                  hitEvent.SetShowValue(damageValue);
  //                  hitEvent.SetParamsBySkillEffect(effectType);

  //                  GSkillData pSkilldata = GalaxyGameModule.GetGameManager<GModifyDataManager>().GetSkillData(skillID);
  //                  if (pSkilldata == null)
  //                      return;
  //                  RoutineRunner.instance.StartCoroutine(ShowHitDamage(hitEvent, pSkilldata.MSV_CombatPerformanceTimes));
  //              }
  //          }

  //          if ((effectType & (int)eTriggerNotify.TriggerNotify_Heal) != 0)
  //          {
  //              HitUIParamsBase hitUIParams = GalaxyGameModule.GetGameManager<HitUIParamManager>()
  //                  .GetHitUIParamsByType(eTriggerNotifyType.NotifyType_Heal);
  //              if (pTarAvatar.Pool != null 
  //                  // 满血不显示恢复数字
  //                  && pTarAvatar.Pool.GetFloat("hp", 100) != pTarAvatar.Pool.GetFloat("hpmax", 100)
  //                  && pTarAvatar.GetEngineObj() != null)
  //              {
  //                  hitUIParams.Reset();
  //                  hitUIParams.SetShowValue(pkt.GetInt32("effectValue"));
  //                  hitUIParams.SetTarget(pTarAvatar);
  //                  hitUIParams.SetParamsBySkillEffect(effectType);
                    
  //                  UIMgr.Form3dManager.AddView(EFormGroupType.HUB_TEXT, pTarAvatar.ClientID, hitUIParams);
  //              }
  //              GalaxyGameModule.GetGameManager<HitUIParamManager>().ReturnToPool(hitUIParams);
  //          }

  //          #region 刷UI血条
  //          {
  //              EventListener.Instance.Dispatch(CltEvent.UI.UI_BLOOD, pTarAvatar);
  //          }
  //          #endregion
  //      }

  //      #region 多段假伤害帧飘字

  //      private IEnumerator ShowHitDamage(HitUIParamsBase hitEvent, int times = 0)
  //      {
  //          int nEffectTimes = times;
  //          if (nEffectTimes != 0)
  //          {
  //              object objDamage = hitEvent.ShowValue;
  //              if (objDamage is int || objDamage is float)
  //              {
  //                  int nDamage = (int)objDamage;
  //                  List<int> list = new List<int>();
  //                  WaveDamageNum(nDamage, nEffectTimes, ref list);

  //                  while (nEffectTimes > 0)
  //                  {
  //                      int index = nEffectTimes - 1;
  //                      if (index > list.Count || index < 0)
  //                      {
  //                          GameLogger.DebugLog(LOG_CHANNEL.COMBAT, "WaveDamageNum calculate error");
  //                          hitEvent.SetShowValue(nDamage / nEffectTimes);
  //                      }
  //                      else
  //                      {
  //                          hitEvent.SetShowValue(list[index]);
  //                      }
  //                      hitEvent.GenRandomXY();
  //                      UIMgr.Form3dManager.AddView(EFormGroupType.HUB_TEXT, hitEvent.ClientID, hitEvent);
  //                      nEffectTimes--;
  //                      yield return new WaitForSeconds(0.1f);
  //                  }
  //              }
  //          }
  //          GalaxyGameModule.GetGameManager<HitUIParamManager>().ReturnToPool(hitEvent);
  //      }

  //      //数值浮动效果
  //      private void WaveDamageNum(int number, int times, ref List<int> list)
  //      {
  //          int average = number / times;
  //          int overplus = 0;
  //          int num = 0;
  //          while (times > 0)
  //          {
  //              if (times == 1)
  //              {
  //                  num = average + overplus;
  //                  if (num < 1)
  //                      num = 1;

  //                  list.Add(num);
  //                  times--;
  //                  break;
  //              }
  //              num = Mathf.RoundToInt(Random.Range(average * 0.9f, average * 1.1f));
  //              list.Add(num);
  //              overplus = num - average;
  //              times--;
  //          }
  //      }

  //      #endregion

  //      public void OnSkillBreak(GPacketBase pkt)
  //      {
  //          ActorObj pAvatar = GetByServerID(pkt.mAvatarID) as ActorObj;
  //          if (pAvatar == null)
  //              return;
  //          bool bServer = pkt.GetBool("bServer");
  //          if (!bServer)
  //              return;
  //          HitUIParamsBase uiParams = GalaxyGameModule.GetGameManager<HitUIParamManager>()
  //              .GetHitUIParamsByType(eTriggerNotifyType.NotifyType_Skill);
  //          int direction = 0;
  //          uiParams.SetDir(direction);
  //          uiParams.SetTarget(pAvatar);
  //          uiParams.SetParamsBySkillEffect((int)eTriggerNotify.TriggerNotify_SkillBreak);
  //          UIMgr.Form3dManager.AddView(EFormGroupType.HUB_TEXT, uiParams.ClientID, uiParams);
  //          GalaxyGameModule.GetGameManager<HitUIParamManager>().ReturnToPool(uiParams);
  //      }

  //      public void OnCombatEffect(GPacketBase pkt)
  //      {
  //          ActorObj pAvatar = GetByServerID(pkt.mAvatarID) as ActorObj;
  //          if (pAvatar == null)
  //              return;
  //          int effectType = pkt.GetInt32("effectType");
  //          int effectFlag = pkt.GetInt32("effectFlag");
  //          HitUIParamsBase uiParams = GalaxyGameModule.GetGameManager<HitUIParamManager>()
  //              .GetHitUIParamsByType((eTriggerNotifyType) effectType);//new HitUIParams();
  //          int direction = 0;
  //          uiParams.SetDir(direction);
  //          uiParams.SetTarget(pAvatar);
  //          uiParams.SetParamsBySkillEffect(effectFlag);
  //          UIMgr.Form3dManager.AddView(EFormGroupType.HUB_TEXT, uiParams.ClientID, uiParams);
  //          GalaxyGameModule.GetGameManager<HitUIParamManager>().ReturnToPool(uiParams);
  //      }
        
  //      private void StartHitScreenEffect(ActorObj avatar)
  //      {
  //          if (null == avatar || !avatar.m_bIsControllByLocal)
  //              return;
  //          if (avatar.Pool == null)
  //          {
  //              return;
  //          }
  //          float hp = avatar.Pool.GetFloat("hp");
  //          float hpmax = avatar.Pool.GetFloat("hpmax");
  //          if ((hp/hpmax)<0.4f && null != avatar.CombatCom)
  //          {
  //              avatar.CombatCom.StartHitScreenEffect();
  //          }
  //      }

  //      public void OnSkillEffect(GPacketBase pkt)
  //      {
  //          if (pkt == null)
  //              return;
  //          ActorObj pTarAvatar = GetByServerID(pkt.GetInt32("targetID")) as ActorObj;
  //          ActorObj pAttackAvatar = GetByServerID(pkt.GetInt32("casterID")) as ActorObj;
  //          if (
  //              pTarAvatar == null || 
  //              pAttackAvatar == null ||
  //              pTarAvatar.CombatCom == null ||
  //              pAttackAvatar.CombatCom == null
  //              )
  //              return;
  //          int skillID = pkt.GetInt32("skillID");
  //          int effectType = pkt.GetInt32("effectType");
  //          if ((effectType & (int)eTriggerNotify.TriggerNotify_Damage) != 0)
  //          {
  //              if (pAttackAvatar == GetLocalPlayer())
  //              {
  //                  pAttackAvatar.CombatCom.CameraShake(skillID);
  //              }
  //              if (pTarAvatar == GetLocalPlayer())
  //              {
  //                  pTarAvatar.CombatCom.CameraShake(skillID);
  //              }
  //              pTarAvatar.CombatCom.HurtShake();
  //              DoShowHit(pAttackAvatar, pTarAvatar, skillID, pkt);
  //              DoUpdateUIBlood(pAttackAvatar, pTarAvatar, pkt);
  //              StartHitScreenEffect(pTarAvatar);
  //          }
  //          else if ((effectType & (int)eTriggerNotify.TriggerNotify_Heal) != 0)
  //          {
  //              DoUpdateUIBlood(pAttackAvatar, pTarAvatar, pkt);
  //          }
  //      }

  //      public void PlayerEnterScene()
  //      {
  //          if (m_LocalPlayer == null)
  //          {
  //              return;
  //          }

  //          m_LocalPlayer.OnEnterScene();
  //      }

  //      public void OnPlayerChangeScene()
  //      {
  //          if (m_LocalPlayer == null)
  //              return;

  //          m_LocalPlayer.OnChangeScene();
  //          RemoveAll(true);
  //      }

  //      public GalaxyActor CreateAvatar(int paramType)
  //      {
  //          ParamType paramId = ParamPool.GetParamType(paramType);
  //          GalaxyActor actor = null;
  //          if (paramId == ParamType.Param_avatar)
  //          {
  //              actor = new NetPlayer();
  //          }
  //          else if (paramId == ParamType.Param_monster)
  //          {
  //              actor = new NetNPC();
  //          }
  //          if (paramType != -1)
  //          {
  //              ParamPool pool = ParamPoolDefine.CreateParamPool(paramType);
  //              actor.SetParamPool(pool);
  //          }
  //          return actor;
  //      }

  //      public bool IsLocalPlayer(int avatarID)
  //      {
  //          return (m_LocalPlayer.ServerID == avatarID) ? true : false;
  //      }

  //      public LocalPlayer GetLocalPlayer()
  //      {
  //          return m_LocalPlayer as LocalPlayer;
  //      }
  //      public GalaxyActor GetByServerID(int serverID)
  //      {
  //          if (!ServerIDActorManager.ContainsKey(serverID))
  //              return null;
  //          return ServerIDActorManager[serverID];
  //      }
  //      public GalaxyActor GetByClientID(int clientID)
  //      {
  //          if (!ClientIDActorManager.ContainsKey(clientID))
  //              return null;

  //          return ClientIDActorManager[clientID];
  //      }
  //      public GalaxyActor GetByDataID(int dataID)
  //      {
  //          foreach(KeyValuePair<int, GalaxyActor> actorPair in ServerIDActorManager)
  //          {
  //              if (actorPair.Value == null)
  //                  continue;
  //              if (actorPair.Value.GetParamDataID() == dataID)
  //                  return actorPair.Value;
  //          }
  //          return null;
  //      }
  //      public GalaxyActor GetByAvatarDID(long avatarDID)
  //      {
  //          foreach (KeyValuePair<int, GalaxyActor> actorPair in ServerIDActorManager)
  //          {
  //              if (actorPair.Value == null)
  //                  continue;
  //              if (actorPair.Value.Pool.GetInt64("avatardid", -1) == avatarDID)
  //                  return actorPair.Value;
  //          }
  //          return null;
  //      }

  //      public void UpdateClientID(int serverID, int clientID)
  //      {
  //          if (ClientIDActorManager.ContainsKey(clientID))
  //              return;
  //          GalaxyActor actor = GetByServerID(serverID);
  //          if (actor == null)
  //              return;
  //          if (ClientIDActorManager.ContainsKey(actor.ClientID))
  //          {
  //              ClientIDActorManager.Remove(actor.ClientID);
  //          }
  //          actor.ClientID = clientID;
  //          ClientIDActorManager[clientID] = actor;
  //      }

  //      public void AddActor(GalaxyActor actor)
  //      {
  //          if (actor.ServerID == -1)
  //              return;
  //          if (ServerIDActorManager.ContainsKey(actor.ServerID))
  //              return;
  //          ServerIDActorManager[actor.ServerID] = actor;
  //      }

  //      public void RemoveActor(GalaxyActor actor)
  //      {
  //          if (actor.ServerID == -1)
  //              return;
  //          if (!ServerIDActorManager.ContainsKey(actor.ServerID))
  //              return;

  //          actor.UnloadRes();

  //          ServerIDActorManager.Remove(actor.ServerID);
  //          if (actor.ClientID != -1)
  //          {
  //              if (!ClientIDActorManager.ContainsKey(actor.ClientID))
  //                  return;
  //              ClientIDActorManager.Remove(actor.ClientID);
  //          }
  //      }

  //      public void RemoveActorWithDelay(int nServerID, float fDelayTime)
  //      {
  //          float fTime;
  //          if (!DelayRemoveList.TryGetValue(nServerID, out fTime))
  //          {
  //              DelayRemoveList.Add(nServerID, fDelayTime);
  //          }
  //      }

  //      public void RemoveByServerID(int ServerID,bool bForceRemove)
  //      {
  //          GalaxyActor act = GetByServerID(ServerID);
  //          if (act != null)
  //          {
  //              if (bForceRemove)
  //              {
  //                  RemoveActor(act);
  //              }
  //              else
  //              {
  //                  ActorObj actObj = act as ActorObj;
  //                  if (null != actObj)
  //                  {
  //                      if (actObj.CheckState((int)eState.State_Dead) == false)
  //                      {
  //                          RemoveActor(act);
  //                      }
  //                      else
  //                      {
  //                          RemoveActorWithDelay(ServerID, actObj.GetRemoveDelayTime());
  //                      }
  //                  }
  //              }
  //          }
  //      }
  //      public void RemoveAll(bool bExceptLocalPlayer)
  //      {
  //          List<GalaxyActor> actorList = new List<GalaxyActor>(ServerIDActorManager.Values);
  //          for (int i = 0; i < actorList.Count; i++)
  //          {
  //              if (actorList[i] != null)
  //              {
  //                  if (bExceptLocalPlayer && IsLocalPlayer(actorList[i].ServerID))
  //                  {
  //                      continue;
  //                  }
  //                  RemoveByServerID(actorList[i].ServerID,true);
  //              }
  //          }
  //          DelayRemoveList.Clear();
  //      }

  //      public void UpdateAOIObjView(int AvatarID, bool bInView)
  //      {
  //          if (AvatarID == m_LocalPlayer.ServerID)
  //              return;

  //          if (bInView)
  //          {
  //              GalaxyActor act = GetByServerID(AvatarID);
  //              if (act == null)
  //              {
  //                  //向服务器拽玩家数据
  //                  GPacketBase pkt = m_packetDefineMgr.CreatePacket("GPacketClientAvatarDataRequest");
  //                  if (pkt == null)
  //                      return;
  //                  pkt.SetInt32("TarAvatarID", AvatarID);
  //                  GalaxyNetManager.Instance.SendPacket(pkt);
  //              }
  //          }
  //          else
  //          {
  //              RemoveAvatarList.Add(AvatarID);
  //          }
  //      }

  //      public void InitAvatarData(GPacketBase pkt)
  //      {
  //          int avatarID = pkt.GetInt32("SrcAvatarID");
  //          int paramType = pkt.GetInt32("paramType");
  //          GalaxyActor actor = GetByServerID(avatarID);
  //          if (actor == null)
  //          {
  //              actor = CreateAvatar(paramType);
  //              actor.ServerID = avatarID;
  //              AddActor(actor);
  //          }

  //          byte[] buffer = new byte[pkt.mBuffSize];
  //          pkt.ReadBuffer(buffer, pkt.mBuffSize);
  //          bool bFinish = false;
  //          byte paramFlag = pkt.GetByte("m_ParamFlag");
  //          if ((paramFlag & (byte)eParamPool.LastPacket) == (byte)eParamPool.LastPacket)
  //          {
  //              bFinish = true;
  //          }
  //          actor.UpdataFromPacketBuffer(buffer, bFinish,true);
  //      }

  //      public void UpdateAvatarData(GPacketBase pkt)
  //      {
  //          int avatarID = pkt.GetInt32("SrcAvatarID");
  //          int paramType = pkt.GetInt32("paramType");
  //          GalaxyActor actor = GetByServerID(avatarID);
  //          if (actor == null)
  //          {
  //              //actor = CreateAvatar(paramType);
  //              //actor.ServerID = avatarID;
  //              //AddActor(actor);
  //              return;
  //          }

  //          byte[] buffer = new byte[pkt.mBuffSize];
  //          pkt.ReadBuffer(buffer, pkt.mBuffSize);
  //          bool bFinish = false;
  //          byte paramFlag = pkt.GetByte("m_ParamFlag");
  //          if ((paramFlag & (byte)eParamPool.LastPacket) == (byte)eParamPool.LastPacket)
  //          {
  //              bFinish = true;
  //          }
  //          actor.UpdataFromPacketBuffer(buffer, bFinish,false);
  //      }
  //      private int GetFirstEmptyIndex()
  //      {
  //          int returnIdx = -1;
  //          for (int idx = 0; idx < GalaxyConstant.MAX_CREATE_ROLE_COUNT; idx++)
  //          {
  //              if (!LocalPlayerList.ContainsKey(idx))
  //              {
  //                  returnIdx = idx;
  //                  break;
  //              }
  //          }
  //          return returnIdx;
  //      }
  //      public void PlayerUserData(GPacketBase pkt)
  //      {
  //          int idx = pkt.GetInt32("index");
  //          int paramtype = pkt.GetInt32("m_ParamType");
  //          LocalPlayer player = null;
  //          if (LocalPlayerList.ContainsKey(idx))
  //          {
  //              player = LocalPlayerList[idx];
  //          }
  //          else
  //          {
  //              if (idx != GetFirstEmptyIndex())
  //                  return;
  //              player = new LocalPlayer();
  //              if (player == null)
  //                  return;
  //              ParamPool pool = ParamPoolDefine.CreateParamPool((int)ParamType.Param_avatar, ParamPool.GetParamDataID(paramtype));
  //              player.SetParamPool(pool);
  //              player.ServerID = LocalPlayerServerID;
  //              LocalPlayerList.Add(idx, player);
  //          }
  //          byte flag = pkt.GetByte("m_ParamFlag");
  //          int size = pkt.mBuffSize;
  //          byte[] buff = new byte[size];
  //          pkt.ReadBuffer(buff, size);
  //          player.Pool.Read(buff);
  //      }

  //      public Dictionary<int,GalaxyActor> GetAcotrSvrList()
  //      {
  //          return ServerIDActorManager;
  //      }

  //      public Dictionary<int, LocalPlayer> GetUserList()
  //      {
  //          return LocalPlayerList;
  //      }

  //      public int GetUserListCount()
  //      {
  //          if (LocalPlayerList == null)
  //              return 0;

  //          return LocalPlayerList.Count;
  //      }

  //      public LocalPlayer GetFirstPlayer()
  //      {
  //          if (LocalPlayerList == null || LocalPlayerList.Count == 0)
  //              return null;

  //          foreach(KeyValuePair<int,LocalPlayer> pairs in LocalPlayerList)
  //          {
  //              if (pairs.Value != null)
  //                  return pairs.Value;
  //          }
  //          return null;
  //      }

  //      public bool ContainsAvatar(int id)
  //      {
  //          if (LocalPlayerList.ContainsKey(id))
  //              return true;
  //          else
  //              return false;
  //      }

  //      public void SelectAvatar(int id)
  //      {
  //          if (ContainsAvatar(id))
  //          {
  //              m_LocalPlayer = LocalPlayerList[id];
  //              AddActor(m_LocalPlayer);
  //              LocalPlayerList.Clear();
  //          }
  //      }

  //      public void ReconnectInitAvatar(GPacketBase pkt)
  //      {
  //          m_LocalPlayer = new LocalPlayer();
  //          if (m_LocalPlayer == null)
  //              return;
  //          int paramtype = pkt.GetInt32("m_ParamType");
  //          int dataid = ParamPool.GetParamDataID(paramtype);
  //          ParamPool pool = ParamPoolDefine.CreateParamPool((int)ParamType.Param_avatar, dataid);
  //          m_LocalPlayer.SetParamPool(pool);
  //          LocalPlayerServerID = pkt.mAvatarID;
  //          m_LocalPlayer.ServerID = LocalPlayerServerID;
  //          AddActor(m_LocalPlayer);

  //          byte flag = pkt.GetByte("m_ParamFlag");
  //          int size = pkt.mBuffSize;
  //          byte[] buff = new byte[size];
  //          pkt.ReadBuffer(buff, size);
  //          m_LocalPlayer.Pool.Read(buff);
  //          LocalPlayer mainPlayer = GalaxyGameModule.GetGameManager<ActorManager>().GetLocalPlayer();
  //          if (mainPlayer == null)
  //          {
  //              GameLogger.Error(LOG_CHANNEL.PROCEDURE, "Client Reconnect failed ! LocalPlayer is null! ");
  //              return;
  //          }

  //          Vector3 vServerPos = new Vector3(pkt.GetInt16("PosX") / 10.0f, pkt.GetInt16("PosZ") / 10.0f, pkt.GetInt16("PosY") / 10.0f);
  //          Vector3 vServerDir = new Vector3(pkt.GetInt16("DirX") / 10000.0f, pkt.GetInt16("DirZ") / 10000.0f, pkt.GetInt16("DirY") / 10000.0f);

  //          m_LocalPlayer.SetPos(vServerPos);
  //          m_LocalPlayer.SetDir2D(vServerDir);
  //          m_LocalPlayer.SetBornPos(vServerPos);
  //          m_LocalPlayer.SetBornDir(vServerDir);

  //          mainPlayer.RequestCommonData();
  //      }
  //      public void ClearActorObjSelected()
  //      {
  //          List<GalaxyActor> actorList = new List<GalaxyActor>(ServerIDActorManager.Values);

  //          for (int i = 0; i < actorList.Count; i++)
  //          {
  //              if (actorList[i] == null)
  //              {
  //                  continue;
  //              }
  //              ActorObj act = actorList[i] as ActorObj;
  //              act.m_bSelected = false;
  //          }
  //      }
        
		//public List<GalaxyActor> GetAllActor()
		//{
		//	if(ServerIDActorManager.Values != null)
		//	{
		//		return new List<GalaxyActor>(ServerIDActorManager.Values);
		//	}
		//	return null;
		//}

  //      public void UpdateAOIAdd(ref List<int> list)
  //      {
  //          for (int i = 0; i < list.Count; ++i)
  //          {
  //              int id = list[i];
  //              if (IsLocalPlayer(id))
  //              {
  //                  continue;
  //              }
  //              if (!ServerIDActorManager.ContainsKey(id))
  //              {
  //                  //GameLogger.Warning(LOG_CHANNEL.NETWORK, "Aoi Add : " + id);
  //                  UpdateAOIObjView(id, true);
  //              }
  //          }

  //      }
  //      public void UpdateAOIRemove(ref List<int> list)
  //      {
  //          if (ServerIDActorManager.Count == 0)
  //          {
  //              return;
  //          }
  //          Dictionary<int, GalaxyActor>.KeyCollection key = ServerIDActorManager.Keys;
  //          Dictionary<int, GalaxyActor>.KeyCollection.Enumerator en = key.GetEnumerator();
  //          while (en.MoveNext())
  //          {
  //              int id = en.Current;
  //              if (IsLocalPlayer(id))
  //              {
  //                  continue;
  //              }
  //              if (!list.Contains(id))
  //              {
  //                  //GameLogger.Warning(LOG_CHANNEL.NETWORK, "Aoi Remove : " + id);
  //                  UpdateAOIObjView(id, false);
  //              }
  //          }

  //          if (RemoveAvatarList.Count > 0)
  //          {
  //              foreach (int avatarId in RemoveAvatarList)
  //              {
  //                  RemoveByServerID(avatarId,false);
  //                  //ServerIDActorManager.ForceRemove(avatarId);
                    
  //              }
  //              RemoveAvatarList.Clear();
  //          }
  //      }

  //      public void OnGemOperationRes(GPacketBase pkt)
  //      {
  //          if (pkt == null)
  //              return;

  //          ActorObj pAvatar = GetByServerID(pkt.mAvatarID) as ActorObj;
  //          if (pAvatar == null)
  //              return;
  //          LocalPlayer myPlayer = GetLocalPlayer();
  //          if (null == myPlayer || myPlayer != pAvatar)
  //              return;
  //          GalaxyGameModule.GetGameManager<GGemManager>().HandleGemOperationRes(pkt);
  //      }
        
  //      private int GetNpcBossLv(ActorObj actor)
  //      {
  //          if (actor.IsPlayer())
  //              return 0;

  //          int flag = actor.Pool.GetInt32("npcflag", -1);
  //          if (flag == -1)
  //              return 0;

  //          ConfigData data = ConfigDataTableManager.Instance.GetData("NpcFlagDefine", flag);
  //          return data.GetInt("BossLv");
  //      }
  //      public eAimTargetType GetAvatarType(ActorObj actor)
  //      {
  //          LocalPlayer player = GetLocalPlayer();
  //          if (CreditManager.Instance.CheckRelation(player, actor, TargetType.ToFriend))
  //          {
  //              if (actor.IsPlayer())
  //                  return eAimTargetType.Player;
  //              else if (actor.IsNPC())
  //                  return eAimTargetType.FriendNpc;
  //          }
  //          else if (CreditManager.Instance.CheckRelation(player, actor, TargetType.ToEnemy))
  //          {
  //              if (actor.IsPlayer())
  //                  return eAimTargetType.Player;
  //              else if (actor.IsNPC())
  //              {
  //                  int bossLv = GetNpcBossLv(actor);
  //                  if (bossLv == 1)
  //                      return eAimTargetType.EliteMonster;
  //                  else if (bossLv == 2)
  //                      return eAimTargetType.BossMonster;
  //                  else
  //                      return eAimTargetType.NormalMonster;
  //              }
  //          }
  //          return eAimTargetType.NormalMonster;
  //      }
        
    }
}
