using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	public string gameName = "Super Lava Race";
	public GameObject playerPrefab;
	public Transform spawnObject;
	private bool refreshing = false;
	private HostData[] hostData;
	private float btnX, btnY, btnWidth, btnHeight;
	
	// Use this for initialization
	void Start ()
	{
		btnX = Screen.width * 0.05f;
		btnY = Screen.width * 0.05f;
		btnWidth = Screen.width * 0.1f;
		btnHeight = Screen.width * 0.1f;
	}
	
	void startServer ()
	{
		Network.InitializeServer (32, 25000, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (gameName, "Lava Race", "AWESOME!");
	}
	
	void refreshHostList ()
	{
		MasterServer.RequestHostList (gameName);
		refreshing = true;
		MasterServer.PollHostList ();
	}
	
	void spawnPlayer ()
	{
		Network.Instantiate (playerPrefab, spawnObject.position, Quaternion.identity, 0);		
	}
	
	// Messages
	void OnServerInitialized ()
	{
		Debug.Log ("Server intialized");
		spawnPlayer ();
	}
	
	void OnConnectedToServer ()
	{
		Debug.Log("Player connected, spawning player!");
		spawnPlayer ();	
	}
	
	void OnMasterServerEvent (MasterServerEvent mse)
	{
		if (mse == MasterServerEvent.RegistrationSucceeded) {
			Debug.Log ("Registration of server successful!");		
		}
	}
	// Update is called once per frame
	void Update ()
	{
		if (refreshing) {
			if (MasterServer.PollHostList ().Length != 0) {
				refreshing = false;
				hostData = MasterServer.PollHostList ();
				Debug.Log (MasterServer.PollHostList ().Length);
			}
		}
		
		
	}
	
	//GUI
	void OnGUI ()
	{
		if (!Network.isClient && !Network.isServer) {
			if (GUI.Button (new Rect (btnX, btnY, btnWidth, btnHeight), "Start Server")) {
				Debug.Log ("Starting Server");
				startServer ();
			}
		
		
			if (GUI.Button (new Rect (btnX, btnY + btnHeight, btnWidth, btnHeight), "Refresh Hosts")) {
				Debug.Log ("Refreshing server list");
				refreshHostList ();
			}
			if (hostData != null) {
				for (int i = 0; i <hostData.Length; i++) {
					if (GUI.Button (new Rect (btnX * 1.5f + btnWidth, btnY * 1.2f + (btnHeight * i), btnWidth * 3.0f, btnHeight * 0.5f), hostData [i].gameName)) {
						Network.Connect (hostData [i]);
					}
				}
			}
		
		}
	}
}
