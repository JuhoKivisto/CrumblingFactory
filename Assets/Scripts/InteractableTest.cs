using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTest : MonoBehaviour {

    public GameObject interactable;
    public GameObject alarmLigtht;
    public ControlPanel controlPanel;

    public Objective objectiveInfo;    

    // Use this for initialization
    void Start() {

        if (interactable == null) {
            Debug.LogError("Gameobject interactable has no reference, attach interactable");
            return;
        }
        if (alarmLigtht == null) {
            Debug.LogError("Gameobject alarmLigth has no reference, attach alarmLight");
            return;
        }
        if (controlPanel == null) {
            Debug.LogError("Script controlPanel has no reference, attach controlPanel script");
            return;
        }

        objectiveInfo = new Objective(controlPanel.id, interactable, alarmLigtht);
        ObjectiveManager.instance.PopulateList(objectiveInfo);

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && ObjectiveManager.instance.objectiveList[ObjectiveManager.instance.currentObjectiveId].interactable == gameObject) {
        }
    }
}
