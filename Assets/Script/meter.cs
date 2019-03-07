using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class meter : MonoBehaviour {

    static float maxangle = 220f;
    static float minangle =-48f;
    static float defaultangle = 40f;
    static meter thisheatmeter;
    

 static Gradient gard;
    public Gradient grad;
    GradientAlphaKey[] alphakey;
    GradientColorKey[] colorkey;
    



    
 

    // Use this for initialization
    void Start () {
        thisheatmeter = this;
    

     
   
    }

   
  
   

    public  static   void Heatmeter (float speed, float min, float max)
    {
       GradientColorKey[] colorKey;
           GradientAlphaKey[] alphaKey;
           gard = new Gradient();
           colorKey = new GradientColorKey[3];
           colorKey[0].color = Color.green;
           colorKey[0].time = 0.0f;
           colorKey[2].color = Color.yellow;
           colorKey[2].time = 0.5f;
           colorKey[1].color = Color.red;
           colorKey[1].time = 1.0f;
           alphaKey = new GradientAlphaKey[3];
           alphaKey[0].alpha = 1.0f;
           alphaKey[0].time = 0.0f;
           alphaKey[1].alpha = 0.0f;
           alphaKey[1].time = 1.0f;
           alphaKey[2].alpha = 0.5f;
           alphaKey[2].time = 0.5f;

           gard.SetKeys(colorKey, alphaKey);
        Debug.Log("time" + colorKey[2].time);



        float ang = Mathf.Lerp(minangle, maxangle, Mathf.InverseLerp(min, max, speed));
        Debug.Log("ang:"+ ang);
        thisheatmeter.transform.eulerAngles = new Vector3(0,0,ang);
        if (ang >=-44 && ang <= 47.845)
        {
            float t = Time.time ;
            thisheatmeter.GetComponent<Light>().color = gard.Evaluate(0f);
            //thisheatmeter.GetComponent<Light>().color = Color.Lerp(Color.green, Color.yellow, t);
        }
        else if(ang >= 48 && ang <= 129)
        {
            float t = Time.time;
          thisheatmeter.GetComponent<Light>().color = gard.Evaluate(0.5f);
            //thisheatmeter.GetComponent<Light>().color = Color.Lerp(Color.green, Color.yellow, t);
        }
        else if(ang>= 129 && ang<= 220)
        {
            float t = Time.time;
    thisheatmeter.GetComponent<Light>().color = gard.Evaluate(1f);
           // thisheatmeter.GetComponent<Light>().color = Color.Lerp(Color.green, Color.yellow, t);



        }





    }
   



}
