// Trent Lucas
// 25 June 2023

// Volume Manager
/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Volume : MonoBehaviour
{
    
    public AudioMixer mixer;

    // Set Music Volume Level
    public void SetLevel(float value) {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }
}
