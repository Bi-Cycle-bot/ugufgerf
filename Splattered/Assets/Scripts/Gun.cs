using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    bool readyToShoot;
    public GunData data;
    private void Awake()
    {
        readyToShoot = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (readyToShoot && data.shooting && !data.reloading){
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-data.spread, data.spread);
        float y = Random.Range(-data.spread, data.spread);
        Vector2 direction = (Vector2)Camera.main.WorldToScreenPoint(Input.mousePosition);
        direction.Normalize();
        

        Invoke("ResetShot", data.timeBetweenShooting);

        if(data.springsLeft > 0 && data.springsUsed > 0)
        Invoke("Shoot", data.timeBetweenShots);
    }
}
