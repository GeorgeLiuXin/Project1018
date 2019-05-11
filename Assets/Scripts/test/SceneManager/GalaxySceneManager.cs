//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace XWorld
//{
//    enum GSceneType
//    {
//        SceneCreate_Trunk = 0,  // 主干场景
//        SceneCreate_Copy = 1,   // 副本场景
//        SceneCreate_Token = 2,  // 令牌场景
//        SceneCreate_Guild = 3,  // 公会场景
//        SceneCreate_TrunkCopy = 4,  // 场景
//    };

//    public enum GSpawnerState
//    {
//        SpawnerInfo_Deactive,
//        SpawnerInfo_Active,
//        SpawnerInfo_Finish,
//    };

//    public enum eMaterialType
//    {
//        MaterialType_0,
//        MaterialType_1,
//        MaterialType_2,
//        MaterialType_3,
//        MaterialType_4,
//        MaterialType_5,
//    }

//    public class GalaxySceneManager : GalaxyGameManagerBase
//    {
//        public delegate void CloseSceneHandle();
//        public delegate void LoadSceneHandle(string scenename);

//        public CloseSceneHandle OnSceneCloseHandle;
//        public LoadSceneHandle OnSceneLoadHandle;

//        private Dictionary<int, GSpawnerState> m_SpawnerState = new Dictionary<int, GSpawnerState>();        
//        private Navigation.Grid m_NavGrid = null;
//        public Navigation.Grid NavGrid
//        {
//            get
//            {
//                return m_NavGrid;
//            }
//        }

//        public int CurSceneID
//        {
//            set;
//            get;
//        }

//        public byte[] NavBuffer
//        {
//            get
//            {
//                return m_NavBuffer;
//            }

//            set
//            {
//                m_NavBuffer = value;
//            }
//        }

//        public byte[] GridBuffer
//        {
//            get
//            {
//                return m_GridBuffer;
//            }

//            set
//            {
//                m_GridBuffer = value;
//            }
//        }

//        private byte[] m_NavBuffer;
//        private byte[] m_GridBuffer;

//        private Queue<GPacketBase> m_queueChangeScene = new Queue<GPacketBase>();
        
//        public override void InitManager()
//        {
//            m_ParamPool = ParamPoolDefine.CreateParamPool(ParamPool.GetParamTypeID(ParamType.Param_scene, 0));
//            GalaxyGameModule.GetGameManager<GalaxyPacketProcess>().AddPacketHandler("GPacketLoadSceneNotify", new GalaxyPacketProcess.PacketHandler(this.LoadSceneNotify));
//            GalaxyGameModule.GetGameManager<GalaxyPacketProcess>().AddPacketHandler("GPacketSceneClose", new GalaxyPacketProcess.PacketHandler(this.HandleSceneClose));
//            GalaxyGameModule.GetGameManager<GalaxyPacketProcess>().AddPacketHandler("GPacketSpawnerInfo", new GalaxyPacketProcess.PacketHandler(this.SpawnerInfo));
//            GalaxyGameModule.GetGameManager<GalaxyPacketProcess>().AddPacketHandler("GPacketUpdateSceneData", new GalaxyPacketProcess.PacketHandler(this.UpdateSceneData));
//            EventListener.Instance.AddListener(CltEvent.TarckView.ON_SERVER_SCENARIO_EVENT, OnServerScenarioEvent);
//            CurSceneID = -1;
//        }

//        public void UpdateSceneData(GPacketBase pkt)
//        {
//            if (m_ParamPool != null)
//            {
//                byte[] buff = new byte[pkt.mBuffSize];
//                pkt.ReadBuffer(buff, pkt.mBuffSize);
//                m_ParamPool.Read(buff);

//                EventListener.Instance.Dispatch(CltEvent.Scene.ON_SCENE_DATA_UPDATE);
//            }
//        }

//        public ulong GetSceneParallelInfo()
//        {
//            if (m_ParamPool == null)
//            {
//                return 1;
//            }
//            return m_ParamPool.GetUInt64("ParallelBits", 1);
//        }

//        public override void Update(float fElapseTimes)
//        {
//            UpdateCloseTime(fElapseTimes);
//        }

