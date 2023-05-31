using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Volume : MonoBehaviour
{
    
    public AudioMixer mixer;
    private float valueRef;

    void Start() {
        valueRef = 1.0f;
    }

    public void SetLevel(float value) {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        valueRef = value;
    }

    void OnDisable() {
        PlayerPrefs.SetFloat("volume", valueRef);
        Debug.Log(valueRef);
    }
}
