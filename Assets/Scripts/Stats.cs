using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    [Header("----------HEAT-----------")]
    [Range(50,100)]
    public int[] criticalTemporatures;

    [Space]

    [Header("----------TIME----------")]
    public float gameLenght;
    [Tooltip("Time events are created from random number between timeEventRandomLow + previous time event and timeEventRandomHigh + previous time event")]
    public int timeEventRandomLow;
    public int timeEventRandomHigh;

    [Space]

    [Header("----------ERRORS----------")]
    [Range(2, 10)]
    public int[] errorThresholds;
}
