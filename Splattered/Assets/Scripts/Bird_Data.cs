using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_Data : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed;
    public float accelerationTime;
    public float deccelerationTime;
    [Range(0.01f, 1)] public float slowSpeedMultiplier;
    [HideInInspector] public float targetSpeed;
    [HideInInspector] public float accelAmount;
    [HideInInspector] public float deccelAmount;
    public float slowDistance;
    [HideInInspector] public bool isUsingSlowSpeed;
    public float heightAboveTarget;
    [HideInInspector] public Vector2 targetLocation;
    [Space(10)]

    [Header ("Attack")]
    public float attackRange;
    public float attackSpeed;
    public float attackDamage;
    void OnValidate()
    {
        accelAmount = maxSpeed / accelerationTime;
        deccelAmount = maxSpeed / deccelerationTime;
        slowDistance = attackRange - heightAboveTarget;
    }
}
