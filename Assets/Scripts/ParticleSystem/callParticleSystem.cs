using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class callParticleSystem : MonoBehaviour {

    private ParticleSystem test;

	// Use this for initialization
	void Start () {
        test = ParticleSystemManager.instance.test(ParticleSystemManager.instance.particleSystemPrefab, "none", Vector3.zero, Vector3.down);
    }
	
	// Update is called once per frame
	void Update () {

        Debug.Log(test.isPlaying);
        if (Input.GetKeyDown(KeyCode.Space)) {

            Debug.Log("Space");

            ParticleSystemManager.instance.stopParticleSystem(test);

            //test.Stop();
        }
	}
}
