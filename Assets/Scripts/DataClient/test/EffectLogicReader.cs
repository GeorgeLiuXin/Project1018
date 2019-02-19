using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Xml;
using Extension;

namespace Galaxy
{
    public interface IXmlOperation
    {
        string m_XmlFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// 创建xml文件
        /// </summary>
        void CreateXml();

        /// <summary>
        /// 更新xml数据
        /// </summary>
        /// <param name="data">数据</param>
        void AddXml(EffectLogicParamData data);
        /// <summary>
        /// 更新xml数据
        /// </summary>
        /// <param name="data">数据</param>
        void AddXml(Dictionary<int, EffectLogicParamData> datadict);

        /// <summary>
        /// 更新xml数据
        /// </summary>
        /// <param name="data">数据</param>
        void UpdateXml(EffectLogicParamData data);
        /// <summary>
        /// 更新xml数据
        /// </summary>
        /// <param name="data">数据</param>
        void UpdateXml(Dictionary<int, EffectLogicParamData> datadict);
        
        /// <summary>
        /// 读取xml
        /// </summary>
        /// <param name="dict"></param>
        void ReadXml(ref Dictionary<int, EffectLogicParamData> dict);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        void DeleteXml(string id);
        /// <summary>
        /// 删除所有数据
        /// </summary>
        void DeleteAllXml();

    }

    public class XmlBase
    {
        public string m_XmlFilePath
        {
            get;
            set;
        }

        public XmlBase(string filePath)
        {
            if (filePath.IsNE())
            {
                GameLogger.DebugLog(LOG_CHANNEL.ERROR, "当前XML路径为空");
                return;
            }

            m_XmlFilePath = filePath;
        }
    }

    //XML 表现效果读取方式
    public class EffectLogicReader : XmlBase, IXmlOperation
    {
        private readonly string sRoot = "root";
        private readonly string sDes = "sDes";
        private readonly string sCurid = "curid";

        public EffectLogicReader(string filePath) : base(filePath)
        {
            CheckXml();
        }
        public void CheckXml()
        {
            string path = m_XmlFilePath;
            if (!File.Exists(path))
            {
                GameLogger.DebugLog(LOG_CHANNEL.ERROR, "当前路径下没有对应的XML文件: " + path);
                CreateXml();
            }
        }

        public string GetPropertyValue(EffectLogicParamItem item)
        {
            return item.sValue;
        }

        public void CreateXml()
        {
            if (!File.Exists(m_XmlFilePath))
            {
                // 创建xml文档实例
                XmlDocument xmlDoc = new XmlDocument();
                // 创建根节点
                XmlElement root = xmlDoc.CreateElement(sRoot);
                root.SetAttribute(sCurid, "0");
                xmlDoc.AppendChild(root);

                xmlDoc.Save(m_XmlFilePath);
                GameLogger.DebugLog(LOG_CHANNEL.LOGIC, "已创建: " + m_XmlFilePath);
            }
        }

