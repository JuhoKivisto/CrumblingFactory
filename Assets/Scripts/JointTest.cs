﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionEnter(Collision collision) {
        GetComponent<SpringJoint>().connectedBody = collision.rigidbody;
    }
}