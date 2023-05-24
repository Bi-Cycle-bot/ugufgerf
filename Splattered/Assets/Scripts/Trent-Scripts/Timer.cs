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

    void Start() {
        continueTimer = true;
    }

    void Update()
    {
        if (continueTimer == true) {
            currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;
            timerText.text = currentTime.ToString("0.00");
        } else {
            return;
        }
    }

    public void StopTimer() {
        continueTimer = false;
    }
}
