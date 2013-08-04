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
		splash = GameObject.FindGameObjectWithTag("Water Splash");
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		if (leader == null)
		{
			players = GameObject.FindGameObjectsWithTag("Player");	
			
			foreach (GameObject obj in players)
			{
				if (obj.networkView.isMine)
				{
					leader = obj;	
				}
			}
		}
		
		
		/*foreach (GameObject obj in players)
		{
			if (obj.networkView.isMine)
			{
				leader = obj;
			}
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
		}*/
		
		
		if (leader == null || leader.transform.position.y < 0)
		{
			transform.position = new Vector3(splash.transform.position.x, verticalOffset, distance);
		}
		else
		{
			transform.position = new Vector3(leader.transform.position.x, verticalOffset, distance);	
		}
		
		//transform.LookAt(leader.transform);	
	
		//leader = null;
	}
}
