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
    [Space(10)]

    [Header ("Attack")]
    public float attackRange;
    public float attackSpeed;
    public float attackDamage;

    void OnValidate()
    {
        accelAmount = maxSpeed / accelerationTime;
        slowDistance = attackRange;
    }


}
