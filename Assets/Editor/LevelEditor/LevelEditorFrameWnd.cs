using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using System;
using System.IO;
using System.Diagnostics;

namespace Galaxy
{
    public class LevelEditorFrameWnd : UnityEditor.EditorWindow
    {
        private int m_mapID = 0;
        private string mapName;
        private bool m_bNewPath = false;

        private readonly string ScenePath = "Assets/AssetDatas/Scenes/";

        private List<ParamMonsterData> m_lMonsterList;
        private List<ParamMonsterData> m_lNpcList;
        private List<NpcGrowData> m_lGrowList;
        private List<SceneGrow> m_lSceneGrowList;
        private List<CollisionData> m_lCollisionList;
        private List<TriggerData_Editor> m_lTriggerList;

        private List<string> m_lMonsterNamePop;
        private List<int> m_lMonsterIDPop;
        private int m_curMonsterID;

        private List<string> m_lSelectNamePop;
        private List<int> m_lSelectIDPop;

        private List<string> m_lNpcNamePop;
        private List<int> m_lNpcIDPop;
        private int m_curNpcID;

        private List<string> m_lSelectNpcNamePop;
        private List<int> m_lSelectNpcIDPop;

        private List<string> m_lNpcGrowNamePop;
        private List<int> m_lNpcGrowIDPop;
        private int m_curNpcGrowID;

        private List<string> m_lSelectGrowNamePop;
        private List<int> m_lSelectGrowIDPop;

        private int m_collisonIndex = 0;
        private Dictionary<int, List<CollisionData>> m_dictCollision;
        private Dictionary<int, List<SceneCollision_shapes>> m_dictRunTimeCollisions;

        private GameObject m_rootObj = null;
        private GameObject m_monsterRoot = null;
        private GameObject m_npcRoot = null;
        private GameObject m_growRoot = null;
        private GameObject m_pathRoot = null;
        private GameObject m_triggerRoot = null;
        private GameObject m_spawnerRoot = null;
        private GameObject m_collisionRoot = null;
        private GameObject m_clientTriggerRoot = null;

        private LevelEditorConfigManager m_configMgr = null;
        private LevelEditorSaveManager m_saveMgr = null;

        private Dictionary<GalaxyTriggerDefine.TRIGGER_TYPE, string> m_registerEditorMapping 
                                                 = new Dictionary<GalaxyTriggerDefine.TRIGGER_TYPE, string>();

        private Dictionary<GalaxyTriggerDefine.TRIGGER_TYPE, string> m_registerEditorPrefab
                                                 = new Dictionary<GalaxyTriggerDefine.TRIGGER_TYPE, string>();

        Vector3 GetCreatePos()
        {
            if (SceneView.lastActiveSceneView == null || SceneView.lastActiveSceneView.camera == null)
                return Vector3.zero;

            Vector3 vStartPos = SceneView.lastActiveSceneView.camera.transform.position;
            Vector3 vCamDir = SceneView.lastActiveSceneView.camera.transform.TransformDirection(Vector3.forward);           
            Ray ray = new Ray(vStartPos, -vCamDir);
            RaycastHit hit;
            
            if (Physics.Raycast(vStartPos, vCamDir, out hit, 200.0f))
            {
                return hit.point;
            }           
            return vStartPos;
        }

