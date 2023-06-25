// Trent Lucas
// 25 June 2023

// Script animator for the green tank decor
/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeTankSprite : MonoBehaviour
{
    public Animator anim;
    private int num;

    // Get the animator object of the tank
    // and start the counter at 0
    void Start()
    {
        anim = GetComponent<Animator>();
        num = 0;
    }

    // Numerate through each animation sprite
    void FixedUpdate()
    {
        if (num < 5)
        {
            num++;
        }
        else
        {
            num = 0;
        }

        // change to next sprite
        anim.SetInteger("changeTank", num);
    }
}
