using UnityEngine;
using System.Collections;
using TimeManager;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Reflection;

public class ApplicationControol : UnityEngine.MonoBehaviour 
{
	public GameObject[] PrefabEntityObject;
	public QueueEX<System.Runtime.Remoting.Action> Queues = new QueueEX<System.Runtime.Remoting.Action>();

	public class ClientObject1 : Player
	{
		GameObject[] PrefabEntityObject;
		public ClientObject1(GameObject[] PrefabEntityObject)
		{
			this.PrefabEntityObject=PrefabEntityObject;
		}
		[ExecuteInMainthead]
		public override Entity Create(int ID)
		{
			GameObject tmp=GameObject.Instantiate(PrefabEntityObject[ID]) as GameObject;
			tmp.SetActive(true);
			EntityMonoBehaviour go=tmp.AddComponent<EntityMonoBehaviour>();
			return go;
		}
		[ExecuteInMainthead]
		public override Entity CreateAvatar (TransportEntity mEntity)
		{
			GameObject tmp=GameObject.Instantiate(PrefabEntityObject[0]) as GameObject;
			tmp.SetActive(true);
			Avatar go=tmp.AddComponent<Avatar>();
			go.mAvatar=mEntity;
			return go;
		}
	}
	ClientObject1 mPlayer;
	CoroutineManager mCoroutine=new CoroutineManager();
	void Start () 
	{
		int random=UnityEngine.Random.Range(1,100);
		new Thread(()=>
		{
			ServerObject.InitConnect(9003+random);
			TransportInterface tmp =ServerObject.GetObject<TransportInterface>("tcp://127.0.0.1:9000/RemotingObject");
			mPlayer=new ClientObject1(PrefabEntityObject);
			tmp.Loging(mPlayer);
		}).Start();
	}
	void Update()
	{
		mCoroutine.Update();
		if(mPlayer!=null)mPlayer.Update();
	}
	void OnApplicationQuit()
	{
		ServerObject.Close ();
	}
}
