using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnBoss : MonoBehaviour
{

    public GameObject player;
    public RabbitBoss boss;
    public Transform spawnPoint;
    public Vector2 minXY;
    public Vector2 maxXY;
    private bool spawned;
    private RabbitBoss[] bossCopy;
    public GameObject door;
    public int bossCount;
    private bool[] bossCopyBool;
    public GameObject[] spawnpoints;

    void Start() {
        spawned = false;
        bossCopy = new RabbitBoss[bossCount];
        bossCopyBool = new bool[bossCount];
    }

    void Update()
    {
        if ((player.transform.position.x > minXY[0] && player.transform.position.x < maxXY[0]) && (player.transform.position.y > minXY[1] && player.transform.position.y < maxXY[1]) && spawned == false) {
            for (int i = 0; i < bossCopy.Length; i++) {
                bossCopy[i] = Instantiate(boss, spawnpoints[i].transform.position, Quaternion.identity);
            }
            spawned = true;
        }

        for (int i = 0; i < bossCopy.Length; i++) {
            if (bossCopy[i] != null && bossCopy[i].currentHealth < 0) {
                bossCopyBool[i] = true;
            }
        }

        if (Array.TrueForAll(bossCopyBool, trueCondition)) {
            door.SetActive(false);
        }


    }

    bool trueCondition(bool value) {
        return value == true;
    }
}
