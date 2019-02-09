/********************************************************************
	purpose:AValueStruct ������ֵ�洢�ṹ
*********************************************************************/
using XWorld.AValue;
using System.Collections.Generic;

namespace XWorld
{	
	// ��������ģ�����ݣ�Levels Ϊָ������Լ����ڴ�����
	public class AValueStruct
    {
        //friend class AValueManager;
        //friend class AValueMask;
        public long m_nGroupID;

        public Dictionary<int, AValueData> Datas;
        //��ɫ���Լ������//
        public AValueStruct()
        {
            Datas = new Dictionary<int, AValueData>();
            m_nGroupID = -1;
        }

        public void Reset(AValueMask pMask)
        {
            if (null == pMask)
            { Datas.Clear(); return; }
            for (int i = 0; i < pMask.Mask.Length; ++i)
            {
                if (pMask.Mask[i])
                {//���ñ������������
                    Datas.Remove(i);
                }
            }
        }

        public void SetValueData(int nValueID,ref int nLevel, ref int nMode, ref float absoluteValue, ref float percentValue)
        {
            AValueData pData = GetValueData(nValueID, true);
            if (null != pData)
            {
                pData.AddupBaseValue( nLevel,  absoluteValue);
                pData.AddupPercentValue( nLevel,  nMode, percentValue);
            }
        }

        public void SetValueData(int nValueID, int nLevel, int nMode, float absoluteValue, float percentValue)
        {
            AValueData pData = GetValueData(nValueID, true);
            if (null != pData)
            {
                pData.AddupBaseValue( nLevel,  absoluteValue);
                pData.AddupPercentValue( nLevel,  nMode,  percentValue);
            }
        }


        public AValueData GetValueData(int nValueID, bool bCreateNew)
        {
            if (nValueID < 0 || nValueID >= (int)AValueType.Count)
                return null;

            if (!Datas.ContainsKey(nValueID))
            {
                if (bCreateNew)
                {
                    ConfigData info = XWorldGameModule.GetGameManager<AValueDefine>().GetAValueInfo(nValueID);
                    if (null == info)
                        return null;
                    AValueData pData = new AValueData(info.GetInt("MaxDepth"));
                    Datas.Add(nValueID, pData);
                    return pData;
                }
                else
                    return null;
            }
            else
                return Datas[nValueID];
        }

        public void Combine(AValueStruct pVS, AValueMask pMask, bool bLinear) // ��pStruct��������ֵ�ϲ����Լ�
        {
            if (null == pVS) return;

            if (null != pMask)
            {
                CombineWithMask(pVS, pMask, bLinear);
                return;
            }

            foreach (KeyValuePair<int, AValueData> item in pVS.Datas)
            {
                AValueData pDstData = GetValueData(item.Key, true);
                if (null != pDstData)
                    pDstData.Combine(item.Value, ref bLinear);
            }
        }

        void CombineWithMask(AValueStruct pVS, AValueMask pMask, bool bLinear) // ��pStruct��������ֵ�ϲ����Լ�
        {
            foreach (KeyValuePair<int, AValueData> item in pVS.Datas)
            {
                if (pMask.Mask[item.Key])
                {
                    AValueData pDstData = GetValueData(item.Key, true);
                    if (null != pDstData )
                        pDstData.Combine(item.Value, ref bLinear);
                }
            }
        }

        void Enhance(AValueStruct pVS, AValueMask pMask, bool bLinear) // ��pStruct��������ֵǿ�����Լ�
        {
            if (null == pVS) return;
            if (null != pMask)
            {
                EnhanceWithMask(pVS, pMask, bLinear);
                return;
            }

            for (int i = 0; i < (int)AValueType.Count; i++)
            {
                AValueData pDstData = GetValueData(i, false);
                if (null != pDstData )
                {
                    // ���װ����������ڣ�ǿ�����Լ���Ч��
                    AValueData pSrcData = pVS.GetValueData(i, false);
                    if (null != pSrcData)
                    {
                        pDstData.Combine(pSrcData, ref bLinear);
                    }
                }
            }
        }

        void EnhanceWithMask(AValueStruct pVS, AValueMask pMask, bool bLinear)
        {
            for (int i = 0; i < (int)AValueType.Count; i++)
            {
                if (pMask.Mask[i])
                {// �������ֵ�Ž��кϲ�
                    AValueData pDstData = GetValueData(i, false);
                    if (null != pDstData)
                    {// ���װ����������ڣ�ǿ�����Լ���Ч��
                        AValueData pSrcData = pVS.GetValueData(i, false);
                        if (null != pSrcData)
                        {
                            pDstData.Combine(pSrcData, ref bLinear);
                        }
                    }
                }
            }
        }

        void CalculateOne(RoleAValue result, AValueMask pMask, int valueID, RoleAValue pTransformAV)
        {
            float extraBase = null == pTransformAV ? 0.0f : pTransformAV.Values[valueID];
            AValueData pData = GetValueData(valueID, false);
            if (null != pData)
            {
                result.Values[valueID] = pData.Calculate(false, extraBase);
            }
            else
            {
                result.Values[valueID] = extraBase;
            }
        }
        void CalculateOne(RoleAValue result, AValueMask pMask, int valueID, AValueData pData, RoleAValue pTransformAV)
        {
            float extraBase = null == pTransformAV ? 0.0f : pTransformAV.Values[valueID];
            if (null != pData)
            {
                result.Values[valueID] = pData.Calculate(false, extraBase);
            }
            else
            {
                result.Values[valueID] = extraBase;
            }
        }

