using UnityEngine;
using System.Collections;
using Pathfinding.RVO;
using System.Collections.Generic;
using Pathfinding;

public class Monster : AIPath 
{
	Animator animator;
	int Animator_Z=Animator.StringToHash("Z");
	int Animator_Speed=Animator.StringToHash("Speed");
	int Reach=1;
	protected override void Awake ()
	{
		base.Awake ();
	}
	protected override void Start ()
	{
		base.Start ();
		animator=GetComponent<Animator>();
	}
	public override void OnTargetReached ()
	{
		base.OnTargetReached ();
		Reach=0;
	}
	public override void Update ()
	{
		Vector3 dir = CalculateVelocity (GetFeetPosition());
		animator.SetFloat(Animator_Speed,dir.sqrMagnitude>0.1f?1:0);
		RotateTowards (targetDirection);
		Vector3 pos = dir*animator.GetFloat(Animator_Z);
		rvoController.Move(pos);
	}
	public override void SearchPath ()
	{
		base.SearchPath ();
		Reach=1;
	}
}
