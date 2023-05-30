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
    public bool isRocket = false; // If its a rocket
    public GameObject explosion;
    public GameObject genericHitParticle;
    public GameObject enemyHitParticle;
    

    // Debounces
    private bool hitDebounce = false;
    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start() {
        Destroy(gameObject, 15);
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        transform.position += bulletVelocity * Time.smoothDeltaTime * transform.right;
    }

    // Colliding
    void checkCollision(Collider2D other) {
        bool enemyHit = false;
        if (other.gameObject.layer == 9) {
            SoldierMovement mainScript = other.gameObject.GetComponent<SoldierMovement>();
            mainScript.damageSoldier(baseDamage, transform.right, bulletKnockback);
            enemyHit = true;
        } else if (other.gameObject.layer == 13) {
            DropperBehavior mainScript = other.gameObject.GetComponent<DropperBehavior>();
            mainScript.DamageDropper(baseDamage);
            enemyHit = true;
        } else if (other.gameObject.layer == 8) {
            Bird_Behavior mainScript = other.gameObject.GetComponent<Bird_Behavior>();
            mainScript.DamageBird(baseDamage);
            enemyHit = true;
        } else if (other.gameObject.layer == 14) {
            Boss mainScript = other.gameObject.GetComponent<Boss>();
            mainScript.DamageBoss(baseDamage);
            enemyHit = true;
        } else {
            if (genericHitParticle) {
                GameObject newEffect = GameObject.Instantiate(genericHitParticle, transform.position, transform.rotation);
                newEffect.GetComponent<ParticleSystem>().Play();
                Destroy(newEffect, 3);
            }
        }

        if (enemyHit && enemyHitParticle) {
            GameObject newEffect = GameObject.Instantiate(enemyHitParticle, transform.position, transform.rotation);
            newEffect.GetComponent<ParticleSystem>().Play();
            Destroy(newEffect, 3);
        }
    }

    // Collisions
    void OnTriggerEnter2D(Collider2D other) {
        if (hitDebounce) { return; }
        hitDebounce = true;

        checkCollision(other);

        if (isRocket) {
            GameObject newExplosion = GameObject.Instantiate(explosion, transform.position, transform.rotation);
            ParticleSystem explosEffect = newExplosion.GetComponent<ParticleSystem>();
            RPGExplosion rpgExplos = explosEffect.GetComponent<RPGExplosion>();
            explosEffect.Play();
            StartCoroutine(rpgExplos.Explode());
            Destroy(newExplosion, explosEffect.main.duration);
        }

        Destroy(gameObject);
    }
}
