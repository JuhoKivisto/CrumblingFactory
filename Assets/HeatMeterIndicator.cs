using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMeterIndicator : MonoBehaviour {
    // Start is called before the first frame update

    public Quaternion minAngle;

    [Range(0.01f,0)]
    public float rotationSpeed;
    [Range(-0.5f, 0.5f)]
    public float rotation;

    public bool running;

    void Start() {
        StartCoroutine(RotateIndicator());
    }

    public IEnumerator RotateIndicator() {

        //Quaternion rotation = new Quaternion();

        while (running) {

            bool angleLower = transform.rotation.x >= 0;
            bool angleHigher = transform.rotation.x <= 180;

            //rotation = Quaternion.Euler(0, 0.1f, 0);
            if (angleHigher) {
                
            transform.Rotate(rotation,0, 0);
            }

            //yield return new WaitForSeconds(rotationSpeed);
            yield return null;
        }  
    }
    public float ChangeRotationSpeed() {

        return 0;
    }
}
