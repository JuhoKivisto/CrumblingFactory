using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTest : MonoBehaviour {

    public GameObject interactable;
    public GameObject alarmLigtht;

    // Use this for initialization
    void Start() {

        ObjectiveManager.instance.PopulateList(new Objective(GetComponentInParent<ControlPanel>().id, interactable, alarmLigtht));

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && ObjectiveManager.instance.objectiveList[ObjectiveManager.instance.currentObjectiveId].interactable == gameObject) {
        }
    }
}
