using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {
    // Public Variables
    public float centerDist = 5f; // The distance from the center
    public GameObject player;

    // Private Variables
    private Transform plrTrans; // Parent transform
    private GameObject camObject;
    private Camera cam;

    // Start is called before the first frame update
    void Start() {
        plrTrans = player.GetComponentInParent<Transform>();
        cam = camObject.GetComponent<Camera>();
    }   

    // Update is called once per frame
    void Update() {
        // Values
        Vector3 centerPos = plrTrans.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float xDist = mousePos.x - centerPos.x;
        float yDist = mousePos.y - centerPos.y;

        
    }
}
