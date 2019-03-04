﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatManager : MonoBehaviour {

    //public Text heatText;

    public Slider HeatUI;

    public Stats stats;

    public float heat;
    public int heatLevels;
    public int currentHeatLevel;
    public float heatLevelLenght;
    [Range(-10f, 10f)]
    public float heatMultiplier;
    public float errorMultiplier;

    public bool working = false;

    private float maxHeat;

    // Use this for initialization
    void Start() {
        if (HeatUI == null) {
            Debug.LogWarning("Slider HeatUI has no reference, attach HeatUI Slider");
            //return;
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

        if (heat > heatLevelLenght * currentHeatLevel) {
            currentHeatLevel++;
        }
        if (heat < heatLevelLenght * currentHeatLevel - 1) {
            currentHeatLevel--;
        }
    }

    public void ActiveChangeHeating(Objective objective, float duration, bool increacing) {        
        StartCoroutine(ChangeHeat(objective, duration, increacing));
    }

    /// <summary>
    /// Stops heating for amount of seconds
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private IEnumerator ChangeHeat(Objective objective, float duration, bool increacing) {
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

        while (heat < maxHeat && heat > -1) {

            if (true) {
                heat += stats.heatCurve2.Evaluate(heat / maxHeat) * Time.deltaTime * heatMultiplier/* + (GameManager.instance.errorCount * errorMultiplier)*/;
                yield return null;
                //HeatUI.value = heat;
                ChangeHeatLevel();
            }
        }
    }
}
