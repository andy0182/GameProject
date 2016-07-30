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
	public ClientObject1 mPlayer;
	public static ApplicationControol Instance;
	public List<EntityMonoBehaviour> Entitys=new List<EntityMonoBehaviour>();
	public class ClientObject1 : Player
	{
		List<EntityMonoBehaviour> Entitys;
		GameObject[] PrefabEntityObject;
		public ClientObject1(GameObject[] PrefabEntityObject)
		{
			this.PrefabEntityObject=PrefabEntityObject;
			Entitys=ApplicationControol.Instance.Entitys;
		}
		[ExecuteInMainthead]
		public override Entity Create(int ID)
		{
			GameObject tmp=GameObject.Instantiate(PrefabEntityObject[ID]) as GameObject;
			tmp.SetActive(true);
			EntityMonoBehaviour go=tmp.AddComponent<EntityMonoBehaviour>();
			Entitys.Add(go);
			return go;
		}
		[ExecuteInMainthead]
		public override Entity CreateAvatar (TransportEntity mEntity)
		{
			GameObject tmp=GameObject.Instantiate(PrefabEntityObject[0]) as GameObject;
			tmp.SetActive(true);
			Avatar go=tmp.AddComponent<Avatar>();
			go.mAvatar=mEntity;
			Entitys.Add(go);
			return go;
		}
	}
	void Awake()
	{
		Instance=this;
	}
	void Start () 
	{
		new Thread(()=>
		{
			ServerObject.InitConnect(0);
			TransportInterface tmp =ServerObject.GetObject<TransportInterface>("tcp://127.0.0.1:9000/RemotingObject");
			mPlayer=new ClientObject1(PrefabEntityObject);
			tmp.Loging(mPlayer);
		}).Start();
	}
	void Update()
	{
		if(mPlayer!=null)mPlayer.Update();
	}
	void OnApplicationQuit()
	{
		ServerObject.Close ();
	}
}
