using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {
    // Public Variables
    public float centerDist = 5f; // The distance from the center
    public float currToolLength = 1f; // The length of the tool
    public float yAdjust = 1f; // Adjusts the height/y position. Used to make the barrel of guns be aligned with the mouse.
    public Vector2 leftHandGrip; // Adjusts offset for left hand. Used to make it look like you're holding guns.
    public Vector2 toolTip; // The tip of the tool/gun. Used to make any gun barrels be aligned with the mouse.
    public Transform crosshair;
    public Transform rightHand;
    public Transform leftHand;


    // Start is called before the first frame update
    void Start() {
        // Not used (yet...?)
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
        LayerMask mask = LayerMask.GetMask("Ground");
        Vector3 rayOrigin = transform.position; //+ yAdjust * rightHand.up;
        float rayDist = getScaledX(newCenterDist) + currToolLength;

        RaycastHit2D ray = Physics2D.Raycast(rayOrigin, rightHand.right, rayDist, mask);
        if (ray) {
            rightHand.position += (rayDist - ray.distance) * rightHand.right * -1;
        }

        // Setting Left Hand Values
        leftHand.eulerAngles = rightHand.eulerAngles;
        leftHand.position = rightHand.position;
        leftHand.position += leftHandGrip.x * leftHand.right;
        leftHand.position += leftHandGrip.y * leftHand.up;

        // Mouse Things
        crosshair.position = new Vector3(mousePos.x, mousePos.y, 0);
        Cursor.visible = false;
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
