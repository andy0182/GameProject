using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataBase : ScriptableObject 
{
	static DataBase mInstance;
	public static DataBase Instance
	{
		get
		{
			if(mInstance==null)
				mInstance=Resources.Load<DataBase>("DataBase");
			return mInstance;
		}
	}
	public List<BuffData> buffDatas=new List<BuffData>();
}
