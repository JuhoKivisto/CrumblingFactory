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

    public ParticleSystem test(GameObject particleSys, string stopAction, Vector3 position, Vector3 direction) {

        Quaternion changeDirection = new Quaternion();


        if (direction == Vector3.zero) {                                //check that the direction is not zero
            direction = Vector3.up;                                     //default direction is up
        }

        if (particleSys.GetComponent<ChangeDirection>().canChange) {        //this changes direction of particle      
            changeDirection = Quaternion.LookRotation(direction);
        }

        GameObject clone = Instantiate(particleSys, position, changeDirection);

        ParticleSystem temp = clone.GetComponent<ParticleSystem>();

        stopAction = stopAction.ToLower();                              //to avoid any spelling mistake

        var main = temp.main;

        switch (stopAction) {
            case "disable":
                main.stopAction = ParticleSystemStopAction.Disable;
                break;
            case "destroy":
                main.stopAction = ParticleSystemStopAction.Destroy;
                break;
            case "callback":
                main.stopAction = ParticleSystemStopAction.Callback;
                break;
            default:
                main.stopAction = ParticleSystemStopAction.None;
                break;
        }


        temp.Play();

        if(particleSys.transform.childCount > 0) {
            foreach(Transform child in particleSys.transform) {

                GameObject childGameObject = child.gameObject;

                ParticleSystem tempChild = childGameObject.GetComponent<ParticleSystem>();

                Quaternion childQuaternion = new Quaternion();

                if (childGameObject.GetComponent<ChangeDirection>().canChange) {        //this can change direction of particle

                    childQuaternion = Quaternion.LookRotation(direction);
                }

                childGameObject.transform.rotation= childQuaternion;

                var mainChild = tempChild.main;

                switch (stopAction) {
                    case "disable":
                        mainChild.stopAction = ParticleSystemStopAction.Disable;
                        break;
                    case "destroy":
                        mainChild.stopAction = ParticleSystemStopAction.Destroy;
                        break;
                    case "callback":
                        mainChild.stopAction = ParticleSystemStopAction.Callback;
                        break;
                    default:
                        mainChild.stopAction = ParticleSystemStopAction.None;
                        break;
                }

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
