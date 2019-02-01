using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective {

    public GameObject interactable;

    public GameObject alarmLight;

    public int controlPanelId;

    public bool done;

    /* 1 - 3 */
    [ReadOnly]
    public int warningLevel;

    public Objective(int cPId, GameObject intact, GameObject aLight) {

        interactable = intact;
        alarmLight = aLight;
        controlPanelId = cPId;
        done = false;

    }
}

public class ObjectiveManager : MonoBehaviour {

    public static ObjectiveManager instance = null;

    [Header("Debug")]
    public bool debugMode;

    bool nextSet = false;
    public bool tooManyObjectives;

    /* DO NOT USE */
    public int numberOfPanelSwitch;
    public int currentObjectiveId;
    public int howManyObjectives;
    public int objectivesDone;
    public int objectivesActivated = 0;
    public int warningLevel = 3;

    public List<Objective> objectiveList = new List<Objective>();
    public List<Objective> allObjectivesList = new List<Objective>();

    System.Random random = new System.Random();

    public GameManager gameManager;
    public Stats stats;
    public HeatManager heatManager;

    //public Material alarmLvl_3_material;
    //public Material alarmLvl_2_material;
    //public Material alarmLvl_1_material;

    private Material material;

    public Color alarmLvl_3_color;
    public Color alarmLvl_2_color;
    public Color alarmLvl_1_color;

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

    private void Update() {
        NextObjectiveSet();
    }

    /// <summary>
    /// 
    /// </summary>
    private void CreateObjectives() {

        if ((int) heatManager.heat / 10 == 0) {
            howManyObjectives = 1;
        }
        else {


            //howManyObjectives = (int) heatManager.heat / 10 + 1;
            howManyObjectives = heatManager.currentHeatLevel;

            if (howManyObjectives > stats.maxObjectives) {
                howManyObjectives = stats.maxObjectives;
            }
        }

        int createdObjectives = 0;
        while (createdObjectives != howManyObjectives && !tooManyObjectives) {

            if (objectiveList.Count == allObjectivesList.Count) {
                RemoveObjectives();
                tooManyObjectives = true;
            }
            else {
                tooManyObjectives = false;
            }

            switch (objectivesActivated) {

                case 1:
                    warningLevel--;
                    break;
                case 3:
                    warningLevel--;
                    break;
                case 6:
                    warningLevel = 3;
                    objectivesActivated = 0;
                    break;

            }

            int randomIndex = random.Next(0, allObjectivesList.Count);

            while (objectiveList.Contains(allObjectivesList[randomIndex])) {
                
                randomIndex = random.Next(0, allObjectivesList.Count);
            }

            objectiveList.Add(allObjectivesList[randomIndex]);
            //objectiveList[objectiveList.Count-1].alarmLight.GetComponentInChildren<Renderer>().material = red;
            //objectiveList[objectiveList.Count-1].alarmLight.GetComponentInChildren<Light>().color = Color.red;
            objectiveList[objectiveList.Count-1].alarmLight.GetComponentInChildren<Light>().range = 0.12f;
            objectiveList[objectiveList.Count-1].alarmLight.GetComponentInChildren<Light>().intensity = 2.46f;
            //objectiveList[createdObjectives].alarmLight.GetComponentInChildren<GameObject>().GetComponentInChildren<GameObject>().SetActive(false);
            //objectiveList[createdObjectives].interactable.
            print("switch");
            switch (warningLevel) {
                case 1:
                    objectiveList[objectiveList.Count - 1].warningLevel = 1;
                    AlarmLightController(warningLevel, alarmLvl_1_color);
                    break;
                case 2:
                    objectiveList[objectiveList.Count - 1].warningLevel = 2;
                    AlarmLightController(warningLevel, alarmLvl_2_color);                    
                    break;
                case 3:
                    objectiveList[objectiveList.Count - 1].warningLevel = 3;
                    AlarmLightController(warningLevel, alarmLvl_3_color);
                    break;
                    
            }
            createdObjectives++;
            objectivesActivated++;
            print("activate");


        }

        StartCoroutine(WaitNextSet());
    }

    private void RemoveObjectives() {
       
            objectiveList.RemoveAt(0);
            AlarmLightController(0, Color.black);

        
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
            print("too long");
            CreateObjectives();
            nextSet = false;
        }
    }

    public void CompleteObjective(Objective objective) {

        /* Objective that player is acting with is on objective list*/
        if (objectiveList.Contains(objective)) {
            print("objective DONE!!!");
        currentObjectiveId++;
            /* decreaseHeat bool is true */
            if (stats.decreaseHeat) {
                /* Stops heating for x amount of second(s) and also decrease heat x amount */
                
            }
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
        yield return new WaitForSeconds(4);
        CreateObjectives();
    }
    
    private IEnumerator WaitNextSet() {
        print("Wait next set");
        yield return new WaitForSeconds(5);
        //yield return null;
        nextSet = true;
        RemoveObjectives();
        
    }

    private void AlarmLightController(int alarmLevel, Color color) {

        

        

        //switch (alarmLevel) {

        //    case 1:
        //        material = alarmLvl_1_material;
        //        color = alarmLvl_1_color;
        //        break;
        //    case 2:
        //        material = alarmLvl_2_material;
        //        color = alarmLvl_2_color;
        //        break;
        //    case 3:
        //        material = alarmLvl_3_material;
        //        color = alarmLvl_3_color;
        //        break;
            
        //}

        

        switch (alarmLevel) {
            case 0:
                material = Instantiate(objectiveList[0].alarmLight.GetComponentInChildren<MeshRenderer>().material);
                objectiveList[0].alarmLight.GetComponentInChildren<MeshRenderer>().material = material;
                objectiveList[0].alarmLight.GetComponentInChildren<Light>().intensity = 0;
                material.EnableKeyword("_EMISSION");
                material.SetColor("_Color", Color.grey);
                material.SetColor("_EmissionColor", Color.grey * 0);
                break;

            default:
                material = Instantiate(objectiveList[objectiveList.Count - 1].alarmLight.GetComponentInChildren<MeshRenderer>().material);
                objectiveList[objectiveList.Count - 1].alarmLight.GetComponentInChildren<MeshRenderer>().material = material;
                objectiveList[objectiveList.Count - 1].alarmLight.GetComponentInChildren<Light>().intensity = 2.46f;
                material.EnableKeyword("_EMISSION");
                material.SetColor("_Color", color);
                material.SetColor("_EmissionColor", color * 1);
                objectiveList[objectiveList.Count - 1].alarmLight.GetComponentInChildren<Light>().color = color;
                break;
        }

        //objectiveList[objectiveList.Count - 1].alarmLight.GetComponentInChildren<Renderer>().material = material;

    }
}
