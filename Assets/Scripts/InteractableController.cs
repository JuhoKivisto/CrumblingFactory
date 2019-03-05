using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InteractableController : MonoBehaviour {

    public enum InteractableType {None, Button, Lever, Valve};

    public InteractableType interactableType;

    private Controller handController;

  


    #region Button
    public GameObject button;
    public GameObject triggerDown;
    public GameObject triggerUp;
    public GameObject enableCollider;
    public GameObject disableCollider;

    public bool active;
    public bool isEnabled;

    [Range(2f, 0f)]
    public float buttonHapticPulseInterval;
    public float buttonPulseTime;
    #endregion

    #region Lever
    public GameObject hand;

    public float SpringBreakDistance;

    public SpringJoint spring;

    [Range(2f, 0f)]
    public float leverHapticPulseInterval;

    public bool interacting;

    public float pulseIncreaser;

    public HingeJoint hinge;

    public float startAngle;

    public float endAngle;

    public float previousDAngle;

    #endregion

    public string tag;

    // Use this for initialization
    void Start () {

        switch (interactableType) {
            case InteractableType.None:
                break;
            case InteractableType.Button:
                break;
            case InteractableType.Lever:
        hinge = GetComponent<HingeJoint>();
        startAngle = hinge.limits.min;
        endAngle = hinge.limits.max;
                break;
            case InteractableType.Valve:
                break;
            default:
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "hand" && !interacting)
        {
            //GetComponent<Collider>().enabled = false;
            hand = other.gameObject;
            handController = hand.GetComponent<Controller>();

        }

        switch (interactableType) {
            case InteractableType.None:
                print(string.Format("<color=orange>No interactable type selected on {0}</color>",gameObject));
                break;
            case InteractableType.Button:

                if (active && other == triggerDown.GetComponent<Collider>() && other.tag == tag) {
                    print("Button down");
                    button.GetComponent<Rigidbody>().AddForce(transform.up * 20);
                    active = false;
                    StartCoroutine(OnButtonInteraction());
                    ObjectiveManager.instance.CompleteObjective(button.GetComponentInParent<InteractableTest>().objectiveInfo);
                }
                if (!active && other == triggerUp.GetComponent<Collider>() && other.tag == tag) {
                    active = true;
                    print("Button up");
                }
                if (other.tag == InteractableManager.instance.handTag && active) {
                    print("Add force");
                    button.GetComponent<Rigidbody>().AddForce(-transform.up * 50);
                    //active = false;
                }

                break;
            case InteractableType.Lever:

                //print("switch");
               

                if (other.tag == "hand" && !interacting) {
                    //GetComponent<Collider>().enabled = false;
                    hand = other.gameObject;
                    if (hand.GetComponent<SteamVR_TrackedController>().triggerPressed)
                    {

                    print("if " + other.gameObject);
                    handController = hand.GetComponent<Controller>();
                    StartCoroutine(OnLeverInteraction());
                    }

                   
                }                

                break;
            case InteractableType.Valve:
                break;
            default:
                break;
        }
    }

    public IEnumerator OnLeverInteraction() {

        interacting = true;

        print("lever interaction");

        spring = hand.GetComponent<SpringJoint>();

            if (spring.connectedBody == null)
            {
                spring.connectedBody = GetComponent<Rigidbody>();

            }
            else
        {
            yield break;
        }

        print("Trigger pressed" + hand.GetComponent<SteamVR_TrackedController>().triggerPressed);

        float timer = 0;
        float startTime = 0;

        while (hand.GetComponent<SteamVR_TrackedController>().triggerPressed) {

            //Debug.DrawRay(transform.TransformPoint(spring.anchor), transform.TransformPoint(spring.connectedAnchor), Color.red);
            //Debug.DrawRay(spring.anchor, spring.connectedAnchor, Color.red);
            Debug.DrawRay(spring.transform.TransformPoint(spring.anchor), Vector3.up, Color.red);
            Debug.DrawRay(spring.transform.TransformPoint(spring.connectedAnchor), Vector3.up, Color.red);

            spring.anchor = transform.InverseTransformPoint(hand.GetComponent<SteamVR_TrackedObject>().transform.position);

            print(Mathf.Abs( Mathf.DeltaAngle(hinge.angle, startAngle)));

            if (Mathf.Abs(Mathf.DeltaAngle(hinge.angle, startAngle)) > pulseIncreaser) {
                print("-- interval");
                leverHapticPulseInterval -= 0.01f;
                previousDAngle = Mathf.Abs(Mathf.DeltaAngle(hinge.angle, startAngle));
                startAngle = hinge.angle;
            }

            //else if (Mathf.Abs(Mathf.DeltaAngle(hinge.angle, startAngle)) > previousDAngle)
            //{
            //    print("++ interval");
            //    leverHapticPulseInterval += 0.01f;
            //    startAngle = hinge.angle;
            //}
            else
            {

            }

            if (timer - startTime > leverHapticPulseInterval) {
                handController.EnableHapticFeedBack();
                startTime = timer;

                //print("Haptic start " + startTime + " current " + timer);
            }

            //Debug.DrawRay(transform.TransformPoint(hand.GetComponent<SphereCollider>().center), transform.TransformPoint(GetComponent<CapsuleCollider>().center), Color.red);
            //print(Vector3.Distance(hand.transform.position, transform.TransformPoint(GetComponent<CapsuleCollider>().center)));


            //if (Vector3.Distance(spring.anchor, spring.connectedAnchor) > SpringBreakDistance) {
            //    print("break joint");
            //    DetachSpringJoint();
            //    yield break;
            //}
            
           

        timer += Time.deltaTime;
        //print("time " + timer);
            
            yield return null;

        }

        DetachSpringJoint();

        
    }

    public IEnumerator OnButtonInteraction() {
        interacting = true;


        float timer = 0;
        float startTime = 0;
        while (timer < buttonPulseTime) {
            print("Button pulse");


            if (timer - startTime > buttonHapticPulseInterval) {
                handController.EnableHapticFeedBack();
                startTime = timer;

                //print("Haptic start " + startTime + " current " + timer);
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void DetachSpringJoint() {
        handController.EnableHapticFeedBack(4f);
        hand.GetComponent<SpringJoint>().connectedBody = null;
        //GetComponent<Collider>().enabled = true;
        print("Enabled col");
        interacting = false;
        

    }
}
