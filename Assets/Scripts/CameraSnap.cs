using UnityEngine;
using System.Collections;

public class CameraSnap : MonoBehaviour {
	
	public float distance = -10f;
	
	GameObject target;
	GameObject target2;
	// Update is called once per frame
	void Update () {
		target = GameObject.Find("Player1") as GameObject;
		target2 = GameObject.Find("Player2") as GameObject;
		
		if(target.transform.position.x > target2.transform.position.x)
		{
			transform.position = target.transform.position + new Vector3(0, 0, distance);
			transform.LookAt(target.transform);	
		}
		
		if(target2.transform.position.x > target.transform.position.x)
		{
			transform.position = target.transform.position + new Vector3(0, 0, distance);
			transform.LookAt(target2.transform);
		}
	}
}
