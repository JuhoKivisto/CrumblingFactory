using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InteractableController : MonoBehaviour {

    public enum InteractableType {None, Button, Lever, Valve};

    public InteractableType interactableType;
    
    #region Button
    public GameObject button;
    public GameObject triggerDown;
    public GameObject triggerUp;
    public GameObject enableCollider;
    public GameObject disableCollider;

    public bool active;
    public bool isEnabled;
    #endregion

    #region Lever

    #endregion

    public string tag;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {

        switch (interactableType) {
            case InteractableType.None:
                print(string.Format("<color=orange>No interactable type selected on {0}</color>",gameObject));
                break;
            case InteractableType.Button:

                if (active && other == triggerDown.GetComponent<Collider>() && other.tag == tag) {
                    print("Button down");
                    button.GetComponent<Rigidbody>().AddForce(transform.up * 20);
                    active = false;
                    ObjectiveManager.instance.CompleteObjective(button.GetComponentInParent<InteractableTest>().objectiveInfo);
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

                break;
            case InteractableType.Lever:
                //GetComponent<SpringJoint>().connectedBody = other.GetComponent<Rigidbody>();
                break;
            case InteractableType.Valve:
                break;
            default:
                break;
        }
    }
}
