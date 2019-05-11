using System;
using System.Collections;
using UnityEngine;

namespace Galaxy
{
    public class RoutineRunner : MonoBehaviour
    {
        private static RoutineRunner _instance = null;
        public static RoutineRunner instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject gameObject = new GameObject("RoutineRunner");
                    _instance = gameObject.AddComponent<RoutineRunner>();
                    DontDestroyOnLoad(gameObject);
                }
                return _instance;
            }
        }
        public static T InstantiateGameObject<T>(UnityEngine.Object objectInLibrary) where T : UnityEngine.Object
        {
            UnityEngine.Object obj = Instantiate(objectInLibrary);
            T t = obj as T;
            if (obj != null && t == null)
            {
                GameLogger.Error(LOG_CHANNEL.LOGIC, "Trying to create an object named " + objectInLibrary.name + ", but expected the wrong type.");
            }
            return t;
        }
        public static GameObject InstantiateGameObject(string name, Transform parent = null)
        {
            GameObject gameObject = null;
            try
            {
                gameObject = (Instantiate(Resources.Load(name, typeof(GameObject))) as GameObject);
                if (parent != null)
                {
                    gameObject.transform.SetParent(parent, true);
                }
            }
            catch
            {
                GameLogger.Error(LOG_CHANNEL.LOGIC, "InstantiateGameObject failed. Name: " + name);
            }
            return gameObject;
        }
        public static T InstantiateWithComponent<T>(string name, Transform parent = null) where T : Component
        {
            T result = (T)((object)null);
            try
            {
                GameObject gameObject = Instantiate(Resources.Load(name, typeof(GameObject))) as GameObject;
                result = gameObject.GetComponent<T>();
                if (parent != null)
                {
                    gameObject.transform.SetParent(parent, true);
                }
            }
            catch
            {
                GameLogger.Error(LOG_CHANNEL.LOGIC, "InstantiateWithComponent failed. Name: " + name);
            }
            return result;
        }
        

        public static GameObject GetResFormResourcesFold(string strPath, string strRes)
        {
            string str = strPath;
            str += '/';
            str += strRes;
            return Resources.Load<GameObject>(str);
        }

        public static void WaitForSeconds(float fSeconds, Action action)
        {
            instance.StartCoroutine(instance.WaitForSecondsCoroutine(fSeconds, action));
        }

        private IEnumerator WaitForSecondsCoroutine(float fSeconds, Action action)
        {
            yield return Yielders.WaitSecond(fSeconds);
            if (action != null)
            {
                action();
            }
        }

        public static void WaitOneFrame(Action action)
        {
            instance.StartCoroutine(instance.WaitFrameCoroutine(action, 1));
        }

        public static void WaitFrame(Action action, int count)
        {
            instance.StartCoroutine(instance.WaitFrameCoroutine(action, count));
        }

        private IEnumerator WaitFrameCoroutine(Action action, int count)
        {
            while ((count--) >= 0)
            {
                yield return Yielders.WaitEndOfFrame;
            }
            if (action != null)
            {
                action();
            }
        }
    }
}