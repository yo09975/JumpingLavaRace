using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	
	float midX;
	float midY;
	
	bool preGame;
	bool countingDown = false;
	int countDownStart = 3;
	float countDownTime = 0;
	string countDownText = "GO!";
	
	public GUIStyle countDownStyle;
	
	
	void Start()
	{
		midX = Screen.width * 0.5f;
		midY = Screen.height * 0.5f;
		
		preGame = true;
	}
	
	void OnGUI ()
	{
		if (GUI.Button (new Rect (10, 10, 50, 20), "Exit")) 
		{
			Application.Quit();
		}
		
		if (Network.isServer)
		{
			if (preGame)
			{
				if (GUI.Button (new Rect(midX, midY, 100, 40), "Start Game"))
				{
					Debug.Log("Hit Start Game button");
					preGame = false;
					networkView.RPC("StartGame", RPCMode.AllBuffered);
				}
			}
		}
		
		if (countingDown)
		{
			CountDown();
		}
	}
	
	void CountDown()
	{
		countDownTime = countDownTime - Time.deltaTime; 
		if (countDownTime > 0)
		{
			GUI.Label(new Rect(midX, midY, 100, 100), Mathf.CeilToInt(countDownTime).ToString(), countDownStyle);
		}
		else
		{
			if (countDownTime > -0.5)
			{
				GUI.Label(new Rect(midX, midY, 100, 100), countDownText, countDownStyle);
			}
			else
			{
				countingDown = false;
				foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
				{
					Debug.Log("Calling StartGame");
					obj.networkView.RPC("StartGame", RPCMode.AllBuffered);
				}
			}
		}
		
	}
	
	[RPC]
	void StartGame()
	{
		countDownTime = countDownStart;
		countingDown = true;
	}
}
