using UnityEngine;
using System.Collections.Generic;

namespace XWorld
{
    public class Variant
    {
        public void Init(string[] allAssetBundlesWithVariant, string[] activeVariants)
        {
            this._allAssetBundlesWithVariant = allAssetBundlesWithVariant;
            this._activeVariants = activeVariants;
        }

        public void SetVariants(string[] activeVariants)
        {
            this._activeVariants = activeVariants;
        }

        public string RemapVariantPath(string assetBundlePath)
        {
            if (this._activeVariants == null ||
                this._allAssetBundlesWithVariant == null ||
                !assetBundlePath.Contains(".") ||
                assetBundlePath.LastIndexOf('.') == assetBundlePath.Length - 1)
            {
                return assetBundlePath;
            }

            #pragma warning disable 0429
            string assetBundlePathWithoutHash = Utility.GetAssetBundlePathWithoutHash(assetBundlePath,
                VersionManager.EnableAppendHashToAssetBundlePath);
            #pragma warning restore 0429

            string[] split = assetBundlePathWithoutHash.Split('.');

            int bestFit = int.MaxValue;
            int bestFitIndex = -1;

            // Loop all the assetBundles with variant to find the best fit variant assetBundle.
            for (int i = 0; i < this._allAssetBundlesWithVariant.Length; i++)
            {
                #pragma warning disable 0429
                string curAssetBundlePathWithoutHash = Utility.GetAssetBundlePathWithoutHash(
                    this._allAssetBundlesWithVariant[i], VersionManager.EnableAppendHashToAssetBundlePath);
                #pragma warning restore 0429

                string[] curSplit = curAssetBundlePathWithoutHash.Split('.');
                if (curSplit[0] != split[0])
                {
                    continue;
                }

                int found = System.Array.IndexOf(this._activeVariants, curSplit[1]);

                // If there is no active variant found. We still want to use the first
                if (found == -1)
                {
                    found = int.MaxValue-1;
                }

                if (found < bestFit)
                {
                    bestFit = found;
                    bestFitIndex = i;
                }
            }

            if (bestFit == int.MaxValue-1)
            {
                Debug.Log("<color=#FFBBFF>Ambigious asset bundle variant chosen because there was no matching active variant: "
                    + this._allAssetBundlesWithVariant[bestFitIndex] + "</color>");
            }

            if (bestFitIndex != -1)
            {
                return this._allAssetBundlesWithVariant[bestFitIndex];
            }
            else
            {
                return assetBundlePath;
            }
        }

        private string[] _activeVariants;
        private string[] _allAssetBundlesWithVariant;
    }
}
