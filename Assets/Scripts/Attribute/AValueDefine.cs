using XWorld.AValue;
using System.Collections.Generic;
using System;

namespace XWorld
{

    public enum eDmgAtkType
    {
        DmgAtkType_0,
        DmgAtkType_1,
        DmgAtkType_2,
        DmgAtkType_3,
        DmgAtkType_Size,
    };
    public enum eDmgArmorType
    {
        DmgArmorType_0,
        DmgArmorType_1,
        DmgArmorType_2,
        DmgArmorType_3,
        DmgArmorType_Size,
    };

    public enum AValue_GroupType
    {
        AValue_GroupBasic = 1,
        AValue_GroupExtend = 2,
        AValue_GroupBoth = AValue_GroupBasic | AValue_GroupExtend,
    };

    public class AValueMask
    {
        public AValueMask()
        {
            Mask = new bool[(int)AValueType.Count];
            Reset();
        }

        public void Reset()
        {
            CalculateAll = false;
            for (int i = 0; i < (int)AValueType.Count; i++)
            {
                Mask[i] = false;
            }
        }
        public void Combine(AValueMask mask)
        {
            for (int i = 0; i < (int)AValueType.Count; i++)
            {
                if (mask.Mask[i])
                    Mask[i] = true;
            }
        }
        public void Combine(AValueStruct vs)
        {
            foreach (var item in vs.Datas)
            {
                Mask[item.Key] = true;
            }
        }
        public void SetDirty(int valueID) { Mask[valueID] = true; }
        public bool IsDirty(int valueID) { return Mask[valueID]; }

        public bool[] Mask;
        public bool CalculateAll;
    };

    public class AValueDefine : XWorldGameManagerBase
    {
        private static readonly string AVALUE_DEFINE_PATH = "Config/Dynamic/ClientData/avaluedefine.txt";
        private static readonly string strExtention = ".txt";
        public AValueInfo[] Infos;
        private Dictionary<int, ConfigData> m_avaluedefineDict;
        private Dictionary<string, AValueType> m_mapValueNameToID;

        /// <summary>
        /// 初始化管理者
        /// </summary>
        public override void InitManager()
        {
            m_mapValueNameToID = new Dictionary<string, AValueType>();
            m_avaluedefineDict = new Dictionary<int, ConfigData>();
            Infos = new AValueInfo[(int)AValueType.Count];
            // InitAvalueDefine();
            CreateMapValueNameToID(ref m_mapValueNameToID);
            StartLoader();
        }

        public void StartLoader()
        {
            List<client_config_define> defineList = ClientConfigManager.LAppInitLoader;
            if (defineList.Count == 0)
            {
                GameLogger.Warning(LOG_CHANNEL.ASSET, "NO Param table");
                return;
            }

            List<string> loadPaths = new List<string>();
            List<string> loadNames = new List<string>();
            foreach (client_config_define define in defineList)
            {
                string name = define.tableName;
                string prePath = define.tablePath + name + strExtention;
                loadPaths.Add(prePath);
                loadNames.Add(name);
            }

            Object defineObj = ResourcesProxy.LoadAssetImmediately(AVALUE_DEFINE_PATH);
            if (defineObj != null)
            {
                OnResourcesSingleLoadComplete(defineObj);
            }
        }

        private void OnResourcesSingleLoadComplete(Object defineObj)
        {
            UnityEngine.TextAsset textAsset = defineObj as UnityEngine.TextAsset;
            if (textAsset != null)
            {
                string sContent = textAsset.text;
                ConfigData cd = new ConfigData();

                int idxkey1 = -1;
                string[] values = sContent.Split("\r"[0]);
                string[] lables = values[0].TrimStart('\n').Split('\t');
                string[] types = values[1].TrimStart('\n').Split('\t');

                Dictionary<string, int> ColumnNameMap = new Dictionary<string, int>();
                for (int i = 0; i < lables.Length; ++i)
                {
                    if (string.IsNullOrEmpty(lables[i]))
                    {
                        continue;
                    }
                    ColumnNameMap.Add(lables[i], i);
                }

                List<ConfigData> dataList = new List<ConfigData>();
                for (int i = 2; (i < values.Length); i = (i + 1))
                {
                    string[] subValues = values[i].TrimStart('\n').Split('\t');
                    if (subValues.Length != lables.Length)
                        continue;
                    ConfigData data = new ConfigData();
                    data.Init(lables.Length);

                    data.ColumnName2IDMap = ColumnNameMap;

                    int kV2 = -1;
                    if (idxkey1 >= 0)
                    {
                        kV2 = Convert.ToInt32(subValues[idxkey1]);
                    }
                    for (int a = 0; a < subValues.Length; a++)
                    {
                        data.AddValue(types[a], a, subValues[a]);
                    }
                    OnLoadConfig(data);
                }
            }
            ResourcesProxy.DestroyAsset(AVALUE_DEFINE_PATH);
        }
        
