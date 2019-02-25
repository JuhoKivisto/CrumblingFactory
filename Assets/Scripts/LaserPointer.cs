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

    // laser objects
    public Material mat1;
    public Material mat2;
    public int amount;
    public float cooldown;

    private Transform point0, point1, point2;

    private int numPoints = 50;
    private Vector3[] positions = new Vector3[100];
    private Vector3 RedLaserLength;
    
    private Transform laserTransform;
    private Vector3 startPoint;
    public float length;
    public LineRenderer lineRenderer;
    public GameObject ControllerPrefab;
    private float distanceFromGround;

    RaycastHit hit;
    RaycastHit hit2;

    // teleport objects
    public Transform cameraRigTransform;
    public GameObject teleportReticlePrefab;
    private GameObject reticle;
    private Transform teleportReticleTransform;
    public Transform headTransform;
    public Vector3 teleportReticleOffset;
    private Vector3 newDir;
    public float angle;
    private float angleCount;


    // public LayerMask teleportMask;
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

    private void ShowLaser(RaycastHit hit) // Enables and builds the laser
    {



        lineRenderer.widthMultiplier = 0.1f;
        
        lineRenderer.SetPosition(0, ControllerPrefab.transform.position);
        lineRenderer.SetPosition(1, startPoint);

        /*laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, 0.5f);
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);*/
    }

    private void ShowCurveLaser() 
    {
        lineRenderer.positionCount = amount;
        lineRenderer.SetPosition(0, positions[0]);
        for (int i = 1; i < amount; i++)
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
        Debug.Log("Nonii");
        shouldTeleport = false; // sets the bool back to false
        reticle.SetActive(false); // sets reticle to false
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 0; // keeps you in the correct area on y-axis
        //hitPoint.y = 0;
        cameraRigTransform.position = reticle.transform.position; // teleports Camera Rig to the reticle
    }

    // Use this for initialization
    void Start() {


        if (buttonToUse == EbuttonToUse.Touchpad)
        {
            button = SteamVR_Controller.ButtonMask.Touchpad;

        }
        else 
        {
            button = SteamVR_Controller.ButtonMask.Trigger;
        } 

        /*laser = Instantiate(laserP); // instantiates our laser prefab
        laserTransform = laser.transform;*/

        reticle = Instantiate(teleportReticlePrefab); // instantiates our reticle prefab
        teleportReticleTransform = reticle.transform;

        onCooldown = false;
    }

    // Update is called once per frame
    void Update() {


        if (Controller.GetPress(button)) // checks for a trigger press
        {
            angleCount = angle;

            lineRenderer.widthMultiplier = 0.1f;
            newDir = transform.forward;
            startPoint = trackedObj.transform.position;
            positions[0] = startPoint;
            Vector3 laserAxis = transform.right;

            for (int i = 1; i < 100; i++)
            {
                if (Physics.Raycast(startPoint, newDir, out hit, length))
                {
                    if (hit.collider.tag == "Ground")
                    {
                        positions[i] = hit.point;
                        lineRenderer.material = mat1;
                        amount = i;
                        ShowCurveLaser();
                        reticle.SetActive(true);
                        teleportReticleTransform.position = hit.point + teleportReticleOffset;
                        shouldTeleport = true;
                        break;
                    }
                    else
                    {
                        positions[i] = hit.point;
                        lineRenderer.material = mat2;
                        amount = i;
                        ShowCurveLaser();
                        reticle.SetActive(false);
                        shouldTeleport = false;
                        break;
                    }
                }
                else
                {
                    DisableLaser();
                    reticle.SetActive(false);
                    shouldTeleport = false;
                    lineRenderer.material = mat2;
                    startPoint = startPoint + newDir * length;
                    positions[i] = startPoint;
                    if (angleCount < 90)
                    {
                        
                        angleCount += angle;
                        newDir = Quaternion.AngleAxis(angle, laserAxis) * newDir;
                    }
                }
            }


            /*Debug.Log("Trigger Painettu jes");

            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, length)) // Raycast
            {
                startPoint = hit.point;
                ShowLaser(hit);
                // changes reticles position to raycasts hit point

                if (hit.collider.tag == "Ground")
                {
                    Debug.Log("hitattu");
                    lineRenderer.material = mat1;
                    reticle.SetActive(true); // sets reticle active
                    // teleportReticleTransform.position = hitPoint + teleportReticleOffset; // changes reticles position to raycasts hit point
                    teleportReticleTransform.position = hit.point + teleportReticleOffset;
                    shouldTeleport = true; // enables the use of Teleport();
                }
                else
                {
                    Debug.Log(hit.collider.gameObject.name);
                    shouldTeleport = false;
                    lineRenderer.material = mat2;
                    reticle.SetActive(false);

                }
            }*/
        }
        else // disables laser and reticle if the trigger isn't pressed
        {
            reticle.SetActive(false);
            DisableLaser();

        } 
        



        if (Controller.GetPressUp(button) && shouldTeleport && !onCooldown /*&& hitPoint.y == 0*/) // Teleports when the trigger is released if shouldTeleport bool is true
        {
            Teleport();
        }
    }
    private Vector3 CalculateCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2) // Calculates bezier
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }
}
