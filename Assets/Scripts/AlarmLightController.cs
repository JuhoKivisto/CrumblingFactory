using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLightController : MonoBehaviour {

    public GameObject light;

    public Material alarmRed;
    public Material alarmGreen;
    public Material alarmNormal;

    public float range = 0.12f;
    public float intensity = 2.46f;

    // Use this for initialization
    void Start () {
        gameObject.GetComponent<Light>().intensity = 0f;
        gameObject.GetComponent<Light>().range = 0f;
        gameObject.GetComponent<Light>().color = Color.white;

        light.GetComponent<Renderer>().material = alarmNormal;
        

    }

    // Update is called once per frame
    void Update () {
		
	}
}