//        public override void ShutDown()
//        {
//            GalaxyGameModule.GetGameManager<GalaxyPacketProcess>().RemovePacketHandler("GPacketLoadSceneNotify");
//            GalaxyGameModule.GetGameManager<GalaxyPacketProcess>().RemovePacketHandler("GPacketSceneClose");
//            GalaxyGameModule.GetGameManager<GalaxyPacketProcess>().RemovePacketHandler("GPacketDynamicWall");
//            GalaxyGameModule.GetGameManager<GalaxyPacketProcess>().RemovePacketHandler("GPacketShieldWall");
//            GalaxyGameModule.GetGameManager<GalaxyPacketProcess>().RemovePacketHandler("GPacketSpawnerInfo");
//            EventListener.Instance.RemoveListener(CltEvent.TarckView.ON_SERVER_SCENARIO_EVENT, OnServerScenarioEvent);

//            m_queueChangeScene.Clear();
//        }

//        public void LoadScene(GPacketBase pkt)//GPacketLoadSceneNotify
//        {
//            if(NavService.IsChangeSceneState())
//            {
//                GameLogger.Error(LOG_CHANNEL.NETWORK, "Client recv msg named GPacketLoadSceneNotify when scene is Loading now! ");
//                m_queueChangeScene.Enqueue(pkt);
//                return;
//            }

//            if(OnSceneCloseHandle != null)
//            {
//                OnSceneCloseHandle();
//            }
//            int lastSceneID = CurSceneID;
//            int SceneID = pkt.GetInt32("sceneID");
//            int flag = pkt.GetInt32("flag");

//            SceneID = SceneID >> 16;
//            CurSceneID = SceneID;

//            if (flag == 1)
//            {
//                //open config UI
//                GPacketBase rpkt = GalaxyGameModule.GetGameManager<GPacketDefineManager>().CreatePacket("GPacketLoadSceneResult");
//                if (rpkt == null)
//                    return;

//                rpkt.SetInt32("result", 1);
//                GalaxyNetManager.Instance.SendPacket(rpkt);

//                //GameLogger.Error(LOG_CHANNEL.NETWORK, "收到服务器场景切换 id ： " + SceneID + " flag 是  1!  客户端并没有切换场景！" );
//                return;
//            }

//            ConfigData data = GetSceneDefine(SceneID);
//            if (data != null)
//            {
//                GalaxyGameModule.GetGameManager<GalaxyActorManager>().RemoveAll(true);
//                EventListener.Instance.Dispatch2Lua(CltEvent.Scene.ON_PREPARE_CHANGE_SCENE, CurSceneID, lastSceneID);
//                NavService.ChangeGameScene();
//                m_SpawnerState.Clear();

//                //GameLogger.Error(LOG_CHANNEL.NETWORK, "收到服务器场景切换 id ： " + SceneID + "  客户端并切换场景！");
//            }           
//        }

//        public bool CheckNextScene()
//        {
//            if (m_queueChangeScene == null || m_queueChangeScene.Count == 0)
//                return false;

//            GPacketBase pktNextScene = m_queueChangeScene.Dequeue();
//            RoutineRunner.WaitOneFrame(() =>
//            {
//                LoadScene(pktNextScene);
//            });

//            return true;
//        }
		
//        //Dictionary<int, GameObject> TestObj = new Dictionary<int, GameObject>();
//        public void SpawnerInfo(GPacketBase pkt)
//        {
//            int id = pkt.GetInt32("id");
//            GSpawnerState state = (GSpawnerState)pkt.GetSByte("flag");
//            if (state == GSpawnerState.SpawnerInfo_Deactive)
//            {
//                if (m_SpawnerState.ContainsKey(id))
//                {
//                    m_SpawnerState[id] = GSpawnerState.SpawnerInfo_Deactive;
//                }
//                else
//                {
//                    m_SpawnerState.Add(id, GSpawnerState.SpawnerInfo_Deactive);
//                }
//                EventListener.Instance.Dispatch2Lua(CltEvent.SpawnerState.Deactive, id);
//            }
//            else if(state == GSpawnerState.SpawnerInfo_Active)
//            {
//                if (m_SpawnerState.ContainsKey(id))
//                {
//                    m_SpawnerState[id] = GSpawnerState.SpawnerInfo_Active;
//                }
//                else
//                {
//                    m_SpawnerState.Add(id, GSpawnerState.SpawnerInfo_Active);
//                }
//                EventListener.Instance.Dispatch2Lua(CltEvent.SpawnerState.Active, id);
//            }
//            else if (state == GSpawnerState.SpawnerInfo_Finish)
//            {
//                if (m_SpawnerState.ContainsKey(id))
//                {
//                    m_SpawnerState.Remove(id);
//                    EventListener.Instance.Dispatch2Lua(CltEvent.SpawnerState.Finish, id);
//                }                
//            }
            
