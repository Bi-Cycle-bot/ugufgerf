// Trent Lucas
// 25 June 2023

// FPS on screen display
//////////////////////////

using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class FPSCounter : MonoBehaviour
{
 
    public float timer, refresh, avgFramerate;
    string display = "{0} FPS";
    public TextMeshProUGUI fpsCounterText;

    // Sets framerate settings
    void Awake () {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 300;
    }
 
    // Updates framerate average after each frame
    private void Update()
    {
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;
 
        if(timer <= 0) avgFramerate = (int) (1.0f / timelapse);
        fpsCounterText.text = string.Format(display,avgFramerate.ToString());
    }
}