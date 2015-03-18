using UnityEngine;
using System.Collections;
// This class only provoides properties for the spawn settings in servercallbakcs
public class NZZ_NetworkMeteorSpawn : MonoBehaviour {
	public GameObject meteor;
	public float meteorSpawnRate = 5.0f;

//	void SpawnMeteor() {
//		//Vector3 randomDirection = new Vector3(Random.value, Random.value, Random.value); randomDirection.Normalize ();
//		//Rigidbody instance = (Rigidbody)Network.Instantiate (meteor, new Vector3 (0f, 5f, 0f), Quaternion.identity, 0);
//		//instance.AddForce (randomDirection);
//		BoltNetwork.Instantiate(BoltPrefabs.TheWhiteSphereJR, new Vector3(Random.value * 64f, 32f, Random.value * 64f), Quaternion.identity);
//	}
}
