using UnityEngine;
using System.Collections;

public class InteractableItem : MonoBehaviour {
    public enum ObjectType {
        Button,
        PickableObject,
        Lever,
        Valve
    };
    public ObjectType typeOfObject;

    public bool ButtonPressed;

    public bool PickingUp;

    public bool LeverUp;

    public Vector3 Distance;


    private void Start() {
        
        if(this.typeOfObject == ObjectType.Lever) {
            Distance = this.GetComponent<Transform>().localPosition - this.transform.parent.transform.GetChild(1).transform.localPosition;
        }

    }


}