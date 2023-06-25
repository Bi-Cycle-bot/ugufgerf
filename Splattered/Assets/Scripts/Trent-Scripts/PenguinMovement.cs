// Trent Lucas
// 25 June 2023

// Penguin movement class
// Lerps the penguin towards the player in constant time
///////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinMovement : MonoBehaviour
{
    public Transform target;
    public float lerpSpeed;

    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;

    // Set rigidbody
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Check if penguin is in or out of player range
    private void Update()
    {
            Vector2 newTarget = new Vector2(target.position.x - 1, target.position.y + 1);
            // out of player range(teleport to player)
            if (checkRange(newTarget)) {
                Vector2 teleport = new Vector2(newTarget.x + 1f, newTarget.y - 1f);
                rb.position = teleport;
            } 

            // in player range(lerp to player)
            else {
                Vector3 newPosition = Vector3.SmoothDamp(rb.position, newTarget, ref velocity, lerpSpeed * Time.deltaTime);
                rb.MovePosition(newPosition);
            }
    }

    // Update() helper function to check if penguin is in player range
    private bool checkRange(Vector2 newTarget) {
        if ((rb.position.x < newTarget.x - 10 || rb.position.x > newTarget.x + 10) || (rb.position.y < newTarget.y - 10 || rb.position.y > newTarget.y + 10)) {
            return true;
        } else {
            return false;
        }
    }
}
