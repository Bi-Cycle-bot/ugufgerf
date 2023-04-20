using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementData : MonoBehaviour
{
    [Header("run")]
    public float maxSpeed; //target speed player should reach
    public float accelerationTime; //time it should take for player to accelerate from 0 to max speed\
    [HideInInspector] public float runAccelAmount; //force applied to player to accelerate
    public float decelerationTime; //time it should take for player to decelerate from max speed to 0
    [HideInInspector] public float runDeccelAmount; //force applied to player to decelerate

    [Space(10)]
    [Range(0.01f, 1)] public float accelInAir; //Multipliers applied to acceleration rate when airborne.
    [Range(0.01f, 1)] public float deccelInAir;

    // Runs when a value is changed in the inspector
    private void OnValidate()
    {
        #region calculaitons
        // Calculate acceleration and deceleration amounts
        runAccelAmount = (50f * accelerationTime) / maxSpeed;
        runDeccelAmount = maxSpeed / decelerationTime;
        #endregion
    }
}
