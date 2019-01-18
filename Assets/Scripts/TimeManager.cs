using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

    public static TimeManager instance = null;

    public Text timeText;

    private bool timerRunning;

    public float time;

    public float gameLenght = 125;

    void Awake() {

        if (instance == null) {
            instance = this;
        }

        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Timer
    /// </summary>
    /// <returns></returns>
    private IEnumerator RemainingTime() {

        int seconds = 0;
        int minutes = 0;
        if (time == 0) time = 0;

        seconds = (int)(gameLenght - time);
        minutes = seconds / 60;
        seconds = seconds - minutes * 60;

        timeText.color = Color.yellow;
        timeText.text = "Time left: ";
        yield return new WaitForSeconds(2f);
        timeText.color = Color.white;

        while (timerRunning) {
            timeText.text = "Time left: " + (minutes.ToString("00") + ":" + seconds.ToString("00"));

            if (seconds == 0) minutes--;
            if (seconds == 0) seconds = 59;
            yield return new WaitForSeconds(1f);
            time++;

            seconds = (int)(gameLenght - time);
            minutes = seconds / 60;
            seconds = seconds - minutes * 60;

            if (gameLenght == time) {
                timerRunning = false;
                timeText.color = Color.red;

            }

            timeText.text = "Time left: " + (minutes.ToString("00") + ":" + seconds.ToString("00"));
            GameManager.instance.EventByTime();
            GameManager.instance.EventByHeat();
        }



    }

    /// <summary>
    /// Starts the timer
    /// and creates time events
    /// </summary>
    public void StartTimer() {
        GameManager.instance.CreateTimeEvents();
        StartCoroutine(RemainingTime());
        timerRunning = true;
    }
}
