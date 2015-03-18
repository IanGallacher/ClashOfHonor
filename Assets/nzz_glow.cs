using UnityEngine;
using System.Collections;

public class nzz_glow : MonoBehaviour {

	public string floatRangeProperty = "Saturation";
	public float cycleTime = 2;
	public Renderer rend;
	void Start() {
		rend = GetComponent<Renderer>();

	}

	void Update () {
		ProceduralMaterial substance = rend.sharedMaterial as ProceduralMaterial;
		//rend.material.SetFloat("Octaves",1.0f);
		float lerp = Mathf.PingPong(Time.time * 2 / cycleTime, 2);
		rend.material.SetFloat("_Amplitude",lerp+0.5f);
	}
}
