/*
------------------------------------------
By: Abdul Ahad Naveed
Created: 5/5/2023
Updated: 5/5/2023 @ 5:41 pm

Used to be a gun system
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enumerations
public enum FireMode {
    FullAuto,
    Semi,
};

public class GunSystem : MonoBehaviour {
    // ---------------------- PUBLIC VARIABLES ----------------------
    // General Gun Settings
    [Header("General Settings")]
    public FireMode fireMode = FireMode.Semi; // FullAuto, Semi, etc.
    public float fireRate = .3f; // Fire rate every second
    public float maxCapacity = 30f; // Max capacity of the gun (i.e. mag capacity, clip capacity, etc.)
    public float reloadTime = 1f; // How long it takes to reload


    // Accuracy Settings
    [Header("Accuracy Settings")]
    public float inaccuracyBase = 1f; // Used in conjuction with 'inaccuractDistance': base / distance inaccuracy
    public float inaccuracyDistance = 50f; // Used in conjuction with 'inaccuracyBase': base / distance inaccuracy
    public float bloom = .1f; // Every time the gun fires, increase inaccuracyBase by this value. Can be 0.
    public float maxBloom = 1f; // Max bloom


    // Damage Settings
    [Header("Damage Settings")]
    public float baseDamage = 30f; // Base damage the gun will do
    public float headshotMultiplier = 2f; // Multiplier of the base damage for headshots

    // Bullet Settings
    [Header("Bullet Settings")]
    public float bulletVelocity = 5f;
    public float bulletDrop = .2f;
    public GameObject bulletPrefab; // To the bullet the gun is going to shoot

    // Important Objects
    [Header("Important Objects")]
    public GameObject fireObject; // The object where bullets, effects, etc. will come from

    // Sounds
    [Header("Audio")]
    public AudioClip gunShotAudio;

    // ---------------------- PRIVATE VARIABLES ----------------------
    // Gun Status
    private float currentCapacity;
    private float lastShotTime;
    private bool ready;
    private bool reloading;

    private AudioSource soundEmitter; // Place to have audio


    // Start is called before the first frame update
    void Start() {
        // Setting values
        soundEmitter = GetComponent<AudioSource>();
        currentCapacity = maxCapacity;
        lastShotTime = fireRate * -1;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            shoot();
        }

        // Ready Section
        if (!ready) { return; }

        // Firing
        if (lastShotTime >= fireRate) {
            
        }
    }

    // Shoots one bullet
    private void shoot() {
        // Effects like particles and sounds
        soundEmitter.PlayOneShot(gunShotAudio, 1f);
    }
}
