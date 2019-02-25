using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour {

    public static ParticleSystemManager instance;
    public GameObject particleSystemPrefab;

    private void Awake() {
        instance = this;
    }

    public ParticleSystem test(GameObject particleSys, string stopAction) {

        GameObject clone = Instantiate(particleSys);

        ParticleSystem temp = clone.GetComponent<ParticleSystem>();
        var main = temp.main;

        main.loop = false;

        temp.Play();
        return temp;
    }
}
