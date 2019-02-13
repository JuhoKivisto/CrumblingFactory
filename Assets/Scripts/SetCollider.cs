using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCollider : MonoBehaviour {

    public GameObject button;
    public GameObject hand;
    public GameObject enableCollider;
    public GameObject disableCollider;
    public bool setActive;


    private void OnTriggerEntffer(Collision collision) {
        //if (hand.GetComponent<Collider>() == collider.GetComponent<Collider>() && setActive) {
        //    button.GetComponent<Collider>().enabled = setActive;
        //}
        print("collision");
        if (collision.collider == hand.GetComponent<Collider>() && setActive) {
            print("disable");
            setActive = false;
            button.GetComponent<Collider>().enabled = setActive;
        }
    }

    void OnTriggerStay(Collider other) {
        //print("collision");
        /*if (other == hand.GetComponent<Collider>() && !setActive && gameObject == enableCollider) {
            print("enable");
            setActive = true;
            button.GetComponent<Collider>().enabled = setActive;
        }
        if (other == hand.GetComponent<Collider>() && setActive && gameObject == disableCollider) {
            print("disable");
            setActive = false;
            button.GetComponent<Collider>().enabled = setActive;
        }*/
        //print(other.gameObject.name);
        if (other == hand.GetComponent<Collider>()) {
            button.GetComponent<Collider>().enabled = setActive;
        }
    }

    void OnTriggerExit(Collider other) {
        if (setActive == false && other == hand.GetComponent<Collider>()) {
            button.GetComponent<Collider>().enabled = setActive;
        }
    }
}
