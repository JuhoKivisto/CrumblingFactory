using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePileController : MonoBehaviour {

    public List<GameObject> tiles;

    private void OnEnable() {
        StartCoroutine(WaitList());
    }

    /// <summary>
    /// Adds all objects to list
    /// </summary>
    /// <param name="gO"></param>
    public void PopulateList(GameObject gO) {
        tiles.Add(gO);
        
    }
    /// <summary>
    /// Starts the waiting list for making object
    /// kinematic, and then scalesthem to zero
    /// and then destroys them
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitList() {
        yield return StartCoroutine(WaitCoroutine(9f, "SetKinematic"));
        print("kinamatic");
       
        yield return StartCoroutine(WaitCoroutine(0f, "ScaleDown"));
        
        
    }

    /// <summary>
    /// Calls a given function on given time
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="functionName"></param>
    /// <returns></returns>
    IEnumerator WaitCoroutine(float waitTime, string functionName) {
        yield return new WaitForSeconds(waitTime);
        Invoke(functionName, 0f);
    }

    /// <summary>
    /// Sets all objects in the list kinematic
    /// </summary>
    void SetKinematic() {
        foreach (var item in tiles) {
            item.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    /// <summary>
    /// Scales all object in the list
    /// </summary>
    void ScaleDown() {
        foreach (var item in tiles) {
            StartCoroutine(ScaleObject(item));
            //item.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    /// <summary>
    /// Scales nicely the given object to zero 
    /// </summary>
    /// <param name="objToScale"></param>
    /// <returns></returns>
    IEnumerator ScaleObject(GameObject objToScale) {
        System.Random rnd = new System.Random();
        while (objToScale.transform.localScale.x > 0f) {
            objToScale.transform.localScale *= 0.97f;
            //objToScale.transform.localScale = Vector3.Lerp(objToScale.transform.localScale, Vector3.zero, Time.deltaTime * 2f);
            yield return null;
        }
        Destroy(objToScale);

    }

    /// <summary>
    /// Destroys the given object
    /// </summary>
    /// <param name="objectToDestroy"></param>
    private void Destroy(GameObject objectToDestroy) {

        Destroy(objectToDestroy);
        }
    
}
