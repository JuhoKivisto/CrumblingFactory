using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientLightManager : MonoBehaviour
{
    /// <summary>
    /// This scipt changes light color according to heat so that it is more red when heat increases 
    /// </summary>
    /// 


    [SerializeField]
    private Gradient grad;

    [SerializeField]
    AnimationCurve intensity;

    [SerializeField]
    List<Light> lights;


    private float heatNormalizeNumber;


    // Update is called once per frame
    void Update()
    {
        heatNormalizeNumber = (HeatManager.instance.Heat - Stats.instance.minHeat) / (Stats.instance.maxHeat - Stats.instance.minHeat);

        foreach (Light light in lights) {

            if(light != null) {

                light.color = grad.Evaluate(heatNormalizeNumber);
                light.intensity = intensity.Evaluate(heatNormalizeNumber * intensity.keys[intensity.length - 1].value);
            }
            
        }       

    }
}
