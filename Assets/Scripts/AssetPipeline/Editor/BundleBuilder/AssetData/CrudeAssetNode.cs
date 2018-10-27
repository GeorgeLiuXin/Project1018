/**************************************************
 *  创建人   : 夏佳文
 *  创建时间 : 2018.6.19
 *  说明     : 单个Asset原始信息封装
 * ************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Galaxy.AssetPipeline
{
    [System.Serializable]
    internal class CrudeAssetNode : IComparable, IComparable<CrudeAssetNode>, IEquatable<CrudeAssetNode>
    {
        private bool m_IsRefence = true;
        private EAssetType m_AssetType;
        private EAssetTag m_AssetTag;
        private List<CrudeAssetNode> m_Owners = new List<CrudeAssetNode>();
        private List<CrudeAssetNode> m_Childs = new List<CrudeAssetNode>();

        private string m_AssetName;
        private string m_AssetPathWithoutName;

        private string m_AssetHash;
        private string m_AssetGUID;
        private long m_AssetSize;


        internal string AssetPath
        {
            get
            {
                return AssetPathWithoutName + BundleConfig.DEFAULT_SPLIT + AssetName;
            }
        }

        public bool IsCommon
        {
            get
            {
                return (m_Owners != null && m_Owners.Count > 1);
            }
        }

        public bool IsRefence
        {
            get
            {
                return m_IsRefence;
            }

            set
            {
                m_IsRefence = value;
            }
        }

        internal EAssetType AssetType
        {
            get
            {
                return m_AssetType;
            }

            set
            {
                m_AssetType = value;
            }
        }

        internal EAssetTag AssetTag
        {
            get
            {
                return m_AssetTag;
            }

            set
            {
                m_AssetTag = value;
            }
        }

        internal List<CrudeAssetNode> Owners
        {
            get
            {
                return m_Owners;
            }

            set
            {
                m_Owners = value;
            }
        }
        
        public string AssetName
        {
            get
            {
                return m_AssetName;
            }

            set
            {
                m_AssetName = value;
            }
        }

        public string AssetPathWithoutName
        {
            get
            {
                return m_AssetPathWithoutName;
            }

            set
            {
                m_AssetPathWithoutName = value;
            }
        }

        public string AssetHash
        {
            get
            {
                return m_AssetHash;
            }

            set
            {
                m_AssetHash = value;
            }
        }

        public string AssetGUID
        {
            get
            {
                return m_AssetGUID;
            }

            set
            {
                m_AssetGUID = value;
            }
        }

        public long AssetSize
        {
            get
            {
                return m_AssetSize;
            }

            set
            {
                m_AssetSize = value;
            }
        }

        internal List<CrudeAssetNode> Childs
        {
            get
            {
                return m_Childs;
            }

            set
            {
                m_Childs = value;
            }
        }

        #region Math
        public override bool Equals(object value)
        {
            return (value is CrudeAssetNode) && (this == (CrudeAssetNode)value);
        }

        public bool Equals(CrudeAssetNode value, bool showLog)
        {
            bool isEqual = Equals(value);
            if (!isEqual && showLog)
            {
                string answer = "";
                if ((AssetType != value.AssetType)) answer += string.Format("AssetType : {0},{1}\n", AssetType, value.AssetType);
                if ((AssetName != value.AssetName)) answer += string.Format("AssetName : {0},{1}\n", AssetName, value.AssetName);
                if ((AssetPathWithoutName != value.AssetPathWithoutName)) answer += string.Format("AssetPathWithoutName : {0},{1}\n", AssetPathWithoutName, value.AssetPathWithoutName);
                if ((AssetHash != value.AssetHash)) answer += string.Format("AssetHash : {0},{1}\n", AssetHash, value.AssetHash);
                if ((AssetSize != value.AssetSize)) answer += string.Format("AssetSize : {0},{1}\n", AssetSize, value.AssetSize);

                string content = string.Format("[{0}]比较[{1}]不同 \n{2},", AssetPath, value.AssetPath, answer);
                BundleLog.Log(content);
            }
            return isEqual;
        }

        public bool Equals(CrudeAssetNode value)
        {
            if (ReferenceEquals(value, null))
            {
                return ReferenceEquals(this, null);
            }
            else if (ReferenceEquals(this, null))
            {
                return ReferenceEquals(value, null);
            }

            bool isEqual = ((AssetType == value.AssetType)
               && (AssetTag == value.AssetTag)
               && (AssetName == value.AssetName) && (AssetPathWithoutName == value.AssetPathWithoutName)
               && (AssetHash == value.AssetHash)
               && (AssetSize == value.AssetSize));
            
            return isEqual;
        }

        public int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }
            if (!(value is CrudeAssetNode))
            {
                throw new Exception("Type of value is invalid.");
            }
            return CompareTo((CrudeAssetNode)value);
        }

        public int CompareTo(CrudeAssetNode value)
        {
            if (AssetSize == value.AssetSize) return 0;
            else if (AssetSize > value.AssetSize) return 1;
            else if (AssetSize < value.AssetSize) return -1;
            return 1;
        }

        public static bool operator ==(CrudeAssetNode node1, CrudeAssetNode node2)
        {
            if (ReferenceEquals(node1, null))
            {
                return ReferenceEquals(node2, null);
            }
            else if (ReferenceEquals(node2, null))
            {
                return ReferenceEquals(node1, null);
            }
            else 
                return node1.Equals(node2);
        }

        public static bool operator !=(CrudeAssetNode node1, CrudeAssetNode node2)
        {
            if (ReferenceEquals(node1, null))
            {
                return !ReferenceEquals(node2, null);
            }
            else if (ReferenceEquals(node2, null))
            {
                return !ReferenceEquals(node1, null);
            }
            else
                return !node1.Equals(node2);
        }

        public override int GetHashCode()
        {
            return (AssetPath.GetHashCode() ^ m_AssetGUID.GetHashCode());
        }

        #endregion Math
        internal CrudeAssetNode Clone()
        {
            CrudeAssetNode newNode = new CrudeAssetNode();
            newNode.IsRefence = this.m_IsRefence;
            newNode.AssetType = this.m_AssetType;
            newNode.AssetTag = this.m_AssetTag;
            newNode.Owners = this.m_Owners;
            newNode.AssetName = this.m_AssetName;
            newNode.AssetPathWithoutName = this.m_AssetPathWithoutName;
            newNode.AssetHash = this.m_AssetHash;
            newNode.AssetGUID = this.m_AssetGUID;
            newNode.AssetSize = this.m_AssetSize;
            return newNode;
        }
    }

    [System.Serializable]
    public class VersionInfo
    {
        private int m_GameVersion;
        private int m_First;
        private int m_Second;

        // for Test
        private int m_Test = 0;
        public int GameVersion
        {
            get
            {
                return m_GameVersion;
            }

            set
            {
                m_GameVersion = value;
            }
        }

        public int First
        {
            get
            {
                return m_First;
            }

            set
            {
                m_First = value;
            }
        }

        public int Second
        {
            get
            {
                return m_Second;
            }

            set
            {
                m_Second = value;
            }
        }

        public int Test
        {
            get
            {
                return m_Test;
            }

            set
            {
                m_Test = value;
            }
        }

        public override string ToString()
        {
            string testStr = ((Test == 0) ? string.Empty : ("^" + Test.ToString()));
            return GameVersion.ToString() + "^" + First.ToString() + "^" + Second.ToString() + testStr;
        }

        public static VersionInfo SplitStr(string content)
        {
            VersionInfo versionInfo = new VersionInfo();
            string[] splits = content.Split(BundleConfig.VersionSplit, System.StringSplitOptions.RemoveEmptyEntries);
            if (splits.Length >= 3)
            {
                versionInfo.GameVersion = int.Parse(splits[0]);
                versionInfo.First = int.Parse(splits[1]);
                versionInfo.Second = int.Parse(splits[2]);
            }
            if (splits.Length >= 4)
            {
                versionInfo.Test = int.Parse(splits[3]);
            }
            return versionInfo;
        }
    }
}
