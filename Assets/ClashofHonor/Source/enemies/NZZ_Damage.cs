using UnityEngine;
using System.Collections;

public class NZZ_Damage : MonoBehaviour {
	public byte BaseDamage;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public byte GetDamage(int playerid) {
		return BaseDamage;
	}
}
