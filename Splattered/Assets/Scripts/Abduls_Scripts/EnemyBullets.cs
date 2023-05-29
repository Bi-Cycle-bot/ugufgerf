using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullets : MonoBehaviour {
    // THE DAMAGE AND VELOCITY VALUES WILL BE CHANGED VIA GUN SYSTEM

    // Bullet Settings
    [Header("Settings")]
    public float bulletVelocity = 5f; // Speed of the bullet
    public float baseDamage = 30f; // Base damage the gun will do
    public float stunTime = 0.1f;
    public float knockbackForce = 10f;
    public float horizontalForceMultiplier = 3f;

    // Debounces
    private bool hitDebounce = false;
    private GameObject player;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        transform.right = (Vector2)player.transform.position - (Vector2)transform.position;
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
        if (other.gameObject.layer == 3) {
            playerMovement.damagePlayer(baseDamage, stunTime, transform.right, knockbackForce, true, horizontalForceMultiplier);
        }
        // if(other.tag != "Enemy" && other.tag != "EnemyBullet" && other.tag != "OneWayGround")
        Destroy(gameObject);
    }
}
