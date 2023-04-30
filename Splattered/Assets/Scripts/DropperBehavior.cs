using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperBehavior : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject dropObject;
    private DropperData Data;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Data = GetComponent<DropperData>();
    }

    void FixedUpdate()
    {
        Move();
        if(transform.position.x - target.transform.position.x < Data.dropRange )
        {
            Drop();
            if(Data.destroyOnDrop)
            {
                Destroy(gameObject);
            }
        }
        
    }

    void Drop()
    {
        Instantiate(dropObject, transform.position, Quaternion.identity);
    }

    void Move()
    {
        rb.velocity = (Data.direction == DropperData.Direction.Left) ? Vector2.left * Data.maxSpeed : Vector2.right * Data.maxSpeed;
    }
}
