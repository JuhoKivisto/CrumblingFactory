using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorRoomController : MonoBehaviour {

    public Stats stats;

    public bool isReactorOn;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OpenReactorRoomDoors() {

    }

    public void ShutDownTheReactor() {
        isReactorOn = true;
        HeatManager.instance.ActiveReactorShutdown(10f, 0.1f);

    }

}
