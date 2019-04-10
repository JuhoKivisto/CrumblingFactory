using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientLight : MonoBehaviour
{    
    /// <summary>
    /// This scipt changes light color according to heat so that it is more red when heat increases 
    /// </summary>
    private Light light;

    [SerializeField]
    private Gradient grad;

    [SerializeField]
    AnimationCurve intensity;

    private float heatNormalizeNumber;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        heatNormalizeNumber = (HeatManager.instance.Heat - Stats.instance.minHeat) / (Stats.instance.maxHeat - Stats.instance.minHeat);

        light.color = grad.Evaluate(heatNormalizeNumber);
        light.intensity = intensity.Evaluate(heatNormalizeNumber * intensity.keys[intensity.length - 1].value);

    }
}
