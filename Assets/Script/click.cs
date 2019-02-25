using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class click : MonoBehaviour {
    public AudioClip help;
    /*  public GameObject light1;
      public GameObject light2;
      public GameObject light3;
      public GameObject npcactive;
       bool toggle = false;*/

   
   // public GameObject definedButton;
    public UnityEvent OnClick = new UnityEvent();

    // Use this for initialization
    void Start()
    {
        //definedButton = this.gameObject;

      
    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;





        if (Input.GetMouseButtonDown(0))
        {

          
            
            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
            {
                
                this.gameObject.AddComponent<AudioSource>();
                this.GetComponent<AudioSource>().clip = help;
                this.GetComponent<AudioSource>().Play();




             
               /* toggle = !toggle;
                this.npcactive.SetActive(toggle);*/


                OnClick.Invoke();
                //  colorlerp = Color.Lerp(Color.red, Color.yellow, Mathf.PingPong(Time.time, 1));


            }
        }
    }

    
    public void stopaudio()
    {
        this.gameObject.AddComponent<AudioSource>();
       this.GetComponent<AudioSource>().clip = help;
        this.GetComponent<AudioSource>().Stop();

    }

    public void OnTriggerEnter(Collider other)
    {
        this.gameObject.AddComponent<AudioSource>();
        this.GetComponent<AudioSource>().clip = help;
        this.GetComponent<AudioSource>().Play();
    }
}
