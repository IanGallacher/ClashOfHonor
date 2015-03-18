using UnityEngine;
using System.Collections;
using System.Linq;

public class MyPlayerState 
{

	public int health = 100;
	public bool Dead=false;
}

public class PlayerController : MonoBehaviour
{
	MyPlayerState state = new MyPlayerState();
	const float MOUSE_SENSEITIVITY = 2f;
	
	float horizontal;
	float vertical;
	public bool jump;
	bool aiming;
	public bool fireleft;
	public bool fireright;
	
	public GameObject HoverGlowPrefab;
	bool force;
	
	int weapon;
	
	float yaw;
	float pitch;
	
	
	//Hydra stuff
	SixenseHandsController[] m_HandControllers;
	
	public bool hydraActive = false; 
	private SixenseInput Hydra;


	PlayerMotor _motor;


	
	GameObject[] movelist;
	bool addToQueue;
	void Awake()
	{
		_motor = GetComponent<PlayerMotor>();
		movelist = new GameObject[2];
		Hydra = GetComponent<SixenseInput>();
		m_HandControllers = GetComponentsInChildren<SixenseHandsController>();
	}
	
	// * 
	private void setHandControllersHydraActive(bool isActive)
	{
		foreach ( SixenseHandsController hc in m_HandControllers )
		{
			if (hc != null)
				hc.HydraActive = isActive;
		}
		
		// SixenseHandsController hc2 = transform.Find ("NZZ_HandController").gameObject.GetComponent<SixenseHandsController>();
		// BoltLog.Info ("hc2 = " + hc2);
	}
	private GameObject hoverGlow;
	void Start() {
		hoverGlow = GameObject.Instantiate (HoverGlowPrefab) as GameObject;
	}

	void Update()
	{
		Vector3 handRightPos = transform.Find ("NZZ_HandController/NZZ_HandRight/GameObject").position;
		RaycastHit hitRight;
		//Debug.DrawRay(GameObject.Find ("NZZ_HandController/NZZ_HandRight/GameObject").transform.position, 
		//              (explosionPosRight-GameObject.Find ("NZZ_CameraTransform").transform.position), Color.green, 2,false);	
		if (Physics.Raycast (GameObject.Find ("NZZ_HandController/NZZ_HandRight/GameObject").transform.position, 
		                    handRightPos - GameObject.Find ("NZZ_CameraTransform").transform.position, out hitRight)) {//Physics.OverlapSphere (explosionPos, forceradius);
			
			hoverGlow.transform.position = hitRight.transform.position;

		}
		PollKeys(hydraActive);

		state.health = (byte)Mathf.Clamp(state.health + 1, 0, 100);
		foreach(GrayscaleEffect grayscale in GameObject.FindGameObjectWithTag ("NZZ_CameraTransform").GetComponentsInChildren<GrayscaleEffect>()) { 
			grayscale.ramp = 1f-(float)state.health/100.0f;
		}

		if (force) {
			ForcePush();		
		}

		var result = _motor.Move(horizontal, vertical, jump, yaw, pitch);
		
		if (fireleft)
		{
			VacuumLeft();
		}
		if (fireright)
		{
			VacuumRight();
		}
		if (force)
		{
			ForcePush();
		}
	}	
	
