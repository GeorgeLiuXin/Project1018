using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{

    public class ActorComponent : DataObj
    {

        private List<ComponentBase> m_ComponentsList;

        protected GameObject m_EngineObj = null;
        protected GameObject m_ModelResObj = null;

        //test
        private ComponentBase m_baseCom;
        public ComponentBase BaseCom
        {
            get
            {
                if (m_baseCom == null)
                {
                    if (m_EngineObj != null)
                    {
                        m_baseCom = m_EngineObj.GetComponent<ComponentBase>();
                    }
                }
                return m_baseCom;
            }
        }
        
        public virtual bool CreateEngineObj()
        {
            //占坑
            return true;
        }

        public virtual void AfterCreateEngineObj()
        {
            m_ComponentsList = new List<ComponentBase>();

            AddComponent("ComponentBase");
            //AddComponent("FSMComponent");
        }

        public virtual void BeforeDestroy()
        {
            for (int i = 0; i < m_ComponentsList.Count; ++i)
            {
                if (m_ComponentsList[i] == null)
                {
                    continue;
                }

                m_ComponentsList[i].OnPreDestroy();

                GameObject.Destroy(m_ComponentsList[i]);
                m_ComponentsList[i] = null;
            }

            m_ComponentsList.Clear();
        }

        protected void SetOwner(ActorObj actor)
        {
            foreach (ComponentBase component in m_ComponentsList)
            {
                component.SetOwner(actor);
            }
        }

        public void AddComponent(string componentName)
        {
            if (m_EngineObj == null || componentName == default(string))
                return;

            string strComName = "XWorld." + componentName;
            Type type = Type.GetType(strComName);
            if (type == null)
                return;

            ComponentBase baseCom = m_EngineObj.AddComponent(type) as ComponentBase;
            if (baseCom != null)
            {
                m_ComponentsList.Add(baseCom);
                return;
            }

            GameLogger.Error(LOG_CHANNEL.ERROR, "AddComponent failed! component is not base of ComponentBase! component named : " + strComName);
        }

        public bool RemoveComponent(ComponentBase com)
        {
            if (m_EngineObj == null || com == null)
                return false;

            return m_ComponentsList.Remove(com);
        }

        public void SetEngineObj(GameObject engineObj)
        {
            m_EngineObj = engineObj;
        }
        public GameObject GetEngineObj()
        {
            return m_EngineObj;
        }
        public void SetModelResObj(GameObject modelResObj)
        {
            m_ModelResObj = modelResObj;
        }
        public GameObject GetModelResObj()
        {
            return m_ModelResObj;
        }
    }

}