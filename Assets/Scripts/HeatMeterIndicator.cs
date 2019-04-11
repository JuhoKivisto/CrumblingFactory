using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMeterIndicator : MonoBehaviour {
    // Start is called before the first frame update

    public Quaternion minAngle;

    public Transform startDir;
    public Transform endDir;

    int limit;

    public float angle;

    [Range(0.01f, 0)]
    public float rotationSpeed;
    [Range(-1000f, 10f)]
    public float rotation;

    public bool running;

    void Start() {
        HeatManager.instance.StartHeatIncreace();
        StartCoroutine(RotateIndicator());
    }

    public void ActivateHeatMeter()
    {
         StartCoroutine(RotateIndicator());
    }

    public IEnumerator RotateIndicator() {

        Quaternion rotation = new Quaternion();

        while (running) {

            bool angleLower = transform.rotation.z >= 0;
            bool angleHigher = transform.rotation.z <= 1;

            //rotation = Quaternion.Euler(0, 0.1f, 0);
            if (true) {

                rotation.Set(0, 0, HeatManager.instance.Heat / Stats.instance.maxHeat, 0);

                float newAngle = HeatManager.instance.Heat / Stats.instance.maxHeat;

                //float angle = Mathf.LerpAngle(minAngle, maxAngle, Time.time);
                //transform.eulerAngles = new Vector3(0, angle, 0);

                transform.rotation = Quaternion.AngleAxis(newAngle * 180f, -startDir.right);
                //transform.RotateAround(startDir.transform.position, transform.right, newAngle * 180f);
                    

                print(rotation.z);
                //transform.Rotate(0, 0, HeatManager.instance.deltaHeat);
                //Quaternion rotate = new Quaternion(0, 0, HeatManager.instance.Heat, 0);
                //stransform.rotation = ;
            }
            //angle = transform.rotation.z;
            //yield return new WaitForSeconds(rotationSpeed);

            if (transform.rotation.z == 1) {
                print("MAx");
            }
            yield return null;
        }
    }
    public float ChangeRotationSpeed() {

        return 0;
    }
}
