using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour {

   public static InteractableManager instance = null;

    public string handTag;

    void Awake() {

        if (instance == null) {
            instance = this;
        }

        else if (instance != this) {
            Destroy(gameObject);
        }
    }
}
