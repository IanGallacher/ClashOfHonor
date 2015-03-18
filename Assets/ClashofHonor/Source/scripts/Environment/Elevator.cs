using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

  [SerializeField]
  Vector3 start;

  [SerializeField]
  Vector3 end;

  [SerializeField]
  float time = 10f;

  void FixedUpdate() {
    float t = Mathf.Clamp01(Mathf.PingPong(BoltNetwork.serverTime, time) / time);
    transform.position = Vector3.Lerp(start, end, t);
  }
}