	void PollKeys(bool useHydra)
	{
		bool mouse = true;

		jump = false;
		aiming = false;
		fireleft = false;
		fireright = false;
		force = false;

		horizontal = Input.GetAxis ("Horizontal");
		vertical = Input.GetAxis ("Vertical");
		jump = Input.GetKey(KeyCode.Space);
		aiming = Input.GetMouseButton(1);
		fireleft = Input.GetMouseButton(0);
		fireright = false; // the other inputs will evaluate to false always. 
		force = Input.GetMouseButtonUp(0);
		
		if (Input.GetKeyDown(KeyCode.Alpha1))
			weapon = 0;
		else if (Input.GetKeyDown(KeyCode.Alpha2))
			weapon = 1;
		
		if (useHydra)
		{
			var hydra_left = SixenseInput.GetController(SixenseHands.LEFT);
			var hydra_right = SixenseInput.GetController(SixenseHands.RIGHT);
			if (hydra_left != null && hydra_right != null)
			{
				horizontal = hydra_left.JoystickX;
				vertical = hydra_left.JoystickY;
				

				yaw += (hydra_right.JoystickX * MOUSE_SENSEITIVITY);
				yaw %= 360f;
				
				pitch += (-hydra_right.JoystickY * MOUSE_SENSEITIVITY);
				pitch = Mathf.Clamp(pitch, -85f, +85f);
				
				
				if (hydra_left.GetButton(SixenseButtons.BUMPER))
					jump = true;
				
				if (hydra_right.GetButton(SixenseButtons.BUMPER))
					jump = true;
				
				if (hydra_right.GetButton(SixenseButtons.BUMPER))
					jump = true;
				
				if (hydra_left.GetButton(SixenseButtons.TRIGGER))
					aiming = true;

				if (hydra_left.GetButton(SixenseButtons.TRIGGER))
					fireleft = true;

				if (hydra_right.GetButton(SixenseButtons.TRIGGER))
					fireright = true;

				if (hydra_left.GetButtonUp(SixenseButtons.TRIGGER) || hydra_right.GetButtonUp(SixenseButtons.TRIGGER)) 
					force = true;

				/*
            if (!jump)
                jump = ssc_right.GetButton(SixenseButtons.TRIGGER);
             */
			}
		}
		
		if (mouse)
		{
			yaw += (Input.GetAxisRaw("Mouse X") * MOUSE_SENSEITIVITY);
			yaw %= 360f;
			
			pitch += (-Input.GetAxisRaw("Mouse Y") * MOUSE_SENSEITIVITY);
			pitch = Mathf.Clamp(pitch, -85f, +85f);
		}
	}

	public void OnCollisionEnter (Collision col) {
		if(col.gameObject.GetComponent<NZZ_Damage>())
		{	
			//	Destroy(col.gameObject);
			ApplyDamage(col.gameObject.GetComponent<NZZ_Damage>().GetDamage(1));
		}
	}
	
	public void ApplyDamage(byte damage)
	{
		if (!state.Dead)
		{
			state.health -= damage;
			
			if (state.health > 100 || state.health < 0)
			{
				state.health = 0;
			}
		}
		
//		if (state.health == 0)
//		{
//			entity.controller.GetPlayer().Kill();//wut?
//		}
		foreach(GrayscaleEffect grayscale in GameObject.FindGameObjectWithTag ("NZZ_CameraTransform").GetComponentsInChildren<GrayscaleEffect>()) { 
			grayscale.ramp = 1f-(float)state.health/100.0f;
		}
	}


//	_motor.SetState(cmd.Result.position, cmd.Result.velocity, cmd.Result.isGrounded, cmd.Result.jumpFrames);
//			cmd.Result.position = result.position;
//			cmd.Result.velocity = result.velocity;
//			cmd.Result.jumpFrames = result.jumpFrames;
//			cmd.Result.isGrounded = result.isGrounded;
//	if (resetState)
//	{
//		_motor.SetState(cmd.Result.position, cmd.Result.velocity, cmd.Result.isGrounded, cmd.Result.jumpFrames);
//		if (entity.hasControl == false)
//		{
//			transform.Find("NZZ_HandController/NZZ_HandLeft").transform.localPosition = new Vector3(cmd.Result.HydraLeftPositionX,
//			                                                                                        cmd.Result.HydraLeftPositionY,
//			                                                                                        cmd.Result.HydraLeftPositionZ);
//			transform.Find("NZZ_HandController/NZZ_HandLeft").transform.localRotation = cmd.Result.HydraLeftRotation;
//			
//			
//			transform.Find("NZZ_HandController/NZZ_HandRight").transform.localPosition = new Vector3(cmd.Result.HydraRightPositionX,
//			                                                                                         cmd.Result.HydraRightPositionY,
//			                                                                                         cmd.Result.HydraRightPositionZ);
//			transform.Find("NZZ_HandController/NZZ_HandRight").transform.localRotation = cmd.Result.HydraRightRotation;
//		}
//	}

