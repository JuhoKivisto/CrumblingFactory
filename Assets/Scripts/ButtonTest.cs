using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour {
   
    public GameObject button;
    public GameObject triggerDown;
    public GameObject triggerUp;
    public GameObject enableCollider;
    public GameObject disableCollider;
    public bool active;
    public bool isEnabled;
    public string tag;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //CheckIfInsideOfBoundries();
	}
    private void OnTriggerEnter(Collider other) {

        //print(other.gameObject.name);

        if (active && other == triggerDown.GetComponent<Collider>() && other.tag == tag  ) {
            print("Button down");
            button.GetComponent<Rigidbody>().AddForce(transform.up * 20);
            active = false;
            ObjectiveManager.instance.CompleteObjective(button.GetComponentInParent<Interactable>().objectiveInfo);
        }
        if (!active && other == triggerUp.GetComponent<Collider>() && other.tag == tag) {
            active = true;
            print("Button up");
        }
        if (other.tag == InteractableManager.instance.handTag && active) {
            print("Add force");
            button.GetComponent<Rigidbody>().AddForce(-transform.up * 50);
            //active = false;
        }
        //if (other == enableCollider.GetComponent<Collider>()) {
        //    print("enable");
        //    //button.GetComponent<Collider>().enabled = true;
        //}
        //if (other == disableCollider.GetComponent<Collider>()) {
        //    print("disable");
        //    button.GetComponent<Collider>().enabled = false;
        //}


    }

    //private void CheckIfInsideOfBoundries() {
    //    if (hand.GetComponent<SphereCollider>().bounds.Intersects(enableCollider.GetComponent<BoxCollider>().bounds) && !isEnabled) {
    //        print("enable");
    //        isEnabled = true;
    //        button.GetComponent<Collider>().enabled = true;
    //    }
    //    if (hand.GetComponent<SphereCollider>().bounds.Intersects(disableCollider.GetComponent<BoxCollider>().bounds) && isEnabled) {
    //        print("disable");
    //        isEnabled = false;
    //        button.GetComponent<Collider>().enabled = false;
    //    }
    //}
    
}
