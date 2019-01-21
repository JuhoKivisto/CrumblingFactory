using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {


    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }        //get hand sets
    private SteamVR_TrackedObject trackedObj;

    //HashSet<InteractableItem> objectsHoveringOver = new HashSet<InteractableItem>();

    //private InteractableItem closestItem;
    //private InteractableItem interactingItem;

    private GameObject lever;
    private GameObject pickUp;


    // Use this for initialization
    void Start() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();

    }

    // Update is called once per frame
    void Update() {
        if (controller == null) {
            Debug.Log("Controller not initialized");
            return;
        }

        if (controller.GetPress(gripButton) && lever != null) {
            lever.transform.LookAt(new Vector3(lever.transform.position.x, this.transform.position.y, this.transform.position.z));
        }

        if (controller.GetPressDown(gripButton) && pickUp != null) {

            pickUp.transform.parent = this.transform;
            pickUp.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (controller.GetPressUp(gripButton) && pickUp != null) {
            pickUp.transform.parent = null;
            pickUp.GetComponent<Rigidbody>().isKinematic = false;
        }
        
    }

    private void OnTriggerEnter(Collider collider) {                                            //when controller collides with object

        InteractableItem colliderItem = collider.GetComponent<InteractableItem>();

        if (collider.gameObject.tag == "Pickable") {
            pickUp = collider.gameObject;

            Debug.Log("Pickable");
        }
        if (collider.gameObject.tag == "Lever")
            lever = collider.gameObject;

        if (collider.gameObject.tag == "Button") {

            GameObject cube = collider.gameObject.transform.GetChild(0).gameObject;
            Vector3 buttonLocalPosition = collider.gameObject.transform.GetChild(0).gameObject.GetComponent<Transform>().localPosition;
            buttonLocalPosition = new Vector3(buttonLocalPosition.x, -0.2f, buttonLocalPosition.z);

            cube.GetComponent<Transform>().localPosition = buttonLocalPosition;
        }
    }

    private void OnTriggerExit(Collider collider) {                                             //when controller exits object                    
        //InteractableItem collidedItem = collider.GetComponent<InteractableItem>();
        //if (collidedItem) {
        //    objectsHoveringOver.Remove(collidedItem);
        //}

        if (collider.gameObject.tag == "Pickable") {
            pickUp = null;
        }
        if (collider.gameObject.tag == "Lever")
            lever = null;

        if (collider.gameObject.tag == "Button") {

            GameObject cube = collider.gameObject.transform.GetChild(0).gameObject;
            Vector3 buttonLocalPosition = collider.gameObject.transform.GetChild(0).gameObject.GetComponent<Transform>().localPosition;
            buttonLocalPosition = Vector3.zero;
            cube.GetComponent<Transform>().localPosition = buttonLocalPosition;
        }

    }

}
