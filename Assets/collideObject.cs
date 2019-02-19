using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collideObject : MonoBehaviour {

    private AudioSource test;
    private AudioSource backgroudMusic;



    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            test = SoundManager.instance.audioToPlay(SoundManager.instance.FireAlarmSound, true, 1);
            SoundManager.instance.playAudio(test, true);
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            SoundManager.instance.playAudio(test, false);
        }
    }



}
