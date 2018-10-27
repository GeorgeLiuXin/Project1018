/**************************************************
 *  创建人   : 夏佳文
 *  创建时间 : 2018.6.20
 *  说明     : Asset类型配置（类型 + 后缀名）
 * ************************************************/

using System;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

namespace Galaxy.AssetPipeline
{
    internal enum EAssetType
    {
        UNKOWN,

        [AssetBuild(new Type[1] { typeof(Material) },
            ".mat")]
        Material,

        [AssetBuild(new Type[] { typeof(Texture), typeof(Texture2D), typeof(Texture3D), typeof(Sprite) },
            ".png", ".jpg", ".psd", ".bmp", ".tga", ".tif", ".dds", ".exr")]
        Texture,

        [AssetBuild(new Type[] { typeof(GameObject) },
            ".prefab")]
        Prefab,

        [AssetBuild(new Type[] { typeof(GameObject), typeof(Mesh) },
            ".fbx", ".FBX", ".mesh")]
        Mesh,

        [AssetBuild(new Type[] { typeof(Animation), typeof(AnimationClip), typeof(AnimatorController), typeof(TimelineAsset) },
            ".anim", ".controller", ".playable")]
        Animation,

		[AssetBuild(new Type[] { typeof(UnityEditor.SceneAsset),typeof(UnityEditor.LightingDataAsset) },
            ".unity",".asset")]
        Scene,

        [AssetBuild(new Type[] { typeof(TextAsset) },
            ".bytes", ".xml", ".txt", ".html", ".htm")]
        Bytes,

        [AssetBuild(new Type[] { typeof(Shader) },
           ".shader")]
        Shader,

        [AssetBuild(new Type[] { typeof(AudioClip) },
           ".aiff", ".wav", ".mp3", ".ogg")]
        Audio,

        [AssetBuild(new Type[] { typeof(Font) },
           ".ttf", ".TTF")]
        Font,
    }
}
