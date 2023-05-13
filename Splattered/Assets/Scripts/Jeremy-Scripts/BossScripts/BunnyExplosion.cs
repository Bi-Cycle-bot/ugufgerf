using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyExplosion : MonoBehaviour
{
    [HideInInspector] public float jumpAttackExplosionDamage;
    [HideInInspector] public float jumpAttackExplosionKnockback;
    [HideInInspector] public float jumpAttackExplosionStunDuration;
    [HideInInspector] public bool isExploding;
    private RabbitBoss rabbitBoss;
    private Collider2D hitbox;
    private ParticleSystem explosionParticles;
    private PlayerMovement playerMovement;
    void Start()
    {
        rabbitBoss = GetComponentInParent<RabbitBoss>();
        jumpAttackExplosionDamage = rabbitBoss.jumpAttackExplosionDamage;
        jumpAttackExplosionKnockback = rabbitBoss.jumpAttackExplosionKnockback;
        jumpAttackExplosionStunDuration = rabbitBoss.jumpAttackExplosionStunDuration;
        hitbox = GetComponent<Collider2D>();
        hitbox.enabled = false;
        explosionParticles = GetComponent<ParticleSystem>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    public IEnumerator Explode()
    {
        isExploding = true;
        hitbox.enabled = true;
        explosionParticles.Play();
        yield return new WaitForSeconds(0.1f);
        hitbox.enabled = false;
        isExploding = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerMovement.damagePlayer(jumpAttackExplosionDamage, jumpAttackExplosionStunDuration, transform.position, jumpAttackExplosionKnockback, false);
        }
    }


}
