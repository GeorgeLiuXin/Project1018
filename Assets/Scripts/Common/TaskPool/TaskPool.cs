using System.Collections.Generic;

namespace XWorld
{
    public sealed class TaskPool<T> where T : class, ITask
    {
        private readonly Stack<ITaskAgent<T>> m_FreeAgents;
        private readonly LinkedList<ITaskAgent<T>> m_WorkingAgents;
        private readonly KeyedPriorityQueue<int, T, int> m_WaitingTasks;

        public TaskPool()
        {
            m_FreeAgents = new Stack<ITaskAgent<T>>();
            m_WorkingAgents = new LinkedList<ITaskAgent<T>>();
            m_WaitingTasks = new KeyedPriorityQueue<int, T, int>();
        }

        public int TotalAgentCount
        {
            get
            {
                return FreeAgentCount + WorkingAgentCount;
            }
        }

        public int FreeAgentCount
        {
            get
            {
                return m_FreeAgents.Count;
            }
        }

        public int WorkingAgentCount
        {
            get
            {
                return m_WorkingAgents.Count;
            }
        }

        public int WaitingTaskCount
        {
            get
            {
                return m_WaitingTasks.Count;
            }
        }

        public void Update(float elapseSeconds)
        {
            LinkedListNode<ITaskAgent<T>> current = m_WorkingAgents.First;
            while (current != null)
            {
                if (current.Value.Task.Done)
                {
                    LinkedListNode<ITaskAgent<T>> next = current.Next;
                    current.Value.Reset();
                    m_FreeAgents.Push(current.Value);
                    m_WorkingAgents.Remove(current);
                    current = next;
                    continue;
                }

                current.Value.Update(elapseSeconds);
                current = current.Next;
            }

            while (FreeAgentCount > 0 && WaitingTaskCount > 0)
            {
                ITaskAgent<T> agent = m_FreeAgents.Pop();
                LinkedListNode<ITaskAgent<T>> agentNode = m_WorkingAgents.AddLast(agent);
                T task = m_WaitingTasks.Dequeue();
                agent.Start(task);
                if (task.Done)
                {
                    agent.Reset();
                    m_FreeAgents.Push(agent);
                    m_WorkingAgents.Remove(agentNode);
                }
            }
        }

        public void Shutdown()
        {
            while (FreeAgentCount > 0)
            {
                m_FreeAgents.Pop().Shutdown();
            }

            foreach (ITaskAgent<T> workingAgent in m_WorkingAgents)
            {
                workingAgent.Shutdown();
            }
            m_WorkingAgents.Clear();

            m_WaitingTasks.Clear();
        }

        public void AddAgent(ITaskAgent<T> agent)
        {
            if (agent == null)
            {
                GameLogger.Error(LOG_CHANNEL.LOGIC, "Task agent is invalid.");
            }

            agent.Initialize();
            m_FreeAgents.Push(agent);
        }

        public void AddTask(T task)
        {
            AddTask(task, 1);
        }

        public void AddTask(T task, int priority)
        {
            m_WaitingTasks.Enqueue(task.SerialId, task, priority);
        }

        public T RemoveTask(int serialId)
        {
            foreach (T waitingTask in m_WaitingTasks.Values)
            {
                if (waitingTask.SerialId == serialId)
                {
                    m_WaitingTasks.Remove(serialId);
                    return waitingTask;
                }
            }

            foreach (ITaskAgent<T> workingAgent in m_WorkingAgents)
            {
                if (workingAgent.Task.SerialId == serialId)
                {
                    workingAgent.Reset();
                    m_FreeAgents.Push(workingAgent);
                    m_WorkingAgents.Remove(workingAgent);
                    return workingAgent.Task;
                }
            }

            return default(T);
        }

        public void RemoveAllTasks()
        {
            m_WaitingTasks.Clear();
            foreach (ITaskAgent<T> workingAgent in m_WorkingAgents)
            {
                workingAgent.Reset();
                m_FreeAgents.Push(workingAgent);
            }
            m_WorkingAgents.Clear();
        }
    }
}
