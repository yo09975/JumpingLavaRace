using UnityEngine;
using System.Collections;

public class FinishLine : MonoBehaviour {
	
	public GUIManager guiManager;
	
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			guiManager.GameOver(col);
		}
	}
}
