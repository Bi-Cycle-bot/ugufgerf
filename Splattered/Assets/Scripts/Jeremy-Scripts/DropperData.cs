using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperData : MonoBehaviour
{
    public enum Direction {Left, Right};
    public float maxSpeed = 8;
    public float dropRange = 2;
    public float dropCooldown = 3;
    public bool destroyOnDrop = true;
    public bool dropOnDeath = false;
    public float maxHealth = 50;
    public Direction direction;
}
