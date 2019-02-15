using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance = null;

    public int health;

    public PlayerStatus playerStatus;

    void Awake() {

        if (instance == null) {
            instance = this;
        }

        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        print(collision.gameObject.GetComponent<Rigidbody>().velocity.y);
        if (Mathf.Abs(collision.gameObject.GetComponent<Rigidbody>().velocity.y) > 2) {
            print("hit");
            health -= 50;
        }
    }
}

