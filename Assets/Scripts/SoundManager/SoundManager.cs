using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager instance;

    [SerializeField] private GameObject audioPrefab;

    [Header("Audio Clip Names")]
    public AudioClip FireAlarmSound;
    public AudioClip BackgroundMusic;

    private void Awake() {
        instance = this;
    }

    public AudioSource audioToPlay(AudioClip clipName, bool isLoop, float volume, float timeToStop, Transform parentTransform, Vector3 newPosition) { //this return audio with setting

        GameObject audioObject = (GameObject)Instantiate(audioPrefab);

        if (parentTransform != null) {                                     //attach to parent and set local position
            audioObject.transform.SetParent(parentTransform);
        }

        audioObject.transform.position = newPosition;
        AudioSource audio = audioObject.GetComponent<AudioSource>();

        audio.clip = clipName;
        audio.volume = volume;
        audio.loop = (isLoop) ? true : false;
        audio.Play();

        if (timeToStop > 0)                                     //if time = 0, do not stop audio
            StartCoroutine(waitForSeconds(audio, timeToStop));

        StartCoroutine(DestroyWhenStop(audio));                 //destroy game object

        return audio;
    }

    public void stopAudio(AudioSource audio) {      //stop audio
        if(audio != null) {
            audio.Stop();
        }               
    }
    IEnumerator DestroyWhenStop(AudioSource audio) { //wait audio to stop then destroy game object which has audio source

        yield return new WaitUntil(() => !audio.isPlaying);

        if(audio != null)
            Destroy(audio.gameObject);
    }

    IEnumerator waitForSeconds(AudioSource audio, float time) {

        yield return new WaitForSeconds(time);
        if(audio != null)
            audio.Stop();
    }
}
