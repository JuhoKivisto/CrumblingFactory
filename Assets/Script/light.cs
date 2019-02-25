using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class light : MonoBehaviour
{

    public float speed;
    private Color firstcolor= Color.blue;
    private Color firstendcolor= Color.blue;
    private Color midcolor = Color.green;
    private Color midendcolor= Color.magenta;
    private Color endcolor = Color.red;
    private Color endlastcolor = Color.grey;
    public Slider slidervalue;
    public AnimationCurve heatcurve;
    float starttime = 1f;
    
    private void Start()
    {
      starttime = Time.time;
        slidervalue.onValueChanged.AddListener(delegate { change(); });
       
     

    }

    public void  change()
    {
        
        if (slidervalue.value > 0 && slidervalue.value <= 40 )
        {
            float t = Time.time * speed;
        
      

            GetComponent<Light>().color = Color.Lerp(firstcolor, firstendcolor, t);
          
           
           
        

        }
        else  if (slidervalue.value > 40 && slidervalue.value <= 80)
        {
            Debug.Log(slidervalue.value);
            float t = Time.time * speed;
            GetComponent<Light>().color = Color.Lerp(midcolor, midendcolor, t);
           
        
        }
         else if (slidervalue.value > 80 && slidervalue.value <= 100)
        {
            float t = (Time.time - starttime) * speed;
           
            GetComponent<Light>().color = Color.Lerp(endcolor, endlastcolor, t);
        }


       


    }


   
    
}
