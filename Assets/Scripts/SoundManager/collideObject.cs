using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collideObject : MonoBehaviour {

    private AudioSource test;
    private AudioSource backgroudMusic;


    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.S)) {
            SoundManager.instance.stopAudio(test);
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            if (test == null) {
                test = SoundManager.instance.audioSourceToPlay(SoundManager.instance.testSound, 0, true, 1f, null, Vector3.zero);
            }
        }
        
    }

}
