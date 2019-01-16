using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{

    public class SkillComponent : ComponentBase
    {
        public Dictionary<int, List<SkillProjectile>> m_ProjectileDict;
        public List<TriggerNotify> m_TriggerNotifyList;
        
        public Dictionary<int, SkillSpellLogic> m_SkillLogicDict;
        public List<SkillSpellLogic> m_PassiveList;
        public SkillSpellLogic[] m_TriggerSkillList;


        protected override void InitComponent()
        {

        }

        public override void OnPreDestroy()
        {

        }
        
        public void Update()
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
