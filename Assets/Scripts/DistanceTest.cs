using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTest : MonoBehaviour {

    public GameObject other;

	// Use this for initialization
	void Start () {
        StartCoroutine(distTest());
	}
	
	public IEnumerator distTest()
    {

        while (true)
        {
            print(Vector3.Distance(gameObject.transform.position, other.transform.position));

            yield return new WaitForSeconds(1f);
        }
    }
}
