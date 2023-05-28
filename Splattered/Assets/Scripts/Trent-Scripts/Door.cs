using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for door trigger detection
// Contains collsion for a open/close point and a reset point
////////////////////////////////////////////////////////////////

public class Door : MonoBehaviour
{
    public GameObject player;
    public GameObject door;
    public bool isClosed;
    public Vector2 startMinXY;
    public Vector2 startMaxXY;
    public bool resetAtPoint;

    [HideInInspector] public Vector2 endMinXY;
    [HideInInspector] public Vector2 endMaxXY;

    void Start() {
        if (isClosed == true) {
            door.SetActive(true);
        } else { 
            door.SetActive(false);
        }
    }

    void Update()
    {
        if ((player.transform.position.x > startMinXY[0] && player.transform.position.x < startMaxXY[0]) && (player.transform.position.y > startMinXY[1] && player.transform.position.y < startMaxXY[1])) {
            if (isClosed == true) {
                door.SetActive(false);
            } else { 
                door.SetActive(true);
            }
        }

        if ((player.transform.position.x > endMinXY[0] && player.transform.position.x < endMaxXY[0]) && (player.transform.position.y > endMinXY[1] && player.transform.position.y < endMaxXY[1])) {
            if (isClosed == true) {
                door.SetActive(true);
            } else { 
                door.SetActive(false);
            }
        }
    }
}
