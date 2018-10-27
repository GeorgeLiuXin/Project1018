using UnityEngine;
using System.Collections.Generic;

namespace XWorld
{
    public class DebugWindow : MonoBehaviour
    {
        private const float WIDTH = 960f;
        private const float HEIGHT = 540f;
        private const float NEW_REF_MILLISECONDS = 5000f;

        private void Start()
        {
            _windowRect = new Rect(0f, 40f, WIDTH, HEIGHT - 40f);
            _titleRect = new Rect(0f, 0f, _windowRect.width, 24f);
            _colomnWidth = new float[] { 0.3f, 0.55f, 0.05f, 0.1f };
        }

        private void OnGUI()
        {
            float ratio = Screen.width / WIDTH < Screen.height / HEIGHT ? Screen.width / WIDTH : Screen.height / HEIGHT;
            if (ratio != _ratio)
            {
                _ratio = ratio;
                _guiMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(ratio, ratio, 1));
            }
            GUI.matrix = _guiMatrix;
            _windowRect = GUI.Window(0, _windowRect, WindowFunction, "AssetPipeline Debug");
        }

        private void WindowFunction(int windowID)
        {
            GUI.DragWindow(_titleRect);

            GUILayout.BeginHorizontal();
            _selectedTab = GUILayout.SelectionGrid(_selectedTab, _tabs, 2);
            _showCaller = GUILayout.Toggle(_showCaller, "Show Caller");
            _showReference = GUILayout.Toggle(_showReference, "Show Reference");
            GUILayout.EndHorizontal();

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition,
                GUILayout.Width(_windowRect.size.x - 14f), GUILayout.Height(_windowRect.size.y - 50f));

            if (_selectedTab == 0)
            {
                ShowAssetRefrences();
            }
            else
            {
                ShowBundleRefrences();
            }