	public float forceradius = 10f;
	public float forcepower = -15f;
	
	public float forcePush = 100f;
	public float forcePull = 100f;
	void VacuumLeft() {
		Vector3 explosionPosLeft = transform.Find ("NZZ_HandController/NZZ_HandLeft/GameObject").position;
		float distLeft;
		RaycastHit hitLeft;
		//Debug.DrawRay(GameObject.Find ("NZZ_HandController/NZZ_HandLeft/GameObject").transform.position, 
		//    (explosionPosLeft-GameObject.Find ("NZZ_CameraTransform").transform.position), Color.green, 2,false);	
		if(Physics.Raycast (GameObject.Find ("NZZ_HandController/NZZ_HandLeft/GameObject").transform.position, 
		   	explosionPosLeft-GameObject.Find ("NZZ_CameraTransform").transform.position, out hitLeft)) {//Physics.OverlapSphere (explosionPos, forceradius);

			//Physics.Raycast(transform.pos
		//		particleEffect.enableEmission = true;
		//			foreach (Collider hit in colliders) {
			if (hitLeft.rigidbody && !hitLeft.transform.gameObject.CompareTag ("NZZ_ExplosionProof")) { 
				Transform hitTransform = hitLeft.transform.gameObject.GetComponentInParent<Transform> ();
				distLeft = Vector3.Distance (hitTransform.position, transform.position);
//					if (hit.transform.gameObject.GetComponent<NZZ_NetworkDamage> ()) {
//						if (entity.hasControl) {	
//							hit.transform.gameObject.GetComponent<NZZ_NetworkDamage> ().ChangePlayerControl (1);
//						} else {
//							hit.transform.gameObject.GetComponent<NZZ_NetworkDamage> ().ChangePlayerControl (2);
//						}
//					}
				if(addToQueue==true){
					movelist[0] = hitLeft.transform.gameObject;
					hitLeft.transform.gameObject.layer = 2;
					hitLeft.transform.gameObject.tag = "NZZ_ForceActive";
					addToQueue=false;
				}
				//hit.rigidbody.AddExplosionForce (forcepower, explosionPos, forceradius, 0.0F);
			}
			//hit.rigidbody.AddForce(-Vector3.MoveTowards(hitTransform.position, transform.position, forcepower*dist));
			//
		}
		if(movelist[0]) {	
			//movelist[0].rigidbody.AddExplosionForce (forcepower, explosionPos, forceradius, 0.0F);
			movelist[0].rigidbody.AddForce( (GameObject.Find ("NZZ_HandController/NZZ_HandLeft/GameObject").transform.position-movelist[0].transform.position) * forcePull);
		}

	}
	

