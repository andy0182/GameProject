using UnityEngine;
using System.Collections;

public class Monster : AIPath 
{
	Animator animator;
	int Animator_Z=Animator.StringToHash("Z");
	int Animator_Speed=Animator.StringToHash("Speed");
	int Reach=1;
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
	void OnAnimatorMove()
	{
		Vector3 dir = CalculateVelocity (GetFeetPosition());
		RotateTowards (targetDirection);
		Vector3 pos = transform.forward*3*animator.GetFloat(Animator_Z)*Time.deltaTime;
		transform.Translate(pos,Space.World);
	}
	public override void Update ()
	{
		float sp=(target.position-transform.position).sqrMagnitude*Reach;
		animator.SetFloat(Animator_Speed,sp>0.1f?1:0);
	}
	public override void SearchPath ()
	{
		base.SearchPath ();
		Reach=1;
	}
}
