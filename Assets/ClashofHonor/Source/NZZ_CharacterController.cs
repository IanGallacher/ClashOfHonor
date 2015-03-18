using UnityEngine;
using System.Collections;

public class NZZ_NetworkCharacter: Bolt.EntityEventListener<IPlayerState> {
	public float speed = 10f;
	public float jumpStrength = 700.0f;
	public float tilt;
	public bool universeControl = false;
	GameObject gameRules = null;
	
	public bool hasJumped = true;
	
	void Start() {
		gameRules = GameObject.Find ("GameSettings");
	}

	// Update is called once per frame
	void Update () {
		if (networkView.isMine)
		{
			InputMovement();
			Transform theCam = GameObject.FindGameObjectWithTag("NZZ_CameraTransform").transform;
			theCam.transform.position = transform.position;
			theCam.transform.rotation = transform.rotation;
		}
		else
		{
			SyncedMovement();
		}
	}
	
	void InputMovement() //and update position
	{		
		float moveHorizontal, moveVertical;
		if(gameRules.GetComponent<NZZ_GameRules>().useHydra) {
			moveHorizontal = SixenseInput.Controllers[0].JoystickX;
			moveVertical = SixenseInput.Controllers[0].JoystickY;
		} else {
			moveHorizontal = Input.GetAxis ("Horizontal");
			moveVertical = Input.GetAxis ("Vertical");
		}
		if (Input.GetMouseButtonUp (1) && hasJumped == false || 
		    gameRules.GetComponent<NZZ_GameRules>().useHydra && 
		    (SixenseInput.Controllers[0].GetButtonDown(SixenseButtons.BUMPER) || SixenseInput.Controllers[1].GetButtonDown(SixenseButtons.BUMPER) )
		    && hasJumped == false) 
		{
			rigidbody.AddForce(new Vector3(0.0f, jumpStrength, 0.0f));
			hasJumped = true;
		}
		
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		if(!universeControl) //move relative to camera or the player?
		{
			movement = transform.TransformDirection (movement);
		}
		movement = new Vector3 (movement.x * speed, rigidbody.velocity.y, movement.z * speed);
		rigidbody.velocity = movement;
		//		rigidbody.position = new Vector3 
		//			(
		//				rigidbody.position.x, 
		//				rigidbody.position.y, 
		//				rigidbody.position.z
		//				);
		
		//rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbody.velocity.x * -tilt);
	}
	
	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag == "NZZ_Ground")
			hasJumped = false;
	}
	
	
	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	private Quaternion syncStartRotation = Quaternion.identity;
	private Quaternion syncEndRotation = Quaternion.identity;
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Quaternion syncRotation = Quaternion.identity;
		if (stream.isWriting)	
		{
			syncPosition = rigidbody.position;
			syncRotation = rigidbody.rotation;
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncRotation);
		}
		else {
			
		}
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncRotation);
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			syncStartPosition = rigidbody.position;
			syncEndPosition = syncPosition;  	
			
			syncStartRotation = rigidbody.rotation;
			syncEndRotation = syncRotation;  				
		}
	}
	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		
		rigidbody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
		rigidbody.rotation = Quaternion.Lerp(syncStartRotation, syncEndRotation, syncTime / syncDelay);
	}
}
