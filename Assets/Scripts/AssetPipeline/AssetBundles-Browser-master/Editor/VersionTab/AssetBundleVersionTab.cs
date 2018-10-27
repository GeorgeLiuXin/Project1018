using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using Galaxy;
using Extension;

namespace Galaxy.AssetBundleBrowser
{
    internal class AssetBundleVersionTab
    {
        const float k_MenubarPadding = 80;
        const float k_HorizonPadding = 50;
        Rect m_Position;
        Rect m_LeftPosition;
        Rect m_RightPosition;

        private string leftPath = "";
        private string rightPath = "";
        private bool ignoreUnchanged = false;
        private bool hasResult = false;

        private Dictionary<string, AssetBundleMap.AssetBundleItem> leftDict = new Dictionary<string, AssetBundleMap.AssetBundleItem>();
        private Dictionary<string, AssetBundleMap.AssetBundleItem> rightDict = new Dictionary<string, AssetBundleMap.AssetBundleItem>();

        private List<AssetBundleMap.AssetBundleItem> leftUnchanged = new List<AssetBundleMap.AssetBundleItem>();
        private List<AssetBundleMap.AssetBundleItem> leftChanged = new List<AssetBundleMap.AssetBundleItem>();
        private List<AssetBundleMap.AssetBundleItem> leftAdded = new List<AssetBundleMap.AssetBundleItem>();

        private List<AssetBundleMap.AssetBundleItem> rightUnchanged = new List<AssetBundleMap.AssetBundleItem>();
        private List<AssetBundleMap.AssetBundleItem> rightChanged = new List<AssetBundleMap.AssetBundleItem>();
        private List<AssetBundleMap.AssetBundleItem> rightAdded = new List<AssetBundleMap.AssetBundleItem>();

        GUIStyle changedStyle;
        GUIStyle unchangedStyle;
        GUIStyle addedStyle;
        GUIStyle windowStyle;
        
        public void OnEnable()
        {
            changedStyle = new GUIStyle();
            changedStyle.fontSize = 12;
            changedStyle.alignment = TextAnchor.UpperLeft;
            changedStyle.normal.textColor = new Color(255 / 256f, 140f / 256f, 0f / 256f, 256f / 256f);

            unchangedStyle = new GUIStyle();
            unchangedStyle.fontSize = 12;
            changedStyle.alignment = TextAnchor.UpperLeft;
            unchangedStyle.normal.textColor = new Color(211f / 256f, 211f / 256f, 211f / 256f, 256f / 256f);

            addedStyle = new GUIStyle();
            addedStyle.fontSize = 12;
            changedStyle.alignment = TextAnchor.UpperLeft;
            addedStyle.normal.textColor = new Color(46f / 256f, 163f / 256f, 256f / 256f, 256f / 256f);

        }

        private void GetSubWindowArea(Rect position)
        {
            m_Position = position;
            float padding = k_MenubarPadding;
            float halfwidth = position.width / 2;

            m_LeftPosition = new Rect(0, padding, position.width / 2 - k_HorizonPadding / 2, position.height - padding);
            m_RightPosition = new Rect(halfwidth, padding, position.width / 2 - k_HorizonPadding / 2, position.height - padding);
        }

        internal void OnGUI(Rect pos)
        {
            GetSubWindowArea(pos);

            TopGUI();

            if (hasResult)
            {
                GUILayout.BeginArea(m_LeftPosition, "", "LockedHeaderBackground");
                m_ScrollPosition1 = EditorGUILayout.BeginScrollView(m_ScrollPosition1); 
                HalfWindow(m_Position.width / 2 - 200, true);
                EditorGUILayout.EndScrollView();
                GUILayout.EndArea();

                GUILayout.BeginArea(m_RightPosition, "", "LockedHeaderBackground");
                m_ScrollPosition2 = EditorGUILayout.BeginScrollView(m_ScrollPosition2); 
                HalfWindow(m_Position.width / 2 - 200, false);
                EditorGUILayout.EndScrollView();
                GUILayout.EndArea();
            }
        }

