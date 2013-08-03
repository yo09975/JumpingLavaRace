using UnityEngine;
using System.Collections;

public class HopAction : MonoBehaviour {

	public GameObject platform;
	
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			col.transform.position = new Vector3(col.transform.position.x, col.transform.position.y, platform.transform.position.z);	
		}
	}
}
