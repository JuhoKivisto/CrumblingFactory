using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum InteractableType { None, Button, Lever, Valve }

[System.Serializable]
public enum LeverType {
    Normal,
    First,
    Reactor
}

public class InteractableController : MonoBehaviour {


    public enum LeverDirection {
        up,
        down
    }

   
    public InteractableType interactableType;

    private Controller handController;

    private InteractableActivator interactableActivator;

    public bool button;
    public bool lever;
    public bool valve;

    /// <summary>
    /// | Used only for button interactable |
    /// -------------------------------------
    /// </summary>
    #region Button
    [Header("Button")]
    [Space]
    /* Used to detect if buttons is pressed down */
    [HideUnless("button")]
    public GameObject triggerDown;

    /* Used to detect if buttons is up */
    [HideUnless("button")]
    public GameObject triggerUp;

    /* indicates if interactable is active and ready to interact */
    [HideUnless("button")]
    public bool active;


    /* Interval how often haptic feedback happens on controller */
    [HideUnless("button")]
    [Range(2f, 0f)]
    public float buttonHapticPulseInterval;

    /* Time how long the feedback last */
    [HideUnless("button")]
    public float buttonPulseTime;

    /* Force to push button up */
    [HideUnless("button")]
    public float buttonUpForce;

    /* Force to push button down */
    [HideUnless("button")]
    public float buttonDownForce;
    #endregion

    /// <summary>
    /// | Used only for lever interactable |
    /// -------------------------------------
    /// </summary>
    #region Lever


    [Header("Lever")]
    [Space]

    /* Spring joint anchor that is used when lever is down position */
    [HideUnless("lever")]
    public Vector3 upAnchor;

    /* Spring joint anchor that is used when lever is up position */
    [HideUnless("lever")]
    public Vector3 downAnchor;

    /* Direction that lever has to next push or pull */
    [HideUnless("lever")]
    public LeverDirection leverDirection;

    /* Indicates what lever type is */
    public LeverType leverType;

    /* [TODO!] Distance when spring breaks if player overs it */
    [HideUnless("lever")]
    public float SpringBreakDistance;

    /* Interval how often haptic feedback happens on controller */
    [HideUnless("lever")]
    [Range(2f, 0f)]
    public float leverHapticPulseInterval;

    /* Lever min angle */
    [HideUnless("lever")]
    public float startAngle;

    /* Lever max angle */
    [HideUnless("lever")]
    public float endAngle;    

    #endregion

    /// <summary>
    /// | Used only for valve interactable |
    /// -------------------------------------
    /// </summary>
    #region Valve
    [Header("Valve")]
    [Space]
        
    /* Spring joint  */
    [HideUnless("valve")]
    public Vector3 valveAnchor;
    #endregion

    [Header("Lever and valve")]
    [Space]
    [Range(0f, 10000f)]
    public float springForce;

    [Range(0f, 100f)]
    public float springDamper;

    public HingeJoint hinge;
    public bool interacting;
    public SpringJoint spring;
    public float pulseIncreaser;
    public GameObject hand;
    public string tag;

    [Range(0f, 0.5f)]
    public float pulseInterval;
    [Range(0f, 4f)]
    public float pulseStrengh;
    [Range(0f, 2f)]
    public float pulseTime;

    public GameObject interactable;

    // Use this for initialization
    void Start() {
        interactableActivator = GetComponent<InteractableActivator>();
        switch (interactableType) {
            case InteractableType.None:
                break;
            case InteractableType.Button:
                break;

            default:
                hinge = GetComponent<HingeJoint>();

                startAngle = hinge.limits.min;
                endAngle = hinge.limits.max;
                break;
        }
    }

    // Update is called once per frame
    void Update() {
        if (interactableType == InteractableType.Button) {

        }
    }

