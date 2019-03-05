using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour {

    public static ParticleSystemManager instance;

    public GameObject particleSystemPrefab;

    public GameObject particle2;

    private void Awake() {
        instance = this;
    }

    public ParticleSystem test(GameObject particleSys, ParticleSystemStopAction stopAction, Vector3 position, Vector3 direction) {

        Quaternion changeDirection = new Quaternion();


        if (direction == Vector3.zero) {                                //check that the direction is not zero
            direction = Vector3.up;                                     //default direction is up
        }
        changeDirection = Quaternion.LookRotation(direction);
        

        GameObject clone = Instantiate(particleSys, position, changeDirection);

        ParticleSystem temp = clone.GetComponent<ParticleSystem>();

        var main = temp.main;

        main.stopAction = stopAction;

        temp.Play();

        if(particleSys.transform.childCount > 0) {
            foreach(Transform child in particleSys.transform) {

                GameObject childGameObject = child.gameObject;

                ParticleSystem tempChild = childGameObject.GetComponent<ParticleSystem>();

                var mainChild = tempChild.main;

                mainChild.stopAction = stopAction;

                tempChild.Play();
            }
        }

        return temp;
    }

    public void stopParticleSystem (ParticleSystem particleSystem) {

        if(particleSystem.gameObject.transform.childCount > 0) {
            foreach (Transform child in particleSystem.gameObject.transform) {
                ParticleSystem childTemp = child.gameObject.GetComponent<ParticleSystem>();

                childTemp.Stop();
            }
        }
        particleSystem.Stop();
    }
}
