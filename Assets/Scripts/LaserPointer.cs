using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserPointer : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;

    // Place this script to the desired controller within the CameraRig
    // To use this script you have to create a tag "Ground" and assign it to all the surfaces you want to be able to teleport on

    // Button objects
    public enum EbuttonToUse // Uses which button you choose from unity editor 
    {
        Touchpad,
        Trigger
    }

    public EbuttonToUse buttonToUse;
    private ulong button;
    private ulong interactionbutton;

    // Materials objects
    public Material canTeleportMat;
    public Material canNotTeleportMat;

    // Variables used to calculate stuff in the code
    private int amount;
    public float maxCooldown; // Use 0 if you don't want a cooldown
    public float maxDistance; // Max allowed teleport distance
    private float cooldown;
    private float angle; 
    private float length;
    private float angleCount;
    private float distanceFromGround;
    private float cooldownTime;
    private float magnitude;

    // Vector3 positions used for calculations
    private Vector3[] positions = new Vector3[100];    
    private Vector3 startPoint;
    private Vector3 teleportReticleOffset;
    private Vector3 newDir;

    public LineRenderer lineRenderer;
    RaycastHit hit;
    private Image cooldownSprite; 

    // Reticle objects
    public GameObject teleportReticlePrefab;
    private GameObject reticle;
    private Transform teleportReticleTransform;

    // Camera and headset transforms
    public Transform headTransform;
    public Transform cameraRigTransform;

    // Teleportation checks
    public GameObject teleportAreaCheck;
    public LayerMask layerMask;
    public static bool reticleCollides;
    private bool shouldTeleport;
    private bool onCooldown;
    public bool areStunned; // This variable should be set to true if you get hit by a Crowd Control, puts you on a 4 second cooldown

    public Image cooldownOnController;

    public bool showDebug = false;


    private SteamVR_Controller.Device Controller // Gets the controller object
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private void Awake() // Starts tracking on Awake
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void ShowCurveLaser() 
    {
        lineRenderer.positionCount = amount;
        lineRenderer.SetPosition(0, positions[0]); // Sets the starting position to the controller
        for (int i = 1; i < amount; i++) // Sets the other positions to the line renderer to create a curve
        {
            lineRenderer.SetPosition(i, positions[i]);
        }
    }

    private void DisableLaser() // Disables the laser
    {
        lineRenderer.positionCount = 0;
    }

    private void Teleport()
    {
        shouldTeleport = false; // sets the bool back to false
        reticle.SetActive(false); // sets reticle to false
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 0; // keeps you in the correct area on y-axis
        cameraRigTransform.position = reticle.transform.position; // teleports Camera Rig to the reticle
        headTransform.position = reticle.transform.position;

        if (magnitude > (maxDistance / 2)) // Calculations to adjust the cooldown according to distance
        {
            cooldown = maxCooldown;
        } else if (magnitude > (maxDistance / 3))
        {
            cooldown = maxCooldown / 2;
        } else
        {
            cooldown = maxCooldown / 4;
        }

        onCooldown = true; // Puts the teleport on cooldown
        cooldownTime = cooldown + Time.time;
        cooldownSprite.fillAmount = 1.0f;
        cooldownOnController.fillAmount = 1.0f;
    }

    private void Stunned() // sets movement on a 4 second cooldown after being stunned
    {
        onCooldown = true;
        cooldown = 4;
        cooldownTime = cooldown + Time.time;
        cooldownSprite.fillAmount = 1.0f;
        cooldownOnController.fillAmount = 1.0f;
        areStunned = false;
    }

    // Use this for initialization
    void Start() {

        cooldownSprite = teleportReticlePrefab.transform.GetChild(1).GetComponent<Image>(); // Gets the cooldown sprite from the reticle prefab

        // Setting the tested "best" values for the curve calculation
        angle = 2f;
        length = 0.1f;

        // Sets reticle offset
        teleportReticleOffset.y = 0.01f;

        // Set the used button for teleportation
        if (buttonToUse == EbuttonToUse.Touchpad)
        {
            button = SteamVR_Controller.ButtonMask.Touchpad;
            interactionbutton = SteamVR_Controller.ButtonMask.Trigger;

        }
        else 
        {
            button = SteamVR_Controller.ButtonMask.Trigger;
            interactionbutton = SteamVR_Controller.ButtonMask.Touchpad;
        }

        reticle = teleportReticlePrefab;
        teleportReticleTransform = reticle.transform;

        onCooldown = false; // Takes the movement off cooldown at launch
    }

    // Update is called once per frame
    void Update() {

        // Resets cooldown after the wait time has passed
        if (cooldownTime < Time.time)
        {
            onCooldown = false;
        }

        // Visualization of cooldown
        if (cooldownSprite.fillAmount > 0f)
        {
            cooldownOnController.fillAmount -= 1.0f / cooldown * Time.deltaTime;
            cooldownSprite.fillAmount -= 1.0f / cooldown * Time.deltaTime;
        }
        
        if (areStunned == true)
        {
            if (showDebug == true)
            {
                Debug.Log("Stunned!");
            }
            Stunned();
        }

        if (Controller.GetPress(button) && !Controller.GetPress(interactionbutton)) // checks for a button press
        {
            if (showDebug == true)
            {
                Debug.Log("Got a button press");
            }

            angleCount = angle; // Variable used to calculate that the curves angle will never go much above 90

            lineRenderer.widthMultiplier = 0.1f; // Sets our line renderers width to 0.1
            newDir = transform.forward;
            startPoint = trackedObj.transform.position;
            positions[0] = startPoint;
            Vector3 laserAxis = transform.right;

            for (int i = 1; i < 100; i++) // Casts multiple raycasts in a curve until it hits something
            {
                if (Physics.Raycast(startPoint, newDir, out hit, length, layerMask, QueryTriggerInteraction.Ignore))
                {
                    if (showDebug == true)
                    {
                        Debug.Log("Hit something");
                    }

                    if (hit.collider.tag == "Ground") // Checks if the hit collided with a gameObject having a tag "Ground" assigned to them
                    {
                        magnitude = Vector3.Magnitude(hit.point - trackedObj.transform.position); // Calculates the distance between the hit and the controller
                        teleportAreaCheck.transform.position = hit.point + teleportReticleOffset;

                        if (magnitude < maxDistance && reticleCollides == false) // Checks if the hit is within max distance. If so, draws the green laser & enables reticle & enables teleportation
                        {
                            if (showDebug == true)
                            {
                                Debug.Log("Succesful hit within the maxDistance, ready to teleport");
                            }
                            positions[i] = hit.point;
                            lineRenderer.material = canTeleportMat;
                            amount = i;
                            reticle.SetActive(true);
                            teleportReticleTransform.position = hit.point + teleportReticleOffset;
                            shouldTeleport = true;
                        } else // If the ray hit distance is higher than the allowed max distance, changes the material to red unlit color & draws the laser
                        {
                            if (showDebug == true)
                            {
                                Debug.Log("Went over the max distance");
                            }
                            positions[i] = hit.point;
                            lineRenderer.material = canNotTeleportMat;
                            amount = i;
                            reticle.SetActive(false);
                            shouldTeleport = false;
                        }
                        break;
                    }
                    else // If the ray hits unteleportable area, changes the material to red unlit color & draws the laser
                    {
                        if (showDebug == true)
                        {
                            Debug.Log("Hit something that doesn't have the Ground tag");
                        }
                        positions[i] = hit.point;
                        lineRenderer.material = canNotTeleportMat;
                        amount = i;
                        reticle.SetActive(false);
                        shouldTeleport = false;
                        break;
                    }
                }
                else // If the ray doesnt hit, does calculations to find out the end point of the ray and new angle for the next ray
                {
                    if (showDebug == true)
                    {
                        Debug.Log("No hit");
                    }
                    DisableLaser();
                    reticle.SetActive(false);
                    shouldTeleport = false;
                    lineRenderer.material = canNotTeleportMat;
                    startPoint = startPoint + newDir * length;
                    positions[i] = startPoint;
                    if (angleCount < 90) // max angle of the curve is 90 degrees, checks that
                    {                
                        angleCount += angle;
                        newDir = Quaternion.AngleAxis(angle, laserAxis) * newDir; // Calculates a new angle for the next ray
                    }
                }
            } // Set the points after loop only
            ShowCurveLaser();
        }
        else // disables laser and reticle if the trigger isn't pressed
        {
            if (showDebug == true)
            {
                Debug.Log("Button isn't pressed");
            }
            reticle.SetActive(false);
            DisableLaser();
        } 

        if (Controller.GetPressUp(button) && shouldTeleport && !onCooldown) // Teleports when the trigger is released if shouldTeleport bool is true and the teleport isn't on cooldown
        {
            if (showDebug == true)
            {
                Debug.Log("Activated Teleport on GetPressUp");
            }
            Teleport();
        }
    }
}
