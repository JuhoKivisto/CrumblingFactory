using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class light : MonoBehaviour
{

    public float speed;
    public Color startcolor;
    public Color endcolor;

    float starttime;
    private void Start()
    {
        starttime = Time.time;
    }

    private void Update()
    {
        float t = (Time.time - starttime) * speed;
        GetComponent<Light>().color = Color.Lerp(startcolor, endcolor, t);


    }
}
