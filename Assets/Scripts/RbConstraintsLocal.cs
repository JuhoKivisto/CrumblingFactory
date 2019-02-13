using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RbConstraintsLocal : MonoBehaviour {

    //   public Vector3 localVelocity;
    //   public Rigidbody rigidbody;


    //   // Use this for initialization
    //   void Start () {
    //       rigidbody = GetComponent<Rigidbody>();

    //}



    //   void Update() {

    //       localVelocity = transform.InverseTransformDirection(rigidbody.velocity);

    //       localVelocity.x = 0;
    //       localVelocity.z = 0;

    //       rigidbody.velocity = transform.TransformDirection(localVelocity);

    //   }

    //[Flags]
    //public enum LockFlags {
    //    None,
    //    Horizontal,
    //    Vertical,
    //    Both
    //}

    //[Range(0, 25)]
    //public float Force = 1;

    //public LockFlags Lock = LockFlags.Both;

    //public Transform Target;

    //private Rigidbody _rigidbody;

    //private Transform _transform;

    //public void Awake() {
    //    _rigidbody = GetComponent<Rigidbody>();
    //    _transform = GetComponent<Transform>();
    //}

    //public void Update() {
    //    if (Lock == LockFlags.None || Force <= 0) return;

    //    bool lockX = (Lock & LockFlags.Horizontal) == LockFlags.Horizontal;
    //    bool lockY = (Lock & LockFlags.Vertical) == LockFlags.Vertical;

    //    Vector3 velocity = _transform.InverseTransformDirection(_rigidbody.velocity);
    //    if (lockX) velocity.x = 0;
    //    if (lockY) velocity.y = 0;
    //    _rigidbody.velocity = _transform.TransformDirection(velocity);

    //    Vector3 distance = Target.position - _transform.position;
    //    Vector3 idealPosition = Target.position - _transform.forward * distance.magnitude;
    //    Vector3 correction = idealPosition - _transform.position;

    //    correction = _transform.InverseTransformDirection(correction);
    //    if (!lockX) correction.x = 0;
    //    if (!lockY) correction.y = 0;
    //    correction.z = 0;
    //    correction = _transform.TransformDirection(correction);

    //    _rigidbody.velocity += correction * Force;
    //}

    [Header("Freeze Local Position")]
    [SerializeField]
    bool x;
    [SerializeField]
    bool y;
    [SerializeField]
    bool z;

    Vector3 localPosition0;    //original local position

    private void Start() {
        SetOriginalLocalPosition();
    }

    private void Update() {
        float x, y, z;


        if (this.x)
            x = localPosition0.x;
        else
            x = transform.localPosition.x;

        if (this.y)
            y = localPosition0.y;
        else
            y = transform.localPosition.y;

        if (this.z)
            z = localPosition0.z;
        else
            z = transform.localPosition.z;


        transform.localPosition = new Vector3(x, y, z);

    }

    public void SetOriginalLocalPosition() {
        localPosition0 = transform.localPosition;
    }

}
