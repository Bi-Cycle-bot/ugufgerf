using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinMovement : MonoBehaviour
{
    public Transform target;
    public float lerpSpeed;

    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
            Vector2 newTarget = new Vector2(target.position.x - 1, target.position.y + 1);
            if (checkRange(newTarget)) {
                Vector2 teleport = new Vector2(newTarget.x + 1f, newTarget.y - 1f);
                rb.position = teleport;
            } else {
                Vector3 newPosition = Vector3.SmoothDamp(rb.position, newTarget, ref velocity, lerpSpeed * Time.deltaTime);
                rb.MovePosition(newPosition);
            }
    }

    bool checkRange(Vector2 newTarget) {
        if ((rb.position.x < newTarget.x - 10 || rb.position.x > newTarget.x + 10) || (rb.position.y < newTarget.y - 10 || rb.position.y > newTarget.y + 10)) {
            return true;
        } else {
            return false;
        }
    }
}
