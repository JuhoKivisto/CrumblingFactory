using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testLight : MonoBehaviour
{
    public float currentHeat = 40;

    private float maxHeat = 100;
    private float minHeat = 40;

    private Color newLightColor;
    private Light light;

    [SerializeField]
    private Gradient grad;

    [SerializeField]
    AnimationCurve intensity;

    public float heatNormalizeNumber;
    // Start is called before the first frame update
    void Start()
    {
        newLightColor = new Color();
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        heatNormalizeNumber = (currentHeat - minHeat) / (maxHeat - minHeat);

        //newLightColor = new Color(1, 1 - heatNormalizeNumber, 1 - heatNormalizeNumber);
        light.color = grad.Evaluate(heatNormalizeNumber);
        light.intensity = intensity.Evaluate(heatNormalizeNumber * 5);
    }
}
