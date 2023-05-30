using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {
    // Public Variables
    [Header("General Settings")]
    public float centerDist = 5f; // The distance from the center
    public float currToolLength = 1f; // The length of the tool
    public float yAdjust = 1f; // Adjusts the height/y position. Used to make the barrel of guns be aligned with the mouse.
    public Transform crosshair;
    public Transform rightHand;

    [Header("Left Hand Settings")]
    public Transform leftHand;
    public Vector2 leftHandGrip; // Adjusts the base offset for left hand. Used to make it look like you're holding guns. (local space)
    public Vector3 leftHandGripRotOffset; // Deals with the base rotation of the hand (local space)

    [Header("Offsets (for gun system)")]
    public Vector2 rightGripOffset; // Adjusts offset for right hand. Used to make it look like you're holding guns. (local space)
    public Vector3 rightRotationOffset; // Deals with rotating the hand (local space)
    public Vector2 leftGripOffset; // Adjusts offset for left hand. Used to make it look like you're holding guns. (local space)
    public Vector3 leftRotationOffset; // Deals with rotating the hand (local space)

    // Start is called before the first frame update
    void Start() {
        leftGripOffset = leftHandGrip;
        leftRotationOffset = leftHandGripRotOffset;
    }   

    // Update is called once per frame
    void Update() {
        // Getting Values
        Vector3 centerPos = transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float xDist = mousePos.x - centerPos.x;
        float yDist = mousePos.y - centerPos.y;
        float dist = Mathf.Sqrt((xDist * xDist) + (yDist * yDist)) - currToolLength;
        float newCenterDist = centerDist;
        if (dist < centerDist) {
            newCenterDist = dist;
        }

        // Getting Rotational Values
        float angle = Mathf.Atan((yDist) / xDist) * Mathf.Rad2Deg;
        float yRot = 0;
        if (centerPos.x > mousePos.x) {
            angle *= -1;
            yRot = 180;
        }

        // Setting Right Hand Values
        rightHand.eulerAngles = new Vector3(0, yRot, angle);
        rightHand.position = centerPos + getScaledX(newCenterDist) * rightHand.right;
        rightHand.position += yAdjust * rightHand.up;

        // Raycast to check whats in front if the hand is getting blocked
        LayerMask mask = LayerMask.GetMask("Ground", "Wall");
        Vector3 rayOrigin = transform.position; //+ yAdjust * rightHand.up;
        float rayDist = getScaledX(newCenterDist) + currToolLength;

        RaycastHit2D ray = Physics2D.Raycast(rayOrigin, rightHand.right, rayDist, mask);
        if (ray) {
            rightHand.position += (rayDist - ray.distance) * rightHand.right * -1;
        }
        rightHand.position += rightGripOffset.x * rightHand.right;
        rightHand.position += rightGripOffset.y * rightHand.up;

        // Applying RightHand Offsets
        Vector3 newRightLocalRot = rightHand.localEulerAngles;
        newRightLocalRot.x += rightRotationOffset.x;
        newRightLocalRot.y += rightRotationOffset.y;
        newRightLocalRot.z += rightRotationOffset.z;
        rightHand.localEulerAngles = newRightLocalRot;

        // Setting Left Hand Values
        leftHand.eulerAngles = rightHand.eulerAngles;
        leftHand.position = rightHand.position;
        leftHand.position += leftGripOffset.x * leftHand.right;
        leftHand.position += leftGripOffset.y * leftHand.up;

        // Applying LeftHand Offsets
        Vector3 newLeftLocalRot = leftHand.localEulerAngles;
        newLeftLocalRot.x += leftRotationOffset.x;
        newLeftLocalRot.y += leftRotationOffset.y;
        newLeftLocalRot.z += leftRotationOffset.z;
        leftHand.localEulerAngles = newLeftLocalRot;

        // Mouse Things
        crosshair.position = new Vector3(mousePos.x, mousePos.y, 0);
        Cursor.visible = false;
    }

    // Resets the right hand offsets to just 0
    public void resetRightOffsets() {
        rightGripOffset = new Vector2(0, 0);
        rightRotationOffset = new Vector3(0, 0, 0);
    }

    // Resets the left hand offsets to the base ones
    public void resetLeftOffsets() {
        leftGripOffset = leftHandGrip;
        leftRotationOffset = leftHandGripRotOffset;
    }

    // Returns the correctly scaled x position
    private float getScaledX(float val) {
        return val / transform.localScale.x;
    }

    // Returns the correctly scaled x position
    private float getScaledY(float val) {
        return val / transform.localScale.y;
    }
}
