using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunData : MonoBehaviour
{
    #region Gun Stats
    [Header("Gun Stats")]
    public float fireRate;
    public float damage;
    public float range;
    public float bulletSpeed;
    public bool isHitscan;
    public int numberOfBullets;
    public float spread;
    public bool knockback;
    public int springsLeft, springsUsed;
    #endregion
    [HideInInspector] public bool shooting, reloading;
    [HideInInspector] public float bulletLifeTime;
    [HideInInspector] public float timeBetweenShots;
    [HideInInspector] public float timeBetweenShooting;

    //All references
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    
    //Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI text;
    

    void OnValidate(){
        bulletLifeTime = range / bulletSpeed;
        timeBetweenShots = 1 / fireRate;

    }

    
}
