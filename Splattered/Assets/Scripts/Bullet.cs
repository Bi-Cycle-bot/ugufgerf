using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float bulletSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        bulletFire();
    }

    // Update is called once per frame
    void Update()
    {

        
        //transform.position += transform.right * (bulletSpeed * Time.smoothDeltaTime);
        
        /*
        if (GlobalBehavior.sTheGlobalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds) == GlobalBehavior.WorldBoundStatus.Outside)
        {
            Destroy(gameObject);  // this.gameObject, this is destroying the game object
        }
        */
    }

    void bulletFire(){

        // get mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        
        // get direction vector from bullet to mouse
        Vector3 direction = (mousePos - transform.position).normalized;

        // set the bullet velocity to match the direction and speed
        GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        // rotate the bullet to face the mouse position
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }
}
