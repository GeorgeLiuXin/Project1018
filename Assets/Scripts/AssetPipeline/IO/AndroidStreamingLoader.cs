using UnityEngine;
using System;

namespace XWorld.AssetPipeline
{
    public class AndroidStreamingLoader : StreamingAssetsLoader
    {
        private AndroidJavaObject activity_;
        private AndroidJavaObject ActivityJO
        {
            get
            {
                if (activity_ != null)
                    return activity_;

                activity_ = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                return activity_;
            }
        }

        public override byte[] Load(string name)
        {
            //取得应用的Activity
            //LogSystem.Info(bundlename);
            //从Activity取得AssetManager实例
            AndroidJavaObject assetManager = ActivityJO.Call<AndroidJavaObject>("getAssets");

            //打开文件流
            AndroidJavaObject stream = assetManager.Call<AndroidJavaObject>("open", name);
            //获取文件长度
            int availableBytes = stream.Call<int>("available");

            //取得InputStream.read的MethodID
            IntPtr clsPtr = AndroidJNI.FindClass("java/io/InputStream");
            IntPtr METHOD_read = AndroidJNIHelper.GetMethodID(clsPtr, "read", "([B)I");
            //IntPtr METHOD_read = AndroidJNIHelper.GetMethodID(clsPtr, "read");
            //申请一个Java ByteArray对象句柄
            IntPtr byteArray = AndroidJNI.NewByteArray(availableBytes);
            //调用方法
            int readCount = AndroidJNI.CallIntMethod(stream.GetRawObject(), METHOD_read, new[] { new jvalue() { l = byteArray } });
            //从Java ByteArray中得到C# byte数组
            byte[] bytes = AndroidJNI.FromByteArray(byteArray);
            //删除Java ByteArray对象句柄
            AndroidJNI.DeleteLocalRef(clsPtr);
            AndroidJNI.DeleteLocalRef(byteArray);

            //关闭文件流
            stream.Call("close");
            stream.Dispose();
            //返回结果
            return bytes;
        }
    }
}