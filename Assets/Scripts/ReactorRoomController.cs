using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorRoomController : MonoBehaviour {

    public Stats stats;

    public Animator doorAnimator;

    public bool isReactorOn;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OpenReactorRoomDoors() {
        doorAnimator.SetTrigger("Open");
    }

    public void ShutDownTheReactor() {
        isReactorOn = false;
        HeatManager.instance.ActiveReactorShutdown(40f, 0.1f);


    }

}
