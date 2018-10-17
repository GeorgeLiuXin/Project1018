using System;
using System.Collections;
using System.Collections.Generic;

namespace XWorld
{
	public class EventListener : Singleton<EventListener>
	{
		private static Dictionary<string,EventsDelegate> m_dictDelegate = new Dictionary<string, EventsDelegate>();

		public EventListener()
		{
			
		}

		public void AddListener(string strEvent,EventsDelegate.EventFunction eventFunc)
		{
			if (eventFunc == null)
				return;

			if (!m_dictDelegate.ContainsKey (strEvent)) 
			{
				m_dictDelegate.Add (strEvent,new EventsDelegate());
			}

			if (!m_dictDelegate [strEvent].IsContain (eventFunc)) 
			{
				m_dictDelegate [strEvent].Add (eventFunc);
			}
		}

		public void RemoveListener(string strEvent,EventsDelegate.EventFunction eventFunc)
		{
			if (eventFunc == null)
				return;

			if (!m_dictDelegate.ContainsKey (strEvent))
				return;

			EventsDelegate curDelegate = m_dictDelegate [strEvent];
			if (curDelegate == null)
				return;

			curDelegate.Remove (eventFunc);
		}

		public void Dispatch(string strEvent,params object[] values)
		{
			if (!m_dictDelegate.ContainsKey (strEvent)) 
			{
				return;
			}

			m_dictDelegate [strEvent].SendEvent (values);
		}
	}
}