        public void AddXml(EffectLogicParamData data)
        {
            if (File.Exists(m_XmlFilePath))
            {
                // xml文档实例
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlFilePath);
                // 获取根节点
                XmlElement root = xmlDoc.SelectSingleNode(sRoot) as XmlElement;
                if (root == null)
                    return;

                int id = Convert.ToInt32(root.GetAttribute(sCurid));

                id++;
                //创建整个表现节点
                XmlElement curParentElm = xmlDoc.CreateElement(id.ToString());
                curParentElm.SetAttribute(sDes, data.sDescribe);

                foreach (EffectLogicParamList _class in data)
                {
                    //创建单个表现类型
                    XmlElement elm = xmlDoc.CreateElement(_class.sLogicName);
                    foreach (EffectLogicParamItem _property in _class)
                    {
                        XmlElement elmProperty = xmlDoc.CreateElement(_property.sName);
                        elmProperty.SetAttribute("Type", _property.sType);
                        elmProperty.InnerText = GetPropertyValue(_property);
                        elm.AppendChild(elmProperty);
                    }
                    curParentElm.AppendChild(elm);
                }
                root.AppendChild(curParentElm);

                root.SetAttribute(sCurid, id.ToString());
                xmlDoc.Save(m_XmlFilePath);
            }
        }
        public void AddXml(Dictionary<int, EffectLogicParamData> datadict)
        {
            if (File.Exists(m_XmlFilePath))
            {
                // xml文档实例
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlFilePath);
                // 获取根节点
                XmlElement root = xmlDoc.SelectSingleNode(sRoot) as XmlElement;
                if (root == null)
                    return;

                int id = Convert.ToInt32(root.GetAttribute(sCurid));

                foreach (KeyValuePair<int, EffectLogicParamData> pair in datadict)
                {
                    id = Mathf.Max(id, pair.Key);
                    //创建整个表现节点
                    XmlElement curParentElm = xmlDoc.CreateElement(pair.Key.ToString());
                    curParentElm.SetAttribute(sDes, pair.Value.sDescribe);

                    foreach (EffectLogicParamList _class in pair.Value)
                    {
                        //创建单个表现类型
                        XmlElement elm = xmlDoc.CreateElement(_class.sLogicName);
                        foreach (EffectLogicParamItem _property in _class)
                        {
                            XmlElement elmProperty = xmlDoc.CreateElement(_property.sName);
                            elmProperty.SetAttribute("Type", _property.sType);
                            elmProperty.InnerText = GetPropertyValue(_property);
                            elm.AppendChild(elmProperty);
                        }
                        curParentElm.AppendChild(elm);
                    }
                    root.AppendChild(curParentElm);
                }
                id++;

                root.SetAttribute(sCurid, id.ToString());
                xmlDoc.Save(m_XmlFilePath);
            }
        }

        public void UpdateXml(EffectLogicParamData data)
        {
            if (File.Exists(m_XmlFilePath))
            {
                // xml文档实例
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlFilePath);
                // 获取根节点
                XmlElement root = xmlDoc.SelectSingleNode(sRoot) as XmlElement;
                if (root == null)
                    return;

                XmlElement curParentElm = root.SelectSingleNode(data.iIndex.ToString()) as XmlElement;
                if (curParentElm == null)
                    return;

                curParentElm.SetAttribute(sDes, data.sDescribe);
                foreach (EffectLogicParamList _class in data)
                {
                    bool bClassCreate = false;
                    XmlElement elmClass;
                    elmClass = root.SelectSingleNode(_class.sLogicName) as XmlElement;
                    if (elmClass == null)
                    {
                        elmClass = xmlDoc.CreateElement(_class.sLogicName);
                        bClassCreate = true;
                    }
                    
                    foreach (EffectLogicParamItem _property in _class)
                    {
                        bool bPropertyCreate = false;
                        XmlElement elmProperty;
                        elmProperty = elmClass.SelectSingleNode(_property.sName) as XmlElement;
                        if (elmProperty == null)
                        {
                            elmProperty = xmlDoc.CreateElement(_property.sName);
                            bPropertyCreate = true;
                        }

                        elmProperty.SetAttribute("Type", _property.sType);
                        elmProperty.InnerText = GetPropertyValue(_property);

                        if (bPropertyCreate)
                            elmClass.AppendChild(elmProperty);
                    }

                    if (bClassCreate)
                        curParentElm.AppendChild(elmClass);
                }

                xmlDoc.Save(m_XmlFilePath);
            }
        }
        public void UpdateXml(Dictionary<int, EffectLogicParamData> datadict)
        {
            if (File.Exists(m_XmlFilePath))
            {
                // xml文档实例
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlFilePath);
                // 获取根节点
                XmlElement root = xmlDoc.SelectSingleNode(sRoot) as XmlElement;
                if (root == null)
                    return;

                foreach (KeyValuePair<int, EffectLogicParamData> pair in datadict)
                {
                    bool bDataCreate = false;
                    XmlElement dataElm = root.SelectSingleNode(pair.Key.ToString()) as XmlElement;
                    if (dataElm == null)
                    {
                        dataElm = xmlDoc.CreateElement(pair.Key.ToString());
                        bDataCreate = true;
                    }
                    dataElm.SetAttribute(sDes, pair.Value.sDescribe);
                    foreach (EffectLogicParamList _class in pair.Value)
                    {
                        bool bClassCreate = false;
                        XmlElement elmClass;
                        elmClass = root.SelectSingleNode(_class.sLogicName) as XmlElement;
                        if (elmClass == null)
                        {
                            elmClass = xmlDoc.CreateElement(_class.sLogicName);
                            bClassCreate = true;
                        }

                        foreach (EffectLogicParamItem _property in _class)
                        {
                            bool bPropertyCreate = false;
                            XmlElement elmProperty;
                            elmProperty = elmClass.SelectSingleNode(_property.sName) as XmlElement;
                            if (elmProperty == null)
                            {
                                elmProperty = xmlDoc.CreateElement(_property.sName);
                                bPropertyCreate = true;
                            }

                            elmProperty.SetAttribute("Type", _property.sType);
                            elmProperty.InnerText = GetPropertyValue(_property);

                            if (bPropertyCreate)
                                elmClass.AppendChild(elmProperty);
                        }

                        if (bClassCreate)
                            dataElm.AppendChild(elmClass);
                    }

                    if (bDataCreate)
                        root.AppendChild(dataElm);
                }
                xmlDoc.Save(m_XmlFilePath);
            }
        }
        
        public void ReadXml(ref Dictionary<int, EffectLogicParamData> dict)
        {
            if (File.Exists(m_XmlFilePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlFilePath);
                XmlElement root = xmlDoc.SelectSingleNode(sRoot) as XmlElement;
                if (root == null)
                    return;

                foreach (XmlElement xml_data in root)
                {
                    if (xml_data == null)
                        continue;

                    EffectLogicParamData _data = new EffectLogicParamData();
                    _data.iIndex = Convert.ToInt32(xml_data.LocalName);
                    _data.sDescribe = xml_data.GetAttribute(sDes);

                    foreach (XmlElement xml_class in xml_data)
                    {
                        EffectLogicParamList _class = new EffectLogicParamList();
                        _class.sLogicName = xml_class.LocalName;

                        foreach (XmlElement xml_property in xml_class)
                        {
                            EffectLogicParamItem _property = new EffectLogicParamItem();
                            _property.sName = xml_property.LocalName;
                            _property.sType = xml_property.GetAttribute("Type");
                            _property.sValue = xml_property.InnerText;

                            _class.Add(_property);
                        }

                        _data.Add(_class);
                    }

                    if (dict.ContainsKey(_data.iIndex))
                    {
                        dict.Remove(_data.iIndex);
                    }
                    dict.Add(_data.iIndex, _data);
                }
            }
        }

        public void DeleteXml(string id)
        {
            if (File.Exists(m_XmlFilePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlFilePath);
                XmlNode rootNode = xmlDoc.SelectSingleNode(sRoot);
                if (rootNode == null)
                    return;

                XmlNode xe = rootNode.SelectSingleNode("root/" + id);
                if (xe == null)
                    return;

                rootNode.RemoveChild(xe);

                xmlDoc.Save(m_XmlFilePath);
            }
        }
        public void DeleteAllXml()
        {
            if (File.Exists(m_XmlFilePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlFilePath);
                XmlNode rootNode = xmlDoc.SelectSingleNode(sRoot);
                if (rootNode == null)
                    return;

                rootNode.RemoveAll();

                xmlDoc.Save(m_XmlFilePath);
            }
        }

    }

}