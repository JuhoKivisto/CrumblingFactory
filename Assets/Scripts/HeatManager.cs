using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatManager : MonoBehaviour {

    public Stats stats;

    public int heatId;
    public float heat;

    private float maxHeat; 


    // Use this for initialization
    void Start () {

        maxHeat = stats.maxHeat;

        StartCoroutine(IncreaseHeat());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HeatLevelIncreaser() {

        if (heatId == 8) {
            heatId = 8;
        }

        if (heat > stats.heatLevels[heatId] && heat < stats.heatLevels[heatId + 1]) {
            heatId++;
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

    

    private IEnumerator IncreaseHeat() {
        
        while (heat < maxHeat) {

            if (true) {

            heat += stats.heatCurve2.Evaluate(heat / maxHeat) * Time.deltaTime;
            yield return null;
            }
        }
    }
}
