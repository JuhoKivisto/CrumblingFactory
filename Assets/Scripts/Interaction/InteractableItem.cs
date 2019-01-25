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


}