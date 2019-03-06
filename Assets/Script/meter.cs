using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class meter : MonoBehaviour {

    static float maxangle = 220f;
    static float minangle =-48f;
    static float defaultangle = 40f;
    static meter thisheatmeter;
    
  


    float starttime = 1f;
    public float ang = -39f;

    // Use this for initialization
    void Start () {
        thisheatmeter = this;
        starttime = Time.time;

     
   
    }
   

    public  static void Heatmeter (float defaultangle , float min , float max)
    {

        float ang = Mathf.Lerp(minangle, maxangle, Mathf.InverseLerp(min, max, defaultangle));
        Debug.Log("ang:"+ ang);
        thisheatmeter.transform.eulerAngles = new Vector3(0,0,ang);
        if (ang >=-48 && ang <= 45)
        {
            float t = Time.time;
            thisheatmeter.GetComponent<Light>().color = Color.Lerp(Color.green, Color.yellow, t);
        }
        else if(ang >= 51 && ang <= 150)
        {
            float t1 = Time.time;
            thisheatmeter.GetComponent<Light>().color = Color.Lerp(Color.yellow, Color.red, t1);

        }
        else if(ang>= 151 && ang<= 216)
        {
            float t1 = Time.time;
            thisheatmeter.GetComponent<Light>().color = Color.Lerp(Color.red, Color.black, t1);
          


        }





    }
   



}
