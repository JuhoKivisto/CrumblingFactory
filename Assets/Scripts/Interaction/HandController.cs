using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {

    //private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    //private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    //private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }        //get hand sets
    //private SteamVR_TrackedObject trackedObj;

    //HashSet<InteractableItem> objectsHoveringOver = new HashSet<InteractableItem>();

    //private InteractableItem closestItem;
    //private InteractableItem interactingItem;

    //private GameObject test;

    //// Use this for initialization
    //void Start() {
    //    trackedObj = GetComponent<SteamVR_TrackedObject>();
    //    Debug.Log(objectsHoveringOver);
    //}

    //// Update is called once per frame
    //void Update() {
    //    if (controller == null) {
    //        Debug.Log("Controller not initialized");
    //        return;
    //    }

    //    if (controller.GetPressDown(gripButton) && test != null) {

    //        test.transform.parent = this.gameObject.transform;

    //        float minDistance = float.MaxValue;

    //        Debug.Log("min distance " + minDistance);

    //        float distance;
    //        foreach (InteractableItem item in objectsHoveringOver) {                        //get all interactable items
    //            distance = (item.transform.position - transform.position).sqrMagnitude;     //get distance between controller and object

    //            Debug.Log("distance " + distance);
    //            if (distance < minDistance) {
    //                minDistance = distance;
    //                closestItem = item;
    //            }
    //        }

    //        interactingItem = closestItem;

    //        if (interactingItem) {
    //            if (interactingItem.IsInteracting()) {
    //                interactingItem.EndInteraction(this);
    //            }

    //            interactingItem.BeginInteraction(this);
    //        }
    //    }

    //    if (controller.GetPressUp(gripButton) && interactingItem != null && test != null) {
    //        interactingItem.EndInteraction(this);

    //        test.transform.parent = null;
    //    }
    //}

    //private void OnTriggerEnter(Collider collider) {                                            //when controller collides with object
    //    InteractableItem collidedItem = collider.GetComponent<InteractableItem>();
    //    if (collidedItem) {
    //        objectsHoveringOver.Add(collidedItem);
    //    }

    //    test = collider.gameObject;
    //}

    //private void OnTriggerExit(Collider collider) {                                             //when controller exits object                    
    //    InteractableItem collidedItem = collider.GetComponent<InteractableItem>();
    //    if (collidedItem) {
    //        objectsHoveringOver.Remove(collidedItem);
    //    }

    //    test = null;
    //}



    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;


    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObject.index); } }
    private SteamVR_TrackedObject trackedObject;

    private GameObject pickUp;


    void Start() {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    }


    void Update() {
        if (controller == null) {
            Debug.Log("Controller not initialized");
            return;
        }


        if (controller.GetPressDown(gripButton) && pickUp != null) {

            pickUp.transform.position = this.transform.position;
            //pickUp.transform.parent = this.transform;
            //pickUp.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (controller.GetPressUp(gripButton) && pickUp != null) {
            //pickUp.transform.parent = null;
            //pickUp.GetComponent<Rigidbody>().isKinematic = false;
        }


    }

    private void OnTriggerEnter(Collider other) {
        pickUp = other.gameObject;
    }

    private void OnTriggerExit(Collider other) {
        pickUp = null;
    }
}
