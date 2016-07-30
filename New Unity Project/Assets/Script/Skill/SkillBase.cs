using UnityEngine;
using System.Collections;

public class SkillBase : BufferObject 
{
	[HideInInspector]
	public BuffData skilldata;
	protected TimeManager.TimerSpawnHelp m_timer=new TimeManager.TimerSpawnHelp();
	protected int LifeTime=Animator.StringToHash("LifeTime");
	public override void OnEnter (BufferInterface parasitifer)
	{
		base.OnEnter (parasitifer);
		m_timer.AddTimer(skilldata.LifeTime,LifeTime,a=>Parasitifer.RemoveBuffer(this));
	}
	public override void OnUpdate ()
	{
		base.OnUpdate ();
		m_timer.Update();
	}
}
