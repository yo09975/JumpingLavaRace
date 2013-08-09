using UnityEngine;
using System.Collections;

public class CameraSnap : MonoBehaviour {
	
	public float distance = -10f;
	public float verticalOffset = 5f;
	
	GameObject[] players;
	GameObject leader;
	GameObject splash;
	
	// Update is called once per frame
	void LateUpdate () {
		
		// Set leader to client
		if (leader == null)
		{
			players = GameObject.FindGameObjectsWithTag("Player");	
			
			foreach (GameObject obj in players)
			{
				if (obj.networkView.isMine)
				{
					leader = obj;
					break;
				}
			}
		}
		
		// Set splash for player
		if (leader != null && splash == null)
		{
			splash = leader.GetComponent<PlayerRun>().getSplash();
		}
		
		// Position camera
		if (leader != null)
		{
			if (leader.transform.position.y < -1 && splash != null)
			{
				transform.position = new Vector3(splash.transform.position.x, verticalOffset, distance);
			}
			else
			{
				transform.position = new Vector3(leader.transform.position.x, verticalOffset, distance);	
			}
		}
	}
}
