using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
            ParticleManager.instance.PlayParticle(ParticleManager.instance.Spark, ParticleSystemStopAction.Destroy, ParticleManager.instance.SparkPosition[0], Vector3.up);
    }
}
