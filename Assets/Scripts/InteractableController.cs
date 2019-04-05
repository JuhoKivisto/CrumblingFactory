using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableController : MonoBehaviour {

    public enum InteractableType { None, Button, Lever, Valve };

    public enum LeverDirection {
        up,
        down
    }
    public InteractableType interactableType;

    private Controller handController;

    public bool button;
    public bool lever;
    public bool valve;

    #region Button
    [Header("Button")]
    [Space]

    [HideUnless("button")]
    public GameObject triggerDown;
    [HideUnless("button")]
    public GameObject triggerUp;
    [HideUnless("button")]
    public GameObject enableCollider;
    [HideUnless("button")]
    public GameObject disableCollider;
    [HideUnless("button")]
    public bool active;
    [HideUnless("button")]
    public bool isEnabled;
    [HideUnless("button")]
    [Range(2f, 0f)]
    public float buttonHapticPulseInterval;
    [HideUnless("button")]
    public float buttonPulseTime;
    [HideUnless("button")]
    public float buttonUpForce;
    [HideUnless("button")]
    public float buttonDownForce;
    #endregion


    #region Lever


    [Header("Lever")]
    [Space]
    [HideUnless("lever")]
    public Vector3 upAnchor;
    [HideUnless("lever")]
    public Vector3 downAnchor;
    [HideUnless("lever")]
    public LeverDirection leverDirection;
    [HideUnless("lever")]
    public float SpringBreakDistance;
    [HideUnless("lever")]
    [Range(2f, 0f)]
    public float leverHapticPulseInterval;
    [HideUnless("lever")]
    public float startAngle;
    [HideUnless("lever")]
    public float endAngle;
    [HideUnless("lever")]
    public float previousDAngle;
    [HideUnless("lever")]
    public float leverHeight;
    [HideUnless("lever")]
    public Text LeverAngle;
    public bool isReactorShutdownLever;
    public bool isTheFirstLever;

    #endregion
    #region Valve
    [Header("Valve")]
    [Space]

    [HideUnless("valve")]
    public Text valveAngle;
    [HideUnless("valve")]
    public Text valveRounds;

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

    private void OnTriggerStay(Collider other) {

        if (GetComponent<Rigidbody>().isKinematic == false) {

            if ((other.tag == "hand" || other.tag == tag) && !interacting) {
                //GetComponent<Collider>().enabled = false;
                hand = other.gameObject;
                handController = hand.GetComponent<Controller>();

                switch (interactableType) {
                    case InteractableType.None:
                        print(string.Format("<color=orange>No interactable type selected on {0}</color>", gameObject));
                        break;

                    case InteractableType.Lever:

                        //print("switch");


                        if (other.tag == "hand" && !interacting) {
                            //GetComponent<Collider>().enabled = false;
                            hand = other.gameObject;
                            if (hand.GetComponent<SteamVR_TrackedController>().triggerPressed) {

                                print("if " + other.gameObject);
                                handController = hand.GetComponent<Controller>();
                                StartCoroutine(OnInteraction());
                            }
                        }
                        break;
                    case InteractableType.Valve:

                        if (other.tag == "hand" && !interacting && GetComponent<Rigidbody>().isKinematic == false) {
                            //GetComponent<Collider>().enabled = false;
                            hand = other.gameObject;
                            if (hand.GetComponent<SteamVR_TrackedController>().triggerPressed) {

                                print("if " + other.gameObject);
                                handController = hand.GetComponent<Controller>();
                                StartCoroutine(OnInteraction());
                            }
                        }
                        break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {

        switch (interactableType) {
            case InteractableType.None:
                print(string.Format("<color=orange>No interactable type selected on {0}</color>", gameObject));
                break;
            case InteractableType.Button:
                if (active && other == triggerDown.GetComponent<Collider>() && other.tag == tag) {
                    print("Button down");
                    interactable.GetComponent<Rigidbody>().AddForce(transform.up * buttonUpForce);
                    active = false;
                    StartCoroutine(OnInteraction());
                    ObjectiveManager.instance.CompleteObjective(interactable.GetComponentInParent<Interactable>().objectiveInfo);
                }
                if (!active && other == triggerUp.GetComponent<Collider>() && other.tag == tag) {
                    active = true;
                    print("Button up");
                }
                if (other.tag == InteractableManager.instance.handTag && active) {
                    print("Add force");
                    interactable.GetComponent<Rigidbody>().AddForce(-transform.up * buttonDownForce);
                    //active = false;
                }
                break;
        }
    }

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

        //int wholeRounds = 360;

        //spring.connectedAnchor = new Vector3(distanceX, distanceY, distanceZ);

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

                    switch (leverDirection) {
                        case LeverDirection.up:
                            if (angle > hinge.limits.max) {
                                GetComponent<Rigidbody>().isKinematic = true;
                                leverDirection = LeverDirection.down;
                                DetachSpringJoint();
                                if (isTheFirstLever) ObjectiveManager.instance.ActivateCrumbling();
                                if (isReactorShutdownLever) ObjectiveManager.instance.CompleteReactorShutDown();
                                else ObjectiveManager.instance.CompleteObjective(interactable.GetComponentInParent<Interactable>().objectiveInfo);
                                yield break;
                            }
                            break;
                        case LeverDirection.down:
                            if (angle < hinge.limits.min) {
                                GetComponent<Rigidbody>().isKinematic = true;
                                leverDirection = LeverDirection.up;
                                DetachSpringJoint();
                                if (isTheFirstLever) ObjectiveManager.instance.ActivateCrumbling();
                                if (isReactorShutdownLever) ObjectiveManager.instance.CompleteReactorShutDown();
                                else ObjectiveManager.instance.CompleteObjective(interactable.GetComponentInParent<Interactable>().objectiveInfo);
                                yield break;
                            }
                            break;
                    }

                    //LeverAngle.text = angle.ToString();

                    break;
                #endregion

                #region Switch Valve
                case InteractableType.Valve:

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

                    if (Mathf.Abs(angle) > 360) {

                        rounds++;
                        //valveRounds.text = rounds.ToString();
                        if (rounds == 1) {
                            GetComponent<Rigidbody>().isKinematic = true;
                            ObjectiveManager.instance.CompleteObjective(interactable.GetComponentInParent<Interactable>().objectiveInfo);
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

        hinge.useMotor = true;
        JointMotor motor = hinge.motor;
        switch (interactableType) {

            case InteractableType.Lever:
                switch (leverDirection) {
                    case LeverDirection.up:
        motor.force = 1000;
                        motor.targetVelocity = -1000;
                        break;
                    case LeverDirection.down:
                        motor.targetVelocity = 1000;
                        break;
                }
                hinge.motor = motor;
                StartCoroutine(ResetHinge());
                break;
            case InteractableType.Valve:
                //motor.force = 5000;
                if (hinge.angle < 0) {
                    motor.targetVelocity = 1000;
                }
                else {
                    motor.targetVelocity = -1000;

                }
                hinge.motor = motor;
                StartCoroutine(ResetHinge());
                break;
        }

        DetachSpringJoint();

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
