using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class callParticleSystem : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ParticleSystemManager.instance.test(ParticleSystemManager.instance.particleSystemPrefab);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