        private void TopGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("", "sv_label_0", GUILayout.Width(30f)); GUILayout.Label("不变", GUILayout.Width(60f)); GUILayout.Space(5f);
                GUILayout.Label("", "sv_label_5", GUILayout.Width(30f)); GUILayout.Label("修改", GUILayout.Width(60f)); GUILayout.Space(5f);
                GUILayout.Label("", "sv_label_1", GUILayout.Width(30f)); GUILayout.Label("增加", GUILayout.Width(60f)); GUILayout.Space(5f);
                GUILayout.FlexibleSpace();
                ignoreUnchanged = GUILayout.Toggle(ignoreUnchanged, "忽略不变", GUILayout.Width(100f));
                if (GUILayout.Button("开始", GUILayout.Width(150f)))
                {
                    OnBtnStart();
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                leftPath = EditorGUILayout.TextField(leftPath, GUILayout.Width(m_Position.width / 2 - k_HorizonPadding));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("浏览", GUILayout.MaxWidth(75f)))
                    BrowseForFolder(true);

                rightPath = EditorGUILayout.TextField(rightPath, GUILayout.Width(m_Position.width / 2 - k_HorizonPadding));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("浏览", GUILayout.MaxWidth(75f)))
                    BrowseForFolder(false);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void BrowseForFolder(bool isLeft)
        {
            var newPath = EditorUtility.OpenFolderPanel("Bundle Folder", isLeft ? leftPath : rightPath, string.Empty);
            if (!string.IsNullOrEmpty(newPath))
            {
                var gamePath = System.IO.Path.GetFullPath(".");
                gamePath = gamePath.Replace("\\", "/");
                if (newPath.StartsWith(gamePath) && newPath.Length > gamePath.Length)
                    newPath = newPath.Remove(0, gamePath.Length + 1);
            }
            if (isLeft)
                leftPath = newPath;
            else
                rightPath = newPath;
        }

        private void HalfWindow(float width, bool isLeft)
        {
            int totalChangedLength = 0;
            int totalChangedCount = 0;

            int totalUnchangedLength = 0;
            int totalUnchangedCount = 0;

            int totalAddedLength = 0;
            int totalAddedCount = 0;

            GUILayout.BeginVertical();
            foreach (AssetBundleMap.AssetBundleItem item in isLeft ? leftChanged : rightChanged)
            {
                totalChangedLength += item.length;
                totalChangedCount++;
                GUIAssetBundleItem(width, changedStyle, item);
            }
            string content = string.Format("修改{0}个文件,大小为: {1}", totalChangedCount, LengthFormatStr(totalChangedLength));
            GUILayout.Label(content, "IN BigTitle");
            foreach (AssetBundleMap.AssetBundleItem item in isLeft ? leftAdded : rightAdded)
            {
                totalAddedLength += item.length;
                totalAddedCount++;
                GUIAssetBundleItem(width, addedStyle, item);
            }
            content = string.Format("新增{0}个文件,大小为: {1}", totalAddedCount, LengthFormatStr(totalAddedLength));
            GUILayout.Label(content, "IN BigTitle");
            if (!ignoreUnchanged)
            {
                foreach (AssetBundleMap.AssetBundleItem item in isLeft ? leftUnchanged : rightUnchanged)
                {
                    totalUnchangedLength += item.length;
                    totalUnchangedCount++;
                    GUIAssetBundleItem(width, unchangedStyle, item);
                }
                content = string.Format("不变{0}个文件,大小为: {1}", totalUnchangedCount, LengthFormatStr(totalUnchangedLength));
                GUILayout.Label(content, "IN BigTitle");
            }

            GUILayout.Space(20f);
            content = string.Format("该版本需下载{0}个文件,大小为: {1}", totalChangedCount + totalAddedCount, LengthFormatStr(totalAddedLength + totalChangedLength));
            GUILayout.Label(content, "GroupBox");
            GUILayout.EndVertical();
        }

        private void GUIAssetBundleItem(float width, GUIStyle style, AssetBundleMap.AssetBundleItem item)
        {
            string name = Utility.GetAssetBundlePathWithoutHash(item.assetBundlePath, true);
            string length = LengthFormatStr(item.length);
            GUILayout.BeginHorizontal();
            GUILayout.Label(length, style, GUILayout.Width(75f));
            GUILayout.Space(10f);
            if (GUILayout.Button(name, GUILayout.Width(width - 75f)))
            {
                if (itemIsClickedDict.ContainsKey(item))
                {
                    itemIsClickedDict[item] = !itemIsClickedDict[item];
                }
                else
                {
                    itemIsClickedDict.Add(item, true);
                }
            }
            GUILayout.EndHorizontal();

            if (itemIsClickedDict.ContainsKey(item))
            {
                if (itemIsClickedDict[item])
                {
                    OnClickBundleItem(width, item);
                }
            }

        }

