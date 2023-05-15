using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{

    public GameObject player;
    public GameObject door;
    public Vector2 position;

    void Update()
    {
        if (player.transform.position.x > position[0] && player.transform.position.y > position[1]) {
            door.SetActive(true);
        } else {
            door.SetActive(false);
        }
    }
}
