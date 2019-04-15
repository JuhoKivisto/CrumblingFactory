using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

    public static ParticleManager instance;


    [Header("Particle Effects")]

    public GameObject Spark;

    public GameObject Dust;

    public GameObject SmallExplosion;

    public GameObject MediumExplosion;

    public GameObject BigExplosion;

    [Header ("Particle Positions")]
    public List<Transform> SparkPosition;

    public List<Transform> DustPosition;

    public List<Transform> ExplosionPosition;

    private float normalizeNumber;
    private float waitTime;
    private float timeRateBetweenExplosions = 7;
    private int triggerHeat;

    private void Awake() {
        instance = this;
    }
    private void Start() {

        StartCoroutine(randomGenerateParticle());
        triggerHeat = 50;
    }
    
    /// <summary>
    /// spawn a particle in a position
    /// </summary>
    /// <param name="particle"></param>
    /// <param name="stopAction"></param>
    /// <param name="position">position of particle</param>
    /// <param name="direction">direction of particle</param>
    /// <returns></returns>
    public ParticleSystem PlayParticle(GameObject particle, ParticleSystemStopAction stopAction, Transform position, Vector3 direction) {

        Quaternion changeDirection = new Quaternion();


        if (direction == Vector3.zero) {                                //check that the direction is not zero
            direction = Vector3.up;                                     //default direction is up
        }
        changeDirection = Quaternion.LookRotation(direction);

        if (position != null) {     //if the list of position is empty or there is no game object - don't spawn the particle
            GameObject clone = Instantiate(particle, position.position, changeDirection);

            ParticleSystem temp = clone.GetComponent<ParticleSystem>();

            var main = temp.main;

            main.stopAction = stopAction;

            temp.Play();

            if (particle.transform.childCount > 0) {
                foreach (Transform child in particle.transform) {

                    GameObject childGameObject = child.gameObject;

                    ParticleSystem tempChild = childGameObject.GetComponent<ParticleSystem>();

                    var mainChild = tempChild.main;

                    mainChild.stopAction = stopAction;

                    tempChild.Play();
                }
            }

            return temp;
        }
        else
            return null;
        
    }


    /// <summary>
    /// stop particle
    /// </summary>
    /// <param name="particleSystem"></param>
    public void stopParticleSystem (ParticleSystem particleSystem) {

        if(particleSystem != null) {
            if (particleSystem.gameObject.transform.childCount > 0) {
                foreach (Transform child in particleSystem.gameObject.transform) {
                    ParticleSystem childTemp = child.gameObject.GetComponent<ParticleSystem>();

                    childTemp.Stop();
                }
            }
            particleSystem.Stop();
        }        
    }


    /// <summary>
    /// spawn explosion according to current heat
    /// when current heat is 51 an explosion is triggered 
    /// when current heat increasing, explosions are more often 
    /// the time between explosions is changing
    /// </summary>
    /// <returns></returns>
    IEnumerator randomGenerateParticle() {

        while (true) {
            int randomNumber = Random.Range(0, ExplosionPosition.Count);


            if (HeatManager.instance.Heat > triggerHeat) {            //trigger an explosion when heat at 51

                normalizeNumber = (HeatManager.instance.Heat - Stats.instance.minHeat) / (Stats.instance.maxHeat - Stats.instance.minHeat);
                
                waitTime = (1.5f - Random.Range(normalizeNumber - 0.1f, normalizeNumber + 0.1f)) * timeRateBetweenExplosions;        //Heat higher, time shorter

                yield return new WaitForSeconds(waitTime);
                if(ExplosionPosition.Count > 0)
                    ParticleManager.instance.PlayParticle(ParticleManager.instance.SmallExplosion, ParticleSystemStopAction.Destroy, ExplosionPosition[randomNumber], Vector3.right);
                //activate sound also
            }
            else {
                yield return new WaitUntil(() => HeatManager.instance.Heat > triggerHeat);
            }
        }


    }
}
