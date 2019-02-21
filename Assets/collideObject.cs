using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collideObject : MonoBehaviour {

    private AudioSource test;
    private AudioSource backgroudMusic;


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if(test == null)                //avoid to be called twice
                test = SoundManager.instance.audioClipToPlay(SoundManager.instance.FireAlarmSound, true, 1, 0, this.transform, Vector3.zero);
        }

        else if (Input.GetKeyDown(KeyCode.S)) {
            SoundManager.instance.stopAudio(test);
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            if (test == null) {
                test = SoundManager.instance.audioSourceToPlay(SoundManager.instance.AudioSourceList[0], 0, null, Vector3.zero);
            }
        }
        
    }

}
