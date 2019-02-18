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

    bool b = false;
    // Use this for initialization
    private void Awake() {
        instance = this;
    }

    public AudioSource audioToPlay(AudioClip clipName) {
        GameObject audioObject = (GameObject)Instantiate(audioPrefab);
        AudioSource test = audioObject.GetComponent<AudioSource>();
        test.clip = clipName;
        //test.Play();
        return test;
    }

    public void playAudio(AudioSource audio, bool isPlaying) {
        if (isPlaying)
            audio.Play();
        if (!isPlaying)
            audio.Stop();

        StartCoroutine(stopAudioCoroutine(audio));
        
    }
    IEnumerator stopAudioCoroutine(AudioSource audio) {
        bool test = audio.isPlaying;
        Debug.Log(test);
        yield return new WaitUntil(() => test = false);
        Debug.Log(audio.gameObject.name);
        Debug.Log(test);

        Debug.Log("ther");
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
