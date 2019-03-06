using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    Rigidbody rb;
    float speed = 25f;

	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float translation = Input.GetAxis("Vertical") * speed;
   
    
    
        translation *= Time.deltaTime;
       
        rb.AddForce(this.transform.forward*translation*50);
       
       meter.Heatmeter(rb.velocity.magnitude, 0, 100);
    }
}
