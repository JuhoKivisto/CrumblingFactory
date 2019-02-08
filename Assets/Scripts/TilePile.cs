using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePile : MonoBehaviour {

    public TilePileController tpc;
	// Use this for initialization
	void Start () {
        tpc.PopulateList(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    
}
