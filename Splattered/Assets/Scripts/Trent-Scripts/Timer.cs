// Trent Lucas
// 25 June 2023

// Script for on screen timer
////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{

    [Header("Component")]
    public TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    public float currentTime;
    public bool countDown;
    private bool continueTimer;

    // Start Timer
    void Start() {
        continueTimer = true;
    }

    // Check if timer should stop
    void Update()
    {
        if (continueTimer == true) {
            currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;
            timerText.text = currentTime.ToString("0.00");
        } else {
            return;
        }
    }

    // Stop timer
    public void StopTimer() {
        continueTimer = false;
    }
}
