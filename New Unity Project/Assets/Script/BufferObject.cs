using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


public class BufferObject : ScriptableObject 
{
	[HideInInspector]
	public int ID;
	bool isEnter=false;
	[HideInInspector]
	public BufferInterface Parasitifer;
	public event System.Action EventOnUpdate;
	public event System.Action EventOnEnable;
	public event System.Action EventOnDisable;
	public event System.Action EventOnDestroy;
	public event System.Action EventOnExit;
	public event System.Action<BufferInterface> EventOnEnter;
	public event System.Action<BufferObject> EventOnAdd;
	public event System.Action<BufferObject> EventOnRemove;

	/// <summary>
	/// 依附体每帧调用
	/// </summary>
	public virtual void OnUpdate()
	{
		if(EventOnUpdate!=null)EventOnUpdate();
	}
	/// <summary>
	/// 当依附体打开的时候调用
	/// </summary>
	public virtual void EnityOnEnable()
	{
		if(EventOnEnable!=null)EventOnEnable();
	}
	/// <summary>
	/// 当依附体禁用的时候调用
	/// </summary>
	public virtual void EntityOnDisable()
	{
		if(EventOnDisable!=null)EventOnDisable();
	}
	/// <summary>
	/// 当依附体销毁的时候调用
	/// </summary>
	public virtual void EnitityOnDestroy()
	{
		if(EventOnDestroy!=null)EventOnDestroy();
	}
	/// <summary>
	/// 刚刚构造出来时调用
	/// </summary>
	public virtual void Awake()
	{
		ID=GetType().ToString().GetHashCode();
	}
	/// <summary>
	/// 当列表里增加一个buffer的时候调用,不包括自己
	/// </summary>
	/// <param name="buffer">Buffer.</param>
	public virtual void OnAdd(BufferObject buffer)
	{
		if(EventOnAdd!=null)EventOnAdd(buffer);
	}
	/// <summary>
	/// 当列表移除一个buffer的时候调用，不包括自己
	/// </summary>
	/// <param name="buffer">Buffer.</param>
	public virtual void OnRemove(BufferObject buffer)
	{
		if (EventOnRemove != null)EventOnRemove (buffer);
	}
	/// <summary>
	/// 当自己增加到列表的时候调用
	/// </summary>
	public virtual void OnEnter(BufferInterface parasitifer)
	{
		Parasitifer=parasitifer;
		if (EventOnEnter != null)EventOnEnter (Parasitifer);
	}
	public void RemoveAllEvent()
	{
		ClearAllEvents("EventOnUpdate");
		ClearAllEvents("EventOnEnable");
		ClearAllEvents("EventOnDisable");
		ClearAllEvents("EventOnDestroy");
		ClearAllEvents("EventOnExit");
		ClearAllEvents("EventOnEnter");
		ClearAllEvents("EventOnAdd");
		ClearAllEvents("EventOnRemove");
	}
	/// <summary>
	/// 当自己已经从列表移除时调用
	/// </summary>
	public virtual void OnExit()
	{
		if(EventOnExit!=null)
			EventOnExit();
		RemoveAllEvent();
	}
	static DictionaryEx<int,Queue<BufferObject>> Buffs=new DictionaryEx<int,Queue<BufferObject>>(); 
	public static BufferObject Instantiate(BufferObject mObject)
	{
		if(Buffs[mObject.ID].Count>0)
			return Buffs[mObject.ID].Dequeue();
		return Object.Instantiate(mObject) as BufferObject;
	}
	public void ClearAllEvents(string eventName)
	{
		object objectHasEvents=this;
		try
		{
			EventInfo[] events = objectHasEvents.GetType().GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			if (events == null || events.Length < 1)
			{
				return;
			}
			for (int i = 0; i < events.Length; i++)
			{
				EventInfo ei = events[i];
				if (ei.Name == eventName)
				{
					FieldInfo fi = ei.DeclaringType.GetField(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
					if (fi != null)
					{
						fi.SetValue(objectHasEvents, null);
					}
					break;
				}
			}
		}
		catch
		{
		}
	}
}
