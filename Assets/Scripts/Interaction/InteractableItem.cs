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


    private void Start() {
        Debug.Log(new Vector3(-0.77f, -0.767f, -2.145f) - new Vector3(-0.361f, -1.09f, 2.145f));
    }


}