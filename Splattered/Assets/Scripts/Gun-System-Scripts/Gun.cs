using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    bool readyToShoot;
    public GunData data;
    public GameObject pBullet;
    public float maxdown;
    public float cdown;
    
    private void Awake()
    {
        data.springsLeft = data.springMag;
        readyToShoot = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        if (pBullet == null)
            pBullet = Resources.Load("Prefabs/Bullet") as GameObject;
        */
        cdown = maxdown;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        if (readyToShoot && data.shooting && !data.reloading){
            Shoot();
        }
        */
        MyInput();
    }

    private void MyInput()
    {
        if (data.allowButtonHold) 
        {
            data.shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            data.shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        //Will shoot if requirements are met
        if (readyToShoot && data.shooting && !data.reloading && data.springsLeft > 0){
            data.springsUsed = data.springsPerTap;
            Shoot();
        }

        if(Input.GetKey(KeyCode.Mouse0) && cdown <= 0)
        {
            Instantiate(pBullet, transform.position, transform.rotation);
            cdown = maxdown;
        }
        if(cdown > 0)
        {
            cdown -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Mouse0))  // VS. GetKeyDown <<-- even, one per key press
        { // Mouse0 bar hit
            GameObject b = Instantiate(pBullet) as GameObject;
            Bullet bullet = b.GetComponent<Bullet>(); // Shows how to get the script from GameObject
            if (null != bullet)
            {
                b.transform.position = transform.position;
            }
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

        /*
        if (Physics.Raycast(fpsCam.transform.position, direction, out data.rayHit, range, whatIsEnemy))
        {
            Debug.Log(data.rayHit.collider.name);
            
            //May need to change later depending on Enemy tags
            if (data.rayHit.collider.CompareTag("Enemy"))
            {
                data.rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
            }
        }
        */

        //Decrement amount of springs left after each shot
        data.springsLeft--;
        data.springsUsed--;

        Invoke("ResetShot", data.timeBetweenShooting);

        if(data.springsLeft > 0 && data.springsUsed > 0)
        {
            Invoke("Shoot", data.timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        data.reloading = true;
        Invoke("ReloadFinished", data.reloadTime);
    }

    private void ReloadFinished()
    {
        data.springsLeft = data.springMag;
        data.reloading = false;
    }
}
