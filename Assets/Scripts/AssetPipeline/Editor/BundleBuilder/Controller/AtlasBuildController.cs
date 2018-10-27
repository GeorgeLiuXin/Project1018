using Galaxy.AssetPipeline;
using Galaxy.DataNode;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy.AssetPipeline
{
    internal class AtlasBuilderController : BaseBuilderController
    {
        AtlasOutputData m_AtlasBuildData;
        public AtlasBuilderController(EAssetTag assetTag, CrudeAssetNode[] builds, DataNodeManager assetNodeManager, bool ignoreLastVersion, DataNodeManager newRecorder, DataNodeManager lastRecorder, VersionInfo versionInfo) : base(assetTag, builds, assetNodeManager, ignoreLastVersion, newRecorder, lastRecorder, versionInfo)
        {
            assetTag = EAssetTag.Atlas;
            m_AtlasBuildData = new AtlasOutputData();
            m_BuildData = m_AtlasBuildData;
        }

        public override IOutputData GetBuildData()
        {
            return m_AtlasBuildData;
        }

        public override void Prepare()
        {
            //一个atlas是一个Bundle
            InitAtlasAssetMap(this.m_Builds);
            PrepareDeleteAssetByLastVersion(m_Builds, m_LastRecorder);
            PrepareAddedAndChangedAsset(m_Builds, m_LastRecorder);
        }

        private void PrepareDeleteAssetByLastVersion(CrudeAssetNode[] builds, DataNodeManager lastRecorder)
        {
            if (lastRecorder == null)
            {
                return;
            }
            else
            {
                // 当前
                List<string> allRecievedNames = new List<string>();
                Array.ForEach(builds, delegate (CrudeAssetNode node)
                {
                    allRecievedNames.Add(node.AssetName);
                });

                //以前
                IDataNode tagNode = lastRecorder.GetNode(EAssetTag.Atlas.ToString());
                if (tagNode != null)
                {
                    List<IDataNode> lastRecordNodes = new List<IDataNode>();
                    tagNode.GetAllLeafNodes(ref lastRecordNodes);

                    Array.ForEach(lastRecordNodes.ToArray(), delegate (IDataNode node)
                    {
                        if (!allRecievedNames.Contains(node.Name))
                        {
                            BundleLog.Log("Atlas 删除  " + node.RelativeFullPath());
                            m_BuildData.DeleteAssetMap.SafeAdd(node.RelativeFullPath(), null, true, "写入缺少的文件");
                        }
                    });
                }
            }
        }

        private void PrepareAddedAndChangedAsset(CrudeAssetNode[] builds, DataNodeManager lastRecorder)
        {
            Dictionary<string, LinkedList<CrudeAssetNode>>.Enumerator itor = m_AtlasBuildData.AtlasCrudeAssetNodeMap.GetEnumerator();
            while (itor.MoveNext())
            {
                string atlasName = itor.Current.Key;
                LinkedList<CrudeAssetNode> allChilds = itor.Current.Value;

                string fullBundlePath = "";

                fullBundlePath = m_AssetTag.ToString() + BundleConfig.DEFAULT_SPLIT + atlasName;

                bool isAdded = true;
                if (lastRecorder != null)
                {
                    IDataNode node = lastRecorder.GetNode(fullBundlePath);
                    if (node != null)
                    {
                        foreach (CrudeAssetNode child in allChilds)
                        {
                            fullBundlePath = m_AssetTag.ToString() + BundleConfig.DEFAULT_SPLIT + atlasName + BundleConfig.DEFAULT_SPLIT + child.AssetName;
                            node = lastRecorder.GetNode(fullBundlePath);
                            if (node != null)
                            {
                                isAdded = false;
                                RefinedAssetNode oldRan = node.GetData<RecordVariable>().GetValue<RefinedAssetNode>();
                                if (oldRan.CrudeNode.Equals(child, true))
                                {
                                    continue;
                                }
                                else
                                {
                                    BundleLog.Log("Atlas 改变  " + fullBundlePath);
                                    m_BuildData.ChangedAssetMap.SafeAdd(fullBundlePath, child, true,
                                        "有变化的节点:" + fullBundlePath);
                                }
                            }
                        }
                    }
                }

                if (isAdded)
                {
                    foreach (CrudeAssetNode child in allChilds)
                    {
                        string fullPath = m_AssetTag.ToString() + BundleConfig.DEFAULT_SPLIT + atlasName + BundleConfig.DEFAULT_SPLIT + child.AssetName;
                        BundleLog.Log("Atlas 新增  " + fullPath);
                        m_BuildData.AddAssetMap.SafeAdd(fullPath, child, true,
                                "添加节点:" + fullPath);
                    }
                }
            }
        }


        private void InitAtlasAssetMap(CrudeAssetNode[] builds)
        {
            IDataNode commonNode = m_AssetNodeManager.GetNode(EAssetTag.Atlas + "/True");
            
            IDataNode[] commonChilds = new IDataNode[0];
            if(commonNode != null)
                commonChilds = commonNode.GetAllChild();
            
            // 先分类成各个Atlas
            m_AtlasBuildData.AtlasCrudeAssetNodeMap.Clear();

            List<CrudeAssetNode> commonAssets = new List<CrudeAssetNode>();
            foreach (IDataNode commonChild in commonChilds)
            {
                commonAssets.SafeAdd(commonChild.GetData<RecordVariable>().GetValue<RefinedAssetNode>().CrudeNode, false);
            }

            Array.ForEach(builds.ToArray(), delegate (CrudeAssetNode node)
            {
                bool isCommon = false;
                foreach (IDataNode commonChild in commonChilds)
                {
                    if (commonChild.GetData<RecordVariable>().GetValue<RefinedAssetNode>().CrudeNode == node)
                    {
                        LinkedList<CrudeAssetNode> nodeL = new LinkedList<CrudeAssetNode>();
                        if (m_AtlasBuildData.AtlasCrudeAssetNodeMap.ContainsKey(BundleConfig.COMMON_CLASSIFY))
                        {
                            nodeL = m_AtlasBuildData.AtlasCrudeAssetNodeMap[BundleConfig.COMMON_CLASSIFY];
                        }
                        else
                        {
                            m_AtlasBuildData.AtlasCrudeAssetNodeMap.Add(BundleConfig.COMMON_CLASSIFY, nodeL);
                        }
                        nodeL.AddFirst(node);
                        isCommon = true;
                    }
                }

                if (!isCommon)
                {
                    CrudeAssetNode topAsset = node.FindTopTarget();
                    if (topAsset.AssetType == EAssetType.Prefab)
                    {
                        string atlasName = System.IO.Path.GetFileNameWithoutExtension(topAsset.AssetName);
                        if (!m_AtlasBuildData.AtlasCrudeAssetNodeMap.ContainsKey(atlasName))
                        {
                            List<CrudeAssetNode> Childs = topAsset.Childs;
                            foreach (CrudeAssetNode n in commonAssets)
                            {
                                if (Childs.Contains(n))
                                    Childs.Remove(n);
                            }

                            LinkedList<CrudeAssetNode> list = new LinkedList<CrudeAssetNode>(Childs.ToArray());
                            list.AddFirst(topAsset);
                            // 不需要循环迭代
                            m_AtlasBuildData.AtlasCrudeAssetNodeMap.SafeAdd(atlasName, list, false);
                        }
                    }
                }
            });
        }

        public override void Handle()
        {
            // 顺序不能改变
            // 1.处理删除
            BundleBuildHandle.HandleRemoveAsset(m_NewRecorder, m_BuildData.DeleteAssetMap);

            BundleBuildHandle.HandleEmptyAsset(m_NewRecorder);
            // 2.处理改变
            BundleBuildHandle.HandleChangedAsset(m_NewRecorder, m_VersionInfo, m_BuildData.ChangedAssetMap);
            // 3.处理新增
            BundleBuildHandle.HandleAddedAsset(m_NewRecorder, m_VersionInfo, m_BuildData.AddAssetMap);
        }
    }
}