//        }

//        public GSpawnerState GetSpawnerState(int id)
//        {
//            GSpawnerState val;
//            if (m_SpawnerState.TryGetValue(id, out val))
//            {
//                return val;
//            }
//            return GSpawnerState.SpawnerInfo_Finish;
//        }

//        public bool IsSpawnerActive(int id)
//        {
//            GSpawnerState val;
//            if (m_SpawnerState.TryGetValue(id, out val))
//            {
//                return val == GSpawnerState.SpawnerInfo_Active;
//            }
//            return true;
//        }

//        public bool IsSpawnerDeactive(int id)
//        {
//            GSpawnerState val;
//            if (m_SpawnerState.TryGetValue(id, out val))
//            {
//                return val == GSpawnerState.SpawnerInfo_Deactive;
//            }
//            return false;
//        }

//        public bool IsSpawnerFinish(int id)
//        {
//            if (m_SpawnerState.ContainsKey(id))
//            {
//                return false;
//            }
//            return true;
//        }

//        public override void OnAfterChangeScene()
//        {
//            AfterLoadScene();
//        }

//        public void AfterLoadScene()
//        {
//            //load nav data
//            if (m_NavGrid == null)
//                m_NavGrid = new Navigation.Grid();       
                 
//            m_NavGrid.ReadData(NavBuffer, OnReadNavDataComplte);
//        }
        
//        private void OnReadNavDataComplte()
//        {
//            EventListener.Instance.Dispatch2Lua(CltEvent.Scene.ON_SCENE_LOADED);
//        }

//        public float GetCurHeight(Vector3 vCurPos)
//        {
//            if (NavGrid == null)
//            {
//                return vCurPos.y;
//            }
//            return NavGrid.GetHeight(vCurPos);
//            //Vector3 sample = vCurPos;
//            //sample.y += fRaiseHeight;

//            //Ray ray = new Ray(sample, Vector3.down);
//            //RaycastHit hit;

//            //string[] layerNames = { StaticParam.LAYER_TERRAIN, StaticParam.LAYER_BUILDING };
//            //if (Physics.Raycast(ray, out hit, 60.0f, LayerMask.GetMask(layerNames)))
//            //{
//            //    {
//            //        //Debug.DrawLine(sample, hit.point);
//            //        return hit.point.y;
//            //    }
//            //}
//            //return vCurPos.y;
//            ////return Terrain.activeTerrain.SampleHeight(vCurPos);
//        }

//        public void LoadSceneNotify(GPacketBase pkt)
//        {
//            GameLogger.DebugLog(LOG_CHANNEL.NETWORK, "Recv Packet : LoadSceneNotify!");

//            ResetCloseScene();

//            LoadScene(pkt);
//        }

//        public ConfigData GetSceneDefine(int sceneSID)
//        {
//            return ConfigDataTableManager.Instance.GetData("scenedefine", sceneSID);
//        }

//        public ConfigData GetCurrentSceneDefine()
//        {
//            return GetSceneDefine(CurSceneID);
//        }

//        public ConfigData GetCopyTypeData(int sceneSID)
//        {
//            ConfigData sceneDefine = GetSceneDefine(sceneSID);
//            if (null == sceneDefine)
//                return null;
//            return ConfigDataTableManager.Instance.GetData("copytypedata", sceneDefine.GetInt("CopyType"));
//        }

