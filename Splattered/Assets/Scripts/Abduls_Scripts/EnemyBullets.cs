using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullets : MonoBehaviour {
    // THE DAMAGE AND VELOCITY VALUES WILL BE CHANGED VIA GUN SYSTEM

    // Bullet Settings
    [Header("Settings")]
    public float bulletVelocity = 5f; // Speed of the bullet
    public float baseDamage = 30f; // Base damage the gun will do

    // Debounces
    private bool hitDebounce = false;
    private GameObject player;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.right = player.transform.position - transform.position;
        Destroy(gameObject, 15);
    }

    // Update is called once per frame
    void Update() {
        transform.position += bulletVelocity * Time.smoothDeltaTime * transform.right;
    }

    // Collisions
    void OnTriggerEnter2D(Collider2D other) {
        if (hitDebounce) { return; }
        hitDebounce = true;
        if (other.gameObject.layer == 9) {
            
        }

        Destroy(gameObject);
    }
}
