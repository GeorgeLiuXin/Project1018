using Galaxy.DataNode;
using System;
using System.Collections.Generic;

namespace Galaxy.AssetPipeline
{
    internal class DefaultBuilderController : BaseBuilderController
    {
        public DefaultBuilderController(EAssetTag assetTag, CrudeAssetNode[] builds, DataNodeManager assetNodeManager, bool ignoreLastVersion, DataNodeManager newRecorder, DataNodeManager lastRecorder, VersionInfo versionInfo) : base(assetTag, builds, assetNodeManager, ignoreLastVersion, newRecorder, lastRecorder, versionInfo)
        {
            assetTag = EAssetTag.Normal;
            this.m_BuildData = new DefaultOutputData();
        }

        public override IOutputData GetBuildData()
        {
            return (m_BuildData as DefaultOutputData);
        }

        public override void Prepare()
        {
            PrepareDeleteAssetByLastVersion(m_Builds, m_LastRecorder);
            PrepareChangedAndAddedAssetByLastVersion(m_Builds, m_LastRecorder);
        }

        #region Prepare

        // 检测已经在上次记录里没有的资源
        // 出现的问题是unity对大小写敏感，如果修改了大小写，也会导致Bundle变化
        private void PrepareDeleteAssetByLastVersion(CrudeAssetNode[] builds, DataNodeManager lastRecorder)
        {
            if (lastRecorder == null) {
                return;
            }

            // 当前
            List<string> allRecievedNames = new List<string>();
            Array.ForEach(builds, delegate (CrudeAssetNode node)
            {
                allRecievedNames.Add(node.AssetName);
            });

            //以前
            IDataNode tagNode = lastRecorder.GetNode(EAssetTag.Normal.ToString());
            if (tagNode != null)
            {
                List<IDataNode> lastRecordNodes = new List<IDataNode>();
                tagNode.GetAllLeafNodes(ref lastRecordNodes);

                Array.ForEach(lastRecordNodes.ToArray(), delegate (IDataNode node)
                {
                    if (!allRecievedNames.Contains(node.Name))
                    {
                        BundleLog.Log("Default 删除  " + node.RelativeFullPath());
                        m_BuildData.DeleteAssetMap.SafeAdd(node.RelativeFullPath(), null, true, "写入缺少的文件");
                    }
                });
            }
        }

        private void PrepareChangedAndAddedAssetByLastVersion(CrudeAssetNode[] builds, DataNodeManager lastRecorder)
        {
            foreach (CrudeAssetNode node in builds)
            {
                CrudeAssetNode origin = node;
                CrudeAssetNode topTarget = node.FindTopTarget();
                string splitPath = topTarget.AssetPathWithoutName;
                EAssetType assetType = origin.AssetType;

                // 通过最父级的节点找到匹配的部分字段
                // Effects/HeroEffect/HeadEffect/Effect01.prefab ---- Effect01.Texture
                // => Texture/HeroEffect/HeadEffect
                string crudeSplitBundlePath = "";
                string relativePathWithoutAssetType = topTarget.GetRelativePathWithoutAssetTypeAndName();
                if (!string.IsNullOrEmpty(relativePathWithoutAssetType))
                    crudeSplitBundlePath = m_AssetTag.ToString() + BundleConfig.DEFAULT_SPLIT + assetType.ToString() + BundleConfig.DEFAULT_SPLIT +
                    relativePathWithoutAssetType;
                else
                    crudeSplitBundlePath = m_AssetTag.ToString() + BundleConfig.DEFAULT_SPLIT + assetType.ToString();

                bool isAdded = false;

                if (lastRecorder != null)
                {
                    IDataNode lastRecordNode = lastRecorder.GetNode(crudeSplitBundlePath);
                    if (lastRecordNode == null)
                    {
                        isAdded = true;
                    }

                    else
                    {
                        // 找到匹配到的最底层节点
                        // Effect01.prefab
                        // => Texture/HeroEffect/HeadEffect/Effect0001/Effect01.prefab
                        IDataNode matchNode = lastRecordNode.GetMatchedBottomRecordNode(origin.AssetName);
                        if (matchNode == null)
                        {
                            isAdded = true;
                        }
                        else
                        {
                            RefinedAssetNode lastRefinedNode = matchNode.GetData().GetValue() as RefinedAssetNode;
                            CrudeAssetNode lastNode = lastRefinedNode.CrudeNode;
                            if (lastNode != null)
                            {
                                // 原来就有
                                // 策略上，原来就有的资源，不论变大变小，不再考虑是否大小大于标准包大小，仅仅修改参考数值
                                // ==已经被重写，只比较数值
                                if (origin.Equals(lastNode, true))
                                {
                                    continue;
                                }
                                else
                                {
                                    string fullPath = matchNode.RelativeFullPath();
                                    BundleLog.Log("Default 修改  " + fullPath);
                                    m_BuildData.ChangedAssetMap.SafeAdd(fullPath, origin, true,
                                        "变化节点:" + fullPath);
                                }
                            }
                            else
                                isAdded = true;
                        }
                    }
                }
                else
                {
                    isAdded = true;
                }

                if (isAdded)
                {
                    // 新增
                    // 策略上，只考虑上次最后一次的节点，别的节点将不考虑填充资源，是防止塞入的过程导致多份AB包发生变化，会导致4 * nM的变化

                    // Effects/HeroEffect/HeadEffect/Effect01.prefab ---- Effect01.Texture
                    // => Texture/HeroEffect/HeadEffect

                    BundleLog.Log("Default 新增  " + crudeSplitBundlePath + " || " + origin.AssetPath);
                    m_BuildData.AddRefinedAssetListMap.ForceListAdd(crudeSplitBundlePath, origin, false, true,
                                    "添加节点:" + crudeSplitBundlePath);
                }
            }
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
            m_BuildData.AddedAssetPathsAfterHandle = BundleBuildHandle.HandleCrudeAddedAsset(m_NewRecorder, m_VersionInfo, m_BuildData.AddRefinedAssetListMap);
        }

        #endregion
    }
}
