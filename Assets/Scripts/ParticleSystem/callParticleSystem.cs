using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class callParticleSystem : MonoBehaviour {

    private ParticleSystem test;
    private ParticleSystem particle2;
    private ParticleSystem particle3;

	// Use this for initialization
	void Start () {
        //test = ParticleSystemManager.instance.test(ParticleSystemManager.instance.particleSystemPrefab, "none", Vector3.zero);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space)) {

            if(test == null)
                test = ParticleSystemManager.instance.test(ParticleSystemManager.instance.particleSystemPrefab, ParticleSystemStopAction.None, Vector3.zero, Vector3.up);

            //test.Stop();
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            particle2 = ParticleSystemManager.instance.test(ParticleSystemManager.instance.particleSystemPrefab, ParticleSystemStopAction.None, new Vector3(10, 0, 0), Vector3.down);

        }
        if (Input.GetKeyDown(KeyCode.B)) {
            particle3 = ParticleSystemManager.instance.test(ParticleSystemManager.instance.particleSystemPrefab, ParticleSystemStopAction.None, new Vector3(20, -10, 0), Vector3.right);
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            ParticleSystemManager.instance.stopParticleSystem(test);
        }
	}
}
