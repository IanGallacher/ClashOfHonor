using UnityEngine;
using System.Collections;

public class NZZ_FirstPersonCamera : MonoBehaviour {
	
	public Transform target;
	public float xoffset = 0f;
	public float yoffset = 0f;
	public float zoffset = 0f;
	// Use this for initialization
	void Start () {
		
	}
		
	// Update is called once per frame
	void Update () {
		if(target) {
			transform.position = target.transform.position+new Vector3(xoffset,yoffset,zoffset);
			float yaw = target.transform.localEulerAngles.y;
			float pitch = target.transform.Find("NZZ_CameraPitch").localEulerAngles.x;
//			print (yaw);
			transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
		}
	}
	
	public void SetTarget(BoltEntity entity) {
		target = entity.transform;
	}
}
