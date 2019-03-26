using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableController : MonoBehaviour
{

    public enum InteractableType { None, Button, Lever, Valve };

    public enum LeverDirection
    {
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
    public GameObject buttonObj;
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
    public Text LeverAngle;
    

    #endregion
    #region Valve
    [Header("Valve")]
    [Space]

    [HideUnless("valve")]
    public Text valveAngle;
    [HideUnless("valve")]
    public Text valveRounds;

    [HideUnless("valve")]
    public GameObject valveObj;

    [HideUnless("valve")]
    public Vector3 valveAnchor;
    #endregion

    [Header("Lever and valve")]
    [Space]
    [Range(0f, 10000f)]
    public float springForce;

    [Range(0f, 100f)]
    public float springDamper;

    [Range(-10f, 10f)]
    public float distanceX;

    [Range(-10f, 10f)]
    public float distanceY;

    [Range(-10f, 10f)]
    public float distanceZ;

    public HingeJoint hinge;
    public bool interacting;
    public SpringJoint spring;
    public float pulseIncreaser;
    public GameObject hand;
    public string tag;

    [Range(0f, 0.5f)]
    public float pulseIntervall;
    [Range(0f, 4f)]
    public float pulseStrengh;
    [Range(0f, 2f)]
    public float pulseTime;

    // Use this for initialization
    void Start()
    {
        hinge = GetComponent<HingeJoint>();

        startAngle = hinge.limits.min;
        endAngle = hinge.limits.max;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactableType == InteractableType.Button)
        {

        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (GetComponent<Rigidbody>().isKinematic == false)
        {

            if (other.tag == "hand" && !interacting)
            {
                //GetComponent<Collider>().enabled = false;
                hand = other.gameObject;
                handController = hand.GetComponent<Controller>();

                switch (interactableType)
                {
                    case InteractableType.None:
                        print(string.Format("<color=orange>No interactable type selected on {0}</color>", gameObject));
                        break;
                    case InteractableType.Button:

                        if (active && other == triggerDown.GetComponent<Collider>() && other.tag == tag)
                        {
                            print("Button down");
                            buttonObj.GetComponent<Rigidbody>().AddForce(transform.up * 20);
                            active = false;
                            StartCoroutine(OnButtonInteraction());
                            ObjectiveManager.instance.CompleteObjective(buttonObj.GetComponentInParent<InteractableTest>().objectiveInfo);
                        }
                        if (!active && other == triggerUp.GetComponent<Collider>() && other.tag == tag)
                        {
                            active = true;
                            print("Button up");
                        }
                        if (other.tag == InteractableManager.instance.handTag && active)
                        {
                            print("Add force");
                            buttonObj.GetComponent<Rigidbody>().AddForce(-transform.up * 50);
                            //active = false;
                        }

                        break;
                    case InteractableType.Lever:

                        //print("switch");


                        if (other.tag == "hand" && !interacting)
                        {
                            //GetComponent<Collider>().enabled = false;
                            hand = other.gameObject;
                            if (hand.GetComponent<SteamVR_TrackedController>().triggerPressed)
                            {

                                print("if " + other.gameObject);
                                handController = hand.GetComponent<Controller>();
                                StartCoroutine(OnInteraction());
                            }


                        }

                        break;
                    case InteractableType.Valve:

                        if (other.tag == "hand" && !interacting && GetComponent<Rigidbody>().isKinematic == false)
                        {
                            //GetComponent<Collider>().enabled = false;
                            hand = other.gameObject;
                            if (hand.GetComponent<SteamVR_TrackedController>().triggerPressed)
                            {

                                print("if " + other.gameObject);
                                handController = hand.GetComponent<Controller>();
                                StartCoroutine(OnInteraction());
                            }


                        }

                        break;
                    default:
                        break;
                }

            }
        }       
    }

    public IEnumerator OnInteraction()
    {


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

        float angle = 0;
        float previosAngle;
        int rounds = 0;
        //int wholeRounds = 360;

        spring.spring = springForce;
        spring.damper = springDamper;
        //spring.connectedAnchor = new Vector3(distanceX, distanceY, distanceZ);

        switch (interactableType)
        {
            case InteractableType.Lever:
                switch (leverDirection)
                {
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
        while (hand.GetComponent<SteamVR_TrackedController>().triggerPressed)
        {

            Debug.DrawRay(spring.transform.TransformPoint(spring.anchor), Vector3.up, Color.red);
            Debug.DrawRay(spring.transform.TransformPoint(spring.connectedAnchor), Vector3.up, Color.red);


            switch (interactableType)
            {
                case InteractableType.None:
                    break;
                case InteractableType.Button:
                    handController.EnableHapticFeedBackLoop(buttonHapticPulseInterval, 4f, buttonPulseTime);
                    break;
                #region Switch Lever
                case InteractableType.Lever:



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

                    if (timer - startTime > 0.1)
                    {
                        if (Mathf.Abs(angle) > Mathf.Abs(previosAngle + 1))
                        {
                            handController.EnableHapticFeedBack(0.5f);

                        }
                        else if(Mathf.Abs(angle) < Mathf.Abs(previosAngle - 1))
                        {
                            handController.EnableHapticFeedBack(0.5f);

                        }
                        startTime = timer;
                        previosAngle = angle;

                    }

                    if (hinge.angle < 0)
                    {
                        angle += Mathf.DeltaAngle(angle, (hinge.angle));

                    }
                    else
                    {
                        angle += Mathf.DeltaAngle(angle, Mathf.Abs(hinge.angle));

                    }

                    switch (leverDirection)
                    {
                        case LeverDirection.up:
                            if (angle < hinge.limits.min)
                            {
                                DetachSpringJoint();
                                GetComponent<Rigidbody>().isKinematic = true;                                
                                leverDirection = LeverDirection.down;
                                yield break;
                            }
                            break;
                        case LeverDirection.down:
                            if (angle > hinge.limits.max)
                            {
                                DetachSpringJoint();
                                GetComponent<Rigidbody>().isKinematic = true;                                                    
                                leverDirection = LeverDirection.up;
                                yield break;
                            }
                            break;                       
                    }
                    
                    //LeverAngle.text = angle.ToString();
                   
                    break;
                #endregion

                #region Switch Valve
                case InteractableType.Valve:

                    if (timer - startTime > 0.1)
                    {
                        if (Mathf.Abs(angle) > Mathf.Abs(previosAngle + 5))
                        {
                            handController.EnableHapticFeedBack(0.5f);

                        }
                        //else if (Mathf.Abs(angle) < Mathf.Abs(previosAngle - 10))
                        //{
                        //    handController.EnableHapticFeedBack(0.5f);
                        //}
                        startTime = timer;
                        previosAngle = angle;

                    }

                    if (hinge.angle < 0)
                    {
                        angle += Mathf.DeltaAngle(angle, (hinge.angle));

                    }
                    else
                    {
                        angle += Mathf.DeltaAngle(angle, Mathf.Abs(hinge.angle));

                    }


                    //valveAngle.text = angle.ToString();

                    if (Mathf.Abs(angle) > 360)
                    {

                        rounds++;
                        //valveRounds.text = rounds.ToString();
                        if (rounds == 1)
                        {
                            GetComponent<Rigidbody>().isKinematic = true;
                            DetachSpringJoint();
                            ObjectiveManager.instance.CompleteObjective(valveObj.GetComponentInParent<InteractableTest>().objectiveInfo);
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

        DetachSpringJoint();

    }

    public IEnumerator OnButtonInteraction()
    {
        interacting = true;


        float timer = 0;
        float startTime = 0;
        while (timer < buttonPulseTime)
        {
            print("Button pulse");


            if (timer - startTime > buttonHapticPulseInterval)
            {
                handController.EnableHapticFeedBack();
                startTime = timer;

                //print("Haptic start " + startTime + " current " + timer);
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void DetachSpringJoint()
    {
        handController.EnableHapticFeedBackLoop(pulseIntervall, pulseStrengh, pulseTime);
        hand.GetComponent<SpringJoint>().connectedBody = null;
        //GetComponent<Collider>().enabled = true;
        print("Enabled col");
        interacting = false;
        StartCoroutine(ReActive());

    }

    private IEnumerator ReActive()
    {
        yield return new WaitForSeconds(2.5f);
        GetComponent<Rigidbody>().isKinematic = false;

    }
}
