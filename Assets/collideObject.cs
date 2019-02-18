using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collideObject : MonoBehaviour {

    AudioSource test;

    private void Start() {

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            test = SoundManager.instance.audioToPlay(SoundManager.instance.FireAlarmSound);
            SoundManager.instance.playAudio(test, true);
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            SoundManager.instance.playAudio(test, false);
        }
    }


    private void OnCollisionEnter(Collision collision) {
        //AudioSource test = SoundManager.instance.audioToPlay(SoundManager.instance.FireAlarmSound);
        
        //if (this.name == "Cube")
        //    SoundManager.instance.playAudio(test, true);
        

    }
}
