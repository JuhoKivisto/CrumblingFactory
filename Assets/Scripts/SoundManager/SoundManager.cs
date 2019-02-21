using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager instance;

    [SerializeField] private GameObject audioPrefab;

    [Header("Audio Clip Names")]
    public AudioClip FireAlarmSound;
    public AudioClip BackgroundMusic;
    public List<GameObject> AudioSourceList;


    private void Awake() {
        instance = this;
    }


    //this method is used when there are too many settings on audio source and audio source settings are done at inspector
    public AudioSource audioSourceToPlay(GameObject audioSource, float timeToStop, Transform parentTransform, Vector3 newPosition) {   

        GameObject audioObject = (GameObject)Instantiate(audioSource, newPosition, new Quaternion());
        AudioSource temp = audioObject.GetComponent<AudioSource>();

        if(parentTransform != null) {
            audioObject.transform.SetParent(parentTransform);
        }

        temp.Play();

        if (timeToStop > 0)                                     //if time = 0, do not stop audio
            StartCoroutine(waitForSeconds(temp, timeToStop));

        StartCoroutine(DestroyWhenStop(temp));                 //destroy game object

        return temp;
    }


    //this method is used when there isn't many settings on audio source
    public AudioSource audioClipToPlay(AudioClip clipName, bool isLoop, float volume, float timeToStop, Transform parentTransform, Vector3 newPosition) { 

        GameObject audioObject = (GameObject)Instantiate(audioPrefab, newPosition, new Quaternion());

        if (parentTransform != null) {                                     //attach to parent and set local position
            audioObject.transform.SetParent(parentTransform);
        }

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

    //stop audio
    public void stopAudio(AudioSource audio) {      
        if(audio != null) {
            audio.Stop();
        }               
    }


    //wait audio to stop then destroy game object which has audio source
    IEnumerator DestroyWhenStop(AudioSource audio) { 

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
