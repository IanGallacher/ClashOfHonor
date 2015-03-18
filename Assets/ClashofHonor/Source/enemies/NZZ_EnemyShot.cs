using UnityEngine;
using System.Collections;

public class NZZ_EnemyShot : MonoBehaviour {

	public GameObject shotlocation;
	public bool keepShooting =true;
	public GameObject enemyshot;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("ShootPlayer", 0.5f, 2.0f);
	}

	// Update is called once per frame
	void Update () {


	}
	void ShootPlayer() {
		if(keepShooting) {
			if(Vector3.Distance(GameObject.FindGameObjectWithTag ("Player").transform.position, transform.position)<75) //TODO: MULTIPLAYER POTENTIAL ERROR
			{
				if(shotlocation)
					GameObject.Instantiate(enemyshot, shotlocation.gameObject.transform.position, shotlocation.gameObject.transform.rotation);
				//myObject.transform.LookAt(GameObject.FindGameObjectWithTag ("Player"));
			}
		}
	}
	public void OnCollisionEnter(Collision col) {
		if(col.gameObject.CompareTag("NZZ_ForceActive")) {
			keepShooting=false;
		}
	}
}
