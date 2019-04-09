using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientLight : MonoBehaviour
{
    public float currentHeat = 40;

    private float maxHeat = 100;
    private float minHeat = 40;

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
        heatNormalizeNumber = (currentHeat - minHeat) / (maxHeat - minHeat);

        light.color = grad.Evaluate(heatNormalizeNumber);
        light.intensity = intensity.Evaluate(heatNormalizeNumber * intensity.keys[intensity.length - 1].value);

    }
}
