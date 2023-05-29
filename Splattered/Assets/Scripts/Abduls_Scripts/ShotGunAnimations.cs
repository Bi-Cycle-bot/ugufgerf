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

public class ShotGunAnimations : GunAnimations {
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
}
