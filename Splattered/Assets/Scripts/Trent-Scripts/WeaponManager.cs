using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public SpriteRenderer playerWeapon;
    public Sprite gunSprite;
    public Sprite grenadeSprite;
    public Sprite skillSprite;

    void Start()
    {
        playerWeapon.sprite = gunSprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Gun")
        {
            playerWeapon.sprite = gunSprite;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Grenade")
        {
            playerWeapon.sprite = grenadeSprite;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Skill")
        {
            playerWeapon.sprite = skillSprite;
            Destroy(collision.gameObject);
        }
    }
}
