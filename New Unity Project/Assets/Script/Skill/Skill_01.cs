using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill_01 : SkillBase 
{
	public List<int> OutBuffs=new List<int>();
	public override void OnEnter (BufferInterface parasitifer)
	{
		base.OnEnter (parasitifer);
		Parasitifer.gameObject.GetComponent<Animator>().Play(Animator.StringToHash(skilldata.animatorname));
		var entitys=ApplicationControol.Instance.Entitys;
		OutBuffs.ForEach(a=>entitys.ForEach(c=>c.AddBuffer(DataBase.Instance.buffDatas.Find(b=>b.ID==a).buff)));
	}
}
