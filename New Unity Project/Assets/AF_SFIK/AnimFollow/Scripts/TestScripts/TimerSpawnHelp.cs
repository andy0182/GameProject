using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace TimeManager
{
	public class TimerSpawn
	{
		float interval;
		int number;
		System.Action<int> sender;
		float _interval;
		public TimerSpawn(float _interval, int _number, System.Action<int> _sender)
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
		public static bool operator ==(TimerSpawn a,int number)
		{
			return a.number==number;
		}
		public static bool operator !=(TimerSpawn a,int number)
		{
			return a.number!=number;
		}
		public static bool operator ==(TimerSpawn a,TimerSpawn b)
		{
			return a.number==b.number;
		}
		public static bool operator !=(TimerSpawn a,TimerSpawn b)
		{
			return a.number!=b.number;
		}
	}

	public class TimerSpawnHelp 
	{
		List<TimerSpawn> Timehelp = new List<TimerSpawn>();
		List<TimerSpawn> mAddtime = new List<TimerSpawn>();
		List<TimerSpawn> mRemove = new List<TimerSpawn>();

		public void AddTimer(float interval, int number, System.Action<int> sender)
		{
			TimerSpawn t = new TimerSpawn(interval, number, sender);
			mAddtime.Add (t);
		}
		public void RemoveTimer(int number)
		{
			mRemove.Add (Timehelp.Find (a => a == number));
		}
		public void Clear()
		{
			Timehelp.Clear();
		}
		// Update is called once per frame
		public void Update () 
		{
			mAddtime.ForEach (a => {Timehelp.RemoveAll(b=>b==a);Timehelp.Add (a);});
			mAddtime.Clear ();
			mRemove.ForEach (a => Timehelp.Remove (a));
			mRemove.Clear ();
			Timehelp.ForEach (a => a.Update ());
		}
	}
}
