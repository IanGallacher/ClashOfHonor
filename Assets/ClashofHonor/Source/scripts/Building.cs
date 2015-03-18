using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {
  public override void Attached() {
    state.Transform.SetTransforms(transform);
  }
}
