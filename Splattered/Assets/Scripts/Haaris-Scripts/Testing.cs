using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private void Start() {
        DamagePopUp.Create(Vector3.zero, 300);
        Debug.Log("Create is here");
    }
}
