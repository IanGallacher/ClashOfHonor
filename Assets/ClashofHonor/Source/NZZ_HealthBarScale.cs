using UnityEngine;
using System.Collections;

public class NZZ_HealthBarScale : MonoBehaviour {

	private float startingScale;
	private float healthPercentRemaining;

	// eventually use this for smooth scaling transitions. 
	// private float currentHealthScale = null;

	void Start () {
		startingScale = transform.localScale.x;
		healthPercentRemaining = 1f;
	}

	void Update() {
		if(healthPercentRemaining>0.0f) {
			transform.localScale = new Vector3 (healthPercentRemaining * startingScale,
			                                    transform.localScale.y, 
			                                    transform.localScale.z);
		} else {
			transform.localScale = new Vector3 (0.0f,
		                                        transform.localScale.y, 
		                                        transform.localScale.z);
		}
	}

	public void UpdateHealth( float HealthPercentage ) {
		healthPercentRemaining = HealthPercentage;
	}
}
