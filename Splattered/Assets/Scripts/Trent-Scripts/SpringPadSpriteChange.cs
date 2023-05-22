using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPadSpriteChange : MonoBehaviour
{

    public Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            anim.SetBool("onSpring", true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            anim.SetBool("onSpring", false);
        }
    }
}
