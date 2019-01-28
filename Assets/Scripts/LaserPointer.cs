﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;

    // material objects
    public Material mat1;
    public Material mat2;
    private Renderer rend;

    // laser objects
    public GameObject laserP;
    public GameObject laserR;
    private GameObject laser;
    private GameObject laserRed;
    private Transform laserTransform;
    private Vector3 hitPoint;
    public int length;

    // teleport objects
    public Transform cameraRigTransform;
    public GameObject teleportReticlePrefab;
    private GameObject reticle;
    private Transform teleportReticleTransform;
    public Transform headTransform;
    public Vector3 teleportReticleOffset;
    public LayerMask teleportMask; // You have to create a new layer and put it here to be able to teleport on the floor
    private bool shouldTeleport;
    private bool areStunned;

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
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, 0.5f);
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
    }

    private void Teleport()
    {
        shouldTeleport = false; // sets the bool back to false
        reticle.SetActive(false); // sets reticle to false
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 0; // keeps you in the correct area on y-axis
        //hitPoint.y = 0;
        cameraRigTransform.position = hitPoint + difference; // teleports Camera Rig to the reticle
    }

    // Use this for initialization
    void Start() {

        laser = Instantiate(laserP); // instantiates our laser prefab
        laserTransform = laser.transform;

        reticle = Instantiate(teleportReticlePrefab); // instantiates our reticle prefab
        teleportReticleTransform = reticle.transform;

        areStunned = false;
    }

    // Update is called once per frame
    void Update() {

        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger)) // checks for a trigger press
        {
            RaycastHit hit;
            Debug.Log("Trigger Painettu jes");
            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, length)) // Raycast
            {
                hitPoint = hit.point;
                Debug.Log("hitattu");
                ShowLaser(hit);

                if (hit.collider.tag == "Ground")
                {
                    reticle.SetActive(true); // sets reticle active
                    teleportReticleTransform.position = hitPoint + teleportReticleOffset; // changes reticles position to raycasts hit point

                    shouldTeleport = true; // enables the use of Teleport();

                    if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && shouldTeleport && !areStunned /*&& hitPoint.y == 0*/) // Teleports when the trigger is released if shouldTeleport bool is true
                    {
                        Teleport();
                    }

                } else
                {
                    shouldTeleport = false;
                    reticle.SetActive(false);
                }
            }
            else // disables laser and reticle if the trigger isn't pressed
            {
                laser.SetActive(false);
            }
        }

       
    }
}
