using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace Galaxy.XmlData
{
    //XML读取
    public class XmlReaderBase : XmlBase, IXmlOperation
    {
        protected readonly string sRoot = "root";
        protected readonly string iIndex = "iIndex";
        protected readonly string sDes = "sDes";
        protected readonly string sCurid = "curid";

        protected readonly string sPreName = "ID_";

        public XmlReaderBase(string filePath) : base(filePath)
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

        public string GetPropertyValue(XmlParamItem item)
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

        public void AddXml(XmlDataList data)
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
                XmlElement curParentElm = xmlDoc.CreateElement(sPreName + id.ToString());
                curParentElm.SetAttribute(iIndex, id.ToString());
                curParentElm.SetAttribute(sDes, data.sDescribe);

                foreach (XmlClassData _class in data)
                {
                    //创建单个表现类型
                    XmlElement elm = xmlDoc.CreateElement(_class.sLogicName);
                    foreach (XmlParamItem _property in _class)
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
        public void AddXml(Dictionary<int, XmlDataList> datadict)
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

                foreach (KeyValuePair<int, XmlDataList> pair in datadict)
                {
                    id = Mathf.Max(id, pair.Key);
                    //创建整个表现节点
                    XmlElement curParentElm = xmlDoc.CreateElement(sPreName + pair.Key.ToString());
                    curParentElm.SetAttribute(iIndex, pair.Key.ToString());
                    curParentElm.SetAttribute(sDes, pair.Value.sDescribe);

                    foreach (XmlClassData _class in pair.Value)
                    {
                        //创建单个表现类型
                        XmlElement elm = xmlDoc.CreateElement(_class.sLogicName);
                        foreach (XmlParamItem _property in _class)
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

        public void UpdateXml(XmlDataList data)
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

                XmlElement curParentElm = root.SelectSingleNode(sPreName + data.iIndex.ToString()) as XmlElement;
                if (curParentElm == null)
                    return;

                curParentElm.SetAttribute(iIndex, data.iIndex.ToString());
                curParentElm.SetAttribute(sDes, data.sDescribe);
                foreach (XmlClassData _class in data)
                {
                    bool bClassCreate = false;
                    XmlElement elmClass;
                    elmClass = curParentElm.SelectSingleNode(_class.sLogicName) as XmlElement;
                    if (elmClass == null)
                    {
                        elmClass = xmlDoc.CreateElement(_class.sLogicName);
                        bClassCreate = true;
                    }

                    foreach (XmlParamItem _property in _class)
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
        public void UpdateXml(Dictionary<int, XmlDataList> datadict)
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

                int maxID = 0;

                foreach (KeyValuePair<int, XmlDataList> pair in datadict)
                {
                    bool bDataCreate = false;
                    XmlElement dataElm = root.SelectSingleNode(sPreName + pair.Key.ToString()) as XmlElement;
                    if (dataElm == null)
                    {
                        dataElm = xmlDoc.CreateElement(sPreName + pair.Key.ToString());
                        bDataCreate = true;
                    }
                    dataElm.SetAttribute(iIndex, pair.Key.ToString());
                    dataElm.SetAttribute(sDes, pair.Value.sDescribe);
                    foreach (XmlClassData _class in pair.Value)
                    {
                        bool bClassCreate = false;
                        XmlElement elmClass;
                        elmClass = dataElm.SelectSingleNode(_class.sLogicName) as XmlElement;
                        if (elmClass == null)
                        {
                            elmClass = xmlDoc.CreateElement(_class.sLogicName);
                            bClassCreate = true;
                        }

                        foreach (XmlParamItem _property in _class)
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

                    maxID = Math.Max(pair.Key, maxID);
                    if (bDataCreate)
                        root.AppendChild(dataElm);
                }
                root.SetAttribute(sCurid, maxID.ToString());
                xmlDoc.Save(m_XmlFilePath);
            }
        }
        public void UpdateXml(Dictionary<int, XmlDataList> datadict, bool bRemove)
        {
            if (!bRemove)
            {
                UpdateXml(datadict);
            }
            else
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

                    root.RemoveAll();

                    xmlDoc.Save(m_XmlFilePath);

                    UpdateXml(datadict);
                }
            }
        }

        public void UpdateXmlByAllClass()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(ref Dictionary<int, XmlDataList> dict)
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

                    XmlDataList _data = new XmlDataList();
                    _data.iIndex = Convert.ToInt32(xml_data.GetAttribute(iIndex));
                    _data.sDescribe = xml_data.GetAttribute(sDes);

                    foreach (XmlElement xml_class in xml_data)
                    {
                        XmlClassData _class = new XmlClassData();
                        _class.sLogicName = xml_class.LocalName;

                        foreach (XmlElement xml_property in xml_class)
                        {
                            XmlParamItem _property = new XmlParamItem();
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

                XmlNode xe = rootNode.SelectSingleNode(sPreName + id);
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