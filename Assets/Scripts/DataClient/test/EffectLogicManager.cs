using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Galaxy.XmlData;

namespace Galaxy
{
    /// <summary>
    /// 表现效果id To 效果集合  classDict
    /// </summary>
    public class EffectLogicManager : GalaxyGameManagerBase
    {
        private PerformanceLogicFactory factory;
        private EffectLogicReader reader;
        private Dictionary<int, XmlDataList> m_dict;

        private Dictionary<int, List<long>> m_AvatarOwnLogicDict;
        private Dictionary<long, List<PerformanceLogic>> m_PerformanceLogicDict;
        private List<long> m_RemoveList;

        public override void InitManager()
        {
            factory = new PerformanceLogicFactory();
            reader = new EffectLogicReader();
            InitDataDict();

            m_AvatarOwnLogicDict = new Dictionary<int, List<long>>();
            m_PerformanceLogicDict = new Dictionary<long, List<PerformanceLogic>>();
            m_RemoveList = new List<long>();
        }
        private void InitDataDict()
        {
            m_dict = new Dictionary<int, XmlDataList>();
            reader.ReadXml(ref m_dict);
        }

        public override void ShutDown()
        {

        }

        public override void Update(float fElapseTimes)
        {
            if (m_PerformanceLogicDict == null)
                return;

            foreach (KeyValuePair<long, List<PerformanceLogic>> vList in m_PerformanceLogicDict)
            {
                for (int i = vList.Value.Count - 1; i >= 0; i--)
                {
                    if (vList.Value[i] == null)
                    {
                        vList.Value.RemoveAt(i);
                        continue;
                    }

                    vList.Value[i].Tick(fElapseTimes);
                    if (vList.Value[i].IsDestroy())
                    {
                        vList.Value.RemoveAt(i);
                        continue;
                    }
                }
                if (vList.Value.Count == 0)
                {
                    m_RemoveList.Add(vList.Key);
                }
            }

            if (m_RemoveList.Count != 0)
            {
                foreach (var key in m_RemoveList)
                {
                    m_PerformanceLogicDict.Remove(key);
                }
                m_RemoveList.Clear();
            }
        }

        /// <summary>
        /// 开始某个表现效果
        /// </summary>
        /// <param name="nAvatarID">目标server ID</param>
        /// <param name="nPerformanceLogicID">表现效果逻辑ID</param>
        /// <param name="longID">当前表现效果唯一ID</param>
        /// <returns></returns>
        public bool StartPerformanceLogic(int nAvatarID, int nPerformanceLogicID, ref long longID)
        {
            if (m_dict == null || !m_dict.ContainsKey(nPerformanceLogicID))
                return false;

            List<PerformanceLogic> list = new List<PerformanceLogic>();
            XmlDataList dataList = m_dict[nPerformanceLogicID];
            foreach (XmlClassData data in dataList)
            {
                PerformanceLogic logic = factory.GetPerformanceLogic(data.sLogicName);
                logic.SetOwner(nAvatarID);
                SetLogicFieldInfo(logic, data);
                logic.Init();
                list.Add(logic);
            }
            longID = GTime.Current.MilliSecond() << 32 + nAvatarID;
            m_PerformanceLogicDict.Add(longID, list);
            AddPerformanceLogicToAvatar(nAvatarID, longID);
            return true;
        }
        /// <summary>
        /// 开始某个表现效果
        /// </summary>
        /// <param name="nAvatarID">目标server ID</param>
        /// <param name="nPerformanceLogicID">表现效果逻辑ID</param>
        /// <param name="fTime">当前效果共持续多久</param>
        /// <param name="longID">当前表现效果唯一ID</param>
        /// <returns></returns>
        public bool StartPerformanceLogic(int nAvatarID, int nPerformanceLogicID, float fTime, ref long longID)
        {
            if (m_dict == null || !m_dict.ContainsKey(nPerformanceLogicID))
                return false;

            List<PerformanceLogic> list = new List<PerformanceLogic>();
            XmlDataList dataList = m_dict[nPerformanceLogicID];
            foreach (XmlClassData data in dataList)
            {
                PerformanceLogic logic = factory.GetPerformanceLogic(data.sLogicName);
                logic.SetOwner(nAvatarID);
                SetLogicFieldInfo(logic, data);
                logic.Init();
                logic.SetTotalTime(fTime);
                list.Add(logic);
            }
            longID = GTime.Current.MilliSecond() << 32 + nAvatarID;
            m_PerformanceLogicDict.Add(longID, list);
            AddPerformanceLogicToAvatar(nAvatarID, longID);
            return true;
        }
        /// <summary>
        /// 开始某个表现效果
        /// </summary>
        /// <param name="nAvatarID">目标server ID</param>
        /// <param name="nPerformanceLogicID">表现效果逻辑ID</param>
        /// <param name="longID">当前表现效果唯一ID</param>
        /// <param name="values">相关参数</param>
        /// <returns></returns>
        public bool StartPerformanceLogic(int nAvatarID, int nPerformanceLogicID, ref long longID, params object[] values)
        {
            if (m_dict == null || !m_dict.ContainsKey(nPerformanceLogicID))
                return false;

            List<PerformanceLogic> list = new List<PerformanceLogic>();
            XmlDataList dataList = m_dict[nPerformanceLogicID];
            foreach (XmlClassData data in dataList)
            {
                PerformanceLogic logic = factory.GetPerformanceLogic(data.sLogicName);
                logic.SetOwner(nAvatarID);
                SetLogicFieldInfo(logic, data);
                logic.Init(values);
                list.Add(logic);
            }
            longID = GTime.Current.MilliSecond() << 32 + nAvatarID;
            m_PerformanceLogicDict.Add(longID, list);
            AddPerformanceLogicToAvatar(nAvatarID, longID);
            return true;
        }
        /// <summary>
        /// 开始某个表现效果
        /// </summary>
        /// <param name="nAvatarID">目标server ID</param>
        /// <param name="nPerformanceLogicID">表现效果逻辑ID</param>
        /// <param name="fTime">当前效果共持续多久</param>
        /// <param name="longID">当前表现效果唯一ID</param>
        /// <param name="values">相关参数</param>
        /// <returns></returns>
        public bool StartPerformanceLogic(int nAvatarID, int nPerformanceLogicID, float fTime, ref long longID, params object[] values)
        {
            if (m_dict == null || !m_dict.ContainsKey(nPerformanceLogicID))
                return false;

            List<PerformanceLogic> list = new List<PerformanceLogic>();
            XmlDataList dataList = m_dict[nPerformanceLogicID];
            foreach (XmlClassData data in dataList)
            {
                PerformanceLogic logic = factory.GetPerformanceLogic(data.sLogicName);
                logic.SetOwner(nAvatarID);
                SetLogicFieldInfo(logic, data);
                logic.Init(values);
                logic.SetTotalTime(fTime);
                list.Add(logic);
            }
            longID = GTime.Current.MilliSecond() << 32 + nAvatarID;
            m_PerformanceLogicDict.Add(longID, list);
            AddPerformanceLogicToAvatar(nAvatarID, longID);
            return true;
        }

