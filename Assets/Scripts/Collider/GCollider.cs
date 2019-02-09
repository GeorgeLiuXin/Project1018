using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public class SSphere
    {
        public float r;
        public Vector3 pos;
        public SSphere(Vector3 _pos, float _r)
        {
            r = _r;
            pos = _pos;
        }
    }

    public class SRect
    {
        public float length;
        public float width;
        public Vector3 center;

        public SRect(Vector3 _center, float _length, float _width)
        {
            center = _center;
            length = _length;
            width = _width;
        }
    }

    public class SSector
    {
        public float r_max;
        public float r_min;
        public float angle;
        public Vector3 pos;
        public Vector3 dir;

        public SSector(Vector3 _pos, Vector3 _dir, float _rmax, float _rmin, float _angle)
        {
            r_max = _rmax;
            r_min = _rmin;
            angle = _angle;
            pos = _pos;
            dir = _dir;
        }
    }

    public class SRing
    {
        public float r_max;
        public float r_min;
        public Vector3 pos;

        public SRing(Vector3 _pos, float _rmax, float _rmin)
        {
            r_max = _rmax;
            r_min = _rmin;
            pos = _pos;
        }
    }



    /// <summary>
    /// SkillArea_Singleton = 1, //单体
    /// SkillArea_Sphere = 2, //球形范围
    /// SkillArea_Sector = 3, //扇形范围
    /// 参数1：最近边距自己的距离，参数2：半径，参数3：弧度
    /// SkillArea_Ring = 4, //环形范围
    /// SkillArea_Rect = 5, //矩形范围
    /// 参数1：最近边距自己的距离，参数2：最远边距自己的距离，参数3：宽度
    /// </summary>
    public class GCollider
    {
        /// <summary>
        /// 单体判定默认可以打到
        /// </summary>
        /// <returns></returns>
        public static bool SingletonCollideCheck()
        {
            return true;
        }

        /// <summary>
        /// 球体判定只要比R+r小即可（分离轴即为两个圆心的连线）
        /// </summary>
        /// <returns></returns>
        public static bool SphereCollideCheck(SSphere sSphere, SSphere pTarget, Vector3 vDir)
        {
            Vector2 sSpherePos = new Vector2(sSphere.pos.x, sSphere.pos.z);
            Vector2 sTargetPos = new Vector2(pTarget.pos.x, pTarget.pos.z);

            float distance = Vector2.Distance(sSpherePos, sTargetPos);

            if (distance <= sSphere.r + pTarget.r)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 矩形判定
        /// 1、求圆心与中心向量（取绝对值）
        /// 2、求圆心与第一象限顶点距离
        /// 3、得到点到矩形边的距离（分离轴即为圆心与最近的一边的垂线段）
        /// 4、与半径对比即可
        /// </summary>
        /// <returns></returns>
        public static bool RectCollideCheck(SRect sRect, SSphere pTarget, Vector3 vDir)
        {
            Vector2 sRectPos = new Vector2(sRect.center.x, sRect.center.z);
            Vector2 sTargetPos = new Vector2(pTarget.pos.x, pTarget.pos.z);

            Vector2 vAimDir = new Vector2(1, 0);
            Vector2 vOpAimDir = new Vector2(0, 1);

            //test

            //取二维向量绝对值
            Vector2 offestVec = Vector2.Max((sRectPos - sTargetPos), (sTargetPos - sRectPos));
            //取第一象限顶点
            Vector2 vertexVec = vAimDir * (sRect.length * 0.5f) + vOpAimDir * (sRect.width * 0.5f);
            //当x,y某值小于0时即非垂直向量,归置为0即可
            Vector2 verticalVec = Vector2.Max(offestVec - vertexVec, Vector3.zero);

            return verticalVec.sqrMagnitude <= pTarget.r * pTarget.r;
        }

        /// <summary>
        /// 扇形判定
        /// 共有三种分离轴
        /// 1、扇形圆心和圆盘圆心的方向（扇形的圆弧部分）
        /// 2、扇形两边的法线
        /// 3、扇形三个顶点和圆盘圆心的方向
        /// 
        /// 1、先判断扇形与圆盘的方向
        /// 2、计算圆盘圆心与扇形的最近点的向量 p
        /// 3、计算当前圆心是否在扇形内部
        /// 4、计算扇形左边半径是否与圆形相交
        /// </summary>
        /// <returns></returns>
        public static bool SectorCollideCheck(SSector sSector, SSphere pTarget, Vector3 vDir)
        {
            Vector2 sSectorPos = new Vector2(sSector.pos.x, sSector.pos.z);
            Vector2 sTargetPos = new Vector2(pTarget.pos.x, pTarget.pos.z);

            vDir.Normalize();
            Vector2 vAimDir = new Vector2(vDir.x, vDir.z);
            Vector2 vOpAimDir = new Vector2(-vDir.z, vDir.x);

            float angle = sSector.angle * 0.5f;

            // 1. 如果扇形圆心和圆盘圆心的方向
            Vector2 vOffest = sTargetPos - sSectorPos;
            if (vOffest.sqrMagnitude >= (sSector.r_max + pTarget.r) * (sSector.r_max + pTarget.r))
                return false;
            if (vOffest.sqrMagnitude <= (sSector.r_min - pTarget.r) * (sSector.r_min - pTarget.r))
                return false;

            // 2. 计算出扇形局部空间的 p
            float px = Vector2.Dot(vOffest, vAimDir);
            float py = Mathf.Abs(Vector2.Dot(vOffest, vOpAimDir));      //取仅为第一\二象限的情况化简难度

            // 3. 如果 p_x > ||p|| cos theta，两形状相交    当前圆心在扇形内部
            if (px > vOffest.magnitude * Mathf.Cos(angle))
                return true;

            // 4. 求左边线段与圆盘是否相交
            Vector2 q = sSector.r_max * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));    //得到扇形左边坐标
            Vector2 p = new Vector2(px, py);                                            //得到扇形与圆形相对坐标

            //计算出左边线段与圆盘的分离轴，之后通过长度对比即可
            float t = Vector2.Dot(p - Vector2.zero, q) / q.sqrMagnitude;
            float length = (p - (Vector2.zero + Mathf.Clamp(t, 0, 1) * q)).sqrMagnitude;

            return length <= pTarget.r * pTarget.r;
        }

        /// <summary>
        /// 环形判定
        /// 简单进行半径与敌人比对即可
        /// </summary>
        /// <returns></returns>
        public static bool RingCollideCheck(SRing sRing, SSphere pTarget, Vector3 vDir)
        {
            Vector2 sRingPos = new Vector2(sRing.pos.x, sRing.pos.z);
            Vector2 sTargetPos = new Vector2(pTarget.pos.x, pTarget.pos.z);

            vDir.Normalize();
            Vector2 vAimDir = new Vector2(vDir.x, vDir.z);
            Vector2 vOpAimDir = new Vector2(-vDir.z, vDir.x);

            Vector2 vOffest = sTargetPos - sRingPos;
            if (vOffest.sqrMagnitude >= (sRing.r_max + pTarget.r) * (sRing.r_max + pTarget.r))
                return false;
            if (vOffest.sqrMagnitude <= (sRing.r_min - pTarget.r) * (sRing.r_min - pTarget.r))
                return false;

            return true;
        }

    }

}
