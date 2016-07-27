using UnityEngine;
using System.Collections;
using System.Runtime.Remoting;

public class Avatar : EntityMonoBehaviour 
{
	public TransportEntity mAvatar;
	Transform target;
	Vector3 Position=Vector3.zero;
	// Use this for initialization
	void Start () 
	{
		target=GetComponent<AIPath>().target;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if((target.position-Position).sqrMagnitude>5.5f)
		{
			Position=target.position;
			mAvatar.SetTarget(Position.x,Position.y,Position.z);
		}
	}
}
