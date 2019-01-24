using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {


    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }        //get hand sets
    private SteamVR_TrackedObject trackedObj;

    private InteractableItem interactingItem;

    private GameObject handlingObject;




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

        if (controller.GetPress(gripButton) && handlingObject != null && 
                            interactingItem.typeOfObject == InteractableItem.ObjectType.Lever) {

            Debug.Log(interactingItem.Distance);
            Transform parentOfLever = handlingObject.transform.parent.gameObject.transform;
            GameObject lever = parentOfLever.GetChild(1).gameObject;                //get the lever mesh render

            Vector3 temp = (this.GetComponent<Transform>().position - parentOfLever.GetComponent<Transform>().position);

            Vector3 newLocalPosition = new Vector3(handlingObject.GetComponent<Transform>().localPosition.x, temp.y / parentOfLever.localScale.y, handlingObject.GetComponent<Transform>().localPosition.z);

            handlingObject.GetComponent<Transform>().localPosition = newLocalPosition;

            lever.transform.LookAt(handlingObject.transform);
            interactingItem.LeverUp = true;
        }

        if (controller.GetPressDown(gripButton) && handlingObject != null 
                        && interactingItem.typeOfObject == InteractableItem.ObjectType.PickableObject) {

            handlingObject.transform.parent = this.transform;
            handlingObject.GetComponent<Rigidbody>().isKinematic = true;

            interactingItem.PickingUp = true;
        }

        if (controller.GetPressUp(gripButton) && handlingObject != null
                    && interactingItem.typeOfObject == InteractableItem.ObjectType.PickableObject) {
            handlingObject.transform.parent = null;
            handlingObject.GetComponent<Rigidbody>().isKinematic = false;

            interactingItem.PickingUp = false;
        }
        
    }

    private void OnTriggerEnter(Collider collider) {                                            //when controller collides with object

        InteractableItem collidedItem = collider.GetComponent<InteractableItem>();

        handlingObject = collider.gameObject;
        interactingItem = collidedItem;



        if (collidedItem.typeOfObject == InteractableItem.ObjectType.Button) {

            GameObject cube = collider.gameObject.transform.GetChild(0).gameObject;

            
            Vector3 buttonLocalPosition = collider.gameObject.transform.GetChild(0).gameObject.GetComponent<Transform>().localPosition;
            buttonLocalPosition = new Vector3(buttonLocalPosition.x, -0.2f, buttonLocalPosition.z);

            cube.GetComponent<Transform>().localPosition = buttonLocalPosition;

            collidedItem.ButtonPressed = true;

        }
    }

    private void OnTriggerExit(Collider collider) {                                             //when controller exits object                    

        InteractableItem colliedItem = collider.GetComponent<InteractableItem>();

        interactingItem = null;
        handlingObject = null;

        if (colliedItem.typeOfObject == InteractableItem.ObjectType.Button) {

            GameObject cube = collider.gameObject.transform.GetChild(0).gameObject;
            Vector3 buttonLocalPosition = collider.gameObject.transform.GetChild(0).gameObject.GetComponent<Transform>().localPosition;
            buttonLocalPosition = Vector3.zero;
            cube.GetComponent<Transform>().localPosition = buttonLocalPosition;

            colliedItem.ButtonPressed = false;

        }

    }

}
