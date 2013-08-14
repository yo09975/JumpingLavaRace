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
	GameObject[] players;
	
	public GUIStyle countDownStyle;
	public GameObject leaderboardPrefab;
	public string[] playerNames;
	public Texture[] playerColors;
	Hashtable nameToTextureMap;
	LeaderboardController[] leaderboardController;
	PositionComparer leaderboardComparer;
	float leaderboardWidth = 100.0f;
	float leaderboardBuffer = 10.0f;
	
	
	void Start()
	{
		midX = Screen.width * 0.5f;
		midY = Screen.height * 0.5f;
		
		leaderboardComparer = new PositionComparer();
		
		nameToTextureMap = new Hashtable();
		for (int index = 0; index < playerNames.Length; index++)
		{
			nameToTextureMap.Add(playerNames[index], playerColors[index]);
		}
	}
	
	void Update()
	{
		if (!preGame)
		{
			UpdateLeaderboard();	
		}
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
	
	void InitializeLeaderboard()
	{
		if (startText == "Start Game")
		{
			players = GameObject.FindGameObjectsWithTag("Player");
			int size = players.Length;
			leaderboardController = new LeaderboardController[size];
			float spaceFromTop = 1 - ((leaderboardWidth + 2 * leaderboardBuffer) / Screen.height);
			float spaceBetween = (leaderboardBuffer + leaderboardWidth) / Screen.width;
			float startPosition = (midX + ((size - 2) * leaderboardWidth/2) + (size%2==0?leaderboardBuffer/2:0) + Mathf.FloorToInt((size - 1) / 2.0f) * leaderboardBuffer) / Screen.width;
			
			for (int index = 0; index < size; index++)
			{
				GameObject leaderboard = (GameObject)GameObject.Instantiate(leaderboardPrefab, new Vector3(startPosition - (spaceBetween * index), spaceFromTop, 0), Quaternion.identity);
				leaderboardController[index] = leaderboard.GetComponent<LeaderboardController>();
				leaderboardController[index].text.text = (index+1).ToString();
			}
		}
	}
	
	void UpdateLeaderboard()
	{
		ArrayList tempPlayers = new ArrayList(players);
		tempPlayers.Sort(leaderboardComparer);
		int index = 0;
		foreach (GameObject player in tempPlayers)
		{
			leaderboardController[index++].color.texture = (Texture)nameToTextureMap[player.name];
		}
	}
	
	private class PositionComparer : IComparer
	{
		public int Compare (object x, object y)
		{
			return ((GameObject)y).transform.position.x.CompareTo(((GameObject)x).transform.position.x);	
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
		preGame = false;
		gameOver = false;
		InitializeLeaderboard();
		countDownTime = countDownStart;
		foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
		{
			Debug.Log("Calling Position Player");
			obj.GetComponent<PlayerRun>().PositionPlayer();
		}
		countingDown = true;
	}
}
