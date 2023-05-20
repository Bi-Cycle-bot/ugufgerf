using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayCollider : MonoBehaviour
{
    private Collider2D hitbox;
    private Bounds hitboxBounds;
    private GameObject player;
    private Rigidbody2D playerRB;
    private Collider2D playerHitbox;
    private Bounds playerHitboxBounds;
    private Rigidbody2D platformRB;
    private float timeToGoTrough;
    private float timeSinceButton;
    public float upperY;
    public float bottomOfPlayer;
    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
        platformRB = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        playerRB = player.GetComponent<Rigidbody2D>();
        playerHitbox = player.GetComponent<Collider2D>();
        timeToGoTrough = 0.1f;
        hitboxBounds = hitbox.bounds;
        playerHitboxBounds = playerHitbox.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 globalPosition = transform.TransformVector(transform.position);
        Vector3 globalBounds = transform.TransformVector(hitboxBounds.extents);
        bottomOfPlayer = player.transform.position.y - playerHitboxBounds.extents.y;
        upperY = globalPosition.y/* + globalBounds.y*/;
        if(Input.GetKey(KeyCode.S))
        {
            timeSinceButton = Time.time;
        }
        if(/*playerRB.velocity.y > 0 ||*/ Time.time - timeSinceButton < timeToGoTrough ||
            player.transform.position.y - playerHitboxBounds.extents.y < globalPosition.y/* + globalBounds.y*/)
        {
            Physics2D.IgnoreCollision(hitbox, playerHitbox, true);
        }
        else
        {
            Physics2D.IgnoreCollision(hitbox, playerHitbox, false);
        }
    }
}
