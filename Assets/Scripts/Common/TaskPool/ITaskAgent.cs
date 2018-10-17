
namespace XWorld
{
    public interface ITaskAgent<T> where T : ITask
    {
        T Task
        {
            get;
        }
        
        void Initialize();

        void Update(float deltaTime);

        /// <summary>
        /// 关闭并清理任务代理。
        /// </summary>
        void Shutdown();

        /// <summary>
        /// 开始处理任务。
        /// </summary>
        /// <param name="task">要处理的任务。</param>
        void Start(T task);

        /// <summary>
        /// 停止正在处理的任务并重置任务代理。
        /// </summary>
        void Reset();
    }
}
