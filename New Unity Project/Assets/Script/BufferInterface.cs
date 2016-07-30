using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public interface BufferInterface 
{
	void RemoveBuffer(BufferObject buff);
	void AddBuffer(BufferObject buff);
	List<BufferObject> buffers
	{
		get;
	}
	GameObject gameObject
	{
		get;
	}
	Transform transform
	{
		get;
	}
}

public class DictionaryEx<T1,T2>:Dictionary<T1,T2> where T2:new()
{
	public T2 this[T1 key]
	{
		get
		{
			if(!ContainsKey(key))
			{
				Add(key,new T2());
			}
			return base[key];
		}
		set
		{
			base[key]=value;
		}
	}
	public void FoReach(System.Action<T1,T2> Callback)
	{
		var keys =new List<T1>(Keys);
		for(int i=0;i<keys.Count;i++)
		{
			Callback(keys[i],this[keys[i]]);
		}
	}
}
