using UnityEngine;
using System.Collections;
//require NZZ_Damage
public class NZZ_StandardBullet : MonoBehaviour {

	public float speed = 1.0f;
	public GameObject explosioneffect;
	// Use this for initialization
	void Start () {
		rigidbody.velocity = new Vector3 (2, 0, 0);
		//transform.LookAt(GameObject.FindGameObjectWithTag ("Player").transform);
		rigidbody.velocity = (GameObject.FindGameObjectWithTag ("Player").transform.position - transform.position).normalized * speed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.CompareTag ("Player")) {
			if(col.gameObject.GetComponent<NZZ_Health>()) {
				col.gameObject.GetComponent<NZZ_Health>().ApplyDamage(this.GetComponent<NZZ_Damage>().BaseDamage);
			} { //if it does not have NZZ_Health, it must be a child. 
				transform.root.GetComponentInChildren<NZZ_Health>().ApplyDamage(this.GetComponent<NZZ_Damage>().BaseDamage);
			}
		}
		if(explosioneffect) {
			Instantiate(explosioneffect, transform.position, transform.rotation);
		}
		Destroy (this.gameObject);
	}
}
