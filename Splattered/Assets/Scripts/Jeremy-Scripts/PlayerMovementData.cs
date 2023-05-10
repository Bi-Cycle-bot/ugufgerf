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

    [Space(4)]
    [Range(0.01f, 1)] public float accelInAir; //Multipliers applied to acceleration rate when airborne.
    [Range(0.01f, 1)] public float deccelInAir;

    [Space(10)]
    [Header("Jump")]
    public float jumpHeight;
    public float jumpToApexTime;
    [Range(0.01f, 0.1f)]public float coyoteTime;
    [HideInInspector] public float jumpForce;

    [Space(10)]
    [Header("Slide")]
    // public float slideDistance;
    public bool slideStopsYMovement;
    public float slideTime;
    public float slideSpeed;
    public float slideCoolodown;
    public float slideAccelMultiplier;
    
    [HideInInspector] public float lastSlideTime;
    [HideInInspector] public float lastSlideTimeStart;

    [Space(10)]
    [Header("Gravity")]
    [HideInInspector] public float gravityStrength;
    [HideInInspector] public float gravityScale;
    public float fallGravityMultiplier;
    public float maxFallSpeed;
    [Space(10)]

    [Header("Health")]
    public float maxHealth = 20;
    public float invincibilityTime = 1.1f;
    [Range(0.01f, 0.3f)]public float hitFlashTime;



    // Runs when a value is changed in the inspector
    private void OnValidate()
    {
        #region Run Calculations
        // Calculate acceleration and deceleration amounts
        runAccelAmount = (50f * accelerationTime) / maxSpeed;
        runDeccelAmount = maxSpeed / decelerationTime;
        #endregion

        #region Gravity Calculations
        gravityStrength = -(2 * jumpHeight) / (jumpToApexTime * jumpToApexTime);
        gravityScale = gravityStrength / Physics2D.gravity.y;
        #endregion

        #region Jump Calculations
        jumpForce = Mathf.Abs(gravityStrength) * jumpToApexTime;
        #endregion

        #region Slide Calculations
        // slideTime =  slideDistance / slideSpeed;
        #endregion
    }
}
