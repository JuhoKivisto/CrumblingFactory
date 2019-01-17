using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePileController : MonoBehaviour {

    public List<GameObject> tiles;

    private void OnEnable() {
        StartCoroutine(WaitList());
    }

    public void PopulateList(GameObject gO) {
        tiles.Add(gO);
        
    }

    IEnumerator WaitList() {
        yield return StartCoroutine(WaitCoroutine(9f, "SetKinematic"));
        print("kinamatic");
        //Do something
        yield return StartCoroutine(WaitCoroutine(0f, "ScaleDown"));
        //Do something
        yield return StartCoroutine(WaitCoroutine(20f, "Destroy"));
        //Do something
    }

    IEnumerator WaitCoroutine(float waitTime, string functionName) {
        yield return new WaitForSeconds(waitTime);
        Invoke(functionName, 0f);
    }

    void SetKinematic() {
        foreach (var item in tiles) {
            item.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void ScaleDown() {
        foreach (var item in tiles) {
            StartCoroutine(ScaleObject(item));
            //item.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    IEnumerator ScaleObject(GameObject objToScale) {
        System.Random rnd = new System.Random();
        while (objToScale.transform.localScale.x > 0f) {
            objToScale.transform.localScale *= 0.99f;
            //objToScale.transform.localScale = Vector3.Lerp(objToScale.transform.localScale, Vector3.zero, Time.deltaTime * 2f);
            yield return null;
        }
        Destroy(objToScale);

    }

    private void Destroy(GameObject objectToDestroy) {

        Destroy(objectToDestroy);
        }
    
}
