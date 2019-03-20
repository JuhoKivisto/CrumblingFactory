using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class callParticleSystem : MonoBehaviour {

    private ParticleSystem test;
    private ParticleSystem explosion;
    private ParticleSystem particle3;

    private int countExplosion = 0;
    public int currentTemperature = 0;

    public List<Vector3> position;

	
	// Update is called once per frame
	void Update () {

        if(currentTemperature > 50 && currentTemperature <= 60 && countExplosion == 0) {

            explosion = ParticleSystemManager.instance.test(ParticleSystemManager.instance.particleSystemPrefab, ParticleSystemStopAction.Destroy, position[Random.Range(0, position.Count)], Vector3.down);
            countExplosion++;
            Debug.Log(currentTemperature);
        }
        else if(currentTemperature > 60 && currentTemperature <= 70 && countExplosion == 1) {

            Vector3 explosionPosition1 = position[Random.Range(0, position.Count)];
            Vector3 explosionPosition2 = position[Random.Range(0, position.Count)];

            while(explosionPosition1 == explosionPosition2) {
                explosionPosition2 = position[Random.Range(0, position.Count)];
            }

            explosion = ParticleSystemManager.instance.test(ParticleSystemManager.instance.particleSystemPrefab, ParticleSystemStopAction.Destroy, explosionPosition1, Vector3.down);
            explosion = ParticleSystemManager.instance.test(ParticleSystemManager.instance.particleSystemPrefab, ParticleSystemStopAction.Destroy, explosionPosition2, Vector3.down);
            countExplosion++;

        }else if(currentTemperature > 70 && currentTemperature <=80 && countExplosion == 2) {
            explosion = ParticleSystemManager.instance.test(ParticleSystemManager.instance.particleSystemPrefab, ParticleSystemStopAction.Destroy, position[Random.Range(0, position.Count)], Vector3.down);
            countExplosion++;
        }



        //if (Input.GetKeyDown(KeyCode.Space)) {

        //    if(test == null)
        //        test = ParticleSystemManager.instance.test(ParticleSystemManager.instance.particleSystemPrefab, ParticleSystemStopAction.None, Vector3.zero, Vector3.up);

        //    //test.Stop();
        //}
        //if (Input.GetKeyDown(KeyCode.A)) {
        //    particle2 = ParticleSystemManager.instance.test(ParticleSystemManager.instance.particleSystemPrefab, ParticleSystemStopAction.None, new Vector3(10, 0, 0), Vector3.down);

        //}
        //if (Input.GetKeyDown(KeyCode.B)) {
        //    particle3 = ParticleSystemManager.instance.test(ParticleSystemManager.instance.particleSystemPrefab, ParticleSystemStopAction.None, new Vector3(20, -10, 0), Vector3.right);
        //}

        //if (Input.GetKeyDown(KeyCode.S)) {
        //    ParticleSystemManager.instance.stopParticleSystem(test);
        //}
	}
}
