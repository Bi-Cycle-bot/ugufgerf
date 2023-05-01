/*
------------------------------------------
By: Abdul Ahad Naveed
Created: 4/29/2023
Updated: 4/29/2023 @ 11:30 pm

Represents an oscillation
Learn the sine wave if you want to be a pro at using this
*/
using UnityEngine;

public class Oscillation : MonoBehaviour {
    // Public Variables
    public bool destroyOnIdle = true; // If set to true and if either stretch or period become 0, it will destroy the oscillation
    public bool idle = false; // Haults the oscillation. Can set this after the creation of the oscillation to pause the oscillation.
    public int updateTick = 2; // Every # of ticks it takes before values get updated

    // Private Variables
    private float value = 0f;
    private int currUpdateTick = 0;
    private int tick = 0; // The current tick the oscillation is at

    // Sine Wave Variables
    public float stretch = 1f;
    public float period = 1f;
    public float yIntercept = 0f;
    public float stretchDecay = .3f;
    public float periodDecay = .3f;

    // Update is called once per frame
    void Update() {
        // Checking if idling
        if (idle) { 
            if (destroyOnIdle) {
                Destroy(this);
            }
            return; 
        }

        float realTick = tick * (Mathf.PI / 32);

        // Updating oscillation
        float newStretch = 0f;
        float newPeriod = 0f;
        if (stretchDecay != 1) {
            newStretch = stretch * Mathf.Pow((1 - stretchDecay), realTick);
        }
        if (periodDecay != 1) {
            newPeriod = period * Mathf.Pow((1 - periodDecay), realTick);
        }
        
        value = newStretch * Mathf.Sin(realTick * newPeriod) + yIntercept;

        // Updating the values
        if (currUpdateTick >= updateTick) {
            tick++;
            currUpdateTick = 0;
        } else {
            currUpdateTick++;
        }

        // Checking for idle
        if ((newStretch <= 0.001) || (newPeriod <= 0.001)) {
            value = 0f;
            idle = true;
        }
    }

    // Returns the current value of the oscillation
    public float getValue() { return value; }

    // Sets the update tick. Smaller values will make the effect "smoother", but faster.
    // A big value can have its place just experiment to see what you like.
    public void setUpdateTick(int newTick) { updateTick = newTick; }

    // Sets the sine wave variables y = asin(bx) + c
    // a is the stretch, b is the period, and c is the yIntercept
    // for most cases, c should be 0.
    public void setSineWaveValues(float a, float b, float c) {
        stretch = a;
        period = b;
        yIntercept = c;
    }

    // Sets the decay values. Higher decay values means the stretch and/or period will decay faster
    public void setDecayValues(float a, float b) {
        stretchDecay = a;
        periodDecay = b;
    }

    // Resets the oscillation
    public void reset() {
        tick = 0;
        currUpdateTick = 0;
        idle = false;
    }
}