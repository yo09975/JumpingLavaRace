using UnityEngine;
using System.Collections;

public class PlayerRun : MonoBehaviour {
	
	public float speed = 6.0f;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;
	
	private Vector3 moveDirection = Vector3.zero;
	
	// Update is called once per frame
	void Update () 
	{
		CharacterController controller = GetComponent<CharacterController>();
		if(controller.isGrounded)
		{
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
			moveDirection *= speed;
		
			if(moveDirection.sqrMagnitude > 0.01)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 1);
			}
				
			if(Input.GetButton("Jump"))
			{
				moveDirection.y = jumpSpeed;
			}
		}
		
		moveDirection.y -= gravity * Time.deltaTime;
		
		controller.Move(moveDirection * Time.deltaTime);
	}
}