        private void InitRegisterMapping()
        {
            if (m_registerEditorMapping.Count > 0)
                return;

            m_registerEditorMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_TIMELINE, "Galaxy.TriggerEditor_Timeline");
            m_registerEditorMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_ANIMATOR, "Galaxy.TriggerEditor_AnimatorControl");
            m_registerEditorMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_CINE_MAHCHINE, "Galaxy.Trigger_CineMachine");
            m_registerEditorMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_LUA_ACTIONS, "Galaxy.TriggerEditor_DoLua");
            //m_registerMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_VISABLE_GROUP, "Galaxy.Trigger_NpcTalk");
            m_registerEditorMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_GUIDE, "Galaxy.TriggerEditor_GuideTrigger");
            m_registerEditorMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_OS, "Galaxy.TriggerEditor_OS");
            m_registerEditorMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_NPC_ANIMATION, "Galaxy.TriggerEditor_NpcAnimation");
            m_registerEditorMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_NPC_FINDPATH, "Galaxy.TriggerEditor_FindPath");
            m_registerEditorMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_LOOKATSCENE, "Galaxy.TriggerEditor_LookAtScene");
            m_registerEditorMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_PLAYEFFECT, "Galaxy.TriggerEditor_PlayEffect");
            m_registerEditorMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_SEND_PACKET, "Galaxy.TriggerEditor_SendSrvPacket");
            m_registerEditorMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_CAR_SPEED_CHANGE, "Galaxy.TriggerEditor_CarChangeSpeed");
            m_registerEditorMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_STOP_CAR, "Galaxy.TriggerEditor_StopCar");
            m_registerEditorMapping.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_LUA_ACTION_EX, "Galaxy.TriggerEditor_DoLuaEx");

            m_registerEditorPrefab.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_TIMELINE, "TimeLine_Common.prefab");
            m_registerEditorPrefab.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_ANIMATOR, "Animator_Control.prefab");
            //m_registerEditorPrefab.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_CINE_MAHCHINE, "Galaxy.Trigger_CineMachine");
            m_registerEditorPrefab.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_LUA_ACTIONS, "DoLuaAction.prefab");
            //m_registerEditorPrefab.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_VISABLE_GROUP, "Galaxy.Trigger_NpcTalk");
            m_registerEditorPrefab.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_GUIDE, "GuideTrigger.prefab");
            m_registerEditorPrefab.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_OS, "OS_Config.prefab");
            m_registerEditorPrefab.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_NPC_ANIMATION, "Npc_Animaton.prefab");
            m_registerEditorPrefab.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_NPC_FINDPATH, "Npc_FindPath.prefab");
            m_registerEditorPrefab.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_LOOKATSCENE, "LookAtScene.prefab");
            m_registerEditorPrefab.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_PLAYEFFECT, "PlayEffect_Trigger.prefab");
            m_registerEditorPrefab.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_SEND_PACKET, "SendSrvPacket.prefab");
            m_registerEditorPrefab.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_CAR_SPEED_CHANGE, "Car_ChangeSpeed.prefab");
            m_registerEditorPrefab.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_STOP_CAR, "StopCar_Trigger.prefab");
            m_registerEditorPrefab.Add(GalaxyTriggerDefine.TRIGGER_TYPE.TRIGGER_LUA_ACTION_EX, "Trigger_LuaAction.prefab");
        }

        private void OnEnable()
        {
            m_curPage = 0;
            m_configMgr = new LevelEditorConfigManager();
            m_saveMgr = new LevelEditorSaveManager();

            if (!InitConfigs())
            {
                UnityEngine.Debug.LogError("初始化关卡编辑器失败! 请检查配置！");
                return;
            }

            m_lMonsterList = new List<ParamMonsterData>();
            m_lNpcList = new List<ParamMonsterData>();
            m_lGrowList = new List<NpcGrowData>();
            m_lCollisionList = new List<CollisionData>();
            m_lSceneGrowList = new List<SceneGrow>();

            m_lMonsterNamePop = new List<string>();
            m_lMonsterIDPop = new List<int>();
 
            m_lNpcNamePop = new List<string>();
            m_lNpcIDPop = new List<int>();

            m_lNpcGrowNamePop = new List<string>();
            m_lNpcGrowIDPop = new List<int>();

            m_lSelectIDPop = new List<int>();
            m_lSelectNamePop = new List<string>();

            m_lSelectNpcIDPop = new List<int>();
            m_lSelectNpcNamePop = new List<string>();

            m_lSelectGrowNamePop = new List<string>();
            m_lSelectGrowIDPop = new List<int>();

            m_dictCollision = new Dictionary<int, List<CollisionData>>();
            m_dictRunTimeCollisions = new Dictionary<int, List<SceneCollision_shapes>>();


            ILevelDataRow[] monsterDataList = m_configMgr.m_sceneContentSpawner.GetAllData();
            for(int i = 0; i < monsterDataList.Length; ++i)
            {
                ParamMonsterData data = monsterDataList[i] as ParamMonsterData;
                if (data == null)
                    continue;

                if(data.NpcFlag >= 100 )
                {
                    m_lNpcIDPop.Add(data.MonsterID);
                    m_lNpcNamePop.Add(data.MonsterID + "_" + data.MonsterName); //ConvertToUTF8(data.MonsterID + "_" + data.MonsterName)
                    m_lNpcList.Add(data);
                }
                else
                {
                    m_lMonsterIDPop.Add(data.MonsterID);
                    m_lMonsterNamePop.Add(data.MonsterID + "_" + data.MonsterName);
                    m_lMonsterList.Add(data);
                }
            }

            ILevelDataRow[] npcGrowList = m_configMgr.m_npcGrow.GetAllData();
            for (int i = 0; i < npcGrowList.Length; ++i)
            {
                NpcGrowData data = npcGrowList[i] as NpcGrowData;
                if (data == null)
                    continue;

                m_lNpcGrowIDPop.Add(data.NpcGrowID);
                m_lNpcGrowNamePop.Add(data.NpcGrowID + data.NpcGrowName);
                m_lGrowList.Add(data);
            }

            ILevelDataRow[] sceneGrowList = m_configMgr.m_sceneGrow.GetAllData();
            for (int i = 0; i < sceneGrowList.Length; ++i)
            {
                SceneGrow data = sceneGrowList[i] as SceneGrow;
                if (data == null)
                    continue;

                m_lSceneGrowList.Add(data);
            }

            ILevelDataRow[] arrDataList = m_configMgr.m_collision.GetAllData();
            if (arrDataList != null)
            {
                foreach (ILevelDataRow item in arrDataList)
                {
                    CollisionData data = item as CollisionData;
                    if(m_dictCollision.ContainsKey(data.GroupID))
                    {
                        m_dictCollision[data.GroupID].Add(data);
                    }
                    else
                    {
                        List<CollisionData> t_list = new List<CollisionData>();
                        t_list.Add(data);
                        m_dictCollision.Add(data.GroupID, t_list);
                    }
                }
            }
            m_collisonIndex = m_dictRunTimeCollisions.Count;
            PathIndex = 0;

            InitRegisterMapping();
        }

        private void OnDisable()
        {
            m_lSelectNamePop.Clear();
            m_lSelectIDPop.Clear();
            m_configMgr = null;
            if(m_lCollisionList != null)
            {
                m_lCollisionList.Clear();
            }
            
            m_dictCollision.Clear();
            if(m_collisionRoot != null)
            {
                GameObject.DestroyImmediate(m_collisionRoot);
                m_collisionRoot = null;
            }

            if(m_rootObj != null)
            {
                GameObject.DestroyImmediate(m_rootObj);
                m_rootObj = null;
            }

            if(m_clientTriggerRoot != null)
            {
                GameObject.DestroyImmediate(m_clientTriggerRoot);
                m_clientTriggerRoot = null;
            }

            GameObject root = GameObject.Find("DestPosObjectList");
            if (root != null)
            {
                GameObject.DestroyImmediate(root);
            }
        }

        public void SetConfigMgr(ref LevelEditorConfigManager mgr)
        {
            m_configMgr = mgr;
        }

        private readonly int SPACE_CONTENT = 40;
        private const int LABEL_WIDTH = 100;
        private int m_curPage = 0;

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            DrawMapTitle();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(SPACE_CONTENT);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            DrawPageBtnList();
            EditorGUILayout.EndHorizontal();

            if(m_curPage == 0)
            {
                DrawPageSpwaner();
            }
            else if(m_curPage == 1)
            {
                DrawCollisonShapes();
            }
            else if(m_curPage == 2)
            {
                DrawPageGrowList();
            }
            else if(m_curPage == 3)
            {
                DrawPageTrigger();
            }
        }

        private void DrawPageBtnList()
        {
            if(GUILayout.Button("刷怪相关"))
            {
                m_curPage = 0;
            }
            if(GUILayout.Button("碰撞编辑"))
            {
                m_curPage = 1;
            }
            if (GUILayout.Button("采集编辑"))
            {
                m_curPage = 2;
            }
            if (GUILayout.Button("Trigger编辑"))
            {
                m_curPage = 3;
            }
        }

        private void DrawPageSpwaner()
        {
            EditorGUILayout.BeginHorizontal();
            DrawPathContent();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(SPACE_CONTENT);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(SPACE_CONTENT);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("MonsterTab");
            m_beginID = EditorGUILayout.DelayedIntField(m_beginID, GUILayout.Width(LABEL_WIDTH));
            m_endID = EditorGUILayout.DelayedIntField(m_endID, GUILayout.Width(LABEL_WIDTH));
            if (GUILayout.Button("确定"))
            {
                if (m_beginID != 0 && m_endID != 0 && m_endID > m_beginID)
                {
                    m_lSelectIDPop.Clear();
                    m_lSelectNamePop.Clear();
                    for (int i = 0; i < m_lMonsterIDPop.Count; ++i)
                    {
                        if (m_lMonsterIDPop[i] >= m_beginID && m_lMonsterIDPop[i] <= m_endID)
                        {
                            m_lSelectIDPop.Add(m_lMonsterIDPop[i]);
                            m_lSelectNamePop.Add(m_lMonsterNamePop[i]);
                        }
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            DrawSpawnerContent();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(SPACE_CONTENT);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("NpcTab");
            m_npcBeginID = EditorGUILayout.DelayedIntField(m_npcBeginID, GUILayout.Width(LABEL_WIDTH));
            m_npcEndID = EditorGUILayout.DelayedIntField(m_npcEndID, GUILayout.Width(LABEL_WIDTH));
            if (GUILayout.Button("确定"))
            {
                if (m_npcBeginID != 0 && m_npcEndID != 0 && m_npcEndID > m_npcBeginID)
                {
                    m_lSelectNpcIDPop.Clear();
                    m_lSelectNpcNamePop.Clear();
                    for (int i = 0; i < m_lNpcIDPop.Count; ++i)
                    {
                        if (m_lNpcIDPop[i] >= m_npcBeginID && m_lNpcIDPop[i] <= m_npcEndID)
                        {
                            m_lSelectNpcIDPop.Add(m_lNpcIDPop[i]);
                            m_lSelectNpcNamePop.Add(m_lNpcNamePop[i]);
                        }
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            DrawNpcContent();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(SPACE_CONTENT * 2);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            DrawSaveButton();
            EditorGUILayout.EndHorizontal();
        }

        private int m_growBegin = 0;
        private int m_growEnd = 0;
        private int m_curGrowID = 0;
        private void DrawPageGrowList()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("采集列表");
            m_growBegin = EditorGUILayout.DelayedIntField(m_growBegin);
            m_growEnd = EditorGUILayout.DelayedIntField(m_growEnd);
            #region 区间选择
            if (GUILayout.Button("确定"))
            {
                if (m_growBegin != 0 && m_growEnd != 0 && m_growEnd > m_growBegin)
                {
                    m_lSelectGrowIDPop.Clear();
                    m_lSelectGrowNamePop.Clear();
                    for (int i = 0; i < m_lNpcGrowIDPop.Count; ++i)
                    {
                        if (m_lNpcGrowIDPop[i] >= m_growBegin && m_lNpcGrowIDPop[i] <= m_growEnd)
                        {
                            m_lSelectGrowIDPop.Add(m_lNpcGrowIDPop[i]);
                            m_lSelectGrowNamePop.Add(m_lNpcGrowNamePop[i]);
                        }
                    }
                }
            }
            #endregion
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(SPACE_CONTENT * 2);
            EditorGUILayout.EndHorizontal();

            #region 采集列表
            m_curGrowID = EditorGUILayout.IntPopup("采集列表 ", m_curGrowID, m_lNpcGrowNamePop.ToArray(), m_lSelectGrowIDPop.ToArray());
            if (GUILayout.Button("添加"))
            {
                for (int i = 0; i < m_lGrowList.Count; ++i)
                {
                    if (m_lGrowList[i].NpcGrowID == m_curGrowID)
                    {
                        GameObject newGrow = CreateEditorObject(m_lGrowList[i].ModelPath, m_lGrowList[i].NpcGrowID.ToString(), m_growRoot);
                        newGrow.name = m_growRoot.transform.childCount.ToString();
                        newGrow.transform.SetParent(m_growRoot.transform);
                        newGrow.transform.localPosition = Vector3.zero;
                        newGrow.transform.localRotation = Quaternion.identity;
                        newGrow.transform.localScale = Vector3.one;
                        newGrow.transform.position = GetCreatePos();
                        SceneContentNpcGrow_RunTime growRunTime = newGrow.AddComponent<SceneContentNpcGrow_RunTime>();
                        growRunTime.SceneID = m_mapID;
                        growRunTime.ID = m_curGrowID;
                        growRunTime.bClient = false;
                        newGrow.AddComponent<SphereCollider>();
                        break;
                    }
                }
            }
            #endregion

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(SPACE_CONTENT * 2);
            EditorGUILayout.EndHorizontal();

            #region 读取
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("读取采集列表"))
            {
                if(m_growRoot != null && m_mapID > 0)
                {
                    for (int i = 0; i < m_lSceneGrowList.Count; ++i)
                    {
                        if (m_lSceneGrowList[i].SceneID == m_mapID)
                        {
                            GameObject newGrow = CreateEditorObject(m_lSceneGrowList[i].GrowConfig.ModelPath, m_lSceneGrowList[i].GrowID.ToString(), m_growRoot);
                            newGrow.name = m_growRoot.transform.childCount.ToString();
                            newGrow.transform.SetParent(m_growRoot.transform);
                            newGrow.transform.localPosition = m_lSceneGrowList[i].Pos;
                            newGrow.transform.localEulerAngles = m_lSceneGrowList[i].Dir;
                            newGrow.transform.localScale = Vector3.one;
                            //newGrow.transform.position = GetCreatePos();
                            SceneContentNpcGrow_RunTime growRunTime = newGrow.AddComponent<SceneContentNpcGrow_RunTime>();
                            growRunTime.SceneID = m_mapID;
                            growRunTime.ID = m_lSceneGrowList[i].GrowID;
                            growRunTime.Radius = m_lSceneGrowList[i].Radius;
                            growRunTime.Pos = m_lSceneGrowList[i].Pos;
                            growRunTime.Dir = m_lSceneGrowList[i].Dir;
                            growRunTime.bClient = m_lSceneGrowList[i].ClientBorn;
                            SphereCollider collider = newGrow.AddComponent<SphereCollider>();
                            collider.radius = m_lSceneGrowList[i].Radius;
                        }
                    }
                }
            }
            #endregion

            if (GUILayout.Button("保存"))
            {
                LevelEditor_Grow grow = new LevelEditor_Grow();
                grow.SaveFile(m_mapID, m_saveMgr.CltFilePath, m_growRoot);
            }
            EditorGUILayout.EndHorizontal();

            if(m_growRoot != null)
            {
                SceneContentNpcGrow_RunTime[] arrRunTimeCom =  m_growRoot.GetComponentsInChildren<SceneContentNpcGrow_RunTime>();
                foreach(SceneContentNpcGrow_RunTime item in arrRunTimeCom)
                {
                    item.NeedUpdate();
                }
            }
        }

        private GameObject m_selectGroup = null;
        private void DrawPageTrigger()
        {
            if (m_clientTriggerRoot == null)
            {
                m_clientTriggerRoot = GameObject.Find("ClientTriggerRoot");
                if (m_clientTriggerRoot == null)
                {
                    m_clientTriggerRoot = CreateRootObject("ClientTriggerRoot", null);
                }
            }

            #region  读取选择
            GUILayout.BeginHorizontal();
            GUILayout.Label("================= Trigger触发内容配置 =====================");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("选择场景内容"))
            {
                LevelEditor_TriggerContentWnd triggerContent = EditorWindow.GetWindow<LevelEditor_TriggerContentWnd>();
                triggerContent.FrameWindow = this;
                triggerContent.ShowUtility();
                triggerContent.minSize = new Vector2(300f, 400f);
                triggerContent.maxSize = new Vector2(300f, 700f);

                Rect rect = triggerContent.position;
                rect.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
                rect.x += 200f;
                triggerContent.position = rect;

                triggerContent.wantsMouseMove = true;
                triggerContent.autoRepaintOnSceneChange = true;
                triggerContent.titleContent.text = "请配置场景内容";
                triggerContent.Show();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("================= Trigger组配置 =====================");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("添加一个触发组"))
            {
                int index = 0;
                for (int i = 0; i < m_clientTriggerRoot.transform.childCount; ++i)
                {
                    Transform trans = m_clientTriggerRoot.transform.GetChild(i);
                    string[] arr1 = trans.name.Split('_');
                    int id = int.Parse(arr1[1]);
                    if (index < id)
                        index = id;
                }
                index++;

                CreateRootObject("TriggerGroup_"+ index.ToString(), m_clientTriggerRoot);
            }
            if(GUILayout.Button("删除一个触发组"))
            {
                if(Selection.activeObject != null || Selection.activeObject.name.Contains("TriggerGroup_"))
                {
                    GameObject delGroupObj = Selection.activeObject as GameObject;
                    GameObject.DestroyImmediate(delGroupObj);
                }
                else
                {
                    EditorUtility.DisplayDialog("提示", "请正确选择一个触发组(TriggerGroup层)", "确定", "取消");
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("================= Trigger配置 =====================");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("添加Trigger"))
            {
                if (Selection.activeObject != null || Selection.activeObject.name.Contains("TriggerGroup_"))
                {
                    GameObject TriggerObj = Selection.activeObject as GameObject;
                    int index = 0;
                    for(int i = 0; i < TriggerObj.transform.childCount; ++i)
                    {
                        Transform trans = TriggerObj.transform.GetChild(i);
                        string[] arr = trans.name.Split('_');
                        int id = int.Parse(arr[1]);
                        if (index < id)
                            index = id;
                    }
                    index++;

                    GameObject trigger = CreateRootObject("newTrigger_" + index, TriggerObj);
                    BoxCollider com = trigger.AddComponent<BoxCollider>();
                    com.isTrigger = true;
                }
                else
                {
                    EditorUtility.DisplayDialog("提示", "请正确选择一个触发组(Trigger层)", "确定", "取消");
                }
            }
            if (GUILayout.Button("删除Trigger"))
            {
                if (Selection.activeObject != null || Selection.activeObject.name.Contains("newTrigger_"))
                {
                    GameObject TriggerObj = Selection.activeObject as GameObject;
                    if (TriggerObj != null)
                    {
                        GameObject.DestroyImmediate(TriggerObj);
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("提示", "请正确选择一个触发组(Trigger层)", "确定", "取消");
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("================= Trigger模板配置 =====================");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("选择Trigger配置模板"))
            {
                if (Selection.activeObject != null && Selection.activeObject.name.Contains("newTrigger_"))
                {
                    GameObject selectGroup = Selection.activeObject as GameObject;

                    LevelEditor_SelectTriggerTemplate selectTemplate = EditorWindow.GetWindow<LevelEditor_SelectTriggerTemplate>();
                    selectTemplate.FrameWindow = this;
                    selectTemplate.ShowUtility();
                    selectTemplate.minSize = new Vector2(300f, 400f);
                    selectTemplate.maxSize = new Vector2(300f, 700f);

                    Rect rect = selectTemplate.position;
                    rect.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
                    rect.x += 200f;
                    selectTemplate.position = rect;

                    selectTemplate.wantsMouseMove = true;
                    selectTemplate.autoRepaintOnSceneChange = true;
                    selectTemplate.titleContent.text = "请选择配置Trigger";
                    selectTemplate.Show();

                    m_selectGroup = selectGroup;
                }
                else
                {
                    EditorUtility.DisplayDialog("提示", "请正确选择Trigger(newTrigger)", "确定", "取消");
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("================= Trigger保存 =====================");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if(GUILayout.Button("保存配置"))
            {
                LevelEditor_Trigger saveCom = new LevelEditor_Trigger();
                saveCom.SaveFile(m_mapID, saveCom.CltFullPath, m_clientTriggerRoot);
            }
            GUILayout.EndHorizontal();
            #endregion

            if (m_clientTriggerRoot != null)
            {
                SceneTrigger_RunTime[] timelineCom = m_clientTriggerRoot.transform.GetComponentsInChildren<SceneTrigger_RunTime>();
                foreach(SceneTrigger_RunTime item in timelineCom)
                {
                    item.NeedUpdate();
                }

                TriggerEditor_Base[] arr = m_clientTriggerRoot.transform.GetComponentsInChildren<TriggerEditor_Base>();
                foreach (TriggerEditor_Base item in arr)
                {
                    item.NeedUpdate();
                }
            }
        }

        public void OnCreateNewConfig(GameObject newEvent)
        {
            if (m_clientTriggerRoot == null)
                return;

            int index = 0;
            for (int i = 0; i < m_selectGroup.transform.childCount; ++i)
            {
                Transform trans = m_selectGroup.transform.GetChild(i);
                string[] arr1 = trans.name.Split('_');
                int id = int.Parse(arr1[arr1.Length - 1]);
                if (index < id)
                    index = id;
            }
            index++;

            newEvent.transform.SetParent(m_selectGroup.transform);
            newEvent.name = newEvent.name + "_" + index.ToString();
            newEvent.transform.localPosition = Vector3.zero;
            newEvent.transform.localRotation = Quaternion.identity;
            newEvent.transform.localScale = Vector3.one;
            newEvent.transform.position = GetCreatePos();

            SceneTrigger_RunTime runTimeData = newEvent.AddComponent<SceneTrigger_RunTime>();
            string[] arr = m_selectGroup.name.Split('_');
            runTimeData.ID = int.Parse(arr[1]);
            runTimeData.actionId = newEvent.transform.parent.childCount;
            runTimeData.SceneID = m_mapID;
            arr = m_selectGroup.transform.parent.name.Split('_');
            runTimeData.TriggerGroup = int.Parse(arr[1]);

            BoxCollider coliider = newEvent.GetComponent<BoxCollider>();
            if(coliider != null)
            {
                GameObject.DestroyImmediate(coliider);
            }
        }

        public void OnSelectEnd()
        {
            if (m_selectGroup == null)
                return;

            if (m_selectGroup.transform.childCount == 0)
                GameObject.DestroyImmediate(m_selectGroup);

            m_selectGroup = null;
        }

        private void DrawMapTitle()
        {
       

            GUILayout.Label("地图ID : ");
            m_mapID = EditorGUILayout.DelayedIntField(m_mapID, GUILayout.Width(200));
            if (GUILayout.Button("打开地图"))
            {
                m_saveMgr.CreatePath();

                string strOpenMapName = GetSceneName(m_mapID);
                if (strOpenMapName == null || EditorSceneManager.GetActiveScene().name.Equals(strOpenMapName))
                {
                    if(m_rootObj == null)
                    {
                        m_rootObj = CreateRootObject("ObjectFlash", null);
                        m_monsterRoot = CreateRootObject("MonsterGroup", m_rootObj);
                        m_npcRoot = CreateRootObject("NpcGroup", m_rootObj);
                        m_growRoot = CreateRootObject("GrowNpcGroup", m_rootObj);
                        m_pathRoot = CreateRootObject("FindPath", m_rootObj);
                        m_triggerRoot = CreateRootObject("TriggerGroup", m_rootObj);
                        m_spawnerRoot = CreateRootObject("SpawnerGroup", m_rootObj);
                    }
                    
                    mapName = strOpenMapName;
                   
                }
                else
                {
                    EditorSceneManager.OpenScene(ScenePath + strOpenMapName + ".unity", OpenSceneMode.Single);
                    mapName = strOpenMapName;

                    if (m_rootObj != null)
                    {
                        GameObject.DestroyImmediate(m_rootObj);
                    }

                    m_rootObj = CreateRootObject("ObjectFlash", null);
                    m_npcRoot = CreateRootObject("NpcGroup", m_rootObj);
                    m_growRoot = CreateRootObject("GrowNpcGroup", m_rootObj);
                    m_pathRoot = CreateRootObject("FindPath", m_rootObj);
                    m_triggerRoot = CreateRootObject("TriggerGroup", m_rootObj);
                    m_spawnerRoot = CreateRootObject("SpawnerGroup", m_rootObj);
                }
                                  

                LoadMonster(m_mapID);
                LoadTrigger(m_mapID);
                LoadSpawner(m_mapID);
                LoadClientTrigger(m_mapID);
            }
        }

        private int m_beginID = 0;
        private int m_endID = 0;
        private int m_npcBeginID = 0;
        private int m_npcEndID = 0;
        private int PathIndex = 0;

        Vector3 GetVector(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return new Vector3();
            }
            string[] vs = val.Split(";"[0]);
            Vector3 v = new Vector3();
            v.x = Convert.ToSingle(vs[0]);
            v.z = Convert.ToSingle(vs[1]);
            v.y = Convert.ToSingle(vs[2]);
            return v;
        }

        void LoadMonster(int sceneID)
        {
            string fileName = m_saveMgr.FilePath + "scenecontentnpc" + Convert.ToString(sceneID) + ".txt";
            if (!File.Exists(fileName))
            {                
                return;
            }
            string strContent = "";
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            if (fs != null)
            {
                StreamReader sr = new StreamReader(fs);
                if (sr != null)
                {
                    strContent = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                }
            }
            if (string.IsNullOrEmpty(strContent))
            {
                GameLogger.Error(LOG_CHANNEL.LOGIC, fileName + "  读取失败");
            }

            string[] values = strContent.Split("\n"[0]);
            values[0] = values[0].Replace("\r", "");
            string[] namestr = values[0].Split("\t"[0]);   
            if (values.Length<=2)
            {
                return;
            }
                     
            for (int i = 2; (i < values.Length); ++i)
            {
                if (string.IsNullOrEmpty(values[i]))
                {
                    continue;
                }
                values[i] = values[i].Replace("\r", "");
                Dictionary<string, string> vals = new Dictionary<string, string>();
                string[] subValues = values[i].Split("\t"[0]);
                for (int x=0;x<subValues.Length;++x)
                {
                    vals[namestr[x]] = subValues[x];
                }
                ConfigCreateMonster(vals);
                ConfigCreateNpc(vals);
            }
        }

        void LoadTrigger(int sceneID)
        {
            string fileName = m_saveMgr.FilePath + "scenecontenttrigger" + Convert.ToString(sceneID) + ".txt";
            if (!File.Exists(fileName))
            {
                return;
            }
            string strContent = "";
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            if (fs != null)
            {
                StreamReader sr = new StreamReader(fs);
                if (sr != null)
                {
                    strContent = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                }
            }
            if (string.IsNullOrEmpty(strContent))
            {
                GameLogger.Error(LOG_CHANNEL.LOGIC, fileName + "  读取失败");
            }

            string[] values = strContent.Split("\n"[0]);
            values[0] = values[0].Replace("\r", "");
            string[] namestr = values[0].Split("\t"[0]);
            if (values.Length <= 2)
            {
                return;
            }
            for (int i = 2; (i < values.Length); ++i)
            {
                if (string.IsNullOrEmpty(values[i]))
                {
                    continue;
                }
                values[i] = values[i].Replace("\r", "");
                Dictionary<string, string> vals = new Dictionary<string, string>();
                string[] subValues = values[i].Split("\t"[0]);
                for (int x = 0; x < subValues.Length; ++x)
                {
                    vals[namestr[x]] = subValues[x];
                }
                ConfigCreateTrigger(vals);
            }
        }

        void LoadClientTrigger(int sceneID)
        {
            m_configMgr.LoadClientConfig(m_mapID);
            InitRegisterMapping();

            if (m_clientTriggerRoot == null)
            {
                m_clientTriggerRoot = GameObject.Find("ClientTriggerRoot");
                if (m_clientTriggerRoot == null)
                {
                    m_clientTriggerRoot = CreateRootObject("ClientTriggerRoot", null);
                }
            }

            ILevelDataRow[] arrData = m_configMgr.m_triggerDefine.GetAllData();
            foreach(ILevelDataRow item in arrData)
            {
                TriggerData_Editor data = item as TriggerData_Editor;
                GameObject groupObj = m_clientTriggerRoot.GetChild("TriggerGroup_" + data.GroupID);
                if (groupObj == null)
                {
                    groupObj = CreateRootObject("TriggerGroup_" + data.GroupID, m_clientTriggerRoot);
                }

                GameObject newTrigger = groupObj.GetChild("newTrigger_" + data.DataID.ToString());
                if (newTrigger == null)
                {
                    newTrigger = CreateRootObject("newTrigger_" + data.DataID.ToString(), groupObj);
                    BoxCollider collider = newTrigger.AddComponent<BoxCollider>();
                    collider.isTrigger = true;
                    collider.transform.position = data.Pos;
                    collider.transform.localEulerAngles = data.Dir;
                    collider.size = data.Scale;
                }

                LoadTriggerActions(newTrigger, data);
            }
        }


        private void LoadTriggerActions(GameObject trigger, TriggerData_Editor data)
        {
            if (trigger == null || data == null)
                return;

            if(!m_registerEditorMapping.ContainsKey((GalaxyTriggerDefine.TRIGGER_TYPE)data.TriggerType))
            {
                string strTips = string.Format("Trigger類型 {0}是未知或未实现类型，请联系柳峰添加！",data.TriggerType.ToString());
                EditorUtility.DisplayDialog("提示", strTips, "确定");
                return;
            }

            string strPath = "Assets/AssetPrefabs/Trigger/";
            if(!m_registerEditorPrefab.ContainsKey((GalaxyTriggerDefine.TRIGGER_TYPE)data.TriggerType))
            {
                string strTips = string.Format("Trigger模板類型 {0}是未知或未实现类型，请联系柳峰添加！", data.TriggerType.ToString());
                EditorUtility.DisplayDialog("提示", strTips, "确定");
                return;
            }

            strPath += m_registerEditorPrefab[(GalaxyTriggerDefine.TRIGGER_TYPE)data.TriggerType];
            GameObject newActionPrefab = UnityEditor.AssetDatabase.LoadMainAssetAtPath(strPath) as GameObject;
            if (newActionPrefab == null)
                return;

            GameObject action = GameObject.Instantiate(newActionPrefab);
            if (action == null)
                return;

            action.name = newActionPrefab.name;
            TriggerEditor_Base triggerEditor = action.GetComponent(m_registerEditorMapping[(GalaxyTriggerDefine.TRIGGER_TYPE)data.TriggerType]) as TriggerEditor_Base;
            triggerEditor.ParseTriggerParam(data.TriggerAction);
            //triggerEditor.ParseSpwanerData(data.SpwanerId,data.SpwanerType);

            BoxCollider boxCollider = action.GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                GameObject.DestroyImmediate(boxCollider);
            }

            action.transform.SetParent(trigger.transform);
            action.name = action.name + "_" + data.ActionId.ToString();
            action.transform.localPosition = Vector3.zero;;
            action.transform.localRotation = Quaternion.identity;
            action.transform.localScale = Vector3.one ;

            SceneTrigger_RunTime runTimeData = action.AddComponent<SceneTrigger_RunTime>();
            runTimeData.ID = data.DataID;
            runTimeData.TriggerGroup = data.GroupID;
            runTimeData.actionId = data.ActionId;
            runTimeData.SceneID = m_mapID;
            runTimeData.actionId = trigger.transform.childCount;
            runTimeData.EffectID = data.EffectID;
            runTimeData.ActiveList = data.NextActionList;
            runTimeData.tagType = (GalaxyTriggerDefine.TRIGGER_TAG_TYPE)data.TriggerTag;
            runTimeData.triggerType = (GalaxyTriggerDefine.TRIGGER_TYPE)data.TriggerType;
            runTimeData.SpwanerId = data.SpwanerId;
            runTimeData.SpwanerType = (GSpawnerState)data.SpwanerType;
        }

        void LoadSpawner(int sceneID)
        {
            string fileName = m_saveMgr.FilePath + "scenecontentspawner" + Convert.ToString(sceneID) + ".txt";
            if (!File.Exists(fileName))
            {
                return;
            }
            string strContent = "";
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            if (fs != null)
            {
                StreamReader sr = new StreamReader(fs);
                if (sr != null)
                {
                    strContent = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                }
            }
            if (string.IsNullOrEmpty(strContent))
            {
                GameLogger.Error(LOG_CHANNEL.LOGIC, fileName + "  读取失败");
            }

            string[] values = strContent.Split("\n"[0]);
            values[0] = values[0].Replace("\r","");
            string[] namestr = values[0].Split("\t"[0]);
            if (values.Length <= 2)
            {
                return;
            }

            m_lSpwaner.Clear();

            for (int i = 2; (i < values.Length); ++i)
            {
                if (string.IsNullOrEmpty(values[i]))
                {
                    continue;
                }
                values[i] = values[i].Replace("\r", "");
                Dictionary<string, string> vals = new Dictionary<string, string>();
                string[] subValues = values[i].Split("\t"[0]);
                for (int x = 0; x < subValues.Length; ++x)
                {
                    vals[namestr[x]] = subValues[x];
                }
                ConfigCreateSpawner(vals);
            }
        }

        private void ConfigCreateMonster(Dictionary<string, string> config)
        {
            string sVal = "";
            config.TryGetValue("NpcType", out sVal);
            int typeID = Convert.ToInt32(sVal);
            if (typeID != 3)
            {
                return;
            }
            config.TryGetValue("DataID", out sVal);
            m_curMonsterID = Convert.ToInt32(sVal);

            config.TryGetValue("Pos", out sVal);
            Vector3 pos = GetVector(sVal);

            config.TryGetValue("DirPos", out sVal);
            Vector3 dir = GetVector(sVal);

            config.TryGetValue("ID", out sVal);
            int id = Convert.ToInt32(sVal);

            for (int i = 0; i < m_lMonsterList.Count; ++i)
            {
                if (m_lMonsterList[i].MonsterID == m_curMonsterID)
                {
                    GameObject monsterData = CreateEditorObject(m_lMonsterList[i].ModelPath, m_lMonsterList[i].MonsterID.ToString(), m_monsterRoot);
                    monsterData.transform.position = pos;
                    monsterData.transform.forward = dir;
                    SceneContentNpc_RunTime runTimeData = monsterData.AddComponent<SceneContentNpc_RunTime>();
                    runTimeData.ID = id;
                    runTimeData.SceneID = m_mapID;
                    runTimeData.DataID = m_lMonsterList[i].MonsterID;
                    runTimeData.NpcType = 3;
                    break;
                }
            }
        }
        private void ConfigCreateNpc(Dictionary<string, string> config)
        {
            if (config == null || config.Count == 0 || m_lNpcList == null)
                return;

            string sVal = "";
            config.TryGetValue("NpcType", out sVal);
            int typeID = Convert.ToInt32(sVal);
            if (typeID != 2)
            {
                return;
            }
            config.TryGetValue("DataID", out sVal);
            m_curNpcID = Convert.ToInt32(sVal);

            config.TryGetValue("Pos", out sVal);
            Vector3 pos = GetVector(sVal);

            config.TryGetValue("DirPos", out sVal);
            Vector3 dir = GetVector(sVal);

            config.TryGetValue("ID", out sVal);
            int id = Convert.ToInt32(sVal);

            for (int i = 0; i < m_lNpcList.Count; ++i)
            {
                if (m_lNpcList[i].MonsterID == m_curNpcID)
                {
                    GameObject monsterData = CreateEditorObject(m_lNpcList[i].ModelPath, m_lNpcList[i].MonsterID.ToString(), m_npcRoot);
                    monsterData.transform.position = pos;
                    monsterData.transform.forward = dir;
                    SceneContentNpc_RunTime runTimeData = monsterData.AddComponent<SceneContentNpc_RunTime>();
                    runTimeData.ID = id;
                    runTimeData.SceneID = m_mapID;
                    runTimeData.DataID = m_lNpcList[i].MonsterID;
                    runTimeData.NpcType = 2;
                    break;
                }
            }
        }
        private void ConfigCreateTrigger(Dictionary<string, string> config)
        {
            string sVal = "";

            int groupIndex = m_triggerRoot.transform.childCount;
            GameObject newTriggerObj = new GameObject();
            newTriggerObj.name = groupIndex.ToString();
            newTriggerObj.transform.SetParent(m_triggerRoot.transform);
            newTriggerObj.transform.localPosition = Vector3.zero;
            newTriggerObj.transform.localRotation = Quaternion.identity;
            newTriggerObj.transform.localScale = Vector3.one;
            //newTriggerObj.transform.position = pos;
            //newTriggerObj.transform.forward = dir;
            SceneTrigger_RunTime runTimeData = newTriggerObj.AddComponent<SceneTrigger_RunTime>();
            //runTimeData.ID = id;
            runTimeData.TriggerGroup = groupIndex;
            BoxCollider box = newTriggerObj.AddComponent<BoxCollider>();
            box.isTrigger = true;
        }

        private List<int> m_lSpwaner = new List<int>();

        private void ConfigCreateSpawner(Dictionary<string, string> config)
        {
            string sVal = "";
            config.TryGetValue("ID", out sVal);
            int id = Convert.ToInt32(sVal);

            config.TryGetValue("Range", out sVal);
            float range = Convert.ToSingle(sVal);

            config.TryGetValue("SpawnerTargetData", out sVal);
            m_curMonsterID = Convert.ToInt32(sVal);

            config.TryGetValue("Count", out sVal);
            int count = Convert.ToInt32(sVal);

            config.TryGetValue("Branch", out sVal);
            int branch = Convert.ToInt32(sVal);

            config.TryGetValue("CheckPlayerDis", out sVal);
            float checkplayer = Convert.ToInt32(sVal);

            config.TryGetValue("DelayTime", out sVal);
            int delay = Convert.ToInt32(sVal);

            config.TryGetValue("ParentSpawner", out sVal);
            int parent = Convert.ToInt32(sVal);

            config.TryGetValue("GroupID", out sVal);
            int groupid = Convert.ToInt32(sVal);

            config.TryGetValue("FindPath", out sVal);
            int findpath = Convert.ToInt32(sVal);

            int Hide = 0;
            if (config.TryGetValue("Hide", out sVal))
            {
                Hide = Convert.ToInt32(sVal);
            }
            

            config.TryGetValue("Pos", out sVal);
            Vector3 pos = GetVector(sVal);

            config.TryGetValue("DirPos", out sVal);
            Vector3 dir = GetVector(sVal);

            m_lSpwaner.Add(id);

            GameObject newSpawner = null;
            if (m_curMonsterID == -1)
            {
                newSpawner = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                newSpawner.transform.localScale = new Vector3(0.3f, 1, 0.3f);
                newSpawner.name = id.ToString();
            }
            else
            {
                for (int i = 0; i < m_lMonsterList.Count; ++i)
                {
                    if (m_lMonsterList[i].MonsterID == m_curMonsterID)
                    {
                        if (m_lMonsterList[i].ModelPath == null)
                        {
                            GameLogger.Error(LOG_CHANNEL.ERROR, "Monster Model Path is NULL! id = " + m_lMonsterList[i].MonsterID);
                            continue;
                        }
                        newSpawner = CreateEditorObject(m_lMonsterList[i].ModelPath, m_lMonsterList[i].MonsterID.ToString(), m_npcRoot);
                        newSpawner.name = id.ToString();
                        break;
                    }
                }
            }

            

            ///////////////////
            
            if (newSpawner != null)
            {
                //newSpawner.name = m_spawnerRoot.transform.childCount.ToString();
                newSpawner.transform.SetParent(m_spawnerRoot.transform);
                newSpawner.transform.localPosition = Vector3.zero;
                newSpawner.transform.localRotation = Quaternion.identity;
                newSpawner.transform.localScale = Vector3.one;
                newSpawner.transform.position = pos;
                newSpawner.transform.forward = dir;
                SceneContentSpawner_RunTime sp = newSpawner.AddComponent<SceneContentSpawner_RunTime>();
                sp.SceneID = m_mapID;
                sp.ID = id;
                sp.Range = range;
                sp.ParentSpawnerID = parent;
                sp.SpawnerTargetData = m_curMonsterID;
                sp.Branch = branch;
                sp.Count = count;
                sp.CheckPlayerDistance = checkplayer;
                sp.GroupID = groupid;
                sp.FindPath = findpath;
                sp.Hide = Hide;
                sp.cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                sp.cylinder.transform.SetParent(newSpawner.transform);
                sp.cylinder.transform.localPosition = Vector3.zero;
                sp.cylinder.transform.localRotation = Quaternion.identity;
                sp.cylinder.transform.localScale = Vector3.one;

                sp.cylinderCheckPlayer = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                sp.cylinderCheckPlayer.transform.SetParent(newSpawner.transform);
                sp.cylinderCheckPlayer.transform.localPosition = Vector3.zero;
                sp.cylinderCheckPlayer.transform.localRotation = Quaternion.identity;
                sp.cylinderCheckPlayer.transform.localScale = Vector3.one;
            }
           
            ///////////////////
        }

        public void SelectCollisionGroup(int groupID)
        {
            List<CollisionData> t_collisionList = new List<CollisionData>();
            if (!m_dictCollision.TryGetValue(groupID, out t_collisionList))
            {
                return;
            }

            if(t_collisionList.Count > 0)
            {
                GameObject newGroup = CreateRootObject(groupID.ToString(), m_collisionRoot);
                int index = 0;
                foreach(CollisionData item in t_collisionList)
                {
                    AddCollisonObject(newGroup, index,item);
                    index++;
                }
            }
        }

        private void DrawNpcContent()
        {
            if (m_npcRoot == null)
                return;

            EditorGUILayout.LabelField("Npc  ");
            m_curNpcID = EditorGUILayout.IntPopup("NPC列表", m_curNpcID, m_lSelectNpcNamePop.ToArray(), m_lSelectNpcIDPop.ToArray());
            if (GUILayout.Button("添加"))
            {
                for (int i = 0; i < m_lNpcList.Count; ++i)
                {
                    if (m_lNpcList[i].MonsterID == m_curNpcID)
                    {
                        GameObject monsterData = CreateEditorObject(m_lNpcList[i].ModelPath, m_lNpcList[i].MonsterID.ToString(), m_npcRoot);
                        monsterData.transform.position = GetCreatePos();
                        SceneContentNpc_RunTime runTimeData = monsterData.AddComponent<SceneContentNpc_RunTime>();
                        runTimeData.ID = m_npcRoot.transform.childCount;
                        runTimeData.SceneID = m_mapID;
                        runTimeData.DataID = m_lNpcList[i].MonsterID;
                        runTimeData.NpcType = 2;
                        break;
                    }
                }
            }

            if (m_npcRoot != null)
            {
                SceneContentNpc_RunTime[] arrComs = m_npcRoot.GetComponentsInChildren<SceneContentNpc_RunTime>();
                foreach (SceneContentNpc_RunTime item in arrComs)
                {
                    item.NeedUpdate();
                }
            }
        }
        private void DrawPathContent()
        {
            if (m_pathRoot == null)
                return;

            EditorGUILayout.LabelField("寻路 ");
            m_bNewPath = EditorGUILayout.Toggle("创新建路径",m_bNewPath);
            if (GUILayout.Button("添加"))
            {
                if(!m_bNewPath)
                {
                    if(m_pathRoot.transform.childCount == 0)
                    {
                        GameObject newPathRoot = CreateRootObject(PathIndex.ToString(), m_pathRoot);
                        SceneContentPath_RunTime runTimeRoot = newPathRoot.AddComponent<SceneContentPath_RunTime>();
                        PathIndex++;
                        runTimeRoot.NeedUpdate();
                    }
                    else
                    {
                        Transform lastChild = m_pathRoot.transform.GetChild(m_pathRoot.transform.childCount - 1);
                        CreateRootObject(lastChild.childCount.ToString(), lastChild.gameObject);
                        SceneContentPath_RunTime pathCom =  lastChild.GetComponent<SceneContentPath_RunTime>();
                        if(pathCom != null)
                        {
                            pathCom.NeedUpdate();
                        }
                    }
                }
                else
                {
                    GameObject newPathRoot = CreateRootObject(PathIndex.ToString(), m_pathRoot);
                    SceneContentPath_RunTime runTimeRoot = newPathRoot.AddComponent<SceneContentPath_RunTime>();
                    PathIndex++;

                    CreateRootObject("0", newPathRoot);
                    runTimeRoot.NeedUpdate();
                }
            }

           // DrawGrowContent();
        }
  
        private void DrawMonsterContent()
        {
            // GUILayout.Label("怪物 : ");
            m_curMonsterID = EditorGUILayout.IntPopup("Monster列表", m_curMonsterID, m_lMonsterNamePop.ToArray(), m_lMonsterIDPop.ToArray());
            if (GUILayout.Button("添加"))
            {
                for (int i = 0; i < m_lMonsterList.Count; ++i)
                {
                    if (m_lMonsterList[i].MonsterID == m_curMonsterID)
                    {
                        GameObject monsterData = CreateEditorObject(m_lMonsterList[i].ModelPath, m_lMonsterList[i].MonsterID.ToString(), m_monsterRoot);
                        monsterData.transform.position = GetCreatePos();
                        SceneContentNpc_RunTime runTimeData = monsterData.AddComponent<SceneContentNpc_RunTime>();
                        runTimeData.ID = m_monsterRoot.transform.childCount;
                        runTimeData.SceneID = m_mapID;
                        runTimeData.DataID = m_lMonsterList[i].MonsterID;
                        runTimeData.NpcType = 3;
                        break;
                    }
                }
            }

            if (m_monsterRoot != null)
            {
                SceneContentNpc_RunTime[] arrComs = m_npcRoot.GetComponentsInChildren<SceneContentNpc_RunTime>();
                foreach (SceneContentNpc_RunTime item in arrComs)
                {
                    item.NeedUpdate();
                }
            }
        }

        private int GetDataID()
        {
            if (m_lSpwaner.Count == 0)
                return 0;

            return m_lSpwaner[m_lSpwaner.Count - 1] + 1;
        }

        

        private void DrawSpawnerContent()
        {
            if (m_spawnerRoot == null || m_lSelectNamePop == null || m_lSelectIDPop == null)
                return;

            EditorGUILayout.LabelField("Spawner ");

            m_curMonsterID = EditorGUILayout.IntPopup("Monster列表", m_curMonsterID, m_lSelectNamePop.ToArray(), m_lSelectIDPop.ToArray());

            //m_curMonsterID = EditorGUILayout.IntPopup("Monster列表", m_curMonsterID, m_lMonsterNamePop.ToArray(), m_lMonsterIDPop.ToArray());
            if (GUILayout.Button("添加"))
            {
                
                for (int i = 0; i < m_lMonsterList.Count; ++i)
                {
                    if (m_lMonsterList[i].MonsterID == m_curMonsterID)
                    {
                        int nextNameID = GetDataID();
                        int id = m_spawnerRoot.transform.childCount;
                        GameObject newSpawner = CreateEditorObject(m_lMonsterList[i].ModelPath, m_lMonsterList[i].MonsterID.ToString(), m_npcRoot);
                        newSpawner.name = nextNameID.ToString();
                        newSpawner.transform.SetParent(m_spawnerRoot.transform);
                        newSpawner.transform.localPosition = Vector3.zero;
                        newSpawner.transform.localRotation = Quaternion.identity;
                        newSpawner.transform.localScale = Vector3.one;
                        newSpawner.transform.position = GetCreatePos();
                        SceneContentSpawner_RunTime sp = newSpawner.AddComponent<SceneContentSpawner_RunTime>();
                        sp.SceneID = m_mapID;
                        sp.ID = nextNameID;
                        sp.Range = 2;
                        sp.ParentSpawnerID = id - 1;
                        sp.SpawnerTargetData = m_curMonsterID;
                        sp.Branch = 1;
                        sp.Count = 1;
                        sp.cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);                       
                        sp.cylinder.transform.SetParent(newSpawner.transform);
                        sp.cylinder.transform.localPosition = Vector3.zero;
                        sp.cylinder.transform.localRotation = Quaternion.identity;
                        sp.cylinder.transform.localScale = Vector3.one;

                        sp.cylinderCheckPlayer = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                        sp.cylinderCheckPlayer.transform.SetParent(newSpawner.transform);
                        sp.cylinderCheckPlayer.transform.localPosition = Vector3.zero;
                        sp.cylinderCheckPlayer.transform.localRotation = Quaternion.identity;
                        sp.cylinderCheckPlayer.transform.localScale = Vector3.one;
                        m_lSpwaner.Add(nextNameID);
                        break;
                    }
                }
                
            }

            if (m_triggerRoot != null)
            {
                SceneContentSpawner_RunTime[] arrComs = m_spawnerRoot.GetComponentsInChildren<SceneContentSpawner_RunTime>();
                foreach (SceneContentSpawner_RunTime item in arrComs)
                {
                    item.NeedUpdate();
                }
            }

        }
        private void DrawCollisonShapes()
        {
            if (m_collisionRoot == null)
            {
                m_collisionRoot = CreateRootObject("CollisionRoot", null);
                m_collisionRoot.transform.position = GetCreatePos();
            }

            EditorGUILayout.LabelField("Collision Table");

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(SPACE_CONTENT);
            EditorGUILayout.EndHorizontal();

            #region 管理碰撞组父物体
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("读取碰撞组"))
            {
                if(m_collisionRoot.transform.childCount > 0)
                {
                    if (EditorUtility.DisplayDialog("提示", "当前碰撞组已经有编辑数据,读取表数据会覆盖当前数据，是否确定覆盖？", "确定读取","取消"))
                    {
                        m_collisonIndex = 0;
                        m_lCollisionList.Clear();
                        GameObject.DestroyImmediate(m_collisionRoot);
                        m_collisionRoot = CreateRootObject("CollisionRoot", null);
                        m_collisionRoot.transform.position = GetCreatePos();
                    }
                }

                LevelEditor_CollisionWnd collisionLoaderWnd = EditorWindow.GetWindow<LevelEditor_CollisionWnd>();
                collisionLoaderWnd.FrameWindow = this;
                collisionLoaderWnd.ShowUtility();
                collisionLoaderWnd.minSize = new Vector2(300f, 400f);
                collisionLoaderWnd.maxSize = new Vector2(300f, 700f);
                collisionLoaderWnd.CopyData(ref m_dictCollision);

                Rect rect = collisionLoaderWnd.position;
                rect.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
                rect.x += 200f;
                collisionLoaderWnd.position = rect;

                collisionLoaderWnd.wantsMouseMove = true;
                collisionLoaderWnd.autoRepaintOnSceneChange = true;
                collisionLoaderWnd.titleContent.text = "请选择读取碰撞组";
                collisionLoaderWnd.Show();
            }
            if (GUILayout.Button("创建碰撞组"))
            {
                string str = GetNewGroupID().ToString();
                GameObject groupGame = CreateRootObject(str, m_collisionRoot);
                if (groupGame != null)
                {
                    AddCollisonObject(groupGame,0);
                }
            }
            if (GUILayout.Button("删除碰撞组"))
            {
                GameObject curSelection = Selection.activeGameObject;
                if (curSelection.transform.parent == null)
                {
                    EditorUtility.DisplayDialog("提示", "不能删除，当前选择的不是一个碰撞组的物体", "OK");
                    return;
                }

                int groupId = int.Parse(curSelection.transform.name);
                DeleteCollisionObjectGroup(groupId, curSelection);
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            #region 管理碰撞子物体
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("添加一个碰撞"))
            {
                GameObject curSelection = Selection.activeGameObject;
                if (curSelection.transform.parent == null || curSelection.transform.parent.gameObject != m_collisionRoot)
                {
                    EditorUtility.DisplayDialog("提示", "请选择一个正确的碰撞组", "OK");
                    return;
                }

                bool bNeedDeleteOne = false;
                for(int i = 0; i < curSelection.transform.childCount; ++i)
                {
                    Transform subTran = curSelection.transform.GetChild(i);
                    if(subTran.name.Contains("template"))
                    {
                        bNeedDeleteOne = true;
                        break;
                    }
                }

                int index = bNeedDeleteOne ? curSelection.transform.childCount - 1 : curSelection.transform.childCount;
                AddCollisonObject(curSelection, index);
            }
            if (GUILayout.Button("删除一个碰撞"))
            {
                GameObject curSelection = Selection.activeGameObject;
                if(curSelection.GetComponent<SceneCollision_shapes>() == null)
                {
                    EditorUtility.DisplayDialog("提示", "请选择一个正确的碰撞", "OK");
                    return;
                }

                Transform parent = curSelection.transform.parent;

                int groupID = int.Parse(curSelection.transform.parent.name);
                DeleteCollisionObject(groupID, curSelection);
                if(parent.childCount == 0)
                {
                    GameObject.DestroyImmediate(parent.gameObject);
                    m_collisonIndex--;
                }
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(SPACE_CONTENT);
            EditorGUILayout.EndHorizontal();

            #region 添加一个实例
            EditorGUILayout.BeginHorizontal();
            instanceId = EditorGUILayout.DelayedIntField(instanceId, GUILayout.Width(LABEL_WIDTH));
            if (GUILayout.Button("创建碰撞实例"))
            {
                CreateObject();
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            #region 保存
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("保存碰撞"))
            {
                DrawSaveCollisionData();
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            if (m_collisionRoot != null)
            {
                SceneCollision_shapes[] arrComs = m_collisionRoot.GetComponentsInChildren<SceneCollision_shapes>();
                foreach (SceneCollision_shapes item in arrComs)
                {
                    item.NeedUpdate();
                }
            }
        }

        private int instanceId = 0;

        private void CreateObject()
        {
            GameObject curSelection = Selection.activeGameObject;
            if (curSelection.transform.parent == null || curSelection.transform.parent.gameObject != m_collisionRoot)
            {
                EditorUtility.DisplayDialog("提示", "请选择一个正确的碰撞组", "OK");
                return;
            }

            GameObject instanceObj = null;
            if (instanceId > 0)
            {
                GameObject group = curSelection.GetChild("template");
                if (group == null)
                {
                    group = new GameObject("template");
                    group.transform.SetParent(curSelection.transform);
                    group.transform.localPosition = Vector3.zero;
                    group.transform.localRotation = Quaternion.identity;
                    group.transform.localScale = Vector3.one;
                }

                foreach (ParamMonsterData item in m_lNpcList)
                {
                    if (item.MonsterID == instanceId)
                    {
                        instanceObj = CreateEditorObject(item.ModelPath, item.ModelID.ToString(), group);
                        break;
                    }
                } 

                foreach (ParamMonsterData item in m_lMonsterList)
                {
                    if (item.MonsterID == instanceId)
                    {
                        instanceObj = CreateEditorObject(item.ModelPath, item.ModelID.ToString(), group);
                        break;
                    }
                }

                if (instanceObj == null)
                {
                    GameObject.DestroyImmediate(group);
                    return;
                }
                    
                instanceObj.transform.SetParent(group.transform);
            }
        }

        private int GetNewGroupID()
        {
            List<int> t_lIDs = new List<int>();
            foreach(var item in m_dictCollision)
            {
                t_lIDs.Add(item.Key);
            }

            if (t_lIDs.Count == 0)
                return 0;

            t_lIDs.Sort();
            int newID = t_lIDs[t_lIDs.Count - 1] + 1;
            return newID;
        }

        private void DrawSaveCollisionData()
        {
            if (m_collisionRoot == null)
                return;

            LevelEditor_CollisionShapes collisionCom = new LevelEditor_CollisionShapes();
            collisionCom.CreateServerFile(collisionCom.SrvFullPath, ref m_dictCollision, m_collisionRoot);
            collisionCom.CreateServerFile(collisionCom.CltFullPath, ref m_dictCollision, m_collisionRoot);

            GameObject.DestroyImmediate(m_collisionRoot);
        }


        private void AddCollisonObject(GameObject parentObject,int index, CollisionData oldData = null)
        {
            GameObject childObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            childObject.name = "Sphere" + index.ToString();
            childObject.transform.SetParent(parentObject.transform);

            int groupId = int.Parse(parentObject.name);

            SceneCollision_shapes com = childObject.AddComponent<SceneCollision_shapes>();
            
            if (oldData == null)
            {
                oldData = new CollisionData();
                oldData.GroupID = groupId;
                oldData.SphereID = index;

                com.GroupID = groupId;
                com.ShapeID = index;
                com.damageReduction = 1;

                if (m_dictCollision.ContainsKey(oldData.GroupID))
                {
                    m_dictCollision[oldData.GroupID].Add(oldData);
                }
                else
                {
                    List<CollisionData> t_list = new List<CollisionData>();
                    t_list.Add(oldData);
                    m_dictCollision.Add(oldData.GroupID, t_list);
                }
            }
            else
            {
                com.GroupID = oldData.GroupID;
                com.ShapeID = oldData.SphereID;
                com.damageReduction = oldData.damageReduction;
                childObject.transform.localPosition = oldData.Pos;

                SphereCollider collider = childObject.GetComponent<SphereCollider>();
                collider.radius = oldData.Radius;
                collider.gameObject.transform.localScale = new Vector3(collider.radius, collider.radius, collider.radius);
            }
        }

        private void DeleteCollisionObject(int groupId,GameObject deleObject)
        {
            if (deleObject == null)
                return;

            if(m_dictCollision.ContainsKey(groupId))
            {
                foreach(var item in m_dictCollision[groupId])
                {
                    SceneCollision_shapes com = deleObject.GetComponent<SceneCollision_shapes>();
                    if(item.SphereID == com.ShapeID)
                    {
                        m_dictCollision[groupId].Remove(item);
                        break;
                    }
                }
            }

            Transform groupTrans = deleObject.transform.parent;

            GameObject.DestroyImmediate(deleObject);

            if(groupTrans.childCount != 0)
            {
                for(int i = 0; i < groupTrans.childCount; ++i)
                {
                    Transform child = groupTrans.GetChild(i);
                    if (child == null)
                        continue;

                    child.name = "Sphere" + i.ToString();
                    child.GetComponent<SceneCollision_shapes>().ShapeID = i;
                }
            }
        }

        private void DeleteCollisionObjectGroup(int groupId,GameObject deleteObject)
        {
            if (deleteObject == null)
                return;

            if (m_dictCollision.ContainsKey(groupId))
            {
                m_dictCollision.Remove(groupId);
            }

            GameObject.DestroyImmediate(deleteObject);
        }

        private void DrawSaveButton()
        {
            if(GUILayout.Button("保 存"))
            {                
                LevelEditor_Spawner spawnerSaver = new LevelEditor_Spawner();
                spawnerSaver.SaveFile(m_mapID, m_saveMgr.FilePath, m_spawnerRoot);

                LevelEditor_Path pathSaver = new LevelEditor_Path();
                pathSaver.SaveFile(m_mapID, m_saveMgr.FilePath, m_pathRoot);

                LevelEditor_Npc npcSaver = new LevelEditor_Npc();
                npcSaver.SaveFile(m_mapID, m_saveMgr.FilePath, m_npcRoot, m_monsterRoot);
            }
        }

        private GameObject CreateRootObject(string strName,GameObject parent)
        {
            GameObject newObj = new GameObject();
            newObj.name = strName;
            if(parent != null)
            {
                newObj.transform.SetParent(parent.transform);
            }
            newObj.transform.localPosition = Vector3.zero;
            newObj.transform.localRotation = Quaternion.identity;
            newObj.transform.localScale = Vector3.one;
            return newObj;
        }

        private GameObject CreateEditorObject(string strPath,string strName,GameObject parent)
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(ResourcesProxy.EditorLoadAsset(strPath)) as GameObject;
            if (newObj == null)
                return null;

            GameObject objectGroup = new GameObject();
            objectGroup.name = strName;
            objectGroup.transform.SetParent(parent.transform);
            objectGroup.transform.localPosition = Vector3.zero;
            objectGroup.transform.localRotation = Quaternion.identity;
            objectGroup.transform.localScale = Vector3.one;

            if (parent != null)
            {
                newObj.transform.SetParent(objectGroup.transform);
                newObj.transform.localPosition = Vector3.zero;
                newObj.transform.localRotation = Quaternion.identity;
                newObj.transform.localScale = Vector3.one;
            }

            return objectGroup;
        }

        private bool InitConfigs()
        {
            m_configMgr.LoadEditorConfig();
            return true;
        }
        private string GetSceneName(int sceneID)
        {
            return m_configMgr.m_sceneDefine.GetSceneName(sceneID);
        }
    }
}
