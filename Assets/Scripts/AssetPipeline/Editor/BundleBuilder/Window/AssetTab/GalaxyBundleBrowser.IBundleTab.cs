
using UnityEditor;
using UnityEngine;

namespace Galaxy.AssetPipeline
{
    internal interface IBundleTab
    {
        void OnEnable(Rect pos, EditorWindow parent, BundleBuilderManager manager);

        void OnGUI(Rect pos);

        void OnDisable();

        void Refresh();

        void Update();
    }
}
