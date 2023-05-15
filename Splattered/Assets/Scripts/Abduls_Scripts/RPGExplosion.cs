using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGExplosion : MonoBehaviour
{

    // Damage Settings
    [Header("Damage Settings")]
    public float baseDamage = 11f; // Base damage the gun will do
    public float headshotMultiplier = 2f; // Multiplier of the base damage for headshots

    [HideInInspector] public bool isExploding;
    private CircleCollider2D hitbox;
    private ParticleSystem explosionParticles;
    private PlayerMovement playerMovement;
    void Start()
    {
        hitbox = GetComponent<CircleCollider2D>();
        hitbox.enabled = true;
    }

    public IEnumerator Explode()
    {
        hitbox = GetComponent<CircleCollider2D>();
        explosionParticles = GetComponent<ParticleSystem>();
        isExploding = true;
        hitbox.enabled = true;
        explosionParticles.Play();
        yield return new WaitForSeconds(0.1f);
        hitbox.enabled = false;
        isExploding = false;
    }

    void checkCollision(Collider2D other) {
        if (other.gameObject.layer == 9) {
            SoldierMovement mainScript = other.gameObject.GetComponent<SoldierMovement>();
            mainScript.damageSoldier(baseDamage, transform.right, 10);
        } else if (other.gameObject.layer == 13) {
            DropperBehavior mainScript = other.gameObject.GetComponent<DropperBehavior>();
            mainScript.DamageDropper(baseDamage);
        } else if (other.gameObject.layer == 8) {
            Bird_Behavior mainScript = other.gameObject.GetComponent<Bird_Behavior>();
            mainScript.DamageBird(baseDamage);
        } else if (other.gameObject.layer == 14) {
            Boss mainScript = other.gameObject.GetComponent<Boss>();
            mainScript.DamageBoss(baseDamage);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        checkCollision(collision);
    }


}
