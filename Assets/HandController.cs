using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {

    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObject.index); } }
    private SteamVR_TrackedObject trackedObject;

    private GameObject pickUp;


	void Start () {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if(controller == null) {
            Debug.Log("Controller not initialized");
            return;
        }


        if (controller.GetPressDown(gripButton) && pickUp != null) {
            pickUp.transform.parent = this.transform;
            pickUp.GetComponent<Rigidbody>().useGravity = false;
        }

        if (controller.GetPressUp(gripButton) && pickUp != null) {
            pickUp.transform.parent = null;
            pickUp.GetComponent<Rigidbody>().useGravity = true;
        }


    }

    private void OnTriggerEnter(Collider other) {
        pickUp = other.gameObject;
    }

    private void OnTriggerExit(Collider other) {
        pickUp = null;
    }
}
