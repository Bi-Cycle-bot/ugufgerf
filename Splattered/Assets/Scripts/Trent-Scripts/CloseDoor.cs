using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{

    public GameObject player;
    public GameObject door;

    void Update()
    {
        if (player.transform.position.x > 56) {
            door.SetActive(true);
        } else {
            door.SetActive(false);
        }
    }
}
