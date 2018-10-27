using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ShaderResetTools
{
#if UNITY_EDITOR
    public static void ResetShader(Object root)
    {
        if (root == null)
            return;

        Object[] objs = UnityEditor.EditorUtility.CollectDependencies(new Object[] { root });
        foreach (Object obj in objs)
        {
            Material m = obj as Material;
            if (m)
            {
                m.shader = Shader.Find(m.shader.name);
            }
        }
    }

    public static void ResetScene()
    {
        Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (activeScene != null)
        {
            GameObject[] objs = activeScene.GetRootGameObjects();
            foreach (GameObject obj in objs)
            {
                ResetShader(obj);
            }
        }

        ResetShader(RenderSettings.skybox);
    }
#endif

}

