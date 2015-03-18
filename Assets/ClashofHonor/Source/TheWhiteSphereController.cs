using UnityEngine;
using System.Collections;

public class TheWhiteSphereController : Bolt.EntityEventListener<ISphereState>
{

    public override void Attached(Bolt.IProtocolToken token)
    {
        state.Transform.SetTransforms(transform);
    }
}
