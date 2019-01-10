using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour {

    public Rigidbody rigidbody;

    private bool currentlyInteracting;

    private HandController attachedHand;

    private Transform interactingPoint;
	// Use this for initialization
	void Start () {

        rigidbody = GetComponent<Rigidbody>();
        interactingPoint = new GameObject().transform;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BeginInteraction(HandController hand) {
        attachedHand = hand;
        interactingPoint.position = hand.transform.position;
        interactingPoint.rotation = hand.transform.rotation;
        interactingPoint.SetParent(transform, true);

        currentlyInteracting = true;
    }

    public void EndInteraction(HandController hand) {

    }
}
