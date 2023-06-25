// Trent Lucas
// 25 June 2023

// Script animator for the spring pad
/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPadSpriteChange : MonoBehaviour
{

    public Animator anim;

    // Get the animator object of the spring pad
    void Start() {
        anim = GetComponent<Animator>();
    }

    // When an object has collided with the spring pad
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            anim.SetBool("onSpring", true);
        }
    }

    // When an object has stopped colliding with the spring pad
    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            anim.SetBool("onSpring", false);
        }
    }
}
