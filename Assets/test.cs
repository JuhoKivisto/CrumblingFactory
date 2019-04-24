using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    [SerializeField]
    private GameObject cube;

    public float Heat;
    public float minHeat;
    public float maxHeat;
    public float heatNormalizeNumber;

    // Start is called before the first frame update
    private void Start() {
        StartCoroutine(heatMeterNeedle());
    }

    // Update is called once per frame
    void Update()
    {
            heatNormalizeNumber = (Heat - minHeat) / (maxHeat - minHeat);

        //heatNormalizeNumber = (Heat - minHeat) / (maxHeat - minHeat);
        ////cube.transform.rotation.z = 1;

        //cube.transform.localEulerAngles = new Vector3(cube.transform.localEulerAngles.x, cube.transform.localEulerAngles.y, - heatNormalizeNumber * 180);
        //Debug.Log(heatNormalizeNumber);
        //Debug.Log(cube.transform.rotation);
    }

    IEnumerator heatMeterNeedle() {

        while (true) {

            //cube.transform.rotation.z = 1;

            cube.transform.localEulerAngles = new Vector3(cube.transform.localEulerAngles.x, cube.transform.localEulerAngles.y, -heatNormalizeNumber * 180);
            Debug.Log(heatNormalizeNumber);

            yield return null;
        }
    }
}
