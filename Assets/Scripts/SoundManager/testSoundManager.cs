using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSoundManager : MonoBehaviour {

    AudioSource testSound;

	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (testSound == null)      //avoid to have two same references
                testSound = SoundManager.instance.audioSourceToPlay(SoundManager.instance.testSound, 0, false, 0.5f, null, Vector3.zero) ;
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            SoundManager.instance.stopAudio(testSound);
        }
	}
}
