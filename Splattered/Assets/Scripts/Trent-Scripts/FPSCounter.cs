using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class FPSCounter : MonoBehaviour
{
 
    public float timer, refresh, avgFramerate;
    string display = "{0} FPS";
    public TextMeshProUGUI fpsCounterText;

    void Awake () {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 300;
    }
 
    private void Update()
    {
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;
 
        if(timer <= 0) avgFramerate = (int) (1f / timelapse);
        fpsCounterText.text = string.Format(display,avgFramerate.ToString());
    }
}