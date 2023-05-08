using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D hitbox;
    [Header("Player Reference")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerMovement playerMovement;
    [Space(10)]
    [Header("Bullet Parameters")]
    public float speed = 10f;
    public float damage = 1;
    public Vector2 knockbackDirection;
    public float knockbackForce = 10f;
    public float stunDuration = 0.2f;
    public bool isUsingDirectionalKnockback = false;
    public float lifeSpan = 1.3f;
    private float timeOfCreation;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<BoxCollider2D>();
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
        Vector3.RotateTowards(transform.position, player.transform.position, 2*Mathf.PI, 100);
        timeOfCreation = Time.time;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && playerMovement.damagePlayer(damage, stunDuration, transform.position, knockbackForce, false) || other.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
        Debug.Log("Bullet hit player");

    }

    void Update()
    {
        rb.velocity = (player.transform.position - transform.position).normalized * speed;
        if(Time.time - timeOfCreation >= lifeSpan)
            Destroy(gameObject);
    }
}
