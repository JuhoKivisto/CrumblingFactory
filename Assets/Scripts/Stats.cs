using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentalEvent {

    public GameObject eventObject;
    public int heatOfTheEvent;
    public bool happened;

}

public class Stats : MonoBehaviour {

    [Header("----------HEAT-----------")]
    //[Range(50,100)]
    public EnvironmentalEvent[] criticalTemporatures;
    public int timeDivider;
    public float heatIncreaser;
    public float[] heatLevels;
    public float decreaseHeatOnObjectiveSetComplete;
    public float decreaseHeatOnObjectiveComplete;
    public float waitTimeOnObjectiveComplete;
    public float waitTimeOnObjectiveSetComplete;

    public bool decreaseHeat;
    public bool waitTime;


    [Space]

    [Header("----------TIME----------")]
    public float gameLenght;
    public AnimationCurve heatCurve;
    public AnimationCurve heatCurve2;
    [Tooltip("Time events are created from random number between timeEventRandomLow + previous time event and timeEventRandomHigh + previous time event")]
    public int timeEventRandomLow;
    public int timeEventRandomHigh;

    [Space]

    [Header("----------ERROR----------")]
    [Range(2, 10)]
    public int[] errorThresholds;

    [Space]

    [Header("----------OBJECTTIVE----------")]
    public int maxObjectives;

}
