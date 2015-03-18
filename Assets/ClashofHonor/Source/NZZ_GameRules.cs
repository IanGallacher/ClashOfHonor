using UnityEngine;
using System.Collections;

//ftl soundtrack: void battle theme is a good thing to compare our soundtrack to. 

public class NZZ_GameRules : MonoBehaviour {
	public float playerForceRange = 10F;

	// Use the Razor Hydra for input. 
	// Warning: this value will get over written once "start server" is pressed. 
	public bool useKeyboard = false;
	public bool useXBox = false;
	public bool useHydra = false;
	public bool useOculus = false;
	void Awake() {
		//Make the game settings we set on the main menu stay with us between levels. 
		DontDestroyOnLoad(gameObject); 
	}
}
