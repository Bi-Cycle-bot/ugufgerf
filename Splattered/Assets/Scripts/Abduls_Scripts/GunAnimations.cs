/*
------------------------------------------
By: Abdul Ahad Naveed
Created: 5/6/2023
Updated: 5/20/2023 @ 8:56 pm

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
        public AudioClip sound; // If you want a sound to play
    }

    // ---------------------- RELOAD VARIABLES ----------------------
    [Header("Reload Animation")]
    public List<keyframe> reload_frames; // Make sure the last keyframe contains the default position/rotation values!

    // ---------------------- CHAMBER VARIABLES ----------------------
    [Header("Chamber Animation")]
    public List<keyframe> chamber_frames; // Make sure the last keyframe contains the default position/rotation values!

    // ---------------------- OTHER PRIVATE VARIABLES ----------------------
    protected GameObject player; // Player Game Object
    protected HandManager handManager; // The hand manager this keyframe is acting upon
    protected AudioSource soundEmitter; // Place to have audio
    protected Transform gunTrans; // Gun of these animations
    protected GameObject leftHand; // Left hand of the player

    // Animation Variables
    protected float currentTime = 0;
    protected float animationTime = 1;
    protected Vector3 startPos;
    protected Vector3 startRot;
    protected int currentGoal = 0;

    protected Transform attachedObject;
    protected bool playing = false;
    protected List<keyframe> currentFrames;
    protected int ammoNeeded = 0;

    private bool started = false;

    // Start is called before the first frame update
    void Start() {
        // Setting values
        player = GameObject.FindGameObjectWithTag("Player");
        handManager = player.GetComponent<HandManager>();
        soundEmitter = player.GetComponent<AudioSource>();
        gunTrans = GetComponent<Transform>();
        leftHand = GameObject.Find("/Player/LeftHand");
        started = true;
    }

    // Update is called once per frame
    void Update() {
        if (playing) {
            // Getting Values
            Vector3 currentPos = currentFrames[currentFrames.Count - 1].newLocalPos;
            Vector3 currentRot = currentFrames[currentFrames.Count - 1].newLocalRot;
            float previousFrameTime = 0f;
            if (currentGoal > 0) {
                currentPos = currentFrames[currentGoal - 1].newLocalPos;
                currentPos = currentFrames[currentGoal - 1].newLocalRot;
                previousFrameTime = currentFrames[currentGoal - 1].positionFactor * animationTime;
            }
            float frameTime = currentTime - previousFrameTime;
            float timeToMove = (currentFrames[currentGoal].positionFactor * animationTime) - previousFrameTime;

            // Changing the current pos/rot
            Vector3 goalPos = currentFrames[currentGoal].newLocalPos;
            Vector3 goalRot = currentFrames[currentGoal].newLocalRot;
            handManager.leftGripOffset = Vector3.Lerp(startPos, goalPos, frameTime / timeToMove);
            handManager.leftRotationOffset = Vector3.Lerp(startRot, goalRot, frameTime / timeToMove);

            // Inactivity
            if (currentFrames[currentGoal].inactive) {
                if (attachedObject) {
                    attachedObject.gameObject.SetActive(false);
                }
                leftHand.SetActive(false);
            } else {
                if (attachedObject) {
                    attachedObject.gameObject.SetActive(true);
                }
                leftHand.SetActive(true);
            }

            // Attached Objects
            if (currentFrames[currentGoal].attachedObject) {
                attachedObject = currentFrames[currentGoal].attachedObject;
                currentFrames[currentGoal].attachedObject.parent = leftHand.transform;
            } else {
                if (attachedObject) {
                    attachedObject.parent = gunTrans;
                    attachedObject.localPosition = new Vector3(0, 0, 0);
                    attachedObject.localEulerAngles = new Vector3(0, 0, 0);
                    // attachedObject.localPosition = Vector3.Lerp(attachedObject.localPosition, new Vector3(0, 0, 0), .06f);
                    // attachedObject.localEulerAngles = Vector3.Lerp(attachedObject.localEulerAngles, new Vector3(0, 0, 0), .06f);
                }
            }

            // Updating time
            currentTime += Time.deltaTime;
            if (currentTime >= (animationTime * currentFrames[currentGoal].positionFactor)) {
                handManager.leftGripOffset = currentFrames[currentGoal].newLocalPos;
                handManager.leftRotationOffset = currentFrames[currentGoal].newLocalRot;
                startPos = handManager.leftGripOffset;
                startRot = handManager.leftRotationOffset;
                currentGoal++;
                if (currentGoal == currentFrames.Count) {
                    if (attachedObject) {
                        attachedObject.parent = gunTrans;
                        attachedObject.localPosition = new Vector3(0, 0, 0);
                        attachedObject.localEulerAngles = new Vector3(0, 0, 0);
                    }
                    attachedObject = null;
                    playing = false;
                } else {
                    if (currentFrames[currentGoal].sound) {
                        soundEmitter.PlayOneShot(currentFrames[currentGoal].sound, 1f);
                    }
                }
            }
        }
    }

    // Cancels all animations
    public void cancel() {
        if (!started) {
            Start();
        }
        playing = false;
        currentFrames = null;
        if (attachedObject) {
            attachedObject.parent = gunTrans;
            attachedObject.localPosition = new Vector3(0, 0, 0);
            attachedObject.localEulerAngles = new Vector3(0, 0, 0);
        }
        handManager.resetLeftOffsets();
        leftHand.SetActive(true);
    }

    // Do the reload animation
    public void reload(float reloadTime, int amountNeeded) {
        reset();
        animationTime = reloadTime;
        currentFrames = reload_frames;
        ammoNeeded = amountNeeded;
    }

    // Do the chamber animation
    public void chamber(float chamberTime) {
        reset();
        animationTime = chamberTime;
        currentFrames = chamber_frames;
    }

    protected void reset() {
        playing = true;
        currentGoal = 0;
        currentTime = 0;
        startPos = handManager.leftGripOffset;
        startRot = handManager.leftRotationOffset;
        attachedObject = null;
    }
}
