using UnityEngine;
using System.Collections;

public class CameraSnap : MonoBehaviour {
	
	public float distance = -10f;
	public float verticalOffset = 5f;
	
	GameObject[] players;
	GameObject leader;
	GameObject splash;
	
	void Start()
	{
		players = GameObject.FindGameObjectsWithTag("Player");
		
		splash = GameObject.FindGameObjectWithTag("Water Splash");
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		foreach (GameObject obj in players)
		{
			if (leader == null)
			{
				if (obj.transform.position.y > 0)
				{
					leader = obj;
				}
			}
			else
			{
				if (obj.transform.position.x > leader.transform.position.x)
				{
					leader = obj;
				}
			}
		}
		
		if (leader == null)
		{
			leader = splash;
		}
		
		transform.position = new Vector3(leader.transform.position.x, verticalOffset, distance);
		//transform.LookAt(leader.transform);	
		Debug.Log("Attempting to break the repo");
		leader = null;
	}
}
