using UnityEngine;
using System.Collections;

public class NZZ_NetworkDamage : Bolt.EntityEventListener<ISphereState> {

	public byte BaseDamage = 1;
	void OnCollisionStay(Collision col) {
		//this function goes hand in hand with the function in player controller that changes the color of the object. 
		if(col.gameObject.CompareTag ("NZZ_ExplosionProof") || col.gameObject.CompareTag ("NZZ_Ground") ) {
			using (var mod = state.Modify()) {
				renderer.material.color = Color.white;
				mod.PlayerControl = 0;
			}
		}
	}
	public void ChangePlayerControl(int playerid)
	{
		using (var mod = state.Modify()) {
			if (playerid == 0) {
				renderer.material.color = Color.white;
					mod.PlayerControl = 0;
			}
			if (playerid == 1) {
				renderer.material.color = Color.red;
					mod.PlayerControl = 1;
			}
			if (playerid == 2) {
				renderer.material.color = Color.green;
					mod.PlayerControl = 2;
			}
		}
	}
	void Update(){

		if (state.PlayerControl == 0) {
			renderer.material.color = Color.white;
		}
		if (state.PlayerControl == 1) {
			renderer.material.color = Color.red;
		}
		if (state.PlayerControl == 2) {
			renderer.material.color = Color.green;
		}
	}

	public byte GetDamage(int playerid) {
		if (playerid == state.PlayerControl) {
			return BaseDamage;
		}
		return 0;
	}
}
