﻿using UnityEngine;
using System.Collections;

public class PlayerRun : MonoBehaviour
{
	
	public float speed = 6.0f;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;
	public Material[] playerColors;
	public string[] playerNames;
	
	private Vector3 moveDirection = Vector3.zero;
	
	private CharacterController controller;
	private Animation anim;
	private Vector3 impact;
	
	void Start()
	{
		controller = GetComponent<CharacterController>();
		anim = gameObject.GetComponentInChildren<Animation>();
		
		int numberOfPlayers = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().currentPlayers;
		gameObject.name = playerNames[numberOfPlayers];
		foreach (Transform obj in gameObject.transform)
		{
			foreach (Transform obj2 in obj)
			{
				if (obj2.name == "Cylinder002")
				{
					Debug.Log("Setting Color");
					obj2.renderer.material = playerColors[numberOfPlayers];
					break;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (networkView.isMine && (Network.isClient || Network.isServer)) {
			if (controller.isGrounded) {
				moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0, 0);
				moveDirection *= speed;
		
				if (moveDirection.sqrMagnitude > 0.01) {
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (moveDirection), 1);
				}
				
				if (Input.GetButton ("Jump")) {
					moveDirection.y = jumpSpeed;
				}
			}
			else
			{
				if (transform.position.y < 0 && impact == Vector3.zero)
				{
					impact = transform.position;
				}
				if (transform.position.y < -20)
				{
					controller.velocity.Set(0.0f, 0.0f, 0.0f);
					moveDirection = Vector3.zero;
					transform.position = new Vector3(impact.x - 3, 3.0f, impact.z);
					impact = Vector3.zero;
				}
			}
			
			if (controller.velocity.magnitude > 0)
			{
				networkView.RPC("UpdateAnimation", RPCMode.OthersBuffered, false);
				anim.CrossFade("Run", 0.1f);
			} 
			else
			{
				networkView.RPC("UpdateAnimation", RPCMode.OthersBuffered, true);
				anim.CrossFade("Standing", 0.1f);
			}
		
			moveDirection.y -= gravity * Time.deltaTime;
		
			controller.Move (moveDirection * Time.deltaTime);
		}
	}
	
	[RPC]
	void UpdateAnimation(bool standing)
	{
		if (anim != null)
		{
			if (standing)
			{
				anim.CrossFade("Standing", 0.1f);
			}
			else
			{
				anim.CrossFade("Run", 0.1f);
			}
		}
	}
}
