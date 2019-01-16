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
        yield return StartCoroutine(WaitCoroutine(2f, "SetKinematic"));
        //Do something
        //yield return StartCoroutine(WaitCoroutine(4f, "Scaledown"));
        //Do something
        //yield return StartCoroutine(WaitCoroutine(8f, "Destroy"));
        //Do something
    }

    IEnumerator WaitCoroutine(float waitTime, string functionName) {
        yield return new WaitForSeconds(waitTime);
        Invoke(functionName, 0f);
    }
}
