using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective {

    public GameObject interactable;

    public int controlPanelId;

    public bool done;

    public Objective(int cPId, GameObject intact) {

        interactable = intact;
        controlPanelId = cPId;
        done = false;

    }
}

public class ObjectiveManager : MonoBehaviour {

    public static ObjectiveManager instance = null;

    [Header("Debug")]
    public bool debugMode;

    /* DO NOT USE */
    public int numberOfPanelSwitch;
    public int currentObjectiveId;
    public int howManyObjectives;
    public int objectivesDone;

    public List<Objective> objectiveList = new List<Objective>();
    public List<Objective> allObjectivesList = new List<Objective>();

    System.Random random = new System.Random();

    public GameManager gameManager;
    public Stats stats;

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
        TimeManager.instance.StartTimer();

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

    private void CreateObjectives() {

        if (gameManager.heatMeter / 10 == 0) {
            howManyObjectives = 1;
        }
        else {

            howManyObjectives = (int) gameManager.heatMeter / 10 + 1;

            if (howManyObjectives > 5) {
                howManyObjectives = 5;
            }
        }

        int createdObjectives = 0;
        while (createdObjectives != howManyObjectives) {
            int randomIndex = random.Next(0, allObjectivesList.Count);

            while (!objectiveList.Contains(allObjectivesList[randomIndex])) {
                randomIndex = random.Next(0, allObjectivesList.Count);
            }
            createdObjectives++;
            objectiveList.Add(allObjectivesList[randomIndex]);
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

    public void NextObjective() {

        if (currentObjectiveId < objectiveList.Count - 1) {

            //objectiveList[currentObjectiveId].interactable.SetActive(false);

            //objectiveList[currentObjectiveId++].interactable.SetActive(true);
            objectiveList[currentObjectiveId].done = true;
            objectiveList[currentObjectiveId++].interactable.gameObject.transform.Translate(Vector3.up);
            GameManager.instance.EventByInteraction();

        }
    }

    public IEnumerator CompleteObjective() {

        objectiveList[currentObjectiveId].interactable.gameObject.transform.Translate(-Vector3.up);
        yield return new WaitForSeconds(1f);
        NextObjective();
    }

    public void Test() {
        StartCoroutine(CompleteObjective());
    }
}
