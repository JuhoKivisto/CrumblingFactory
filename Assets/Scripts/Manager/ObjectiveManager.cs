using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective {

    public GameObject interactable;

    public GameObject warningLight;

    public int controlPanelId;

    public int lifeTimeId;

    public InteractableType interactableType;

    public bool done;

    /* 1 - 3 */
    [ReadOnly]
    public WarningLevel warningLevel;

    public Objective(int cPId, GameObject intact, InteractableType intactType, GameObject wLight) {

        interactable = intact;
        interactableType = intactType;
        warningLight = wLight;
        controlPanelId = cPId;
        done = false;
        lifeTimeId = -1;

    }
    public Objective(int cPId, GameObject intact) {

        interactable = intact;
        controlPanelId = cPId;
        done = false;

    }

    public Objective() {

    }
}

[System.Serializable]
public class ControlPanel {
    public int id;
    public int numberOfObjectives;

    public ControlPanel(int cPID) {
        id = cPID;
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
    /*-----------*/
    /// <summary>
    /// How many objectives is done total
    /// </summary>
    public int objectivesDone;

    /// <summary>
    /// Required amount of done objectives for reactor room objectives
    /// </summary>
    public int ObjectivesForReactor;

    /// <summary>
    /// How many objective spaces are left
    /// </summary>
    [ReadOnly]
    public int spacesLeft;

    /// <summary>
    /// Is the reactor room open
    /// </summary>
    public bool isReactorRoomOpen;

    /// <summary>
    /// When user is interacting with correct objectives interactable interactable
    /// </summary>
    public bool completingObjective;

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
    /// Panel id that player is interacting
    /// </summary>
    public int currentInteractingPanel;

    /// <summary>
    /// objectives that are currently activated
    /// </summary>
    public List<Objective> objectiveList = new List<Objective>();
    /// <summary>
    /// All possible objectives that are on the all control panels
    /// </summary>
    public List<Objective> allObjectivesList = new List<Objective>();
    /// <summary>
    /// List of control panels with id and number of interactables
    /// </summary>
    public List<ControlPanel> controlPanelList = new List<ControlPanel>();
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

    public GameObject speaker;

    public float warningLightRange;

    public float warningLightIntensity;

    void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    void Start() {
        if (CheckForMissing()) return;

        //CreateObjectives___OLD();
        //StartCoroutine(StartCrumbling());

    }

    void Update() {
        //NextObjectiveSet();

        if (objcomp) {
            CompleteReactorShutDown();
        }
    }

    /// <summary>
    /// Creates objective set
    /// </summary>
    private void CreateObjectives() {

        if (heatManager.currentHeatLevel == 0) howManyObjectives = 1;
        else {

            howManyObjectives = heatManager.currentHeatLevel;

            /* If amount of objectives to be created is bigger than max allowed objectives */
            if (howManyObjectives > stats.maxObjectives) {
                howManyObjectives = stats.maxObjectives;
            }
        }

        /* If howManyObjectives is greater than all objectives count or smaller than -1 then set howManyObjectives to allObjectives count */
        if (howManyObjectives >= allObjectivesList.Count || howManyObjectives <= -1) {
            howManyObjectives = allObjectivesList.Count;
        }        

        /* If palyer hasn't interacted with any panel don't use control panels numberOfObjectives  */
        if (currentInteractingPanel == -1) spacesLeft = allObjectivesList.Count - objectiveList.Count;
        else spacesLeft = allObjectivesList.Count 
                - (objectiveList.Count 
                + controlPanelList.Find(cp => cp.id == currentInteractingPanel).numberOfObjectives);

        /* If number of objectives to be created is bigger than available spaces on control panels set howManyObjectives to spacesLeft */
        if (howManyObjectives > spacesLeft) howManyObjectives = spacesLeft;
        
        int createdObjectives = 0;

        while (createdObjectives != howManyObjectives) {

            int randomIndex = random.Next(0, allObjectivesList.Count);

            while (objectiveList.Contains(allObjectivesList[randomIndex]) || allObjectivesList[randomIndex].controlPanelId == currentInteractingPanel) {

                randomIndex = random.Next(0, allObjectivesList.Count);
            }

            objectiveList.Add(allObjectivesList[randomIndex]);

            /* Current objective id on the objectiveList */
            int currentId = objectiveList.Count - 1;

            EnableWarningLight(objectiveList[currentId], objectiveList[currentId].warningLevel);

            createdObjectives++;

            if (debugMode) {

                print("activate");
            }

            objectiveLifeTimes.Add(StartCoroutine(DisableObjective(objectiveList[currentId],
                objectiveList[currentId].warningLevel, stats.objectiveLifeTime, lifeTimeId)));

            objectiveList[currentId].lifeTimeId = lifeTimeId;
            lifeTimeId++;

            StartCoroutine(SetObjectiveInfo(objectiveList[currentId]));
        }
        /* Start new objective creation sequnce when reactor is on */
        if (reactorRoomController.isReactorOn) StartCoroutine(WaitNextSet());
    }

    /// <summary>
    /// Removes given objective from objectiveList
    /// </summary>
    /// <param name="objective"></param>
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

    /// <summary>
    /// Creates the allObjectivesList
    /// And sets how many interactable are on specifig control panel
    /// </summary>
    /// <param name="objective"></param>
    public void PopulateList(Objective objective) {
        InitObjective(objective);
        /* Gets objectives control panel and increases number of interactables on that panel  */
        controlPanelList.Find(cp => cp.id == objective.controlPanelId).numberOfObjectives++;
        allObjectivesList.Add(objective);
    }

    /// <summary>
    /// Sets objective warning light
    /// </summary>
    /// <param name="objective"></param>
    private void InitObjective(Objective objective) {
        switch (objective.interactableType) {
            case InteractableType.None:
                Debug.LogError(string.Format("{0} does not have interactable type selected", objective.interactable.name));
                break;
            case InteractableType.Button:
                objective.warningLevel = stats.warningLevels[2];
                break;
            case InteractableType.Lever:
                objective.warningLevel = stats.warningLevels[1];
                break;
            case InteractableType.Valve:
                objective.warningLevel = stats.warningLevels[0];
                break;
        }
    }

    /// <summary>
    /// Adds control panels to control panel list
    /// </summary>
    /// <param name="controlPanel"></param>
    public void InitControlPanel(ControlPanel controlPanel) {
        controlPanelList.Add(controlPanel);
    }

    /// <summary>
    /// Creates new objective set
    /// </summary>
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

        CreateObjectives();        
        if (nextSet) {
            if (debugMode) print("too long");
            nextSet = false;
        }
    }

