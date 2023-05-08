using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    // THE DAMAGE AND VELOCITY VALUES WILL BE CHANGED VIA GUN SYSTEM

    // Damage Settings
    [Header("Damage Settings")]
    public float baseDamage = 30f; // Base damage the gun will do
    public float headshotMultiplier = 2f; // Multiplier of the base damage for headshots

    // Bullet Settings
    [Header("Bullet Settings")]
    public float bulletVelocity = 5f; // Speed of the bullet
    public float bulletKnockback = 1f; // Knockback of the bullet

    // Debounces
    private bool hitDebounce = false;

    // Start is called before the first frame update
    void Start() {
        Destroy(gameObject, 5);
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
            SoldierMovement mainScript = other.gameObject.GetComponent<SoldierMovement>();
            mainScript.damageSoldier(baseDamage, transform.right, bulletKnockback);
        } else if (other.gameObject.layer == 13) {
            DropperBehavior mainScript = other.gameObject.GetComponent<DropperBehavior>();
            mainScript.DamageDropper(baseDamage);
        } else if (other.gameObject.layer == 8) {
            Bird_Behavior mainScript = other.gameObject.GetComponent<Bird_Behavior>();
            mainScript.DamageBird(baseDamage);
        }

        Destroy(gameObject);
    }
}
