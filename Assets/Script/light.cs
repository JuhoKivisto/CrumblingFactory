using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class light : MonoBehaviour
{

    public float speed;
    private Color firstcolor= Color.yellow;
    private Color firstendcolor= Color.blue;
    private Color midcolor = Color.gray;
    private Color midendcolor= Color.green;
    private Color endcolor = Color.red;
    private Color endlastcolor = Color.black;
    public Slider slidervalue;
    public AnimationCurve heatcurve;
    float starttime;
    
    private void Start()
    {
      starttime = Time.time;
        slidervalue.onValueChanged.AddListener(delegate { change(); });
       
     

    }

    public void  change()
    {
        
        if (slidervalue.value > 0 && slidervalue.value <= 40)
        {
            float t = Time.time * speed;
      

            GetComponent<Light>().color = Color.Lerp(firstcolor, firstendcolor, heatcurve.Evaluate(t));
           
           
        

        }
        else  if (slidervalue.value >= 40 && slidervalue.value <= 80)
        {
            Debug.Log(slidervalue.value);
            float t = Time.time * speed;
            GetComponent<Light>().color = Color.Lerp(midcolor, midendcolor, heatcurve.Evaluate(t));
        
        }
         else if (slidervalue.value >= 80 && slidervalue.value <= 100)
        {
            float t = (Time.time - starttime) * speed;
           
            GetComponent<Light>().color = Color.Lerp(endcolor, endlastcolor, heatcurve.Evaluate(t));
        }


       


    }


   
    
}
