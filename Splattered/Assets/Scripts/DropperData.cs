using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperData : MonoBehaviour
{
    public enum Direction {Left, Right};
    public float maxSpeed;
    public float dropRange;
    public float dropCooldown;
    public bool destroyOnDrop;
    public bool dropOnDeath;
    public Direction direction;
}