        /// <summary>
        /// 更新 逻辑切片时间
        /// </summary>
        /// <param name="fElapseTimes"></param>
        public override void Update(float fElapseTimes) { }

        /// <summary>
        /// 游戏退出
        /// </summary>
        public override void ShutDown() { }

        /// <summary>
        /// 断线重连以后的处理
        /// </summary>
        public override void OnReEnterGame() { }

        /// <summary>
        /// 进入登录流程的处理
        /// </summary>
        public override void OnEnterLogin() { }

        /// <summary>
        /// 第一次进入游戏的处理
        /// </summary>
        public override void OnEnterGame() { }
        ////////////////////////////////////////////////////////////////////////////////

        public ConfigData GetAValueInfo(int key)
        {
            // return ConfigDataTableManager.Instance.GetData("avaluedefine", key);
            if (m_avaluedefineDict.ContainsKey(key))
            {
                return m_avaluedefineDict[key];
            }
            return null;
        }

        public ConfigData GetAValueInfo(string name)
        {
            AValueType type = GetValueID(name);
            ConfigData configData = GetAValueInfo((int)type);
            return configData;
        }

        //public void InitAvalueDefine()
        //{
        //    Tableavaluedefine t_avaluedefineList = ClientConfigManager.GetTableByName("Tableavaluedefine") as Tableavaluedefine;
        //    if (t_avaluedefineList == null)
        //        return;

        //    for (int i = 0; i<t_avaluedefineList.m_configList.Count;++i)
        //    {
        //        m_avaluedefineDict.Add(t_avaluedefineList.m_configList[i].ID, t_avaluedefineList.m_configList[i]);
        //    }
        //}
        public AValueType GetValueID(string name)
        {
            string strValueName = GetValidValueName(name);
            if (m_mapValueNameToID.ContainsKey(strValueName))
            {
                return m_mapValueNameToID[strValueName];
            }
            return AValueType.err;
        }
        
        //private void InitialAValueToParamMap(int paramID, bool bWarning)
        //    {
        //        ParamDef* pDef = GetParamDef(paramID);
        //        if (pDef == NULL)
        //            return;

        //        ValueToParamMap* pMap = new ValueToParamMap();
        //        AvalonAssert(pMap);
        //        pMap->Initial(pDef, bWarning);
        //        pDef->m_pValueToParamMap = pMap;
        //    }

        private void CreateMapValueNameToID(ref Dictionary<string, AValueType> map)
        {
            //攻击属性//
            map["atk_d"] = AValueType.atk_d;                 //攻击力//
            map["atk_d_r"] = AValueType.atk_d_r;        //攻击精通//
            map["ad_skill_p"] = AValueType.ad_skill_p;      //技能固定值//
            map["ad_skill_r"] = AValueType.ad_skill_r;          //技能系数//
            map["dm"] = AValueType.dm;                  //真实//
            map["dr"] = AValueType.dr;                  //伤增//
            map["dhp"] = AValueType.dhp;                        //会心等级//
            map["dhr"] = AValueType.dhr;                //会心率//
            map["dhcr"] = AValueType.dhcr;              //会心加成//
            map["threat_r"] = AValueType.threat_r;              //仇恨效果//
            map["threat"] = AValueType.threat;              //仇恨值//
            map["counter_p"] = AValueType.counter_p;        //克制系数//
            map["counter_q"] = AValueType.counter_q;           //被克系数//	
            map["hdr"] = AValueType.hdr;           //护盾伤害效果增强//	

            //防御属性//
            map["d_ac"] = AValueType.d_ac;              //护甲//
            map["d_ac_r"] = AValueType.d_ac_r;              //护甲精通//
            map["da"] = AValueType.da;              //吸收//
            map["cdr"] = AValueType.cdr;                    //伤减//
            map["be_dhp"] = AValueType.be_dhp;              //坚韧等级//
            map["be_dhr"] = AValueType.be_dhr;          //坚韧效果//
            map["be_dhcr"] = AValueType.be_dhcr;        //会心伤害减免//

            map["hpmax"] = AValueType.hpmax;            //最大生命值//
            map["hp_r"] = AValueType.hp_r;  //生命精通//
            map["hhp_r"] = AValueType.hhp_r;    //治疗效果//
            map["be_hhp_r"] = AValueType.be_hhp_r;          //承受治疗效果//
            map["hd_r"] = AValueType.hd_r;    //护盾提升百分比//
            map["be_hd_r"] = AValueType.be_hd_r;          //受到的护盾提升百分比//
            map["epmax1"] = AValueType.epmax1;      //能量最大值1//
            map["epspr1"] = AValueType.epspr1;  //能量恢复速度1//
            map["epmax2"] = AValueType.epmax2;  //能量最大值2//
            map["epsrp2"] = AValueType.epsrp2;        //能量恢复速度2//

            //移动属性
            map["spr"] = AValueType.spr;          //移动速度加成//

            map["vpmax"] = AValueType.vpmax;              //最大精力值//
        }

