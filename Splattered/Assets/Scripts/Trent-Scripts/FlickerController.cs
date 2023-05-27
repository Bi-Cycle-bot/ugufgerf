using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerController : MonoBehaviour
{

    public bool isFlickering = false;
    public float delay;
    private float value;

    void Update()
    {
        if (isFlickering == false) {
            StartCoroutine(FlickeringLight());
        }
    }

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
