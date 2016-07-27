using UnityEngine;
using System.Collections;
using TimeManager;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting;


public class EntityMonoBehaviour : MonoBehaviour
{
	public EntityData entity;
	void Awake()
	{
		entity=new EntityData(gameObject);
	}
	public static implicit operator EntityData(EntityMonoBehaviour entity)
	{
		return entity.entity;
	}
	void Update()
	{
		entity.Update();
	}
}
public class EntityData : Entity
{
	GameObject Target;
	[ExecuteInMainthead]
	public override void SetTarget (float x, float y, float z)
	{
		if(Target==null)
		{
			AIPath ai=gameObject.GetComponent<AIPath>();
			Target=new GameObject(gameObject.name+"Target");
			ai.target=Target.transform;
		}
		Target.transform.position=new Vector3(x,y,z);
	}
	[ExecuteInMainthead]
	public override void SetPosition (float x, float y, float z)
	{
		gameObject.transform.position=new Vector3(x,1.13f,z);
	}
	public EntityData(GameObject gameObject)
	{
		this.gameObject=gameObject;
	}
	GameObject gameObject;
}

