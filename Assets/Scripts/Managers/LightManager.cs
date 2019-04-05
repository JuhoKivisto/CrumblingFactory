using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour {

    public static LightManager instance = null;

    public List<Light> lightList;

    void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start() {
       
    }

    // Update is called once per frame
    void Update() {

    }

    public void ActivateControlPanelLights() {
        lightList[0].gameObject.SetActive(true);
        lightList[1].gameObject.SetActive(true);
        lightList[2].gameObject.SetActive(true);
    }
    public void ActivateReactorLights(bool isOn) {
        lightList[3].gameObject.SetActive(isOn);
        lightList[4].gameObject.SetActive(isOn);
        lightList[5].gameObject.SetActive(isOn);
    }
    public void ActivateFactoryLight() {
        lightList[7].gameObject.SetActive(true);
    }
    public void DisableFirstControlPanelLight() {
        lightList[8].gameObject.SetActive(false);
    }
}
