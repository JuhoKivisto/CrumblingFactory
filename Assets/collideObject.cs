using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collideObject : MonoBehaviour {

    private AudioSource test;
    private AudioSource backgroudMusic;


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if(test == null)                //avoid to be called twice
                test = SoundManager.instance.audioToPlay(SoundManager.instance.FireAlarmSound, true, 1, 0, this.transform, Vector3.zero);
        }

        else if (Input.GetKeyDown(KeyCode.S)) {
            SoundManager.instance.stopAudio(test);
        }
    }

}