        private Dictionary<AssetBundleMap.AssetBundleItem, bool> itemIsClickedDict = new Dictionary<AssetBundleMap.AssetBundleItem, bool>();
        private Vector2 m_ScrollPosition1;
        private Vector2 m_ScrollPosition2;

        private void OnClickBundleItem(float width, AssetBundleMap.AssetBundleItem item)
        {
            string[] assets = item.assetNames;
            foreach (string asset in assets)
            {
                GUILayout.Label(asset, "flow target in", GUILayout.Width(75f));
            }
        }

        public string LengthFormatStr(int length)
        {
            if (length < 1024)
            {
                return length + "B";
            }
            else if (length < 1024 * 1024)
            {
                return ((float)length / 1024f).ToString("0.00") + "KB";
            }
            else
            {
                return ((float)length / (1024f * 1024f)).ToString("0.00") + "M";
            }
        }


        private void OnBtnStart()
        {
            ClearOldData();
            InitAssetBundleMap();
            InitChangeList();
            hasResult = true;
        }

        private void InitChangeList()
        {
            List<AssetBundleMap.AssetBundleItem> tempLeftMayUnchanged = new List<AssetBundleMap.AssetBundleItem>();
            List<AssetBundleMap.AssetBundleItem> tempRightMayUnchanged = new List<AssetBundleMap.AssetBundleItem>();
            foreach (string leftname in leftDict.Keys)
            {
                if (!rightDict.ContainsKey(leftname))
                {
                    leftAdded.SafeAdd(leftDict[leftname], false);
                }
                else
                    tempLeftMayUnchanged.SafeAdd(leftDict[leftname], false);
            }

            foreach (string rightname in rightDict.Keys)
            {
                if (!leftDict.ContainsKey(rightname))
                {
                    rightAdded.SafeAdd(rightDict[rightname], false);
                }
                else
                    tempRightMayUnchanged.SafeAdd(rightDict[rightname], false);
            }
            foreach (AssetBundleMap.AssetBundleItem left in tempLeftMayUnchanged)
            {
                foreach (AssetBundleMap.AssetBundleItem right in tempRightMayUnchanged)
                {
                    if (left.assetBundleHash == right.assetBundleHash)
                    {
                        leftUnchanged.SafeAdd(left, false);
                        rightUnchanged.SafeAdd(right, false);
                    }
                }
            }
            leftChanged = tempLeftMayUnchanged.Except(leftUnchanged).ToList();
            rightChanged = tempRightMayUnchanged.Except(rightUnchanged).ToList();
        }

        private void ClearOldData()
        {
            itemIsClickedDict.Clear();
            leftDict.Clear();
            rightDict.Clear();

            leftUnchanged.Clear();
            leftChanged.Clear();
            leftAdded.Clear();

            rightUnchanged.Clear();
            rightChanged.Clear();
            rightAdded.Clear();
        }

        private void InitAssetBundleMap()
        {
            InitAssetBundleMap(true);
            InitAssetBundleMap(false);
        }

        private bool InitAssetBundleMap(bool isLeft)
        {
            string filePath = isLeft ? leftPath : rightPath;
            AssetBundleMap bundleMap = new AssetBundleMap();
            Dictionary<string, AssetBundleMap.AssetBundleItem> bundleDict = isLeft ? leftDict : rightDict;

            filePath = filePath + "/" + VersionManager.ASSETBUNDLE_MAP_FILENAME;
            if (File.Exists(filePath))
            {
                using (var sr = new StreamReader(filePath))
                {
                    bundleMap = LitJson.JsonMapper.ToObject<AssetBundleMap>(sr);

                    for (int i = 0; i < bundleMap.assetBundleBuilds.Length; i++)
                    {
                        var assetBundleBuild = bundleMap.assetBundleBuilds[i];
                        string nameWithOutHash = Utility.GetAssetBundlePathWithoutHash(assetBundleBuild.assetBundlePath, true);
                        bundleDict.Add(nameWithOutHash, assetBundleBuild);
                    }
                }
                return true;

            }
            return false;
        }
    }
}
     
