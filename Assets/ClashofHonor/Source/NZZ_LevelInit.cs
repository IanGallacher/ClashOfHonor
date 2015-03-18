using UnityEngine;
using System.Collections;

public class NZZ_LevelInit : MonoBehaviour {
	
	GameObject gameRules = null;
	public GameObject gamerules;
	// Use this for initialization
	void Start () {
		gameRules = GameObject.Find ("NZZ_GameSettings");
		if(!gameRules){ Instantiate (gamerules); }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
