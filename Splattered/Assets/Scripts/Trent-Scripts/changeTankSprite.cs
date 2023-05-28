using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script animator for the green tank decor
//////////////////////////////////////////////////////////

public class changeTankSprite : MonoBehaviour
{
    public Animator anim;
    private int num;

    void Start() {
        anim = GetComponent<Animator>();
        num = 0;
    }

    void FixedUpdate() {
        if (num < 5) {
            num++;
        }
        else {
            num = 0;
        }
        
        // change to next sprite
        anim.SetInteger("changeTank", num);
    }
}
