using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{

    public float bounceForce;
    [SerializeField] private PlayerMovement player;
    // Start is called before the first frame update

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
           player.knockbackFromPosition(transform.position, 50f);
           Debug.Log("Bounce");
        }
    }
}
