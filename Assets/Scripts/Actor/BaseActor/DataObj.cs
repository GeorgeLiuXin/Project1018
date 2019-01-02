using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public class DataObj : MonoBehaviour
    {
        protected Vector3 Pos;
        protected Vector3 Dir;

        //eleven randall    obj基本数据存储单位
        //public ParamPool Pool { get; set; }
        public DataObj()
        {
            Pos = new Vector3();
            Dir = new Vector3();
            Pos.x = Pos.y = Pos.z = 0;
            Dir.x = 1;
            Dir.y = 0;
            Dir.z = 0;
        }

        public virtual Vector3 GetPos() { return Vector3.zero; }
        public virtual Vector3 GetDir() { return Vector3.up; }

        public virtual void Tick(float fFrameTime) { }

        public virtual void UnloadRes() { }
    }

}
