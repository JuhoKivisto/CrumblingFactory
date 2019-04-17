using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        LaserPointer.reticleCollides = true;
        Debug.Log("reticleCollides is now true");
    }

    private void OnCollisionExit(Collision collision)
    {
        LaserPointer.reticleCollides = false;
    }
}
