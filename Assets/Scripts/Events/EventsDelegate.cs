using System;
using System.Collections;
using System.Collections.Generic;

public class EventsDelegate
{
	public delegate void EventFunction(params object[] values);

	private List<EventFunction> m_EventList;
	private int m_TriggerCount;
	private Hashtable m_FuncSendFlags;

	public EventsDelegate()
	{
		m_EventList = new List<EventFunction> ();
		m_FuncSendFlags = new Hashtable ();
	}

	public bool IsContain(EventFunction func)
	{
		if (func == null)
			return false;

		return m_EventList.IndexOf (func) >= 0;
	}

	public void Add(EventFunction func)
	{
		m_EventList.Add (func);
		m_TriggerCount++;
	}

	public void Remove(EventFunction func)
	{
		int idx = m_EventList.IndexOf (func);
		if (idx == -1)
			return;

		m_EventList.RemoveAt (idx);
		m_TriggerCount++;
	}

	public void Clear()
	{
		m_EventList.Clear ();
		m_FuncSendFlags.Clear ();
		m_TriggerCount = 0;
	}

	public void SendEvent(object[] values)
	{
		//暂不考虑多线程
		m_FuncSendFlags.Clear ();
		DoSend (values);
	}

	private void DoSend(object[] values)
	{
		m_TriggerCount = 0;

		EventFunction t_eventFunc;
		int t_count = m_EventList.Count;
		for (int i = 0; i < t_count; ++i) 
		{
			t_eventFunc = m_EventList [i];
			if (t_eventFunc == null)
				continue;

			t_eventFunc (values);
		}
	}
}

