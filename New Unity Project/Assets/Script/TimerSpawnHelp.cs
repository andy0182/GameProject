using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace TimeManager
{
	public class TimerSpawn
	{
		float interval;
		public int number;
		System.Action<int> sender;
		float _interval;
		public TimerSpawn(float _interval, int _number, System.Action<int> _sender)
		{
			interval = _interval;
			number = _number;
			sender = _sender;
		}
		public void Reset(float _interval, int _number, System.Action<int> _sender)
		{
			interval = _interval;
			number = _number;
			sender = _sender;
		}
		public void Update()
		{
			_interval += Time.deltaTime;
			if (_interval >= interval)
			{
				_interval = 0;
				sender(number);
			}
		}
	}
	
	public class TimerSpawnHelp 
	{
		List<TimerSpawn> Timehelp = new List<TimerSpawn>();
		List<TimerSpawn> mAddtime = new List<TimerSpawn>();
		List<TimerSpawn> mRemove = new List<TimerSpawn>();
		static List<TimerSpawn> ObjectPool=new List<TimerSpawn>();
		public void AddTimer(float interval, int number, System.Action<int> sender)
		{
			TimerSpawn t;
			if(ObjectPool.Count>0)
			{
				t=ObjectPool[0];
				ObjectPool.Remove(t);
				t.Reset(interval,number,sender);
			}else
			{
				t = new TimerSpawn(interval, number, sender);
			}
			mAddtime.Add (t);
		}
		public void RemoveTimer(int number)
		{
			var t=Timehelp.Find (a => a.number == number);
			mRemove.Add (t);
		}
		public void Clear()
		{
			Timehelp.ForEach(a=>{ObjectPool.RemoveAll(b=>b==a);ObjectPool.Add(a);});
			Timehelp.Clear();
		}
		// Update is called once per frame
		public void Update () 
		{
			mAddtime.ForEach (a => {Timehelp.RemoveAll(b=>b.number==a.number);Timehelp.Add (a);});
			mAddtime.Clear ();
			mRemove.ForEach (a => {Timehelp.Remove (a);ObjectPool.Remove(a);ObjectPool.Add(a);a.Reset(0,0,null);});
			mRemove.Clear ();
			Timehelp.ForEach (a => a.Update ());
		}
	}
}
