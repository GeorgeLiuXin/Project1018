using Galaxy.DataNode;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy.AssetPipeline
{
    internal static class BundleBuildExtensions
    {
        public static bool IsAssetDatasNode(this CrudeAssetNode node)
        {
            string assetPath = node.AssetPath;
            return assetPath.Contains("Assets/AssetDatas");
        }

        // Asset/AssetDatas/Effects/HeroEffect/HeadEffect/Effect01.prefab
        // => HeroEffect/HeadEffect
        public static string GetRelativePathWithoutAssetTypeAndName(this CrudeAssetNode node)
        {
            string assetPath = node.AssetPath;
            if (assetPath.Contains("Assets/AssetDatas"))
            {
                assetPath = assetPath.Substring(assetPath.IndexOf("Assets/AssetDatas") + "Assets/AssetDatas".Length + 1);
            }
            List<string> splits = new List<string>(assetPath.Split('/'));
            if (splits.Count < 3)
            {
                Debug.LogWarning(node.AssetPath + " Very Short");
                return string.Empty;
            }
            splits.RemoveAt(0);
            splits.RemoveAt(splits.Count - 1);
            return string.Join("/", splits.ToArray());
        }

        // Effect01.prefab
        // => Texture/HeroEffect/HeadEffect/Effect0001/Effect01.prefab
        public static IDataNode GetMatchedBottomRecordNode(this IDataNode node, string matchChildName)
        {
            if (node.Name == matchChildName)
            {
                return node;
            }

            else
            {
                IDataNode answer = null;
                DataNode.IDataNode[] nodes = node.GetAllChild();
                foreach (DataNode.IDataNode n in nodes)
                {
                    IDataNode rn = n as IDataNode;
                    answer = rn.GetMatchedBottomRecordNode(matchChildName);
                    if (answer != null)
                    {
                        break;
                    }
                }
                return answer;
            }
        }
        
        public static void GetAllLeafNodes(this IDataNode parent, ref List<IDataNode> targetList)
        {
            if (parent != null)
            {
                if (parent.ChildCount == 0)
                {
                    targetList.Add(parent);
                }
                else
                {
                    DataNode.IDataNode[] nodes = parent.GetAllChild();
                    foreach (DataNode.IDataNode n in nodes)
                    {
                        n.GetAllLeafNodes(ref targetList);
                    }
                }
            }
        }

        public static long GetAllNodesLength(this IDataNode parent)
        {
            long length = 0;
            List<IDataNode> targetList = new List<IDataNode>();
            parent.GetAllLeafNodes(ref targetList);

            foreach (DataNode.IDataNode n in targetList)
            {
                RefinedAssetNode ran = n.GetData<RecordVariable>().GetValue<RefinedAssetNode>();
                length += ran.CrudeNode.AssetSize;
            }
            return length;
        }


        public static CrudeAssetNode FindTopTarget(this CrudeAssetNode node)
        {
            if (!node.IsRefence)
            {
                return node;
            }
            else
            {
                if (node.Owners.Count > 0)
                {
                    return FindTopTarget(node.Owners[0]);
                }
                else
                {
                    return node;
                }
            }
        }

        public static string RelativeFullPath(this IDataNode node)
        {
            if (node.Parent == null)
            {
                throw new Exception("Get Relative FullPath, but has no parent");
            }
            string path = node.FullName;
            string[] splits = DataNodeManager.GetSplitPath(path);
            if (splits == null || splits.Length < 2)
            {
                throw new Exception("Get FullPath splits, but has no parent");
            }

            List<string> spList = splits.ToList();
            spList.RemoveAt(0);
            return string.Join(BundleConfig.DEFAULT_SPLIT, spList.ToArray());
        }
    }
}
