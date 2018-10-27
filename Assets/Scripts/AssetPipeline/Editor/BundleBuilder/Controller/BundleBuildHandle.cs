using Galaxy.DataNode;
using System;
using System.Collections.Generic;

namespace Galaxy.AssetPipeline
{
    internal class BundleBuildHandle
    {
        static internal void HandleChangedAsset(DataNodeManager recorder, VersionInfo versionInfo, Dictionary<string, CrudeAssetNode> changedAsset)
        {
            Dictionary<string, CrudeAssetNode>.Enumerator itor = changedAsset.GetEnumerator();
            while (itor.MoveNext())
            {
                string fullPath = itor.Current.Key;
                CrudeAssetNode crudeNode = itor.Current.Value;

                RefinedAssetNode refinedNode = recorder.GetData<RecordVariable>(fullPath).GetValue<RefinedAssetNode>();
                refinedNode.CrudeNode = crudeNode;
                refinedNode.ChangedVersion.Add(versionInfo);
            }
        }



        static internal List<string> HandleCrudeAddedAsset(DataNodeManager recorder, VersionInfo versionInfo, Dictionary<string, List<CrudeAssetNode>> addedCrudeAsset)
        {
            List<string> handledPaths = new List<string>();
            Dictionary<string, List<CrudeAssetNode>>.Enumerator itor = addedCrudeAsset.GetEnumerator();
            while (itor.MoveNext())
            {
                string refinedPath = itor.Current.Key;
                List<CrudeAssetNode> crudeNodes = itor.Current.Value;

                for (int i = 0; i < crudeNodes.Count; i++)
                {
                    CrudeAssetNode crudeNode = crudeNodes[i];
                    IDataNode node = recorder.GetNode(refinedPath);
                    if (node == null)
                    {
                        BundleLog.Warning("HandleCrudeAddedAsset Fail, " + refinedPath + " crudeNode : " + crudeNode.AssetPath);
                        node = recorder.GetOrAddNode(refinedPath);
                    }

                    int childCount = node.ChildCount;
                    if (childCount == 0)
                    {
                        IDataNode newSecondNode = GetNewRecordNode(crudeNode.AssetType, 0, node);
                        IDataNode newNode = GetNewRecordNode(crudeNode, versionInfo, newSecondNode);
                        handledPaths.Add(newNode.FullName);
                    }
                    else
                    {
                        // Texture000n
                        IDataNode child = node.GetChild(childCount - 1);
                        long allChildLength = child.GetAllNodesLength();
                        int downChildCount = child.ChildCount;

                        //先检测孩子数量
                        bool isChildCountInvalid = (downChildCount + 1) < BundleConfig.SYSTEM_FILE_MAX_COUNT;
                        //再检测孩子大小

                        bool isChildLengthInvalid = (allChildLength + crudeNode.AssetSize) < BundleConfig.SINGLE_BUNDLE_MAX_LENGTH;
                        if (isChildCountInvalid && isChildLengthInvalid)
                        {
                            //直接添加
                            IDataNode newNode = GetNewRecordNode(crudeNode, versionInfo, child);
                            handledPaths.Add(newNode.FullName);
                        }
                        else
                        {
                            // 新建一个节点，再添加
                            IDataNode newSecondNode = GetNewRecordNode(crudeNode.AssetType, childCount, node);
                            IDataNode newNode = GetNewRecordNode(crudeNode, versionInfo, newSecondNode);
                            handledPaths.Add(newNode.FullName);
                        }
                    }
                }
            }
            return handledPaths;
        }

        static internal void HandleAddedAsset(DataNodeManager recorder, VersionInfo versionInfo, Dictionary<string, CrudeAssetNode> addedAsset)
        {
            Dictionary<string, CrudeAssetNode>.Enumerator itor = addedAsset.GetEnumerator();
            while (itor.MoveNext())
            {
                CrudeAssetNode assetNode = itor.Current.Value;
                string addedAssetFullName = itor.Current.Key;
                GetNewRecordNode(recorder, assetNode, versionInfo, addedAssetFullName);
            }
        }

        static internal void HandleRemoveAsset(DataNodeManager recorder, Dictionary<string, CrudeAssetNode> removedAsset)
        {
            Dictionary<string, CrudeAssetNode>.Enumerator itor = removedAsset.GetEnumerator();
            while (itor.MoveNext())
            {
                string removedAssetFullName = itor.Current.Key;
                recorder.RemoveNode(removedAssetFullName);
            }
        }

        static internal void HandleEmptyAsset(DataNodeManager recorder)
        {
            List<IDataNode> leafNodes = new List<IDataNode>();
            recorder.Root.GetAllLeafNodes(ref leafNodes);
            foreach (IDataNode dataNode in leafNodes)
            {
                RemoveEmptyNode(recorder, dataNode);
            }
        }

        static private void RemoveEmptyNode(DataNodeManager recorder, IDataNode dataNode)
        {
            if (dataNode.ChildCount == 0 && dataNode.Parent != null)
            {
                if (dataNode.GetData<RecordVariable>() == null || dataNode.GetData<RecordVariable>().GetValue() == null)
                {
                    recorder.RemoveNode(dataNode.RelativeFullPath());
                    RemoveEmptyNode(recorder, dataNode.Parent);
                }
            }
        }

        // Atlas/baseUI/baseui.prefab
        static internal IDataNode GetNewRecordNode(DataNodeManager recorder, CrudeAssetNode assetNode, VersionInfo versionInfo, string fullPath)
        {
            RefinedAssetNode newRan = new RefinedAssetNode();
            newRan.CrudeNode = assetNode;
            newRan.ChangedVersion.SafeAdd(versionInfo, true, "生成新的节点");
            //生成名字
            string bundleName = string.Empty;
            List<string> splits = new List<string>(DataNodeManager.GetSplitPath(fullPath));
            splits.RemoveAt(splits.Count - 1);
            bundleName = string.Join("/", splits.ToArray());
            newRan.BundleName = bundleName;

            IDataNode newRecord = recorder.GetOrAddNode(fullPath);
            RecordVariable newRv = new RecordVariable(newRan);
            newRecord.SetData<RecordVariable>(newRv);

            return newRecord;
        }

        static internal IDataNode GetNewRecordNode(CrudeAssetNode assetNode, VersionInfo versionInfo, IDataNode parent)
        {
            RefinedAssetNode newRan = new RefinedAssetNode();
            newRan.CrudeNode = assetNode;
            newRan.ChangedVersion.SafeAdd(versionInfo, true, "生成新的节点");
            //生成名字
            string bundleName = string.Empty;
            List<string> splits = new List<string>(DataNodeManager.GetSplitPath(parent.FullName));
            splits.RemoveAt(0);
            bundleName = string.Join("/", splits.ToArray());
            newRan.BundleName = bundleName;

            IDataNode newDn = parent.GetOrAddChild(assetNode.AssetName);
            RecordVariable newRv = new RecordVariable(newRan);
            newDn.SetData<RecordVariable>(newRv);

            return newDn;
        }

        static internal IDataNode GetNewRecordNode(EAssetType assetType, int childCount, IDataNode parent)
        {
            //生成名字
            string bundleName = string.Empty;
            string typeName = assetType.ToString();
            childCount++;
            string countName = childCount.ToString("D4");
            string nodeName = typeName + countName;
            IDataNode newRn = parent.GetOrAddChild(nodeName);
            return newRn;
        }
    }
}
