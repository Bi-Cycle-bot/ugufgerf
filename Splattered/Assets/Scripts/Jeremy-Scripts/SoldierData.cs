using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierData : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed;
    public float accelerationTime;
    public float deccelerationTime;
    [Range(0.01f, 1)] public float slowSpeedMultiplier;
    [HideInInspector] public float accelAmount;
    public float slowDistance;
    [HideInInspector] public bool isUsingSlowSpeed;
    public Vector2 targetFollowRange = new Vector2(20, 20);
    [Space(10)]

    [Header ("Attack")]
    public float attackRange;
    public float attackSpeed;
    public float attackDamage = 1;
    public float stunDuration = 0.2f;
    public float knockbackForce = 20f;
    public Vector2 knockbackDirection;
    public bool hasDirectionalKnockback = false;

    [Space (10)]
    [Header ("Health")]
    public float maxHealth = 20;

    void OnValidate()
    {
        accelAmount = maxSpeed / accelerationTime;
        slowDistance = attackRange;
    }


}
