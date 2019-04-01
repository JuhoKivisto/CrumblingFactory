﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractableType {
    none,
    button,
    lever,
    valve
}

public class Interactable : MonoBehaviour {

    public GameObject interactable;
    public GameObject warningLight;
    public ControlPanel controlPanel;

    public InteractableType interactableType;
    public Objective objectiveInfo;

    // Use this for initialization
    void Start() {        

        if (CheckForMissing()) return;


        objectiveInfo = new Objective(controlPanel.id, interactable, interactableType, warningLight);
        ObjectiveManager.instance.PopulateList(objectiveInfo);

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && ObjectiveManager.instance.objectiveList[ObjectiveManager.instance.currentObjectiveId].interactable == gameObject) {
        }
    }

    bool CheckForMissing() {

        bool missing = false;

        if (interactable == null) {
            Debug.LogError(string.Format("Gameobject '{0}' has no reference of GameObject 'interactable', attach 'Interactable' GameObject reference to it", transform.GetChild(0).name), gameObject);
            missing = true;
        }
        if (warningLight == null) {
            Debug.LogError(string.Format("Gameobject '{0}' has no reference of GameObject 'warningLight', attach 'WarningLight' GameObject reference to it", transform.GetChild(1).name), gameObject);
            missing = true;
        }
        if (controlPanel == null) {
            Debug.LogError(string.Format("Gameobject '{0}' has no reference of Script 'ControlPanel', attach 'Panel[{1}]' GameObject reference to it",
                gameObject.name, GetComponentInParent<ControlPanel>().id), gameObject);
            missing = true;
        }
        if (missing) {
            return missing;
        }
        else {
            return missing;
        }
    }
}
