/*
------------------------------------------
By: Abdul Ahad Naveed
Created: 5/5/2023
Updated: 5/5/2023 @ 8:56 pm

Used to be a gun system
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enumerations
public enum FireMode {
    FullAuto,
    SemiAuto,
};

public class GunSystem : MonoBehaviour {
    // ---------------------- PUBLIC VARIABLES ----------------------
    // General Gun Settings
    [Header("General Settings")]
    public FireMode fireMode = FireMode.SemiAuto; // FullAuto, Semi, etc.
    public float fireRate = .3f; // Fire rate every second
    public float maxCapacity = 30f; // Max capacity of the gun (i.e. mag capacity, clip capacity, etc.)
    public float reloadTime = 2f; // How long it takes to reload


    // Accuracy Settings
    [Header("Accuracy Settings")]
    public float inaccuracyBase = 1f; // Used in conjuction with 'inaccuractDistance': base / distance inaccuracy
    public float inaccuracyDistance = 50f; // Used in conjuction with 'inaccuracyBase': base / distance inaccuracy
    public float bloom = .1f; // Every time the gun fires, increase inaccuracyBase by this value. Can be 0.
    public float maxBloom = 1f; // Max bloom


    // Recoil Settings
    [Header("Recoil Settings")]
    public Vector2 recoilGripPosOffset; // How much positional offset applied each shot
    public Vector3 recoilGripRotOffset; // How much rotational offset applied each shot
    public float recoilTime = .05f; // The total time the recoil will last for until it starts relieving (make it small)
    public float recoilRelief = .1f; // The greater, the faster the offsets will go away
    public float recoilStrength= .1f; // The greater, the faster the offsets will be affected


    // Damage Settings
    [Header("Damage Settings")]
    public float baseDamage = 30f; // Base damage the gun will do
    public float headshotMultiplier = 2f; // Multiplier of the base damage for headshots


    // Bullet Settings
    [Header("Bullet Settings")]
    public float bulletVelocity = 5f; // Speed of the bullet
    public float bulletDrop = .2f; // How much drop the bullet is going to have
    public GameObject bulletPrefab; // To the bullet the gun is going to shoot


    // Important Objects
    [Header("Important Objects")]
    public GameObject fireObject; // The object where bullets, effects, etc. will come from
    public HandManager handManager; // Hand Manger of the Player


    // Sounds
    [Header("Audio")]
    public AudioClip gunShotAudio;

    // Optional Settings
    [Header("Optional Settings")]
    public Transform bolt; // Bolt that goes back when shooting
    public float boltBackValue; // Value the bolt will go back when shooting
    public GunAnimations gunAnims; // Set this if you have a GunAnimation script setup for this gun

    // ---------------------- PRIVATE VARIABLES ----------------------
    // Gun Status
    private float currentCapacity;
    private float lastShotTime;
    private bool ready;
    private bool reloading;
    private bool mouse0Down;
    private bool recoilEnabled;

    private AudioSource soundEmitter; // Place to have audio

    // Offsets for player
    private Vector2 mainGripPosOffset = new Vector2(0, 0);
    private Vector3 mainGripRotOffset = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start() {
        // Setting values
        soundEmitter = GetComponent<AudioSource>();
        currentCapacity = maxCapacity;
        lastShotTime = fireRate;
        ready = true;
    }

    // Update is called once per frame
    void Update() {
        // Mouse Down
        if (Input.GetKey(KeyCode.Mouse0)) {
            mouse0Down = true;
        } else {
            mouse0Down = false;
        }

        // Firing
        if (lastShotTime >= fireRate) {
            if (ready) {
                if ((fireMode == FireMode.SemiAuto) && Input.GetKeyDown(KeyCode.Mouse0)) {
                    shoot(); // Semi-Auto
                } else if ((fireMode == FireMode.FullAuto) && mouse0Down) {
                    shoot(); // Full-Auto
                }
            }
        } else {
            lastShotTime += Time.deltaTime;
        }

        // Reloading
        if (Input.GetKeyDown(KeyCode.R) && ready) {
            if (currentCapacity < maxCapacity) {
                reload();
            }
        }

        // Setting Values in Hand Manager
        if (recoilEnabled) {
            mainGripPosOffset = Vector2.Lerp(mainGripPosOffset, recoilGripPosOffset, recoilStrength / 2);
            mainGripRotOffset = Vector3.Lerp(mainGripRotOffset, recoilGripRotOffset, recoilStrength / 2);
        } else {
            mainGripPosOffset = Vector2.Lerp(mainGripPosOffset, new Vector2(0, 0), recoilRelief / 2);
            mainGripRotOffset = Vector3.Lerp(mainGripRotOffset, new Vector3(0, 0, 0), recoilRelief / 2);
            if (bolt) {
                bolt.localPosition = Vector3.Lerp(bolt.localPosition, new Vector3(0, 0, 0), .01f);
            }
        }
        handManager.rightGripOffset = mainGripPosOffset;
        handManager.rightRotationOffset = mainGripRotOffset;
    }

    // Used to reset some stats (going to be used later when unequipping)
    void reset() {
        ready = false;
        reloading = false;
        recoilEnabled = false;
        mainGripPosOffset = new Vector2(0, 0);
        mainGripRotOffset = new Vector3(0, 0, 0);
        handManager.rightGripOffset = mainGripPosOffset;
        handManager.rightRotationOffset = mainGripRotOffset;
        lastShotTime = fireRate;
        CancelInvoke();

        if (bolt) {
            bolt.localPosition = new Vector3(0, 0, 0);
        }
    }

    // Shoots one bullet
    private void shoot() {
        // Checking Bullets
        if (currentCapacity == 0) { return; }
        currentCapacity--;

        // Setting Values
        lastShotTime = 0;
        recoilEnabled = true;
        Invoke("finishRecoil", recoilTime);

        // Effects like particles and sounds
        soundEmitter.PlayOneShot(gunShotAudio, 1f);
        if (bolt) {
            bolt.localPosition = new Vector3(boltBackValue, 0, 0);
        }

        // Shooting bullet
        mainGripPosOffset = new Vector2(0, 0);
        mainGripRotOffset = new Vector3(0, 0, 0);
        handManager.rightGripOffset = new Vector2(0, 0);
        handManager.rightRotationOffset = new Vector3(0, 0, 0);

        // [SHOOT BULLET HERE]
    }

    // To Reload
    private void reload() {
        if (reloading) { return; }
        reloading = true;
        ready = false;
        if (gunAnims) {
            gunAnims.reload(reloadTime);
        }
        Invoke("finishReload", reloadTime);
    }

    // Used to when reloading is done
    private void finishReload() {
        currentCapacity = maxCapacity;
        reloading = false;
        ready = true;
    }

    // Used for finishing recoil
    private void finishRecoil() {
        recoilEnabled = false;
    }
}
