using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{

    public GameObject player;
    public RabbitBoss boss;
    public Transform spawnPoint;
    public Vector2 minXY;
    public Vector2 maxXY;
    private bool spawned;
    private RabbitBoss bossCopy;
    public GameObject door;

    void Start() {
        spawned = false;
    }

    void Update()
    {
        if ((player.transform.position.x > minXY[0] && player.transform.position.x < maxXY[0]) && (player.transform.position.y > minXY[1] && player.transform.position.y < maxXY[1]) && spawned == false) {
            bossCopy = Instantiate(boss, spawnPoint.transform.position, Quaternion.identity);
            spawned = true;
        }
        if (bossCopy != null && bossCopy.currentHealth < 0) {
            door.SetActive(false);
        }
    }
}
