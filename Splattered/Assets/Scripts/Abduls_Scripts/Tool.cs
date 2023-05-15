/*
------------------------------------------
By: Abdul Ahad Naveed
Created: 5/6/2023
Updated: 5/6/2023 @ 4:46 pm

Used to be a tool with unequip/equip functionalities
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour {
    // Public Variables
    [Header("Tool Settings")]
    public float toolLength; // Lenght of the tool
    public float yAdjust; // Adjust this to lower the tool to match any barrels if you need it
    public Vector2 leftHandGrip; // Where you want the left hand to be on the tool
    public Vector3 leftHandGripRotOffset = new Vector3(0, 0, 0); // Probably dont need to change this
    public bool equipped = false;
    

    // Must-Have Methods
    public abstract void unequip(); // Unequipping the tool

    public abstract void equip(); // Equipping the tool
}
