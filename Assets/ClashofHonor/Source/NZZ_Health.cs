using UnityEngine;
using System.Collections;

public class NZZ_Health : MonoBehaviour {
	public int TotalHealth = 30;
	public int currentHealth = 0;
	// Use this for initialization
	void Start () {
		currentHealth = TotalHealth;
	}

	public void ApplyDamage(byte damage) {
//		if(collision.gameObject.GetComponent<NZZ_Damage>()) {
//			this.currentHealth -= collision.gameObject.GetComponent<NZZ_Damage>().BaseDamage;
//		}
		this.currentHealth -= damage;
		Component[] myChildren = GetComponentsInChildren<NZZ_HealthBarScale>();
		foreach( NZZ_HealthBarScale child in myChildren) {
			child.UpdateHealth((float)currentHealth/(float)TotalHealth);
		}
//		if(collision.gameObject.GetComponent<NZZ_Damage>()) {
//			Destroy (collision.gameObject);
//		}

	}
}
