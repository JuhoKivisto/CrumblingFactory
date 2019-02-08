﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective {

    public GameObject interactable;

    public GameObject warningLight;

    public int controlPanelId;

    public int lifeTimeId;

    public bool done;

    /* 1 - 3 */
    [ReadOnly]
    public int warningLevel;

    public Objective(int cPId, GameObject intact, GameObject wLight) {

        interactable = intact;
        warningLight = wLight;
        controlPanelId = cPId;
        done = false;

    }
}

public class ObjectiveManager : MonoBehaviour {

    public static ObjectiveManager instance = null;

    [Header("Debug")]
    public bool debugMode;

    public bool objcomp;

    bool nextSet = false;
    public bool tooManyObjectives;

    /* DO NOT USE */
    public int numberOfPanelSwitch;
    public int currentObjectiveId;
    [ReadOnly]
    public int howManyObjectives;
    [ReadOnly]
    public int objectivesDone;
    [ReadOnly]
    public int objectivesActivated = 0;
    [ReadOnly]
    public int currentWarningLevel = 3;
    public int lifeTimeId = 0;

    public List<Objective> objectiveList = new List<Objective>();
    public List<Objective> allObjectivesList = new List<Objective>();

    public List<Coroutine> objectiveLifeTimes = new List<Coroutine>();

    System.Random random = new System.Random();

    public GameManager gameManager;
    public Stats stats;
    public HeatManager heatManager;

    //public Material alarmLvl_3_material;
    //public Material alarmLvl_2_material;
    //public Material alarmLvl_1_material;

    private Material material;    

    private Color color;

    void Awake() {

        if (instance == null) {
            instance = this;
        }

        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        //CreateObjectives___OLD();
        StartCoroutine(FirstObjectiveSet());
        TimeManager.instance.StartTimer();

    }

