using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public int errorCount = 0;

    public int doomCounter = 0;

    public int[] timeEvents;

    public int timeEventId;

    public float heatMeter;

    public Stats stats;

    public GameObject player;

    //ObjectiveManager objManager = ObjectiveManager.instance.GetComponent<ObjectiveManager>();

    void Awake() {

        if (instance == null) {
            instance = this;
        }

        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        timeEvents = new int[] {0,0,0,0 };
      
    }

    /// <summary>
    /// After each interaction there might be some events
    /// Events are hapening when player has completed certain amount of tasks
    /// </summary>
    public void EventByInteraction() {

        /* If player has completed 1/4 of the objectives something might hapen */
        if (ObjectiveManager.instance.currentObjectiveId+1 == ObjectiveManager.instance.objectiveList.Count / 4) {
            print("first");
        }
        /* If player has completed 1/3 of the objectives something might hapen */
        else if (ObjectiveManager.instance.currentObjectiveId + 1 == ObjectiveManager.instance.objectiveList.Count / 3) {
            print("second");

        }
        /* If player has completed 1/2 of the objectives something might hapen */
        else if (ObjectiveManager.instance.currentObjectiveId + 1 == ObjectiveManager.instance.objectiveList.Count / 2) {
            print("third");

        }
        /* If player has completed more than 1/2 of the objectives doomCounter starts to work and
         with that even more severe events hapens*/
        else if (ObjectiveManager.instance.currentObjectiveId + 1 > ObjectiveManager.instance.objectiveList.Count / 2) {
            doomCounter++;
            if (doomCounter % 3 == 0) {

            }
            /* If doomCounter is creater than objetive count / 5 */
            else if (doomCounter > ObjectiveManager.instance.objectiveList.Count / 5) {

            }
            
        }
    }

    /// <summary>
    /// After certain amount of time witch is random, some events will hapen
    /// </summary>
    public void EventByTime() {

        /* Increases heat after certain time */
        if (TimeManager.instance.time % stats.timeDivider == 0) {
            heatMeter += stats.heatIncreaser;
            
        }
        
        if (TimeManager.instance.time == timeEvents[timeEventId] && timeEventId < timeEvents.Length) {
            print(TimeManager.instance.gameLenght - TimeManager.instance.time);
            //print(timeEventId);
            timeEventId++;
        }
        if (TimeManager.instance.time == TimeManager.instance.gameLenght) {
            // factory explodes
        }
    }

    /// <summary>
    /// If player does too many errors
    /// game punnishes the player
    /// after the errorThreshold[2] errorCount resets
    /// </summary>
    public void EventByError() {
        if (errorCount == stats.errorThresholds[0]) {
            
        }
        else if (errorCount == stats.errorThresholds[1]) {

        }
        else if (errorCount == stats.errorThresholds[2]) {
            errorCount = 0;
        }
    }

    /// <summary>
    /// When factory heat meter reaches
    /// temperature high enough events happens
    /// </summary>
    public void EventByHeat() {
        if (heatMeter == stats.criticalTemporatures[0].heatOfTheEvent) {
            Vector3 spwnpoint = new Vector3(player.transform.position.x, 60, player.transform.position.z);

            GameObject tempGameO = GameObject.Instantiate(stats.criticalTemporatures[0].eventObject,spwnpoint, Quaternion.identity, null);
            //tempGameO.GetComponentInChildren<Rigidbody>().AddForce(Vector3.up * 1000f);
            // minor explosions
            // factory shaking 
        }
        else if (heatMeter == 75) {
            // screen turns red
            // explosions
            // some of the conrol panel interactables could explode

        }
        else if (heatMeter == 80) {
            // one of the controll panel explode
        }
        else if (heatMeter == 100) {
            // factory explodes
        }
    }

    /// <summary>
    /// Creates times for time based events
    /// </summary>
    public void CreateTimeEvents() {
        System.Random rnd = new System.Random();

        int indexDecreaser = 0;
        for (int i = 0; i < timeEvents.Length; i++) {
            timeEvents[i] = rnd.Next(stats.timeEventRandomLow + timeEvents[i-indexDecreaser],
                stats.timeEventRandomHigh + timeEvents[i-indexDecreaser]);
            indexDecreaser = 1;
        }
    }


    IEnumerator WaitList() 
    {
        yield return StartCoroutine(WaitCoroutine(2f, "SetKinematic"));
        //Do something
        yield return StartCoroutine(WaitCoroutine(4f, "Scaledown"));
        //Do something
        yield return StartCoroutine(WaitCoroutine(8f, "Destroy"));
        //Do something
    }

    IEnumerator WaitCoroutine(float waitTime, string functionName) {
        yield return new WaitForSeconds(waitTime);
        Invoke(functionName, 0f);
    }
}
