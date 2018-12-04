using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld.AssetPipeline
{
    public class ResourcesProxy
    {
        // TODO 具体数值需要后期适配，目前Tick逻辑未全部执行
        public static int TimeIntervalCollect = 900;
        public static int MaxCollect = 130;

        private static int m_LastGCTime = -1;
        private static int m_GCTotalMemory = -1;
        public const string ROOT_PATH = "Assets/AssetDatas/";

        /// <summary>
        /// 读取相对文件夹路径下所有的文件
        /// Resources（DEMO）方式读取时，可能有卡顿
        /// </summary>
        /// <param name="relativePath">相对文件夹路径</param>
        /// <param name="callback"></param>
        /// <param name="singleCallback"></param>
        /// <param name="userdata"></param>
        public static void LoadImagePack(string relativePath, LoadAssetCallback callback, LoadAssetCallback singleCallback = null, object userdata = null)
        {
            ResourcesMgr.Instance.LoadImagePack(relativePath, callback, singleCallback, userdata);
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="relativePath">AssetDatas下相对路径，例如"Prefabs/Hero/Hero1.prefab"</param>
        /// <param name="callback">加载回调，包含加载资源路径，引用与是否成功</param>
        /// <param name="userdata">加载回调带回的参数</param>
        public static int LoadAsset(string relativePath, LoadAssetCallback callback, object userdata = null)
        {
            return ResourcesMgr.Instance.LoadAsset(relativePath, callback, userdata);
        }

        public static int LoadAsset(string relativePath, int priority, LoadAssetCallback callback, object userdata = null)
        {
            return ResourcesMgr.Instance.LoadAsset(relativePath, priority, callback, userdata);
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="relativePaths">AssetDatas下相对路径数组</param>
        /// <param name="callback">加载回调，包含加载资源路径，引用与是否成功，错误列表</param>
        /// <param name="userdata">加载回调带回的参数</param>
        public static int[] LoadAssets(string[] relativePaths, LoadAssetCallback callback, LoadAssetCallback singleCallback = null, object userdata = null)
        {
            return ResourcesMgr.Instance.LoadAssets(relativePaths, callback, singleCallback, userdata);
        }

        public static int[] LoadAssets(string[] relativePaths, int[] priorities, LoadAssetCallback callback, LoadAssetCallback singleCallback = null, object userdata = null)
        {
            return ResourcesMgr.Instance.LoadAssets(relativePaths, priorities, callback, singleCallback, userdata);
        }

        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="path">AssetDatas下相对路径</param>
        public static void DestroyAsset(string path)
        {
            ResourcesMgr.Instance.DestroyAsset(path);
        }

        /// <summary>
        /// 删除资源，删除操作会压栈并由自定义GC维护，如果需要实时删除资源，调用DestroyAssetImmediately
        /// </summary>
        /// <param name="obj">资源源文件，不能是引用</param>
        public static void DestroyAsset(UnityEngine.Object obj)
        {
            ResourcesMgr.Instance.DestroyAsset(obj);
        }

        public static void DestroyAssets(string[] paths)
        {
            ResourcesMgr.Instance.DestroyAssets(paths);
        }


        /// <summary>
        /// 同帧删除资源，适合引用数比较少的资源（如表）或需要及时释放内存时的操作（如地图九宫）
        /// </summary>
        /// <param name="path"></param>
        public static void DestroyAssetImmediately(string path)
        {
            ResourcesMgr.Instance.DestroyAssetImmediately(path);
        }

        public static void DestroyAssetImmediately(UnityEngine.Object obj)
        {
            ResourcesMgr.Instance.DestroyAssetImmediately(obj);
        }

        public static void DestroyAssetsImmediately(string[] paths)
        {
            ResourcesMgr.Instance.DestroyAssetsImmediately(paths);
        }


        /// <summary>
        /// 请慎用此接口,只有无依赖文件允许调用此接口,否则将返回空值
        /// </summary>
        /// <param name="path">AssetDatas下路径</param>
        /// <returns></returns>
        public static UnityEngine.Object LoadAssetImmediately(string path)
        {
            return ResourcesMgr.Instance.LoadAssetImmediate(path);
        }

        /// <summary>
        /// 请慎用此接口,只有无依赖文件允许调用此接口,否则将返回空值
        /// </summary>
        /// <param name="paths">AssetDatas下路径</param>
        /// <returns></returns>
        public static UnityEngine.Object[] LoadAssetsImmediately(string[] paths)
        {
            int length = paths.Length;
            UnityEngine.Object[] answers = new UnityEngine.Object[length];
            for (int i = 0; i < length; i++)
            {
                answers[i] = ResourcesMgr.Instance.LoadAssetImmediate(paths[i]);
            }
            return answers;
        }

        /// <summary>
        /// 二进制流形式读取StreamingAssets下文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] LoadFileInStreamingAssets(string fileName)
        {
            return ResourcesMgr.Instance.LoadFileInStreamingAssets(fileName);
        }

        /// <summary>
        /// 二进制流形式读取StreamingAssets下txt文件
        /// </summary>
        /// <param name="mainPath">StreamingAssets下相对路径，例如"Table/preInit.cfg"</param>
        /// <param name="decode"></param>
        /// <returns></returns>
        public static byte[] LoadTextBuffer(string mainPath, bool decode = true)
        {
            return ResourcesMgr.Instance.LoadTextBuffer(mainPath, decode);
        }

        /// <summary>Instance
        /// 字符串形式读取StreamingAssets下txt文件
        /// </summary>
        /// <param name="mainPath">StreamingAssets下相对路径，例如"Table/preInit.cfg"</param>
        /// <param name="decode"></param>
        /// <returns></returns>
        public static string LoadTextString(string mainPath, bool decode = true)
        {
            return ResourcesMgr.Instance.LoadTextString(mainPath, decode);
        }

        /// <summary>
        /// 主动注册某资源的返回回调
        /// </summary>
        /// <param name="resourcesRelativePath">AssetDatas下相对路径，例如"Prefabs/Hero/Hero1.prefab"</param>
        /// <param name="callBack"></param>
        public static void RegisterCallBack(string resourcesRelativePath, LoadAssetCallback callBack)
        {
            ResourcesMgr.Instance.RegisterCallBack(resourcesRelativePath, callBack);

        }

        /// <summary>
        /// 删除某资源的返回回调
        /// </summary>
        /// <param name="resourcesRelativePath">AssetDatas下相对路径，例如"Prefabs/Hero/Hero1.prefab"</param>
        /// <param name="callBack"></param>
        public static void RemoveCallBack(string resourcesRelativePath, LoadAssetCallback callBack)
        {
            ResourcesMgr.Instance.RemoveCallBack(resourcesRelativePath, callBack);
        }

        /// <summary>
        /// GC操作，会产生大量卡顿
        /// </summary>
        public static void Collect()
        {
            //Resources.UnloadUnusedAssets();   考虑到项目中基本不会用到Resources资源，舍弃该接口
            ResourcesMgr.Instance.Collect();
        }

        public static void Tick(float deltaTime)
        {
            m_GCTotalMemory = (Int32)(GC.GetTotalMemory(false) / (1048576.0f)); // 1024 * 1024
            if (Math.Abs(m_LastGCTime - Time.realtimeSinceStartup) < TimeIntervalCollect)
            {
                return;
            }
            else
            {
                m_LastGCTime = (Int32)Time.realtimeSinceStartup;
                if (m_GCTotalMemory >= (MaxCollect - 10))
                {
                    Collect();
                }
            }
        }

    }

}
