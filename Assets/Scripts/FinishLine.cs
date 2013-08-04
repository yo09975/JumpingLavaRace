﻿using UnityEngine;
using System.Collections;

public class FinishLine : MonoBehaviour {
	
	public GUIText back;
	public GUIText front;
	
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			Time.timeScale = 0;	
			back.text = back.text.Replace("?", col.name).Replace("(Clone)", "");
			front.text = front.text.Replace("?", col.name).Replace("(Clone)", "");
			back.enabled = true;
			front.enabled = true;
		}
	}
}