    /// <summary>
    /// Used when interacting with levers and valves
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other) {

        /* First checks if rigidbody is not kinematic */
        if (GetComponent<Rigidbody>().isKinematic == false) {

            if (other.tag == "hand" && !interacting) {

                hand = other.gameObject;
                handController = hand.GetComponent<Controller>();

                switch (interactableType) {
                    case InteractableType.None:
                        print(string.Format("<color=orange>No interactable type selected on {0}</color>", gameObject));
                        break;

                    case InteractableType.Lever:

                        if (other.tag == "hand" && !interacting) {
                            hand = other.gameObject;
                            if (hand.GetComponent<SteamVR_TrackedController>().triggerPressed) {

                                //print("if " + other.gameObject);                                
                                StartCoroutine(OnInteraction());
                            }
                        }
                        break;
                    case InteractableType.Valve:

                        if (other.tag == "hand" && !interacting) {

                            hand = other.gameObject;
                            if (hand.GetComponent<SteamVR_TrackedController>().triggerPressed) {

                                //print("if " + other.gameObject);

                                StartCoroutine(OnInteraction());
                            }
                        }
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Usen when interacting with buttons
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {

        switch (interactableType) {
            case InteractableType.None:
                print(string.Format("<color=orange>No interactable type selected on {0}</color>", gameObject));
                break;
            case InteractableType.Button:
                /* Checks that the  button is active and buttons collider has hit down trigger collider
                   and that down triggers collider has correct tag
                   
                    -Button interactions is half simulated by adding force when interacting */
                if (active && other == triggerDown.GetComponent<Collider>() && other.tag == tag) {
                    print("Button down");
                    interactable.GetComponent<Rigidbody>().AddForce(transform.up * buttonUpForce);
                    active = false;
                    StartCoroutine(OnInteraction());
                    interactableActivator.Activate(interactableType, interactable);
                }
                /* Checks if buttons in not active and has collided with up trigger and has correct tag */
                if (!active && other == triggerUp.GetComponent<Collider>() && other.tag == tag) {
                    active = true;
                    print("Button up");
                }
                /* Checks if hand has collided with button and is active when interacting */
                if (other.tag == InteractableManager.instance.handTag && active) {
                    print("Add force");
                    interactable.GetComponent<Rigidbody>().AddForce(-transform.up * buttonDownForce);                  
                }
                break;
        }
    }

    /// <summary>
    /// Used for all interactions, but mainly for lever and valve
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnInteraction() {

        interacting = true;

        print("lever interaction");

        switch (interactableType) {
            case InteractableType.Button:
                break;
            default:
                spring = hand.GetComponent<SpringJoint>();

                if (spring.connectedBody == null) {
                    spring.connectedBody = GetComponent<Rigidbody>();

                }
                else {
                    yield break;
                }
                print("Trigger pressed" + hand.GetComponent<SteamVR_TrackedController>().triggerPressed);
                spring.spring = springForce;
                spring.damper = springDamper;
                break;
        }
        float angle = 0;
        float previosAngle;
        int rounds = 0;

        float timer = 0;
        float startTime = 0;

        
        /* Sets anchors for lever and valve also enable buttons haptic feedback */
        switch (interactableType) {
            case InteractableType.Button:
                handController.EnableHapticFeedBackLoop(buttonHapticPulseInterval, 4f, buttonPulseTime);
                break;

            case InteractableType.Lever:
                switch (leverDirection) {
                    case LeverDirection.up:
                        spring.connectedAnchor = upAnchor;
                        break;
                    case LeverDirection.down:
                        spring.connectedAnchor = downAnchor;
                        break;
                }
                break;
            case InteractableType.Valve:
                spring.connectedAnchor = valveAnchor;
                break;

        }

        previosAngle = angle;
        while (hand.GetComponent<SteamVR_TrackedController>().triggerPressed) {

            Debug.DrawRay(spring.transform.TransformPoint(spring.anchor), Vector3.up, Color.red);
            Debug.DrawRay(spring.transform.TransformPoint(spring.connectedAnchor), Vector3.up, Color.red);


            switch (interactableType) {
                case InteractableType.None:
                    break;

                #region Switch Lever
                case InteractableType.Lever:

                    #region old
                    //spring.anchor = transform.InverseTransformPoint(hand.GetComponent<SteamVR_TrackedObject>().transform.position);
                    //spring.connectedAnchor = Vector3.up * leverHeight;

                    //print(Mathf.Abs(Mathf.DeltaAngle(hinge.angle, startAngle)));

                    //if (Mathf.Abs(Mathf.DeltaAngle(hinge.angle, startAngle)) > pulseIncreaser)
                    //{
                    //    print("-- interval");
                    //    leverHapticPulseInterval -= 0.01f;
                    //    previousDAngle = Mathf.Abs(Mathf.DeltaAngle(hinge.angle, startAngle));
                    //    startAngle = hinge.angle;
                    //}

                    //else if (Mathf.Abs(Mathf.DeltaAngle(hinge.angle, startAngle)) > previousDAngle)
                    //{
                    //    print("++ interval");
                    //    leverHapticPulseInterval += 0.01f;
                    //    startAngle = hinge.angle;
                    //}
                    //else
                    //{

                    //}
                    #endregion
                    /*-----------------------Needs to be replaced in some place-------------------------------*/              
                    if (timer - startTime > 0.1) {
                        if (Mathf.Abs(angle) > Mathf.Abs(previosAngle + 1)) {
                            handController.EnableHapticFeedBack(0.5f);

                        }
                        else if (Mathf.Abs(angle) < Mathf.Abs(previosAngle - 1)) {
                            handController.EnableHapticFeedBack(0.5f);

                        }
                        startTime = timer;
                        previosAngle = angle;

                    }

                    if (hinge.angle < 0) {
                        angle += Mathf.DeltaAngle(angle, (hinge.angle));

                    }
                    else {
                        angle += Mathf.DeltaAngle(angle, Mathf.Abs(hinge.angle));

                    }
                    /*-------------------------------------------------------------------------------------------*/
                    switch (leverDirection) {
                        case LeverDirection.up:
                            if (angle > hinge.limits.max) {
                                leverDirection = LeverDirection.down;
                                LeverInteraction();
                                yield break;
                            }
                            break;
                        case LeverDirection.down:                               
                            if (angle < hinge.limits.min) {
                                leverDirection = LeverDirection.up;
                                LeverInteraction();
                                yield break;
                            }
                            break;
                    }

                    //LeverAngle.text = angle.ToString();

                    break;
                #endregion

                #region Switch Valve
                case InteractableType.Valve:

                    /*-----------------------Needs to be replaced in some place-------------------------------*/
                    if (timer - startTime > 0.1) {
                        if (Mathf.Abs(angle) > Mathf.Abs(previosAngle + 5)) {
                            handController.EnableHapticFeedBack(0.5f);

                        }
                        //else if (Mathf.Abs(angle) < Mathf.Abs(previosAngle - 10))
                        //{
                        //    handController.EnableHapticFeedBack(0.5f);
                        //}
                        startTime = timer;
                        previosAngle = angle;

                    }

                    if (hinge.angle < 0) {
                        angle += Mathf.DeltaAngle(angle, (hinge.angle));

                    }
                    else {
                        angle += Mathf.DeltaAngle(angle, Mathf.Abs(hinge.angle));

                    }


                    //valveAngle.text = angle.ToString();
                    /*-------------------------------------------------------------------------------------------*/
                    if (Mathf.Abs(angle) > 360) {

                        rounds++;
                        //valveRounds.text = rounds.ToString();
                        if (rounds == 1) {
                            GetComponent<Rigidbody>().isKinematic = true;

                            interactableActivator.Activate(interactableType, interactable);

                            DetachSpringJoint();
                            yield break;
                        }
                    }

                    break;
                #endregion
                default:
                    break;
            }

            timer += Time.deltaTime;
            //print("time " + timer);

            yield return null;

        }

       
        switch (interactableType) {

            case InteractableType.Lever:
                hinge.useMotor = true;
                JointMotor motorL = hinge.motor;
                switch (leverDirection) {
                    case LeverDirection.up:
                        motorL.force = 1000;
                        motorL.targetVelocity = -1000;
                        break;
                    case LeverDirection.down:
                        motorL.targetVelocity = 1000;
                        break;
                }
                hinge.motor = motorL;
                StartCoroutine(ResetHinge());
                break;
            case InteractableType.Valve:
                hinge.useMotor = true;
                JointMotor motorV = hinge.motor;
                //motor.force = 5000;
                if (hinge.angle < 0) {
                    motorV.targetVelocity = 1000;
                }
                else {
                    motorV.targetVelocity = -1000;

                }
                hinge.motor = motorV;
                StartCoroutine(ResetHinge());
                break;
        }

        DetachSpringJoint();

    }

    /// <summary>
    /// Used when interacting with lever
    /// 
    /// -Set lever to kinematic to prevent its movement
    /// -Detaches spring joint from lever
    /// -calls right function from Obective manager
    /// </summary>
    /// <param name="direction"></param>
    private void LeverInteraction() {
        GetComponent<Rigidbody>().isKinematic = true;        
        DetachSpringJoint();
        interactableActivator.Activate(interactableType ,leverType, interactable);
        
    }

    private void DetachSpringJoint() {
        handController.EnableHapticFeedBackLoop(pulseInterval, pulseStrengh, pulseTime);
        hand.GetComponent<SpringJoint>().connectedBody = null;
        //GetComponent<Collider>().enabled = true;
        print("Enabled col");
        interacting = false;
        StartCoroutine(ReActive());

    }

    private IEnumerator ReActive() {
        yield return new WaitForSeconds(2.5f);
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private IEnumerator ResetHinge() {
        yield return new WaitForSeconds(0.1f);
        hinge.useMotor = false;
    }
}
