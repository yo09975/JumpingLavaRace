using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	
	float midX;
	float midY;
	
	bool preGame = true;
	bool gameOver = false;
	bool countingDown = false;
	int countDownStart = 3;
	float countDownTime = 0;
	string countDownText = "GO!";
	string winnerName;
	
	string startText = "Start Game";
	
	public GUIStyle countDownStyle;
	
	
	void Start()
	{
		midX = Screen.width * 0.5f;
		midY = Screen.height * 0.5f;
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
				if (GUI.Button (new Rect(midX - 50, midY - 100, 100, 40), startText))
				{
					Debug.Log("Hit Start Game button");
					preGame = false;
					networkView.RPC("StartGame", RPCMode.AllBuffered);
				}
			}
		}
		if (gameOver)
		{
			GUI.Label(new Rect(midX, midY, 100, 100), winnerName, countDownStyle);	
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
	
	public void GameOver(Collider winner)
	{
		gameOver = true;
		winnerName = winner.name + " Wins!";
		preGame = true;
		startText = "Restart Game";
		foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
		{
			Debug.Log("Calling AfterGame");
			obj.networkView.RPC("AfterGame", RPCMode.AllBuffered);
		}
	}
	
	[RPC]
	void StartGame()
	{
		gameOver = false;
		countDownTime = countDownStart;
		foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
		{
			Debug.Log("Calling Position Player");
			obj.GetComponent<PlayerRun>().PositionPlayer();
		}
		countingDown = true;
	}
}
