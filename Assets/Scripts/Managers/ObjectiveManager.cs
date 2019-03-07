using System.Collections;
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
        lifeTimeId = -1;
        warningLevel = -1;

    }
    public Objective(int cPId, GameObject intact) {

        interactable = intact;
        controlPanelId = cPId;
        done = false;

    }

    public Objective() {

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

    /// <summary>
    /// How many objectives is done total
    /// </summary>
    public int objectivesDone;

    /// <summary>
    /// Required amount of done objectives for reactor room objectives
    /// </summary>
    public int ObjectivesForReactor;

    public bool isReactorRoomOpen;

    /// <summary>
    /// How many objectives are created for current objective set
    /// </summary>
    [ReadOnly]
    public int howManyObjectives;
    /// <summary>
    /// Keeps track on how many objectives is activated.
    /// Just a variable that helps chainging warning levels
    /// </summary>
    [ReadOnly]
    public int objectivesActivated = 0;
    /// <summary>
    /// Current warning level
    /// </summary>
    [ReadOnly]
    public int currentWarningLevel = 3;
    /// <summary>
    /// Indicates the objective life time place on objectiveLifeTimes
    /// </summary>
    public int lifeTimeId = 0;

    /// <summary>
    /// objectives that are currently activated
    /// </summary>
    public List<Objective> objectiveList = new List<Objective>();
    /// <summary>
    /// All possible objectives that are on the all control panels
    /// </summary>
    public List<Objective> allObjectivesList = new List<Objective>();
    /// <summary>
    /// Stores DisableObjective coroutines
    /// </summary>
    public List<Coroutine> objectiveLifeTimes = new List<Coroutine>();

    System.Random random = new System.Random();

    public GameManager gameManager;
    public Stats stats;
    public HeatManager heatManager;
    public ReactorRoomController reactorRoomController;

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
        if (CheckForMissing()) return;

        //CreateObjectives___OLD();
        StartCoroutine(FirstObjectiveSet());

    }

    void Update() {
        NextObjectiveSet();

        if (objcomp) {
            //CompleteReactorShutDown();
            reactorRoomController.OpenReactorRoomDoors();
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

        /* If howManyObjectives is greater than all objectives count and smaller than -1 then set how many objectives to allObjectives count */
        if (objectiveList.Count + howManyObjectives > allObjectivesList.Count || howManyObjectives <= -1) {
            howManyObjectives = 0;
        }
        //if (howManyObjectives > objectiveList.Count)
        //{
        //    howManyObjectives = 0;
        //}

        int createdObjectives = 0;
        while (createdObjectives != howManyObjectives && !tooManyObjectives && howManyObjectives != 0) {

            if (objectiveList.Count == allObjectivesList.Count) {
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

            int currentId = objectiveList.Count - 1;

            objectiveList[currentId].warningLight.GetComponentInChildren<Light>().range = 0.12f;
            objectiveList[currentId].warningLight.GetComponentInChildren<Light>().intensity = 2.46f;

            if (debugMode) {
                print("switch");

            }

            objectiveList[currentId].warningLevel = currentWarningLevel;
            EnableWarningLight(objectiveList[currentId], stats.warningLevels[stats.warningLevels.Count - currentWarningLevel]);

            createdObjectives++;
            objectivesActivated++;
            if (debugMode) {

                print("activate");
            }

            objectiveLifeTimes.Add(StartCoroutine(DisableObjective(objectiveList[currentId],
                stats.warningLevels[stats.warningLevels.Count - currentWarningLevel], stats.objectiveLifeTime, lifeTimeId)));
            objectiveList[currentId].lifeTimeId = lifeTimeId;
            lifeTimeId++;


            StartCoroutine(SetObjectiveInfo(objectiveList[currentId]));
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

        #region old
        /*
        //current objective == how many objectives total in set 
        if (currentObjectiveId + 1 == howManyObjectives + 1) {
            gameManager.StopHeating(stats.waitTimeOnObjectiveSetComplete, stats.decreaseHeatOnObjectiveSetComplete);
            CreateObjectives();
        }
        */
        #endregion

        // More objectives if heat is high enough
        if (nextSet) {
            if (debugMode) print("too long");

            CreateObjectives();
            nextSet = false;
        }
    }

    public void CompleteObjective(Objective objective) {


        /* Objective that player is acting with is on objective list*/
        if (objectiveList.Contains(objective)) {
            
            print("objective DONE!!!");
            objcomp = false;
            heatManager.ActiveChangeHeating(objective, stats.changeHeatingFor, false);
            StopCoroutine(objectiveLifeTimes[objective.lifeTimeId]);
            //objectiveLifeTimes.RemoveAt(objective.lifeTimeId);
            StartCoroutine(DisableObjective(objective, stats.warningLevels[stats.warningLevels.Count - objective.warningLevel], 0, objective.lifeTimeId));
            objectivesDone++;
            if (CheckForReactorRoomOpening() && !isReactorRoomOpen) {
                reactorRoomController.OpenReactorRoomDoors();
            }
            
        }
        else
        {
            heatManager.ActiveChangeHeating(objective, stats.changeHeatingFor, true);

        }

    }

    public void FailureObjective(Objective objective) {
        heatManager.ActiveChangeHeating(objective, stats.changeHeatingFor, true);
    }

    public void CompleteReactorShutDown() {
        reactorRoomController.ShutDownTheReactor();
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
            int iterations = rnd.Next(1, 5);
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
        heatManager.StartHeatIncreace();
        TimeManager.instance.StartTimer();

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

    /// <summary>
    /// Disables given objective int given duration
    /// and removes DisableObjective from objectiveLiveTimes using lifeTimeId
    /// </summary>
    /// <param name="objective"></param>
    /// <param name="currentWarningLvl"></param>
    /// <param name="duration"></param>
    /// <param name="lifeTimeId"></param>
    /// <returns></returns>
    private IEnumerator DisableObjective(Objective objective, WarningLevel currentWarningLvl, float duration, int lifeTimeId) {
        //print("ghkdgsonfg");
        float time = 0;
        int lightOn = 1;

        if (duration == 0) {

        }
        else {

            while (time < duration) {

                time += Time.deltaTime;

                lightOn = (int)stats.alarmLigthBlinkIntensity.Evaluate(time / duration);

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
        }

        RemoveObjective(objective);
        DisableWarningLight(objective);
        //objectiveLifeTimes.RemoveAt(lifeTimeId);
        //FailureObjective(objective);

        print("<color=blue>time over</color>");
        print("<color=yellow>time: </color>" + time);

    }

    private IEnumerator SetObjectiveInfo(Objective objective) {
        print(objective.interactable);
        objective.interactable.GetComponentInParent<InteractableTest>().objectiveInfo = objective;
        yield return null;
    }

    /// <summary>
    /// Check if there are missing GameObjects or Components
    /// </summary>
    private bool CheckForMissing() {
        bool missing = false;

        if (stats == null) {
            Debug.LogError("Script 'stats' has no reference, attach Stats script");
            missing = true;
        }
        if (heatManager == null) {
            Debug.LogError("Script 'heatManager' has no reference, attach HeatManager script");
            missing = true;
        }
        if (gameManager == null) {
            Debug.LogWarning("Script 'gameManager' has no reference, attach GameManager script");

        }
        if (reactorRoomController == null) {
            Debug.LogError("Script 'reactorRoomController' has no reference, attach ReactorRoomController script");
            missing = true;
        }

        return missing;

    }

    /// <summary>
    /// Check if there are enough objectives done for reactor room opening
    /// </summary>
    /// <returns></returns>
    private bool CheckForReactorRoomOpening() {
        bool open = false;
        if (objectivesDone == ObjectivesForReactor) {
            open = true;
        }
        return open;
    }
}