        private void SetLogicFieldInfo(PerformanceLogic logic, XmlClassData data)
        {
            System.Reflection.FieldInfo field;
            foreach (var item in data)
            {
                field = logic.GetType().GetField(item.sName);
                if (field != null)
                {
                    if (item.sType.Equals("System.Boolean"))
                    {
                        field.SetValue(logic, Convert.ToBoolean(item.sValue));
                    }
                    else if (item.sType.Equals("System.Int32"))
                    {
                        field.SetValue(logic, Convert.ToInt32(item.sValue));
                    }
                    else if (item.sType.Equals("System.Float"))
                    {
                        field.SetValue(logic, Convert.ToSingle(item.sValue));
                    }
                    else if (item.sType.Equals("System.String"))
                    {
                        field.SetValue(logic, item.sValue);
                    }
                }
            }
        }

        /// <summary>
        /// 结束某个表现效果(根据玩家清除所有的对应表现)
        /// </summary>
        /// <param name="nAvatarID">玩家Server ID</param>
        public void EndPerformanceLogic(int nAvatarID)
        {
            if (m_AvatarOwnLogicDict == null || !m_AvatarOwnLogicDict.ContainsKey(nAvatarID))
                return;

            foreach (var item in m_AvatarOwnLogicDict[nAvatarID])
            {
                EndPerformanceLogic(item);
            }
        }
        /// <summary>
        /// 结束某个表现效果
        /// </summary>
        /// <param name="longID">表现效果唯一ID</param>
        public void EndPerformanceLogic(long longID)
        {
            if (m_PerformanceLogicDict == null || !m_PerformanceLogicDict.ContainsKey(longID))
                return;

            foreach (var item in m_PerformanceLogicDict[longID])
            {
                item.Destroy();
            }
        }

        private void AddPerformanceLogicToAvatar(int nAvatarID, long longID)
        {
            if (m_AvatarOwnLogicDict == null)
                return;
            List<long> logicList;
            if (m_AvatarOwnLogicDict.ContainsKey(nAvatarID))
            {
                logicList = m_AvatarOwnLogicDict[nAvatarID];
                if (!logicList.Contains(longID))
                {
                    logicList.Add(longID);
                }
            }
            else
            {
                logicList = new List<long>();
                logicList.Add(longID);
                m_AvatarOwnLogicDict.Add(nAvatarID, logicList);
            }
        }
        private void RemovePerformanceLogicToAvatar(int nAvatarID, long longID)
        {
            if (m_AvatarOwnLogicDict == null
                || !m_AvatarOwnLogicDict.ContainsKey(nAvatarID)
                || !m_AvatarOwnLogicDict[nAvatarID].Contains(longID))
                return;

            m_AvatarOwnLogicDict[nAvatarID].Remove(longID);
        }

        public void ReloadDataDict()
        {
            m_dict.Clear();
            InitDataDict();
        }

    }
}