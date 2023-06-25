// Trent Lucas
// 25 June 2023

// Manages the light flickering
/////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerController : MonoBehaviour
{

    public bool isFlickering = false;
    public float delay;
    private float value;

    // Checks if light should flicker
    void Update()
    {
        if (isFlickering == false) {
            StartCoroutine(FlickeringLight());
        }
    }

    // Looped function to flicker light on and off
    IEnumerator FlickeringLight() {
        isFlickering = true;

        value = Random.Range(0.8f, 0.9f);
        this.gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = value;
        delay = Random.Range(0.04f, 0.08f);
        yield return new WaitForSeconds(delay);

        value = Random.Range(0.8f, 0.9f);
        this.gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = value;
        delay = Random.Range(0.04f, 0.08f);
        yield return new WaitForSeconds(delay);

        isFlickering = false;
    }
}
