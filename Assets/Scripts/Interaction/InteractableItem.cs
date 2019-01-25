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

    public bool ButtonPressed;              //when button is press, return true

    public bool PickingUp;                  //when object is picked up, return tru

    public bool LeverUp;

    public Vector3 startLocalPosition;


    private void Start() {
        Debug.Log(Vector3.Angle(new Vector3(3,2,4), new Vector3(0,0,1)));

        if (this.typeOfObject == ObjectType.Lever) {

            startLocalPosition = this.transform.localPosition;
        }

    }

    private void Update() {
        if (this.typeOfObject == ObjectType.Lever) {

            if(this.transform.localPosition.y >= 1.2f) {            //if lever reach this position
                LeverUp = true;                                     //lever is up
            }
            else {
                LeverUp = false;
            }

        }
    }


}