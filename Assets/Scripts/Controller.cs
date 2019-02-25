using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public int controllerId;

    public void InitController() {
        controllerId = (int) GetComponent<SteamVR_TrackedController>().controllerIndex;
    }

    public void EnableHapticFeedBack() {
        SteamVR_Controller.Input(controllerId).TriggerHapticPulse(2000);
    }
}
