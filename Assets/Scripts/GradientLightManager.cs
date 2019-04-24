using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientLightManager : MonoBehaviour {
    /// <summary>
    /// This scipt changes light color according to heat so that it is more red when heat increases 
    /// </summary>
    /// 
    public static GradientLightManager instance;

    [SerializeField]
    private Gradient grad;

    [SerializeField]
    AnimationCurve intensity;

    [SerializeField]
    List<Light> lights;

    public float Heat;
    public float minHeat;
    public float maxHeat;
    public float heatNormalizeNumber;


    private void Start() {
        instance = this;
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.S)) {

            GradientLightManager.instance.callCoroutine();
            Debug.Log(heatNormalizeNumber);
        }
        heatNormalizeNumber = (Heat - minHeat) / (maxHeat - minHeat);
    }

    public void callCoroutine() {
        StartCoroutine(gradientLight());
    }

    IEnumerator gradientLight() {
        while (true) {

            foreach (Light light in lights) {

                if (light != null) {

                    light.color = grad.Evaluate(heatNormalizeNumber);
                    light.intensity = intensity.Evaluate(heatNormalizeNumber * intensity.keys[intensity.length - 1].value);//intensity.keys[intensity.length - 1].value - to get the biggest value of intensity
                }
            }
            yield return null;
        }
        
    }
}
