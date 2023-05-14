using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    [Header("Boss Settings")]
    [HideInInspector]public GameObject target;
    [HideInInspector] public PlayerMovement playerMovement;
    public float maxHealth;
    public float currentHealth;

    
    public abstract void DamageBoss(float damageAmount);

}