        public void Calculate(RoleAValue result, AValueMask pMask, int groupFlag = (int)AValue_GroupType.AValue_GroupBoth, RoleAValue pTransformAV = null)
        {
            int locali = (int)AValue_GroupType.AValue_GroupExtend;
            locali = (groupFlag & locali);
            if (locali > 0)
            {
                foreach (KeyValuePair<int, AValueData> item in Datas)
                {
                    UnityEngine.Debug.Log("item = " + item);
                    CalculateOne(result, pMask, item.Key, item.Value, pTransformAV);
                }
            }
        }

        void CalculatePrivateValue()
        {
            for (int i = 0; i < (int)AValueType.Count; i++)
            {
                AValueData pData = GetValueData(i, false);
                if (null != pData)
                {
                    pData.Calculate(true);
                }
            }
        }
	};

	class AValueLoadManager : XWorldGameManagerBase
    {// �������Լ��� ������������ɼ���
        public Dictionary<int, AValueStruct> m_AValueMap;
        public Dictionary<int, AValueMask> m_vMaskMap;
        public Dictionary<int, List<ConfigData>> m_vConfigDatas = new Dictionary<int, List<ConfigData>>();

        public ConfigData[] GetConfigDatas(int dataId)
        {
            if (m_vConfigDatas.ContainsKey(dataId))
            {
                return m_vConfigDatas[dataId].ToArray();
            }
            return null;
        }

        public ConfigData GetConfigData(int dataId, int index)
        {
            ConfigData[] datas = GetConfigDatas(dataId);
            if (datas != null && datas.Length > index)
            {
                return datas[index];
            }
            return null;
        }

        public ConfigData GetConfigData(int dataId)
        {
            return GetConfigData(dataId, 0);
        }

        /// <summary>
        /// ��ʼ��������
        /// </summary>
        public override void InitManager()
        {
            m_AValueMap = new Dictionary<int, AValueStruct>();
            m_vMaskMap = new Dictionary<int, AValueMask>();
            //LoadAValueItemAttr();
        }

        public void OnLoadConfig(ConfigData data)
        {
            int dataID = data.GetInt("DataID");
            m_vConfigDatas.ForceAddList(dataID, data);

            AValueStruct pStruct = GetAValue(dataID, 0, true);
            if (null == pStruct)
                return;

            int nValueID = (int)XWorldGameModule.GetGameManager<AValueDefine>().GetValueID(data.GetString("ValueName"));
            if (nValueID < 0)
                return;
            int ComputeLevel = data.GetInt("ComputeLevel");
            int ComputeMode = data.GetInt("ComputeMode");
            float AbsoluteValue = data.GetFloat("AbsoluteValue");
            float PercentValue = data.GetFloat("PercentValue");
            pStruct.SetValueData(nValueID, ComputeLevel, ComputeMode, AbsoluteValue, PercentValue);

            AValueMask pMask = GetAValueMask(dataID, true);
            if (null == pMask)
            {
                pMask.Combine(pStruct);
            }
        }
        /// <summary>
        /// ���� �߼���Ƭʱ��
        /// </summary>
        /// <param name="fElapseTimes"></param>
        public override void Update(float fElapseTimes) { }

        /// <summary>
        /// ��Ϸ�˳�
        /// </summary>
        public override void ShutDown() { }

        /// <summary>
        /// ���������Ժ�Ĵ���
        /// </summary>
        public override void OnReEnterGame() { }

        /// <summary>
        /// �����¼���̵Ĵ���
        /// </summary>
        public override void OnEnterLogin() { }

        /// <summary>
        /// ��һ�ν�����Ϸ�Ĵ���
        /// </summary>
        public override void OnEnterGame() { }
        /////////////////////////////////////////////////////

        public int MakeID(int nDataID, int nSubID)
        {
            int x = nDataID << 16;
            return (x) + nSubID;
        }

        public void Clear()
        {
            m_AValueMap.Clear();
            m_vMaskMap.Clear();
        }

        public virtual void LoadAValueItemAttr() { }

        public void CalcOriginResult(AValueStruct vs, AValueMask pMask,ref RoleAValue av)
        {
            av.Reset(pMask);
            vs.Calculate(av, pMask);
        }

        public AValueStruct GetAValue(int nDataID, int nSubID = 0, bool bCreate = false)
        {
            int nID = MakeID(nDataID, nSubID);
            if (!m_AValueMap.ContainsKey(nID))
            {
                if (bCreate)
                {
                    AValueStruct pRes = new AValueStruct();
                    pRes.m_nGroupID = nID;
                    m_AValueMap[nID] = pRes;
                    return pRes;
                }
                return null;
            }
            return m_AValueMap[nID];
        }

        public AValueMask GetAValueMask(int nDataID, bool bCreate = false)
        {
            if (!m_vMaskMap.ContainsKey(nDataID))
            {
                if (bCreate)
                {
                    AValueMask pRes = new AValueMask();
                    m_vMaskMap[nDataID] = pRes;
                    return pRes;
                }
                return null;
            }
            return m_vMaskMap[nDataID];
        }
	};
}