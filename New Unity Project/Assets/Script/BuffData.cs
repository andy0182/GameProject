using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class BuffData
{
	public int ID;
	public string Name;
	public string animatorname;
	public GameObject Effect;
	public float LifeTime;
	public SkillBase buff
	{
		get
		{
			var mSkillBase = BufferObject.Instantiate(Resources.Load<BufferObject>(Name)) as SkillBase;
			mSkillBase.skilldata=this;
			return mSkillBase;
		}
	}
}
