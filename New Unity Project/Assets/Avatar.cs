using UnityEngine;
using System.Collections;
using System.Runtime.Remoting;

public class Avatar : EntityMonoBehaviour 
{
	public TransportEntity mAvatar;
	public Transform Target;
	Vector3 Position=Vector3.zero;
	// Use this for initialization
	void Start () 
	{
		Target=GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
		GameObject.FindObjectOfType<Pathfinding.TargetMover>().target=Target.transform;
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		base.Update();
		if(Input.GetMouseButtonDown(0))
		{
			mAvatar.SetTarget(Position.x,Position.y,Position.z);
		}
		if(Input.GetKeyDown(KeyCode.A))
		{
			mAvatar.Skill(0);
		}
	}
}
