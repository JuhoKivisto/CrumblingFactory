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

    public float distance;


    private void Start() {
        
        if(this.typeOfObject == ObjectType.Lever) {

            GameObject cube = this.transform.parent.GetChild(1).gameObject;

            distance = Vector3.Distance(this.GetComponent<Transform>().localPosition, this.transform.parent.transform.GetChild(1).transform.localPosition);

            Debug.Log(cube.transform.localEulerAngles.x);
            Debug.Log(cube.transform.rotation.x);

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