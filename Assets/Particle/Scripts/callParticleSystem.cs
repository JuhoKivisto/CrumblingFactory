using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class callParticleSystem : MonoBehaviour {

    private ParticleSystem explosion;
    private ParticleSystem spark;
    private ParticleSystem dust;


    //public int maxHeatLevel = ;

    public int HeatLevel = 3;
    public float minHeat;
    public float maxHeat;
    public float Heat;
    private float normalizeNumber;
    float waitTime;


    public List<Transform> position;

    private void Start() {

        StartCoroutine(randomGenerateParticle());

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            spark = ParticleSystemManager.instance.PlayParticle(ParticleSystemManager.instance.Spark, ParticleSystemStopAction.Destroy, position[0].position, Vector3.up);
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            Debug.Log("dust");
            dust = ParticleSystemManager.instance.PlayParticle(ParticleSystemManager.instance.Dust, ParticleSystemStopAction.Destroy, new Vector3(0, 0, 0), Vector3.up);
        }
    }

    IEnumerator randomGenerateParticle() {

        while (true) {

            if(Heat > 50) {
                normalizeNumber = (Heat - minHeat) / (maxHeat - minHeat);
                Debug.Log(normalizeNumber);

                waitTime = (1 - Random.Range(normalizeNumber - 0.1f, normalizeNumber + 0.1f)) * HeatLevel * 10;        //Heat higher, time shorter

                yield return new WaitForSeconds(waitTime);

                explosion = ParticleSystemManager.instance.PlayParticle(ParticleSystemManager.instance.particleSystemPrefab, ParticleSystemStopAction.Destroy, position[Random.Range(0, position.Count)].position, Vector3.right);
                //activate sound also
            }
            else {
                yield return new WaitUntil(() => Heat > 50);
            }
            Debug.Log("time to wait " + waitTime);
        }
        
        
    }
}
