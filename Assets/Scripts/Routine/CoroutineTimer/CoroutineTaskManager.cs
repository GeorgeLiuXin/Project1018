using System;
using System.Collections;

namespace Galaxy
{
    public class CoroutineTaskManager 
    {
        public class Task
        {
            private bool m_IsRunning;
            private bool m_IsPaused;
            private bool m_IsStopped;
            private WeakReference m_Ref;
            private IEnumerator m_Coroutine;

            public GameAction<bool> Finished;
            public bool Enable
            {
                get
                {
                    if (m_Ref != null && m_Ref.IsAlive)
                    {
                        if (m_Ref.Target != null)
                        {
                            if (m_Ref.Target is UnityEngine.Object)
                            {
                                if ((m_Ref.Target as UnityEngine.Object) == null)
                                {
                                    return false;
                                }
                            }
                        }
                        return true;
                    }
                    return false;
                }
            }

            public bool IsRunning
            {
                get
                {
                    return m_IsRunning;
                }
            }

            public bool IsPaused
            {
                get
                {
                    return m_IsPaused;
                }
            }

            public bool IsStopped
            {
                get
                {
                    return m_IsStopped;
                }
            }

            public Task(object reciever, IEnumerator co)
            {
                m_Coroutine = co;
                m_Ref = new WeakReference(reciever);
            }

            public void Unpause()
            {
                m_IsPaused = false;
            }

            public void Pause()
            {
                m_IsPaused = true;
            }

            public void Start()
            {
                m_IsRunning = true;
                RoutineRunner.instance.StartCoroutine(CallWrapper());
            }

            public void Stop()
            {
                m_IsStopped = true;
                m_IsRunning = false;
            }

            IEnumerator CallWrapper()
            {
                yield return null;
                IEnumerator e = m_Coroutine;
                while (m_IsRunning)
                {
                    if (m_IsPaused)
                        yield return null;
                    else
                    {
                        if (!Enable)
                        {
                            Stop();
                            yield break;
                        }

                        if (e != null && e.MoveNext())
                        {
                            yield return e.Current;
                        }
                        else
                        {
                            m_IsRunning = false;
                        }
                    }
                }

                GameAction<bool> handler = Finished;
                if (handler != null)
                    handler(m_IsStopped);
            }
        }

        private static CoroutineTaskManager m_Instance = new CoroutineTaskManager();
        public static Task CreateTask(object reciever, IEnumerator coroutine)
        {
            return new Task(reciever, coroutine);
        }
    }
}
