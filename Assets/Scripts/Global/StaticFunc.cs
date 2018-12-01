using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public static class StaticFunc
    {
        public static void AttachChild(Transform child, Transform parent)
        {
            if (child == null || parent == null)
                return;

            child.SetParent(parent);
            child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity;
            child.localScale = Vector3.one;
        }

        public static Transform FindChild(Transform tranParent, string childName, bool bAllChildren = false)
        {
            if (tranParent == null)
                return null;

            Transform[] t_childList = tranParent.GetComponentsInChildren<Transform>(bAllChildren);
            for (int idx = 0; idx < t_childList.Length; ++idx)
            {
                if (t_childList[idx].name == childName)
                    return t_childList[idx];
            }

            return null;
        }

        /// <summary>
        /// 添加gm指令通用释放方法
        /// </summary>
        /// <param name="gmstr"></param>
        public static void SendGm(string gmstr)
        {

        }

    }
}