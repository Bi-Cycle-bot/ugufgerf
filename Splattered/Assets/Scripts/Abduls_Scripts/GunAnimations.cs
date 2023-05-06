/*
------------------------------------------
By: Abdul Ahad Naveed
Created: 5/5/2023
Updated: 5/5/2023 @ 8:56 pm

Used to house the animation info for guns
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimations : MonoBehaviour {
    [System.Serializable]
    public struct keyframe {
        public Transform attachedObject; // Object that attaches to the left hand
        public Vector3 newLocalPos; // Position of the keyframe
        public Vector3 newLocalRot; // Rotation of the keyframe
        [Range(0, 1)]
        public float positionFactor; // Where in the animation the keyframe comes (start 0 to end 1)
        public bool inactive; // Determines if the transform can be seen
    }

    public HandManager handManager; // The hand manager this keyframe is acting upon
    public GameObject leftHand; // Left hand of the player
    public Transform gunTrans; // Gun of these animations

    // ---------------------- RELOAD VARIABLES ----------------------
    [Header("Reload Animation")]
    public List<keyframe> reload_frames; // Make sure the last keyframe contains the default position/rotation values!
    private float reload_currentTime = 0;
    private float reload_time = 1;
    private Vector3 reload_startPos;
    private Vector3 reload_startRot;
    private int reload_currentGoal = 0;
    private bool reloading = false;
    private Transform attachedObject;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (reloading) {
            // Getting Values
            Vector3 currentPos = reload_frames[reload_frames.Count - 1].newLocalPos;
            Vector3 currentRot = reload_frames[reload_frames.Count - 1].newLocalRot;
            float previousFrameTime = 0f;
            if (reload_currentGoal > 0) {
                currentPos = reload_frames[reload_currentGoal - 1].newLocalPos;
                currentPos = reload_frames[reload_currentGoal - 1].newLocalRot;
                previousFrameTime = reload_frames[reload_currentGoal - 1].positionFactor * reload_time;
            }
            float frameTime = reload_currentTime - previousFrameTime;
            float timeToMove = (reload_frames[reload_currentGoal].positionFactor * reload_time) - previousFrameTime;

            // Changing the current pos/rot
            Vector3 goalPos = reload_frames[reload_currentGoal].newLocalPos;
            Vector3 goalRot = reload_frames[reload_currentGoal].newLocalRot;
            handManager.leftHandGrip = Vector3.Lerp(reload_startPos, goalPos, frameTime / timeToMove);
            handManager.leftRotationOffset = Vector3.Lerp(reload_startRot, goalRot, frameTime / timeToMove);

            // Inactivity
            if (reload_frames[reload_currentGoal].inactive) {
                leftHand.SetActive(false);
            } else {
                leftHand.SetActive(true);
            }

            // Attached Objects
            if (reload_frames[reload_currentGoal].attachedObject) {
                attachedObject = reload_frames[reload_currentGoal].attachedObject;
                reload_frames[reload_currentGoal].attachedObject.parent = leftHand.transform;
            } else {
                if (attachedObject) {
                    attachedObject.parent = gunTrans;
                    attachedObject.localPosition = new Vector3(0, 0, 0);
                    attachedObject = null;
                }
            }

            // Updating time
            reload_currentTime += Time.deltaTime;
            if (reload_currentTime >= (reload_time * reload_frames[reload_currentGoal].positionFactor)) {
                reload_startPos = handManager.leftHandGrip;
                reload_startRot = handManager.leftRotationOffset;
                reload_currentGoal++;
                if (reload_currentGoal == reload_frames.Count) {
                    reloading = false;
                }
            }
        }
    }

    // Do the reload animation
    public void reload(float reloadTime) {
        reloading = true;
        reload_currentGoal = 0;
        reload_currentTime = 0;
        reload_time = reloadTime;
        reload_startPos = handManager.leftHandGrip;
        reload_startRot = handManager.leftRotationOffset;
    }
}
