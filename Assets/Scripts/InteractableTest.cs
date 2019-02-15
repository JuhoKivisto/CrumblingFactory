using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTest : MonoBehaviour {

    public GameObject interactable;
    public GameObject alarmLigtht;

    public Objective objectiveInfo;

    // Use this for initialization
    void Start() {


        objectiveInfo = new Objective(GetComponentInParent<ControlPanel>().id, interactable, alarmLigtht);
        ObjectiveManager.instance.PopulateList(objectiveInfo);

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && ObjectiveManager.instance.objectiveList[ObjectiveManager.instance.currentObjectiveId].interactable == gameObject) {
        }
    }
}
