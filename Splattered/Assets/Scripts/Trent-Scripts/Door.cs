// Trent Lucas
// 25 June 2023

// Script for door trigger detection
// Contains collision for a open/close point and a reset point
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [HideInInspector] public bool playerDied;

    // Set door's start position(open/closed)
    void Start() {
        door.SetActive(isClosed);
    }

    // Check if the player is in the start or end zone
    void Update()
    {
        // if player is in start zone
        if ((player.transform.position.x > startMinXY[0] && player.transform.position.x < startMaxXY[0]) && (player.transform.position.y > startMinXY[1] && player.transform.position.y < startMaxXY[1])) {
            door.SetActive(!isClosed);
        }

        // if player is in end zone
        if (((player.transform.position.x > endMinXY[0] && player.transform.position.x < endMaxXY[0]) && (player.transform.position.y > endMinXY[1] && player.transform.position.y < endMaxXY[1]))) {
            door.SetActive(isClosed);
        }
    }

    // Reset door
    public void reset() {
        Start();
    }
}