	public GameObject ForceGlow;
	private GameObject theGlow;
	void VacuumRight() {

		Vector3 handRightPos = transform.Find ("NZZ_HandController/NZZ_HandRight/GameObject").position;
		RaycastHit hitRight;
		//Debug.DrawRay(GameObject.Find ("NZZ_HandController/NZZ_HandRight/GameObject").transform.position, 
		//              (explosionPosRight-GameObject.Find ("NZZ_CameraTransform").transform.position), Color.green, 2,false);	
		if(Physics.Raycast (GameObject.Find ("NZZ_HandController/NZZ_HandRight/GameObject").transform.position, 
		                    handRightPos-GameObject.Find ("NZZ_CameraTransform").transform.position, out hitRight)) {//Physics.OverlapSphere (explosionPos, forceradius);

			// Are we allowed to move this object?
			if (hitRight.rigidbody && !hitRight.transform.gameObject.CompareTag ("NZZ_ExplosionProof")) { 
				Transform h0itTransform = hitRight.transform.gameObject.GetComponentInParent<Transform> ();
				if(addToQueue==true){
					if(!theGlow)
					movelist[1] = hitRight.transform.gameObject;
					hitRight.transform.gameObject.layer = 2;
					hitRight.transform.gameObject.tag = "NZZ_ForceActive";
					addToQueue=false;
					theGlow = GameObject.Instantiate(ForceGlow, handRightPos, new Quaternion(0,0,0,0)) as GameObject;
					theGlow.transform.parent=transform.Find ("NZZ_HandController/NZZ_HandRight/GameObject").transform;
				}
			}
		}
		if(movelist[1]) {

			Color color = movelist[1].renderer.material.color;
			color.a = 0.4f;
			Debug.Log("before assignment, current Mat " + movelist[1].renderer.material.name + " alt Mat "+ movelist[1].renderer.material.color.a);

			movelist[1].rigidbody.AddForce( (handRightPos-movelist[1].transform.position) * forcePull);
			if(Vector3.Distance(handRightPos, movelist[1].transform.position) < 10.0f ) {
				movelist[1].rigidbody.AddForce( -(((handRightPos-movelist[1].transform.position) * forcePull)*8)/10);
		
				//movelist[1].rigidbody.velocity = new Vector3 (0,0,0);
			}
			if(Vector3.Distance(handRightPos, movelist[1].transform.position) < 3.0f ) {
				movelist[1].transform.parent = transform.Find ("NZZ_HandController/NZZ_HandRight");
				movelist[1].rigidbody.velocity = new Vector3 (0,0,0);
			}
		}
	}
		//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		void ForcePush() {
			GameObject.Destroy (theGlow);
			Vector3 explosionPos = transform.position;
			Collider[] colliders = Physics.OverlapSphere (explosionPos, forceradius);
			Vector3 myDirection;
			//foreach (Collider hit in colliders) {
				//if (hit && hit.rigidbody && !hit.CompareTag ("NZZ_ExplosionProof")) {
				//if (movelist[0] && movelist[0].CompareTag ("NZZ_ExplosionProof")) {
				//	hit.rigidbody.AddForce (myDirection);
					//because we only have one forcepush function, we have to make sure that we both have an object currently 'forced' AND 
					//the coresponding hand has released the trigger. Otherwise onrelease of one trigger, both forced objects will go flying. 
					if (movelist[0]&&!fireleft) {
						RaycastHit hit;
						if(Physics.Raycast (GameObject.Find ("NZZ_HandController/NZZ_HandLeft/GameObject").transform.position, 
					                        GameObject.Find ("NZZ_HandController/NZZ_HandLeft/GameObject").transform.position
					                       -GameObject.Find ("NZZ_CameraTransform").transform.position, out hit)) {
							myDirection = ( GameObject.FindGameObjectWithTag ("NZZ_CameraTransform").transform.position
				                  	-movelist[0].transform.position) * (-forcePush);

						} else {
							myDirection = (GameObject.FindGameObjectWithTag("NZZ_CameraTransform").transform.position - movelist[0].transform.position) * (-forcePush)*2;
						}


						movelist [0].rigidbody.AddForce (myDirection);
						movelist [0].transform.gameObject.layer = 13;

						movelist[0]=null;
					}


					if (movelist[1]&&!fireright) {

						RaycastHit hit;
						if(Physics.Raycast (GameObject.Find ("NZZ_HandController/NZZ_HandRight/GameObject").transform.position, 
						                    GameObject.Find ("NZZ_HandController/NZZ_HandRight/GameObject").transform.position
						                   -GameObject.Find ("NZZ_CameraTransform").transform.position, out hit)) {
						     myDirection = (movelist[1].transform.position-hit.point//GameObject.Find ("NZZ_CameraTransform").transform.position
							               ) * (-forcePush);
							
						} else {
							myDirection = (GameObject.FindGameObjectWithTag ("NZZ_CameraTransform").transform.position-movelist[0].transform.position) * (-forcePush);
						}


						movelist [1].rigidbody.AddForce (myDirection*(movelist[1].rigidbody.mass+1));
						movelist [1].transform.gameObject.layer = 13;
						movelist [1].transform.gameObject.tag = "NZZ_ForceActive";
						Color color = movelist[1].renderer.material.color;
						color.a = 1.0f;
						movelist[1].transform.parent = null;
						movelist[1]=null;
					}
					addToQueue = true;
				//}
			//}
		}
	
	}
