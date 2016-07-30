using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
	public Vector3 v1;
	public Vector3 v2;
	public float dot;
	public float cos;
	// Use this for initialization
	void Start () {
		v1 = new Vector3(2,2,0);
		v2 = new Vector3(1, 0, 0);
		
		dot = Vector3.Dot(v1.normalized, v2.normalized);
		cos = Mathf.Cos(Mathf.PI/4);
		Debug.Log("dot="+dot+":cos="+cos);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}
