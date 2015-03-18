using UnityEngine;
using System.Collections;

public class NZZ_SuperSpring : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.GetComponent<PlayerController>())
			collision.gameObject.GetComponent<PlayerController>().jump = true;
	}
}
