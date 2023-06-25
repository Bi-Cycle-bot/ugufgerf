// Trent Lucas
// 25 June 2023

// Locks the camera's boundaries based on world coordinates 
// Quality of life effect for camera movement
//////////////////////////////////////////////////////////////

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
    private SpawnPoint spawnPoint;

    // Set default constraints
    void Start() {
        cam = gameObject.GetComponent<Camera>();
        camManager = cam.GetComponent<CameraManager>();
        oldMinLimits = camManager.minLimits;
        oldMaxLimits = camManager.maxLimits;
    }

    // Checks if player is in a given zone
    void Update()
    {
        // floor 1 checker
        if ((player.transform.position.x >= -208 && player.transform.position.x < -3) && player.transform.position.y < -2) {
            Vector2 floor1 = new Vector2(-194, -17);
            Vector2 floor12 = new Vector2(0, 0);
            camManager.minLimits = floor1;
            camManager.maxLimits = floor12;
        }

        // floor 2 checker
        else if ((player.transform.position.y >= 7 && player.transform.position.y < 16) && player.transform.position.x <= 51) {
            Vector2 floor2 = new Vector2(5, 11);
            camManager.minLimits = floor2;
        }

        // floor 3 checker
        else if (player.transform.position.y >= 16 && player.transform.position.x <= 55) {
            Vector2 floor3 = new Vector2(5, 11);
            camManager.minLimits = floor3;
        }

        // vertical room checker
        else if ((player.transform.position.x >= 56 && player.transform.position.x <= 72) && player.transform.position.y >= 16) {
            camManager.minLimits = newMinLimits;
            camManager.maxLimits = newMaxLimits;
            camManager.mouseFactor = 0.0f;
            camManager.mouseLimit = 0.0f;
        }

        // floor4 checker
        else if ((player.transform.position.x >= 72 && player.transform.position.x <= 1000) && player.transform.position.y >= 88) {
            Vector2 floor4 = new Vector2(1, 91);
            Vector2 floor42 = new Vector2(1000, 1000);
            camManager.minLimits = floor4;
            camManager.maxLimits = floor42;
            camManager.mouseFactor = 0.15f;
            camManager.mouseLimit = 3.0f;
        }

        // vent checker
        else if ((player.transform.position.x >= 82 && player.transform.position.x <= 101) && (player.transform.position.y >= 22 && player.transform.position.y <= 23)) {
            cam.orthographicSize = 3.0f;
        }
        else if ((player.transform.position.x >= 82 && player.transform.position.x <= 83) && (player.transform.position.y >= 18 && player.transform.position.y <= 23)) {
            cam.orthographicSize = 3.0f;
        }
        else if ((player.transform.position.x >= 82 && player.transform.position.x <= 90) && (player.transform.position.y >= 18 && player.transform.position.y <= 19)) {
            cam.orthographicSize = 3.0f;
        }
        else if ((player.transform.position.x >= 89 && player.transform.position.x <= 90) && (player.transform.position.y >= 15 && player.transform.position.y <= 19)) {
            cam.orthographicSize = 3.0f;
        }
        else if ((player.transform.position.x >= 87 && player.transform.position.x <= 90) && (player.transform.position.y >= 15 && player.transform.position.y <= 16)) {
            cam.orthographicSize = 3.0f;
        }
        else if ((player.transform.position.x >= 87 && player.transform.position.x <= 88) && (player.transform.position.y >= 15 && player.transform.position.y <= 17)) {
            cam.orthographicSize = 3.0f;
        }
        else if ((player.transform.position.x >= 75 && player.transform.position.x <= 88) && (player.transform.position.y >= 16 && player.transform.position.y <= 17)) {
            cam.orthographicSize = 3.0f;
        }
        else if ((player.transform.position.x >= 75 && player.transform.position.x <= 76) && (player.transform.position.y >= 13 && player.transform.position.y <= 17)) {
            cam.orthographicSize = 3.0f;
        }
        else if ((player.transform.position.x >= 50 && player.transform.position.x <= 76) && (player.transform.position.y >= 13 && player.transform.position.y <= 14)) {
            cam.orthographicSize = 3.0f;
        }

        // set default settings/floor 1 checker
        else {
            camManager.minLimits = oldMinLimits;
            camManager.maxLimits = oldMaxLimits;
            cam.orthographicSize = 7.0f;
        }
    }
}
