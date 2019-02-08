using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

    public bool isStunned;
    public bool isShocked;
    public bool isOnFire;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision) {
        print(collision.gameObject.GetComponent<Rigidbody>().velocity.y);
        if (Mathf.Abs(collision.gameObject.GetComponent<Rigidbody>().velocity.y) > 2) {
            print("hit");
            //health -= 50;
        }
    }
}
