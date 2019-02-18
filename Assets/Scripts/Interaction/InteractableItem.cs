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

    public int count;

    private Vector3 firstPosition;

    private bool left;

    private bool right;

    private float flag;

    private void Start() {
        firstPosition = this.transform.localPosition;
        count = 0;
        flag = 0f;
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

        if(firstPosition.x - this.transform.position.x < 0.05f && firstPosition.x - this.transform.position.x > -0.05f) {
            if (right) {
                count++;
            }else if (left) {
                count--;
            }
        }
    }
}