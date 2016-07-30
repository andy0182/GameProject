using UnityEngine;
using System.Collections;

public class Buff_001 : SkillBase 
{
	GameObject Efface;
	public override void OnEnter (BufferInterface parasitifer)
	{
		base.OnEnter (parasitifer);
		if(skilldata.Effect!=null)
		{
			Efface=GameObject.Instantiate(skilldata.Effect) as GameObject;
			Efface.transform.parent=Parasitifer.transform;
			Efface.transform.localPosition=Vector3.zero;
			Efface.transform.localRotation=Quaternion.identity;
			EventOnExit+=()=>GameObject.Destroy(Efface);
		}

	}
}
