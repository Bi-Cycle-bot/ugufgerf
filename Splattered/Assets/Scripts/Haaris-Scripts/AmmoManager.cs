using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AmmoManager : MonoBehaviour
{
    
    [SerializeField] private Slider ammoSlider;
    private GameObject gun;
    private GunSystem gunData;
    // Start is called before the first frame update
    void Start()
    {
        gun = GameObject.FindGameObjectWithTag("Gun");
        gunData = gun.GetComponent<GunSystem>();


        SetInitialAmmo(gunData.maxCapacity);
    }

    // Update is called once per frame
    void Update()
    {
        gun = GameObject.FindGameObjectWithTag("Gun");
        gunData = gun.GetComponent<GunSystem>();
        ammoSlider.maxValue = gunData.maxCapacity;
        UpdateAmmo(gunData.currentCapacity);
    }

    public void SetInitialAmmo(int maxAmmo)
    {
        ammoSlider.maxValue = maxAmmo;
        ammoSlider.value = maxAmmo;
    }

    public void UpdateAmmo(int currentAmmo)
    {
        ammoSlider.value = currentAmmo;
        //Debug.Log("ammo value: " + gunData.currentCapacity);
    }
}
