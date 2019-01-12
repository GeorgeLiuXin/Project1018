using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{

    public class SkillComponent : ComponentBase
    {
        public Dictionary<int, ConfigData> m_SkillDataDict;


        protected override void InitComponent()
        {

        }

        public override void OnPreDestroy()
        {

        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool AddSkill(int nSkillID)
        {
            return false;
        }

        public void RemoveSkill(int nSkillID)
        {

        }

        public bool HasSkill(int nSkillID)
        {
            return false;
        }

    }

}
