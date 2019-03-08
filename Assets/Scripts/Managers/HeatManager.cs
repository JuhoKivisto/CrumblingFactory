using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatManager : MonoBehaviour {

    public static HeatManager instance = null;

    //public Text heatText;

    public Slider[] HeatUI;

    public Stats stats;
    [SerializeField]
    private float heat;
    /// <summary>
    /// Current heat
    /// </summary>
    public float Heat {
        get {
            return heat;
        }

        set {
            heat = value;
        }
    }
    public int heatLevels;
    public int currentHeatLevel;
    public float heatLevelLenght;
    [Range(-10f, 10f)]
    public float heatMultiplier;
    public float errorMultiplier;
    private float maxHeat;
    private float minHeat;

    public bool working = false;

    public ReactorRoomController reactorRoomController;


    void Awake() {

        if (instance == null) {
            instance = this;
        }

        else if (instance != this) {
            Destroy(gameObject);
        }

    }

    // Use this for initialization
    void Start() {
        if (HeatUI == null) {
            Debug.LogWarning("Slider HeatUI has no reference, attach HeatUI Slider");
            return;
        }

        maxHeat = stats.maxHeat;
        CalculateHeatLevelLenght();

    }

    /// <summary>
    /// Calculates lenght of the heat level
    /// </summary>
    public void CalculateHeatLevelLenght() {
        heatLevelLenght = maxHeat / (float)heatLevels;
    }
    /// <summary>
    /// Incrases heat level when heat is high enough
    /// </summary>
    public void ChangeHeatLevel() {

        if (Heat > heatLevelLenght * currentHeatLevel) {
            currentHeatLevel++;
        }
        if (Heat < heatLevelLenght * currentHeatLevel - 1) {
            currentHeatLevel--;
        }
    }

    public void ActiveChangeHeating(Objective objective, float duration, bool increacing) {        
        StartCoroutine(ChangeHeating(objective, duration, increacing));
    }
    public void ActiveReactorShutdown(float multiplier, float duration) {
        //StartCoroutine(ChangeHeating(multiplier, duration));
        heatMultiplier = -multiplier;
        minHeat = stats.HeatAfterShutdown;
        


    }

    /// <summary>
    /// Stops heating for amount of seconds
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private IEnumerator ChangeHeating(Objective objective, float duration, bool increacing) {
        float currentHeatMultiplier = heatMultiplier;
        print("heat");

        if (!increacing) {

            for (int i = 0; i < stats.warningLevels.Count; i++) {
                if (stats.warningLevels[i].level == objective.warningLevel) {
                    heatMultiplier = -stats.warningLevels[i].heatMultiplier;
                }
            }
        }
        
        else {
            heatMultiplier = stats.IncreaceAtFailure;
        }

        yield return new WaitForSeconds(duration);
        heatMultiplier = currentHeatMultiplier;
        
    }

    /// <summary>
    /// Used only on reactor shutdown
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator ChangeHeating(float multiplier, float duration) {
        float currentHeatMultiplier = heatMultiplier;
        heatMultiplier = multiplier;
        yield return new WaitForSeconds(duration);

        heatMultiplier = currentHeatMultiplier;
        working = false;
    }

    public void StartHeatIncreace() {
        print("Start Heating");
        StartCoroutine(IncreaseHeat());
    }

    /// <summary>
    /// Increases heat for each frame based on animation curve
    /// heat increase value is taken from curves y-axis and multiplied with time between previous and current frame and added additionally errors that player makes
    /// after that if heat is high enough heat level increases
    /// </summary>
    /// <returns></returns>
    private IEnumerator IncreaseHeat() {

        while (Heat < maxHeat && Heat > minHeat) {
            
                Heat += stats.heatCurve2.Evaluate(Heat / maxHeat) * Time.deltaTime * heatMultiplier/* + (GameManager.instance.errorCount * errorMultiplier)*/;
                yield return null;

            foreach (var item in HeatUI) {
                item.value = heat;

            }
                ChangeHeatLevel();            
        }
    }
}
