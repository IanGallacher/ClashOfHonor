using UnityEngine;
using System.Collections;

public class TheWhiteSphere : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (BoltNetwork.isServer == false)
        {
            // Not sure why I have to do this in Update() rather than Start().. TODO: should investigate more.
            Rigidbody rb = this.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
}