    public void CompletingObjective(bool isCompleting) {
        completingObjective = isCompleting;
    }

    /// <summary>
    /// When player is interacting with correct objectives interactable
    ///     -heat is rediced amount of based on warning light color
    ///     -stop life time of the objective
    ///     -objectives warning light is set to green
    ///     -start disable objective sequence
    ///     -increase done objectives by 1
    ///     -set panel where objective is to current interacting panel
    ///     -and check if there are enough objective done to open reactor room
    /// </summary>
    /// <param name="objective"></param>
    public void CompleteObjective(Objective objective) {

        /* Objective that player is acting with is on objective list*/
        if (objectiveList.Contains(objective)) {            
            if (debugMode) print("objective DONE!!!");
            objcomp = false;
            heatManager.ActiveChangeHeating(objective, stats.changeHeatingFor, false);
            StopCoroutine(objectiveLifeTimes[objective.lifeTimeId]);
           
            /* Sets warning light to green */
            objective.warningLevel = stats.warningLevels[3];

            StartCoroutine(DisableObjective(objective, objective.warningLevel, 0, objective.lifeTimeId));
            objectivesDone++;
            currentInteractingPanel = objective.controlPanelId;

            if (CheckForReactorRoomOpening() && !isReactorRoomOpen) {
                reactorRoomController.OpenReactorRoomDoors();
            }            
        }
    }

    /// <summary>
    /// (OPTIONAL) If player is interacting with objective that has no warning light
    ///     -Increase heat amount of failure objective
    /// </summary>
    /// <param name="objective"></param>
    public void FailureObjective(Objective objective) {
        heatManager.ActiveChangeHeating(objective, stats.changeHeatingFor, true);
    }

