using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testLight : MonoBehaviour
{
    public float currentHeat = 30;

    private float maxHeat = 100;
    private float minHeat = 30;

    private Color newLightColor;
    private Light light;

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

        newLightColor = new Color(1, 1 - heatNormalizeNumber, 1 - heatNormalizeNumber);
        light.color = newLightColor;
    }
}
