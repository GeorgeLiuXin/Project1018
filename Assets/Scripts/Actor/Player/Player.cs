using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public class Player : ActorObj
    {
        
        public override void AfterCreateEngineObj()
        {
            base.AfterCreateEngineObj();
        }

        public override bool IsPlayer()
        {
            return true;
        }
   
        public override void BeforeDestroy()
        {
            base.BeforeDestroy();
        }
        
    }

}