    /// <summary>
    /// When player is interacting with reactor lever
    /// </summary>
    public void CompleteReactorShutDown() {
        reactorRoomController.ShutDownTheReactor();
        StopCoroutine(WaitNextSet());
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

    /// <summary>
    /// DO NOT USE !!!
    /// Old way to create objectives was in certain sequence
    /// </summary>
    private void CreateObjectivesInSequence() {
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
        if (howManyObjectives >= allObjectivesList.Count || howManyObjectives <= -1) {
            howManyObjectives = allObjectivesList.Count;
        }

        int createdObjectives = 0;
        while (createdObjectives != howManyObjectives && !tooManyObjectives) {

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

            //objectiveList[currentId].warningLevel = currentWarningLevel;
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

    /// <summary>
    /// ------------[ Use this to start the whole objective system ]------------
    /// __________________________________________________________________________________
    /// | Function first waits the firstObjectiveSet time
    /// | Then it starts heat rising on HeatManager
    /// | Also it starts the timer if needed
    /// | And finally it calls createObjectives function
    /// |_________________________________________________________________________________
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartCrumbling() {

        ParticleManager.instance.PlayParticle(ParticleManager.instance.Spark, ParticleSystemStopAction.None, ParticleManager.instance.SparkPosition[0], Vector3.up);
        yield return new WaitForSeconds(0.5f);

        LightManager.instance.DisableFirstControlPanelLight();
        yield return new WaitForSeconds(0.5f);


        LightManager.instance.ActivateControlPanelLights();
        yield return new WaitForSeconds(1f);

        LightManager.instance.ActivateFactoryLight();
        yield return new WaitForSeconds(stats.waitBeforeFirstSet);

        for (int i = 0; i < 3; i++) {

            SoundManager.instance.audioSourceToPlay(speaker, 0, false, 0.5f, null, speaker.transform.position);
        }

        LightManager.instance.ActivateReactorLights(reactorRoomController.isReactorOn);
        HeatManager.instance.StartHeatIncreace();
        if (stats.useTimer) TimeManager.instance.StartTimer();
        CreateObjectives();
    }
    
    /// <summary>
    /// Calls StartCrumbling
    /// </summary>
    public void ActivateCrumbling() {
        StartCoroutine(StartCrumbling());
    }

    /// <summary>
    /// Waits waitForNextSet before creating new objective set
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitNextSet() {
        if (debugMode) print("Wait next set");

        yield return new WaitForSeconds(stats.waitBeforeNextSet);       
        NextObjectiveSet();       
    }

    /// <summary>
    /// Enables the objectives warning light
    /// </summary>
    /// <param name="objective"></param>
    /// <param name="warningLvl"></param>
    private void EnableWarningLight(Objective objective, WarningLevel warningLvl) {

        material = Instantiate(objective.warningLight.GetComponentInChildren<MeshRenderer>().material);
        objective.warningLight.GetComponentInChildren<MeshRenderer>().material = material;

        objective.warningLight.GetComponentInChildren<Light>().range = warningLightRange;
        objective.warningLight.GetComponentInChildren<Light>().intensity = warningLightIntensity;
        material.SetColor("_EmissionColor", warningLvl.color * 1);

        material.EnableKeyword("_EMISSION");
        material.SetColor("_Color", warningLvl.color);

        objective.warningLight.GetComponentInChildren<Light>().color = warningLvl.color;

        if (debugMode) {

            print("<color=blue>Light enabled</color> warnLvl: " + warningLvl.level);
        }
    }

    /// <summary>
    /// Disables objectives warning light
    /// </summary>
    /// <param name="objective"></param>
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
        float time = 0;
        int lightOn = 1;

        if (duration == 0) {

            EnableWarningLight(objective, currentWarningLvl);
            yield return new WaitForSeconds(0.2f);
        }
        else {

            while (time < duration) {

                if (!completingObjective) time += Time.deltaTime;

                lightOn = (int)stats.alarmLigthBlinkIntensity.Evaluate(time / duration);

                switch (lightOn) {

                    case 1:
                        EnableWarningLight(objective, currentWarningLvl);
                        break;
                    case 0:
                        DisableWarningLight(objective);
                        break;
                }
             
                yield return null;
            }
            if (stats.riseHeatAtFailure) FailureObjective(objective);
        }

        RemoveObjective(objective);
        DisableWarningLight(objective);        

        if (debugMode) {

            print("<color=blue>time over</color>");
            print("<color=yellow>time: </color>" + time);
        }
        /* Sets objectives default values */
        InitObjective(objective);
    }

    /// <summary>
    /// Gives objective basic information
    /// </summary>
    /// <param name="objective"></param>
    /// <returns></returns>
    private IEnumerator SetObjectiveInfo(Objective objective) {
        if (debugMode) print(objective.interactable);
        objective.interactable.GetComponentInParent<InteractableInfo>().objectiveInfo = objective;
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
