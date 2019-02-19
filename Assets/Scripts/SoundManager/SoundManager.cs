using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;

    [SerializeField] private GameObject audioPrefab;

    [Header("Audio Clip Names")]
    public AudioClip FireAlarmSound;
    public AudioClip BackgroundMusic;

    private AudioSource audioSource;


    private void Awake() {
        instance = this;
    }

    public AudioSource audioToPlay(AudioClip clipName, bool isLoop, float volume) { //this return audio with setting

        GameObject audioObject = (GameObject)Instantiate(audioPrefab);
        AudioSource audio = audioObject.GetComponent<AudioSource>();

        audio.clip = clipName;
        audio.volume = volume;
        audio.loop = (isLoop) ? true : false;

        return audio;
    }

    public void playAudio(AudioSource audio, bool isPlaying) {      //play and stop audio
        if(audio != null) {

            if (isPlaying)
                audio.Play();
            else
                audio.Stop();

            StartCoroutine(stopAudioCoroutine(audio));
        }

        
        
    }
    IEnumerator stopAudioCoroutine(AudioSource audio) { //wait audio to stop then destroy game object which has audio source
        bool test = audio.isPlaying;

        yield return new WaitUntil(() => !audio.isPlaying);

        if(audio != null)
            Destroy(audio.gameObject);

    }
       
    
    //public void test(AudioClip clipName, bool isLoop, float volume, float timeToDestroy) {

    //    GameObject sound = (GameObject)Instantiate(audioPrefab);

    //    audioSource = sound.GetComponent<AudioSource>();
    //    audioSource.clip = clipName;
    //    audioSource.volume = volume;
    //    audioSource.loop = isLoop;
    //    audioSource.Play();
    //    Destroy(sound, timeToDestroy);
    //}
}
