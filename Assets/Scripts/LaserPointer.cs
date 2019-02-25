using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;

    // To use this script you have to create a tag "Ground" and assign it to all the object you want to be able to teleport to

    // Button objects
    public enum EbuttonToUse // Uses which button you choose from unity editor 
    {
        Touchpad,
        Trigger
    }

    public EbuttonToUse buttonToUse;
    private ulong button;

    // Materials objects
    public Material canTeleportMat;
    public Material canNotTeleportMat;

    // Variables used to calculate stuff in the code
    private int amount;
    public float cooldown;
    public float angle;
    public float length;
    private float angleCount;
    private float distanceFromGround;
    public float cooldownTime;

    // Vector3 positions used for calculations
    private Vector3[] positions = new Vector3[100];    
    private Vector3 startPoint;
    public Vector3 teleportReticleOffset;
    private Vector3 newDir;

    public LineRenderer lineRenderer;
    RaycastHit hit;

    // Reticle objects
    public GameObject teleportReticlePrefab;
    private GameObject reticle;
    private Transform teleportReticleTransform;

    // Camera and headset transforms
    public Transform headTransform;
    public Transform cameraRigTransform;

    // Teleportation checks
    private bool shouldTeleport;
    private bool onCooldown;


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
        onCooldown = true; // Puts the teleport on cooldown
        cooldownTime += Time.time;
    }

    // Use this for initialization
    void Start() {

        // Set the used button for teleportation
        if (buttonToUse == EbuttonToUse.Touchpad)
        {
            button = SteamVR_Controller.ButtonMask.Touchpad;

        }
        else 
        {
            button = SteamVR_Controller.ButtonMask.Trigger;
        } 

        reticle = Instantiate(teleportReticlePrefab); // instantiates our reticle prefab
        teleportReticleTransform = reticle.transform;

        onCooldown = false; // Takes the movement off cooldown at launch
    }

    // Update is called once per frame
    void Update() {

        if (cooldownTime < Time.time)
        {
            onCooldown = false;
        }

        if (onCooldown)
        {
            // Visualize the cooldown somehow
        }

        if (Controller.GetPress(button)) // checks for a button press
        {
            angleCount = angle; // Variable used to calculate that the curves angle will never go much above 90

            lineRenderer.widthMultiplier = 0.1f; // Sets our line renderers width to 0.1
            newDir = transform.forward;
            startPoint = trackedObj.transform.position;
            positions[0] = startPoint;
            Vector3 laserAxis = transform.right;

            for (int i = 1; i < 100; i++) // Casts multiple raycasts in a curve until it hits something
            {
                if (Physics.Raycast(startPoint, newDir, out hit, length))
                {
                    if (hit.collider.tag == "Ground") // If the ray hits ground (teleportable area), changes the material to green unlit color & draws the laser & enables the reticle & enables teleportation
                    {
                        positions[i] = hit.point; 
                        lineRenderer.material = canTeleportMat;
                        amount = i;
                        ShowCurveLaser();
                        reticle.SetActive(true);
                        teleportReticleTransform.position = hit.point + teleportReticleOffset;
                        shouldTeleport = true;
                        break;
                    }
                    else // If the ray hits unteleportable area, changes the material to red unlit color & draws the laser
                    {
                        positions[i] = hit.point;
                        lineRenderer.material = canNotTeleportMat;
                        amount = i;
                        ShowCurveLaser();
                        reticle.SetActive(false);
                        shouldTeleport = false;
                        break;
                    }
                }
                else // If the ray doesnt hit, does calculations to find out the end point of the ray and new angle for the next ray
                {
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
            }
        }
        else // disables laser and reticle if the trigger isn't pressed
        {
            reticle.SetActive(false);
            DisableLaser();
        } 

        if (Controller.GetPressUp(button) && shouldTeleport && !onCooldown) // Teleports when the trigger is released if shouldTeleport bool is true and the teleport isn't on cooldown
        {
            Teleport();
        }
    }
}
