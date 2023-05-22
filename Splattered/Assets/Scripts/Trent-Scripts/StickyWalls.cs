using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyWalls : MonoBehaviour
{

    /*public GameObject player;
    public GameObject wall;
    public PhysicsMaterial2D newMaterial;
    private PhysicsMaterial2D oldMaterial;
    public Vector2 vel;
    public Vector2 vel2;

    // Start is called before the first frame update
    void Start()
    {
        oldMaterial = player.GetComponent<Rigidbody2D>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Rigidbody2D>().velocity == vel2) {
            //player.GetComponent<Rigidbody2D>().grav = vel;
            player.GetComponent<Rigidbody2D>().velocity = vel;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player.GetComponent<Rigidbody2D>().sharedMaterial = newMaterial;
            player.GetComponent<BoxCollider2D>().sharedMaterial = newMaterial;
            wall.GetComponent<Rigidbody2D>().sharedMaterial = newMaterial;
            wall.GetComponent<Collider2D>().sharedMaterial = newMaterial;

            player.GetComponent<Rigidbody2D>().velocity = vel;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player.GetComponent<Rigidbody2D>().sharedMaterial = oldMaterial;
            player.GetComponent<BoxCollider2D>().sharedMaterial = oldMaterial;
            wall.GetComponent<Rigidbody2D>().sharedMaterial = oldMaterial;
            wall.GetComponent<Collider2D>().sharedMaterial = oldMaterial;

            player.GetComponent<Rigidbody2D>().velocity = vel2;
            
        }
    }*/
}
