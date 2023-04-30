/*
------------------------------------------
By: Abdul Ahad Naveed
Created: 4/29/2023
Updated: 4/29/2023 @ 11:30 pm

Handles camera manipulation
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    // Public Variables
    public float speed = 15f;
    public float life = 5f; // The lifetime of the bullet in seconds

    // Private Variables
    private float startTime;

    // Start is called before the first frame update
    void Start() {
        // Setting Values
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update() {
        // Checking lifetime
        if ((Time.time - startTime) >= life) {
            Destroy(gameObject);
            return;
        }

        // Updating Position
        transform.position += speed * Time.smoothDeltaTime * transform.right;
    }
}
