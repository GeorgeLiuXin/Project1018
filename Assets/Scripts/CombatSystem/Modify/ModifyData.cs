using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace XWorld
{
    #region typedef
    public class ModifyItem
    {
        public int m_DataID = 0;
        public int m_ModifyID = 0;
        public int m_ValueID = 0;
        public int m_IValue = 0;
        public float m_FValue = 0;
        public float m_Precent = 0;
    }

    public class ModifyList : List<ModifyItem>
    {

    }

    public class ModifyData : Dictionary<int, ModifyList>
    {
        public void AddModifyItem(ModifyItem modifyItem)
        {
            ModifyList modifyList = null;
            if (!TryGetValue(modifyItem.m_ModifyID, out modifyList))
            {
                modifyList = new ModifyList();
                Add(modifyItem.m_ModifyID, modifyList);
            }
            modifyList.Add(modifyItem);
        }
        public ModifyList GetModifyList(int modifyID)
        {
            ModifyList modifyList = null;
            if (TryGetValue(modifyID, out modifyList))
                return modifyList;
            return null;
        }
    }

    public class ModifyDict : Dictionary<int, ModifyData>
    {
        public void AddModifyItem(ModifyItem modifyItem)
        {
            ModifyData modifyData = null;
            if (!TryGetValue(modifyItem.m_DataID, out modifyData))
            {
                modifyData = new ModifyData();
                Add(modifyItem.m_DataID, modifyData);
            }
            modifyData.AddModifyItem(modifyItem);
        }

        public ModifyData GetModifyData(int dataID)
        {
            ModifyData modifyData = null;
            if (TryGetValue(dataID, out modifyData))
                return modifyData;
            return null;
        }

        public ModifyList GetModifyList(int dataID, int modifyID)
        {
            ModifyData modifyData = GetModifyData(dataID);
            if (modifyData == null)
                return null;

            return modifyData.GetModifyList(modifyID);
        }
    }
    #endregion

    public abstract class ModifyObject<T> : System.ICloneable
    {
        protected static System.Reflection.FieldInfo[] m_DataFields = null;

        protected static Dictionary<string, int> m_ModifyDefine = null; //Enum_name, Enum_index
        protected static Dictionary<int, int> m_ModifyValue = null; //Enum_index, Index
        protected static Dictionary<int, eModifyLogic> m_ModifyLogic = null; //Enum_index，ModifyLogic

        public int m_nLevel = 1;
        public int m_nSlots = 0;
        public int m_nSlotsMask = 0;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public virtual int DataID
        {
            get { return 0; }
        }

        public virtual void OnLoadData(ConfigData data)
        {
            foreach(KeyValuePair<string, int> item in m_ModifyDefine)
            {
                int _index = GetModifyIndex(item.Value);
                object _value = GetValue(_index);
                if (_value == null)
                    continue;

                if (_value.GetType() == typeof(int))
                {
                    SetValue(_index, data.GetInt(item.Key));
                }
                else if (_value.GetType() == typeof(float))
                {
                    SetValue(_index, data.GetFloat(item.Key));
                }
            }        
        }

        public static eModifyLogic GetModifyLogic(int valueID)
        {
            eModifyLogic eLogic;
            if (m_ModifyLogic.TryGetValue(valueID, out eLogic))
                return eLogic;

            return eModifyLogic.Null;
        }
        public static int GetModifyValueID(string name)
        {
            int _id;
            if (m_ModifyDefine.TryGetValue(name, out _id))
                return _id;
            return -1;
        }
        public static int GetModifyIndex(int valueID)
        {
            int _index;
            if (m_ModifyValue.TryGetValue(valueID, out _index))
                return _index;           
            return -1;
        }

        public void SetValue(int i, object _value)
        {
            if (i >= 0 && i < m_DataFields.Length)
            {
                m_DataFields[i].SetValue(this, _value);
            }
        }
        public object GetValue(int i)
        {
            if (i >= 0 && i < m_DataFields.Length)
            {
                return m_DataFields[i].GetValue(this);
            }
            return null;
        }
        public void Combine(ModifyList data)
        {
            if (data == null)
                return;

            foreach(ModifyItem item in data)
            {
                if (item != null)
                {
                    Combine(item.m_ValueID, item.m_IValue, item.m_FValue);
                }
            }
        }
		public void Combine(int nValueID, int nValue, float fValue)
        {
            eModifyLogic eLogic = GetModifyLogic(nValueID);
            if (eLogic == eModifyLogic.Null)
                return;

            int _index = GetModifyIndex(nValueID);
            object _value = GetValue(_index);
            if (_value == null)
                return;

            if (eLogic == eModifyLogic.Sum) //加减
            {
                if (_value.GetType() == typeof(int))
                {
                    nValue += (int)_value;
                    SetValue(_index, nValue);
                }
                else if (_value.GetType() == typeof(float))
                {
                    fValue += (float)_value;
                    SetValue(_index, fValue);
                }
            }
            else if (eLogic == eModifyLogic.REP) //替换
            {
                if (_value.GetType() == typeof(int))
                {
                    SetValue(_index, nValue);
                }
                else if (_value.GetType() == typeof(float))
                {
                    SetValue(_index, fValue);
                }
            }
            else if (eLogic == eModifyLogic.NOR) //或非
            {
                if (_value.GetType() == typeof(int))
                {
                    int nCurValue = (int)_value;
                    if ((nCurValue & nValue) > 0)
                        nCurValue ^= nCurValue & nValue;
                    else
                        nCurValue |= nValue;

                    SetValue(_index, nValue);
                }
            }
        }
    }
}