//        public ConfigData GetCopyTaskData(int sceneSID, eCopyTask copyTask)
//        {
//            return ConfigDataTableManager.Instance.GetData("copytaskdata", sceneSID, (int)copyTask);

//        }

//        public string GetCopyTaskDescribe(int sceneSID, eCopyTask copyTask)
//        {
//            return GetCopyTaskDescribe(GetCopyTaskData(sceneSID, copyTask));
//        }

//        public string GetCopyTaskDescribe(ConfigData taskData)
//        {
//            if (null == taskData)
//                return null;
//            return GCopyTaskData.GetCopyTaskDes(taskData);
//        }

//        #region SceneRequest

//        public void RequestEnterScene(int nSceneID)
//        {
//            GPacketBase pkt = GalaxyGameModule.GetGameManager<GPacketDefineManager>().CreatePacket("GPacketClientSceneReq");
//            if (pkt == null)
//                return;
//            pkt.SetInt32("sceneID", nSceneID);
//            GalaxyNetManager.Instance.SendPacket(pkt);

//            //GameLogger.Error(LOG_CHANNEL.ERROR,"客户端申请切换场景 id ： " + nSceneID);
//        }

//        #endregion

//        #region SceneClose
//        public void HandleSceneClose(GPacketBase pkt) // GPacketSceneClose
//        {
//            m_CloseSceneID = pkt.GetInt32("sceneid");
//            m_CloseTime = (float)pkt.GetInt32("closeTime");
//        }
//        protected void ResetCloseScene()
//        {
//            m_CloseSceneID = 0;
//            m_CloseTime = 0;
//        }
//        protected void UpdateCloseTime(float fElapseTimes)
//        {
//            if (m_CloseSceneID > 0)
//            {

//                if (m_CloseTime > 100)//时间超大，就是特殊情况，不播倒计时
//                    return;
 
//                m_CloseTime -= fElapseTimes;
//                if (m_CloseTime > 0)
//                {
//                    int curTime = (int)m_CloseTime;
//                    if (m_CurCloseTime != curTime)
//                    {
//                        m_CurCloseTime = curTime;
//                        PopupManager.DisplayNews(m_CurCloseTime.ToString());
//                    }
//                }
//                else
//                {
//                    ResetCloseScene();
//                }
//            }
//        }
//        private int m_CloseSceneID = 0;
//        private int m_CurCloseTime = 0;
//        private float m_CloseTime = 0;
//        #endregion

//        public bool IsCopy(int scenesid = -1)
//        {
//            int realsid = scenesid;
//            if(scenesid == -1)
//            {
//                realsid = CurSceneID;
//            }
//            ConfigData sceneData = GetSceneDefine(realsid);
//            if (sceneData == null)
//                return false;

//            int mapType = sceneData.GetInt("MapType");
//            return (mapType == (int)GSceneType.SceneCreate_Copy || mapType == (int)GSceneType.SceneCreate_TrunkCopy | mapType == (int)GSceneType.SceneCreate_Token);
//        }
//        public bool IsMain(int scenesid = -1)
//        {
//            int realsid = scenesid;
//            if (scenesid == -1)
//            {
//                realsid = CurSceneID;
//            }
//            ConfigData sceneData = GetSceneDefine(realsid);
//            if (sceneData == null)
//                return false;

//            int mapType = sceneData.GetInt("MapType");
//            return (mapType == (int)GSceneType.SceneCreate_Trunk);
//        }

//        private void OnServerScenarioEvent(object[] values)
//        {
//            GPacketBase pkt = values[0] as GPacketBase;
//            if (pkt == null)
//                return;
//            int scenarioType = pkt.GetInt32("scenarioType");
//            if (scenarioType == (int)CltScenarioType.CltScenarioType_SceneEvent)
//            {
//                int id1 = pkt.GetInt32("param1");
//                if (id1 == -1)
//                    return;
//                EventListener.Instance.Dispatch2Lua(CltEvent.Scene.ON_SERVER_SCENE_EVENT, id1);
//            }
//        }

//        ParamPool m_ParamPool = null;
//    }
//}
