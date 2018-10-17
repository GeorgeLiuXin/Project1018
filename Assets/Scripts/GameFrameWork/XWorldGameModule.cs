using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace XWorld
{
    public class XWorldGameModule
    {
        private static List<XWorldGameManagerBase> m_lManagers = new List<XWorldGameManagerBase>();

        public static T GetGameManager<T>() where T : class
        {
           Type classType = typeof(T);

            if (!classType.FullName.StartsWith("XWorld"))
            {
                return null;
            }

            Type fullClassType = Type.GetType(classType.FullName);
            XWorldGameManagerBase findClass = FindManager(fullClassType);
            if (findClass == null)
            {
                findClass = RegisterGameManager(fullClassType);
            }

            return findClass as T;
        }

        public static void Update(float fElapseTimes)
        {
            if (m_lManagers == null || m_lManagers.Count == 0)
                return;

            //foreach (XWorldGameManagerBase item in m_lManagers)
            for(int i = 0; i < m_lManagers.Count; ++i)
            {
                if (m_lManagers[i] == null)
                    continue;

                m_lManagers[i].Update(fElapseTimes);
            }
        }

        public static void RemoveGameManager(XWorldGameManagerBase gameManager)
        {
            if (gameManager == null)
                return;

            foreach (XWorldGameManagerBase item in m_lManagers)
            {
                if (item == null)
                    continue;

                if (item == gameManager)
                {
                    item.ShutDown();
                    m_lManagers.Remove(item);
                    break;
                }
            }
        }

        private static XWorldGameManagerBase RegisterGameManager(Type managerClass)
        {
            XWorldGameManagerBase newManager = Activator.CreateInstance(managerClass) as XWorldGameManagerBase;
            if(newManager == null)
            {
                GameLogger.Error(LOG_CHANNEL.LOGIC,"Register manager " + managerClass.ToString() + " failed!");
                return null;
            }

            GameLogger.DebugLog(LOG_CHANNEL.LOGIC, " Register manager " + managerClass.ToString());
            InsertManagerToList(newManager);
            newManager.InitManager();
            return newManager;
        }

        private static void InsertManagerToList(XWorldGameManagerBase manager)
        {
            m_lManagers.Add(manager);
        }

        private static XWorldGameManagerBase FindManager(Type managerClass)
        {
            foreach(XWorldGameManagerBase item in m_lManagers)
            {
                if (item == null)
                    continue;

                if (item.GetType().Equals(managerClass))
                    return item;
            }

            return null;
        }
    }
}
