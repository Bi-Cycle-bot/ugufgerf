using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class despawner : MonoBehaviour
{
    float spawnTime;
    float despawnTime = 3f;
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - spawnTime > despawnTime)
            Destroy(gameObject);
    }
}
