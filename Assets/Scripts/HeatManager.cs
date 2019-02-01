using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatManager : MonoBehaviour {

    public Stats stats;

    public float heat;
    public int heatLevels;
    public int currentHeatLevel;
    public float heatLevelLenght;
    [Range(0.1f,10f)]
    public float heatMultiplier;
    public float errorMultiplier;

    private float maxHeat;
    
    // Use this for initialization
    void Start () {

        maxHeat = stats.maxHeat;
        CalculateHeatLevelLenght();
        StartCoroutine(IncreaseHeat());
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
    public void IncreaseHeatLevel () {

        if (heat > heatLevelLenght * currentHeatLevel) {
            currentHeatLevel++;
        }
    }

    /// <summary>
    /// Stops heating for amount of seconds
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public IEnumerator StopHeating(float seconds, float heatDec) {

        stats.timeDivider = (int)seconds;
        yield return new WaitForSeconds(seconds);
        stats.timeDivider = 1;
        heat -= heatDec;

    }
    
    /// <summary>
    /// Increases heat for each frame based on animation curve
    /// heat increase value is taken from curves y-axis and multiplied with time between previous and current frame and added additionally errors that player makes
    /// after that if heat is high enough heat level increases
    /// </summary>
    /// <returns></returns>
    private IEnumerator IncreaseHeat() {
        
        while (heat < maxHeat) {

            if (true) {

                heat += stats.heatCurve3.Evaluate(heat / maxHeat) * Time.deltaTime * heatMultiplier + (GameManager.instance.errorCount * errorMultiplier);
                yield return null;
                IncreaseHeatLevel();
            }
        }
    }
}
