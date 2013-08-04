using UnityEngine;
using System.Collections;

public class WaterTrigger : MonoBehaviour {
	
	GameObject water;
	GameObject splash;
	
	void Start()
	{
		water = GameObject.FindGameObjectWithTag("Water");
		splash = GameObject.FindGameObjectWithTag("Water Splash");
	}
		
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			splash.transform.position = new Vector3(col.transform.position.x, water.transform.position.y, col.transform.position.z);
			splash.GetComponent<AudioSource>().Play();
			splash.particleSystem.Play();
		}
	}
}