            GUILayout.EndScrollView();
        }

        private void ShowAssetRefrences()
        {
            //XWorldGameModule.GetGameManager<AssetManager>().GetAssetRefList().Sort((a, b) =>
            //    b.reference.referenceTime.CompareTo(a.reference.referenceTime));

            //foreach (AssetRef assetRef in XWorldGameModule.GetGameManager<AssetManager>().GetAssetRefList())
            //{
            //    bool newReference = IsNewReference(assetRef.reference.references);
            //    GUI.color = newReference ? Color.green : Color.white;

            //    AssetItem(assetRef);
            //}
        }

        private void ShowBundleRefrences()
        {
            //BundleManager bundleMgr =  XWorldGameModule.GetGameManager<BundleManager>();
            //bundleMgr.GetBundleRefList().Sort((a, b) =>
            //    b.reference.referenceTime.CompareTo(a.reference.referenceTime));

            //foreach (var bundleRef in bundleMgr.GetBundleRefList())
            //{
            //    bool newReference = IsNewReference(bundleRef.reference.references);
            //    GUI.color = newReference ? Color.green : Color.white;

            //    BundleItemWithReference(bundleRef, 0f, true);
            //}
        }

        //private void AssetItem(AssetRef assetRef)
        //{
        //    string state = string.Empty;
        //    string assetName = System.IO.Path.GetFileNameWithoutExtension(assetRef.assetPath);
        //    assetRef.toggle = TableItem(0f, true, assetRef.toggle, assetName,
        //        assetRef.assetPath, assetRef.refCount.ToString(), state);

        //    if (assetRef.toggle)
        //    {
        //        BundleItem(assetRef.bundleRef, 10f, true);
        //        ReferenceItem(assetRef.reference, 0f, _showCaller, _showReference);
        //    }
        //}

        //private void BundleItem(BundleRef bundleRef, float intent, bool hasToggle)
        //{
        //    if (bundleRef == null)
        //    {
        //        return;
        //    }

        //    string state = bundleRef.IsLoaded() ? "Loaded" : "Loading";
        //    string bundlePath = bundleRef.path;

        //    #pragma warning disable 0429
        //    bundlePath = bundlePath.LastIndexOf("_") > 0 ?
        //        bundlePath.Substring(0, bundlePath.LastIndexOf("_")) : bundlePath;
        //    #pragma warning restore 0429

        //    bundleRef.toggle = TableItem(intent, hasToggle, bundleRef.toggle,
        //        bundlePath, bundleRef.filePath, bundleRef.refCount.ToString(), state);

        //    if (bundleRef.dependencies != null && (!hasToggle || bundleRef.toggle))
        //    {
        //        for (int i = 0; i < bundleRef.dependencies.Length; i++)
        //        {
        //            if (hasToggle)
        //            {
        //                intent += 20f;
        //            }
        //            BundleItem(bundleRef.dependencies[i], intent + 10f, false);
        //        }
        //    }
        //}

        //private void BundleItemWithReference(BundleRef bundleRef, float intent, bool hasToggle)
        //{
        //    if (bundleRef == null)
        //    {
        //        return;
        //    }

        //    string state = bundleRef.IsLoaded() ? "Loaded" : "Loading";
        //    string bundlePath = bundleRef.path;

        //    #pragma warning disable 0429
        //    bundlePath = bundlePath.LastIndexOf("_") > 0 ?
        //        bundlePath.Substring(0, bundlePath.LastIndexOf("_")) : bundlePath;
        //    #pragma warning restore 0429

        //    bundleRef.toggle = TableItem(intent, hasToggle, bundleRef.toggle,
        //        bundlePath, bundleRef.filePath, bundleRef.refCount.ToString(), state);

        //    if (!hasToggle || bundleRef.toggle)
        //    {
        //        if (bundleRef.dependencies != null)
        //        {
        //            for (int i = 0; i < bundleRef.dependencies.Length; i++)
        //            {
        //                if (hasToggle)
        //                {
        //                    intent += 20f;
        //                }
        //                BundleItemWithReference(bundleRef.dependencies[i], intent + 10f, false);
        //            }
        //        }
        //        ReferenceItem(bundleRef.reference, intent, _showCaller, _showReference);
        //    }
        //}

        private void ReferenceItem(Reference reference, float intent, bool showCaller = true, bool showReference = true)
        {
            if (showCaller)
            {
                reference.toggleCallers = Title(intent + 10f, "callers " + reference.callers.Count,
                    reference.callers.Count > 0, reference.toggleCallers);
                if (reference.toggleCallers)
                {
                    ReferenceSubItem(reference.callers, intent);
                }
            }

            if (showReference)
            {
                reference.toggleReferences = Title(intent + 10f, "references " + reference.references.Count,
                    reference.references.Count > 0, reference.toggleReferences);
                if (reference.toggleReferences)
                {
                    ReferenceSubItem(reference.references, intent);
                }
            }
        }

        private void ReferenceSubItem(System.Collections.Generic.List<Reference.Item> items, float intent)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                TableItem(intent + 40f, false, false,
                    item.type, item.path, item.refCount.ToString(), item.time.ToString("HH:mm:ss fff"));
            }
        }

        private bool IsNewReference(List<Reference.Item> references)
        {
            for (int i = 0; i < references.Count; i++)
            {
                var item = references[i];
                var timeOffset = System.DateTime.Now - item.time;
                if (timeOffset.TotalMilliseconds < NEW_REF_MILLISECONDS)
                {
                    return true;
                }
            }

            return false;
        }

        private bool Title(float intent, string title, bool hasToggle, bool toggle)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(intent);
            if (hasToggle)
            {
                toggle = GUILayout.Toggle(toggle, "");
            }
            else
            {
                GUILayout.Space(20f);
            }
            GUILayout.Label(title);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return toggle;
        }

        private bool TableItem(float intent, bool hasToggle, bool toggle,
            string colomn1, string column2, string column3, string column4)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(intent);
            if (hasToggle)
            {
                toggle = GUILayout.Toggle(toggle, "");
            }
            GUILayout.Label(colomn1);
            GUILayout.FlexibleSpace();
            try
            {
                GUILayout.Label(column2, GUILayout.Width(_colomnWidth[1] * _windowRect.width));
                GUILayout.Label(column3.ToString(), GUILayout.Width(_colomnWidth[2] * _windowRect.width));
                GUILayout.Label(column4, GUILayout.Width(_colomnWidth[3] * _windowRect.width));
            }
            catch { }
            GUILayout.EndHorizontal();
            return toggle;
        }

        private Rect _windowRect;
        private Rect _titleRect;
        private float[] _colomnWidth;
        private Vector2 _scrollPosition;
        private float _ratio;
        private Matrix4x4 _guiMatrix;
        private string[] _tabs = new string[]{ "AssetRef", "BundleRef" };
        private int _selectedTab = 0;
        private bool _showCaller = false;
        private bool _showReference = true;
    }
}
