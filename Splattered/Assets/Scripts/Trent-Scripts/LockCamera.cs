using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCamera : MonoBehaviour
{
    public GameObject player;
    public Vector2 newMinLimits;
    public Vector2 newMaxLimits;

    private Camera cam;
    private CameraManager camManager;
    private Vector2 oldMinLimits;
    private Vector2 oldMaxLimits;

    void Start() {
        cam = gameObject.GetComponent<Camera>();
        camManager = cam.GetComponent<CameraManager>();
        oldMinLimits = camManager.minLimits;
        oldMaxLimits = camManager.maxLimits;
    }

    void Update()
    {
    
        // floor 1 checker
        if (player.transform.position.y < 7) {
            camManager.minLimits = oldMinLimits;
            camManager.maxLimits = oldMaxLimits;
        }

        //floor 2 checker
        if (player.transform.position.y >= 7 && player.transform.position.y < 16) {
            Vector2 floor2 = new Vector2(1, 10);
            camManager.minLimits = floor2;
        }

        //floor 3 checker
        if (player.transform.position.y >= 16) {
            Vector2 floor3 = new Vector2(1, 19);
            camManager.minLimits = floor3;
        }

        // vertical room checker
        if (player.transform.position.x > 56) {
            camManager.minLimits = newMinLimits;
            camManager.maxLimits = newMaxLimits;
            camManager.mouseFactor = 0.0f;
            camManager.mouseLimit = 0.0f;
        }
    }
}
