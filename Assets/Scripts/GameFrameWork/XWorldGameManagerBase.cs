using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XWorld
{
    public abstract class XWorldGameManagerBase
    {
        /// <summary>
        /// 初始化管理者
        /// </summary>
        public abstract void InitManager();

        /// <summary>
        /// 更新 逻辑切片时间
        /// </summary>
        /// <param name="fElapseTimes"></param>
        public abstract void Update(float fElapseTimes);

        /// <summary>
        /// 游戏退出
        /// </summary>
        public abstract void ShutDown();

        /// <summary>
        /// 断线重连以后的处理
        /// </summary>
        public virtual void OnReEnterGame() { }

        /// <summary>
        /// 进入登录流程的处理
        /// </summary>
        public virtual void OnEnterLogin() { }

        /// <summary>
        /// 第一次进入游戏的处理
        /// </summary>
        public virtual void OnEnterGame() { }

    }
}