        private string GetValidValueName(string name)
        {
            return name.ToLower();
        }

        public void OnLoadConfig(ConfigData data)
        {
            string valueName = data.GetString("ValueName");
            int valueId = data.GetInt("ID");

            m_avaluedefineDict.SafeAdd(valueId, data, false);
            m_mapValueNameToID.SafeAdd(valueName, (AValueType)valueId, false);
        }

        //temp
        public static AValueType[] s_FiredAttrs = new AValueType[]
        {
            AValueType.atk_d,      //攻击
            AValueType.d_ac,      //防御
            AValueType.hpmax,      //血值
            AValueType.dhp,      //暴击值
            AValueType.be_dhp,      //抗暴值
        };
    };

    public class RoleAValue
    {
        public RoleAValue()
        {
            Values = new float[(int)AValueType.Count];
            Reset(null);
        }

        public float[] Values;

        public void Reset(AValueMask pMask)
        {
            if (pMask == null)
            {// 全部复位
                for (int i = 0; i < Values.Length; ++i)
                { Values[i] = 0f; }
            }
            else
            {
                for (int i = 0; i < (int)AValueType.Count; ++i)
                {
                    if (pMask.Mask[i])
                        Values[i] = 0;
                }
            }
        }
        public void Copy(RoleAValue r)
        {
            Values = r.Values;
        }
        public void SetValue(int nValueID, float fValue)
        {
            if (nValueID >= 0 && nValueID < (int)AValueType.Count)
                Values[nValueID] += fValue;
        }
        public void AddupValue(int nValueID, float fValue)
        {
            if (nValueID >= 0 && nValueID < (int)AValueType.Count)
                Values[nValueID] += fValue;
        }

        public void Combine(RoleAValue srcXValue) // 将srcFValue的属性数值合并给自己
        {
            if (srcXValue == null)
                return;
            for (int i = 0; i < (int)AValueType.Count; ++i)
            {
                Values[i] += srcXValue.Values[i];
            }
        }

        public void Combine(RoleAValue srcXValue, AValueMask pMask)
        {
            if (srcXValue == null)
                return;
            if (pMask == null)
            {
                for (int i = 0; i < (int)AValueType.Count; ++i)
                {
                    Values[i] += srcXValue.Values[i];
                }
            }
            else
            {
                for (int i = 0; i < (int)AValueType.Count; ++i)
                {
                    if (pMask.Mask[i])
                        Values[i] += srcXValue.Values[i];
                }
            }
        }

        public float CalculateFC()
        {
            return 0;
        }

        public float CalcuteScore(bool bUseBalanceValue, bool bPlayer)
        {
            float fScoreAttr = 0;
            for (int i = 0; i < (int)AValueType.Count; ++i)
            {
                AValueInfo info = XWorldGameModule.GetGameManager<AValueDefine>().Infos[i];
                if (bUseBalanceValue)
                {
                    if (bPlayer)
                    {
                        fScoreAttr += (Values[i] - info.fBaseValueBalance) * info.FightFactor;
                    }
                }
                else
                {
                    fScoreAttr += Values[i] * info.FightFactor;
                }
            }
            return fScoreAttr;
        }
    };

}