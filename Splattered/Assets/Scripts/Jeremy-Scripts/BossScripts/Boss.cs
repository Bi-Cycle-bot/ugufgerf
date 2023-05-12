using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    [Header("Boss Settings")]
    public GameObject target;
    public float maxHealth;
    public float currentHealth;

    
    public abstract void DamageBoss(float damageAmount);

}
