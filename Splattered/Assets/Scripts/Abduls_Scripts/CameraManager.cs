/*
------------------------------------------
By: Abdul Ahad Naveed
Created: 4/29/2023
Updated: 4/29/2023 @ 11:30 pm

Handles camera manipulation
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    // Public Variables
    public GameObject player;
    public float dampTime = .23f; // Time it takes for the camera to reach a position (longer = slower)
    public Vector2 minLimits; // The lower bound values for x and y the camera will never pass
    public Vector2 maxLimits; // The upper bound values for x and y the camera will never pass

    // Private Variables
    private Transform plrTrans; 
    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    private List<Oscillation> sizeOsc = new List<Oscillation>();
    private float baseSize; // The main/base size of the camera.

    // Start is called before the first frame update
    void Start() {
        // Setting variables
        plrTrans = player.GetComponent<Transform>();
        cam = gameObject.GetComponent<Camera>();
        baseSize = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update() {
        // Player Position
        float xPlayerPos = plrTrans.position.x;
        float yPlayerPos = plrTrans.position.y;

        // Setting up a new camera position
        float xCamPos = Mathf.Clamp(xPlayerPos, minLimits.x, maxLimits.x);
        float yCamPos = Mathf.Clamp(yPlayerPos, minLimits.y, maxLimits.y);
        
        // Updating Oscillations
        float newSize = baseSize;
        for (int i = 0; i < sizeOsc.Count; i++) {
            if (sizeOsc[i] == null) {
                sizeOsc.RemoveAt(i);
            } else {
                newSize += sizeOsc[i].getValue();
            }
        }

        // Setting Camera Values
        Vector3 newCamPos = new Vector3(xCamPos, yCamPos, -10f);
        cam.orthographicSize = newSize;
        transform.position = Vector3.SmoothDamp(gameObject.transform.position, newCamPos, ref velocity, dampTime);
    }

    // Add an oscillation effect to the size of the camera
    public void addSizeOscillation(Oscillation newOscillation) { sizeOsc.Add(newOscillation); }

    // Sets the basesize
    public void setCameraSize(float size) { baseSize = size; }
}
