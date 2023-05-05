using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayCollider : MonoBehaviour
{
    private Collider2D hitbox;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Collider2D playerHitbox;
    [SerializeField] private Rigidbody2D platformRB;
    private float timeToGoTrough;
    private float timeSinceButton;

    void Start()
    {
        hitbox = GetComponent<Collider2D>();
        platformRB = GetComponent<Rigidbody2D>();

        timeToGoTrough = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.S))
        {
            timeSinceButton = Time.time;
        }
        if(playerRB.velocity.y > 0 || Time.time - timeSinceButton < timeToGoTrough)
        {
            Physics2D.IgnoreCollision(hitbox, playerHitbox, true);
        }
        else
        {
            Physics2D.IgnoreCollision(hitbox, playerHitbox, false);
        }
    }
}
