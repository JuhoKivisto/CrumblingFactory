﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentalEvent {

    public GameObject eventObject;
    public int heatOfTheEvent;
    public bool happened;

}

[System.Serializable]
public class WarningLevel {

    public int level;

    public float heatChange;

    public Color color;

    public WarningLevel(int lvl, Color colour) {

        lvl = level;
        colour = color;

    }
}

public class Stats : MonoBehaviour {

    public static Stats instance = null;

    [Header("----------HEAT-----------")]
    #region old
    /*
    //[Range(50,100)]
    public EnvironmentalEvent[] criticalTemporatures;
    public int timeDivider;
    public float[] heatLevels;
    public float heatIncreaser;
    public float decreaseHeatOnObjectiveSetComplete;
    public float decreaseHeatOnObjectiveComplete;
    public float waitTimeOnObjectiveComplete;
    public float waitTimeOnObjectiveSetComplete;
    */
    #endregion
    [Space]
    public AnimationCurve heatCurve2;
    public float startHeat;
    public float startHeatMultiplier;
    public float maxHeat;
    [Range(0f,1f)]
    public float changeHeatingFor;
    public float HeatAfterShutdown;

    //public bool decreaseHeat;
    //public bool waitTime;



    [Space]

    [Header("----------TIME----------")]
    [Space]
    public bool useTimer;
    public float gameLenght;
    public AnimationCurve heatCurve;
    [Tooltip("Time events are created from random number between timeEventRandomLow + previous time event and timeEventRandomHigh + previous time event")]
    public int timeEventRandomLow;
    public int timeEventRandomHigh;

    [Space]

    [Header("----------ERROR----------")]
    [Space]
    [Range(2, 10)]
    public int[] errorThresholds;

    [Space]

    [Header("----------OBJECTTIVE----------")]
    [Space]

    public bool riseHeatAtFailure;

    [Range(0, 15)]
    public int maxObjectives;

    [Range(0f,20f)]
    public float objectiveLifeTime;
    [Range(0f, 20f)]
    public float waitBeforeFirstSet;
    [Range(0f, 20f)]
    public float waitBeforeNextSet;

    public float IncreaceAtFailure;
    
    public AnimationCurve alarmLigthBlinkIntensity;

    public List<WarningLevel> warningLevels;

    void Awake() {

        if (instance == null) {
            instance = this;
        }

        else if (instance != this) {
            Destroy(gameObject);
        }
    }
}