    void Update() {
        NextObjectiveSet();

        if (objcomp) {
            CompleteObjective(objectiveList[0]);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CreateObjectives() {

        if (heatManager.currentHeatLevel == 0) {
            howManyObjectives = 1;
        }
        else {
            
            howManyObjectives = heatManager.currentHeatLevel;

            if (howManyObjectives > stats.maxObjectives) {
                howManyObjectives = stats.maxObjectives;
            }
        }

        int createdObjectives = 0;
        while (createdObjectives != howManyObjectives && !tooManyObjectives) {

            if (objectiveList.Count == allObjectivesList.Count) {
                //RemoveObjectives();
                tooManyObjectives = true;
            }
            else {
                tooManyObjectives = false;
            }

            switch (objectivesActivated) {

                case 1:
                    currentWarningLevel--;
                    break;
                case 3:
                    currentWarningLevel--;
                    break;
                case 6:
                    currentWarningLevel = 3;
                    objectivesActivated = 0;
                    break;

            }

            int randomIndex = random.Next(0, allObjectivesList.Count);

            while (objectiveList.Contains(allObjectivesList[randomIndex])) {
                
                randomIndex = random.Next(0, allObjectivesList.Count);
            }

            objectiveList.Add(allObjectivesList[randomIndex]);
           
            objectiveList[objectiveList.Count-1].warningLight.GetComponentInChildren<Light>().range = 0.12f;
            objectiveList[objectiveList.Count-1].warningLight.GetComponentInChildren<Light>().intensity = 2.46f;
            if (debugMode) {
            print("switch");

            }
            
            objectiveList[objectiveList.Count - 1].warningLevel = currentWarningLevel;
            EnableWarningLight(objectiveList[objectiveList.Count - 1], stats.warningLevels[stats.warningLevels.Count - currentWarningLevel]);

            createdObjectives++;
            objectivesActivated++;
            if (debugMode) {

            print("activate");
            }

            objectiveLifeTimes.Add(StartCoroutine(DisableObjective(objectiveList[objectiveList.Count - 1],
                stats.warningLevels[stats.warningLevels.Count - currentWarningLevel], stats.objectiveLifeTime, lifeTimeId)));
            objectiveList[objectiveList.Count - 1].lifeTimeId = lifeTimeId;
            lifeTimeId++;
        }

        StartCoroutine(WaitNextSet()); 
    }

    private void RemoveObjective(Objective objective) {

        if (objectiveList.Contains(objective)) {
            objectiveList.Remove(objective);
        }
    }              
    
    /// <summary>
    /// Shuffles the given list
    /// </summary>
    /// <param name="list"></param>
    /// <returns>list</returns>
    private List<Objective> Shuffle(List<Objective> list) {
        System.Random rnd = new System.Random();
        for (int i = 0; i < list.Count; i++) {

            Objective temp = list[i];
            int rndIndex = rnd.Next(list.Count);
            list[i] = list[rndIndex];
            list[rndIndex] = temp;

        }
        return list;
    }

    public void PopulateList(Objective objective) {

        allObjectivesList.Add(objective);
    }

    public void NextObjectiveSet() {

        /*
        //current objective == how many objectives total in set 
        if (currentObjectiveId + 1 == howManyObjectives + 1) {
            gameManager.StopHeating(stats.waitTimeOnObjectiveSetComplete, stats.decreaseHeatOnObjectiveSetComplete);
            CreateObjectives();
        }
        */
        // More objectives if heat is high enough
        if (nextSet) {
            if (debugMode) {

            print("too long");
            }
            CreateObjectives();
            nextSet = false;
        }
    }

    public void CompleteObjective(Objective objective) {

        /* Objective that player is acting with is on objective list*/
        if (objectiveList.Contains(objective)) {
            print("objective DONE!!!");
            objcomp = false;
            StartCoroutine(heatManager.StopHeating(objective));
            DisableObjective(objective, stats.warningLevels[stats.warningLevels.Count - objective.warningLevel], 0, objective.lifeTimeId);
            /* decreaseHeat bool is true */
            
            //NextObjectiveSet();
        }
        
    }

    /// <summary>
    /// DO NOT USE!!!!!!!!!!!!!!
    /// Creates random objective list from all possible objectives
    /// Also the number of objectives is random
    /// </summary>
    /// <param name="AllObjectivesList"></param>
    /// 
    private void CreateObjectives___OLD() {
        System.Random rnd = new System.Random();

        //int maxIterations = 1;
        numberOfPanelSwitch = rnd.Next(3, 8); // Generates random number between 3-7 for how meny panel switch happens
        if (debugMode) {

            print("number of panel switch: " + numberOfPanelSwitch);
        }

        /* Creates randomized number of actions for each control panel */
        for (int index = 0; index < numberOfPanelSwitch; index++) {

            int panelId = rnd.Next(1, 4);
            if (debugMode) {

                print("Panel id: " + panelId);
            }

            /* How many actions need to be done before changing panel*/
            int iterations = rnd.Next(1,5);
            if (debugMode) {

            print("iterations: " + iterations);
            }

            int id = 0;
            while (id < iterations) {
                //int numberOfInteractions = 0;

                /* List that holds temporary all interactions on specifig panel */
                List<Objective> controlPanel = new List<Objective>();

                /* Adds all interactables with the same panel id to the controlPanel- list */
                foreach (var objective in allObjectivesList) {
                    if (objective.controlPanelId == panelId) {
                        controlPanel.Add(objective);
                        id++;
                    }
                }

                controlPanel = Shuffle(controlPanel);

                foreach (var objective in controlPanel) {
                    objectiveList.Add(objective);

                }  
            }
        }
        currentObjectiveId = 0;
        objectiveList[0].interactable.gameObject.transform.Translate(Vector3.up);

        TimeManager.instance.StartTimer();
    }

    private IEnumerator FirstObjectiveSet() {
        yield return new WaitForSeconds(stats.waitBeforeFirstSet);
        CreateObjectives();
        //heatManager.working = true;
        StartCoroutine(heatManager.IncreaseHeat());
    }
    
    private IEnumerator WaitNextSet() {
        print("Wait next set");
        yield return new WaitForSeconds(stats.waitBeforeNextSet);
        //yield return null;
        nextSet = true;
        //RemoveObjectives();
        
    }   

    private void EnableWarningLight(Objective objective, WarningLevel warningLvl) {

        material = Instantiate(objective.warningLight.GetComponentInChildren<MeshRenderer>().material);
        objective.warningLight.GetComponentInChildren<MeshRenderer>().material = material;

        objective.warningLight.GetComponentInChildren<Light>().intensity = 2.46f;
        material.SetColor("_EmissionColor", warningLvl.color * 1);

        material.EnableKeyword("_EMISSION");
        material.SetColor("_Color", warningLvl.color);

        objective.warningLight.GetComponentInChildren<Light>().color = warningLvl.color;

        if (debugMode) {

        print("<color=blue>Light enabled</color> warnLvl: " + warningLvl.level);
        }
    }

    private void DisableWarningLight(Objective objective) {

        material = Instantiate(objective.warningLight.GetComponentInChildren<MeshRenderer>().material);
        objective.warningLight.GetComponentInChildren<MeshRenderer>().material = material;

        objective.warningLight.GetComponentInChildren<Light>().intensity = 0;

        material.SetColor("_EmissionColor", Color.gray * 0);
        material.EnableKeyword("_EMISSION");
        material.SetColor("_Color", Color.gray);
    }

    private IEnumerator DisableObjective(Objective objective,WarningLevel currentWarningLvl, float duration, int lifeTimeId) {
        //print("ghkdgsonfg");
        float time = 0;
        int lightOn = 1;        

        while (time < duration) {

            time += Time.deltaTime;

           lightOn = (int) stats.heatCurve3.Evaluate(time / duration);

            switch (lightOn) {

                case 1:
                    EnableWarningLight(objective, currentWarningLvl);
                    break;
                case 0:
                    DisableWarningLight(objective);
                    break;               
            }

            //time++;
            yield return null;
        }

        RemoveObjective(objective);
        DisableWarningLight(objective);
        objectiveLifeTimes.RemoveAt(lifeTimeId);

        print("<color=blue>time over</color>");
        print("<color=yellow>time: </color>"+ time);

    }
}
