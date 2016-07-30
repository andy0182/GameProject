using UnityEngine;
using System.Collections;
using TimeManager;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting;
using Pathfinding.RVO;


public class EntityMonoBehaviour : UnityEngine.MonoBehaviour,BufferInterface
{
	public List<BufferObject> Buffers=new List<BufferObject>();
	List<BufferObject> mAddBuffer=new List<BufferObject>();
	List<BufferObject> mRemoveBuffer=new List<BufferObject>();
	public List<BufferObject> buffers {
		get {
			return Buffers;
		}
	}
	public EntityData entity;
	public virtual void Awake()
	{
		entity=new EntityData(gameObject);
	}
	public static implicit operator EntityData(EntityMonoBehaviour entity)
	{
		return entity.entity;
	}
	public virtual void Update()
	{
		entity.Update();
		BufferUpdate();
	}
	protected void BufferUpdate()
	{
		mAddBuffer.RemoveAll(a=>a==null);
		///通知其他buffer，有buffer加入列表
		mAddBuffer.ForEach(a=>{Buffers.ForEach(b=>b.OnAdd(a));if(!mRemoveBuffer.Contains(a)){a.OnEnter(this);Buffers.Add(a);}});
		mAddBuffer.Clear();
		///通知所有buffer，有buffer从列表移除
		mRemoveBuffer.ForEach(a=>{Buffers.Remove(a);a.OnExit();Buffers.ForEach(b=>b.OnRemove(a));});
		mRemoveBuffer.Clear();
		///更新buffer里的update
		Buffers.ForEach(a=>a.OnUpdate());
	}
	public virtual void AddBuffer(BufferObject buffer)
	{
		mAddBuffer.Remove (buffer);
		mAddBuffer.Add(buffer);
	}
	public virtual void RemoveBuffer(BufferObject buffer)
	{
		mRemoveBuffer.Remove (buffer);
		mRemoveBuffer.Add(buffer);
	}
	public virtual void OnDestroy()
	{
		Buffers.ForEach(a=>a.EnitityOnDestroy());
	}
	public virtual void OnDesable()
	{
		Buffers.ForEach(a=>a.EntityOnDisable());
	}
}
public class EntityData : Entity
{
	BufferInterface Parasitifer;
	AIPath ai;
	GameObject Target;
	CoroutineManager mCoroutin=new CoroutineManager();
	[ExecuteInMainthead]
	public override void SetTarget (float x, float y, float z)
	{
		//ai.SetTarget(new Vector3(x,y,z));
		Target.transform.position=new Vector3(x,y,z);
	}
	[ExecuteInMainthead]
	public override void SetPosition (float x, float y, float z)
	{
		gameObject.transform.position=new Vector3(x,1.13f,z);
	}
	[ExecuteInMainthead]
	public override void OnDestroy ()
	{
		GameObject.Destroy(gameObject);
	}
	[ExecuteInMainthead]
	public override void Skill (int id)
	{
		Parasitifer.AddBuffer(DataBase.Instance.buffDatas.Find(a=>a.ID==id).buff);
	}
	public EntityData(GameObject gameObject)
	{
		this.gameObject=gameObject;
		ai=gameObject.GetComponent<AIPath>();
		Target=new GameObject(gameObject.name+"Target");
		ai.target=Target.transform;
		Parasitifer=gameObject.GetComponent<EntityMonoBehaviour>();
	}
	public override void Update ()
	{
		base.Update ();
		mCoroutin.Update();
	}
	GameObject gameObject;
}

