/*
------------------------------------------
By: Abdul Ahad Naveed
Created: 5/5/2023
Updated: 5/6/2023 @ 4:46 pm

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

public class GunSystem : Tool {
    // ---------------------- PUBLIC VARIABLES ----------------------
    // General Gun Settings
    [Header("General Settings")]
    public FireMode fireMode = FireMode.SemiAuto; // FullAuto, Semi, etc.
    public float fireRate = .3f; // Fire rate every second
    public int maxCapacity = 30; // Max capacity of the gun (i.e. mag capacity, clip capacity, etc.)
    public float reloadTime = 2f; // How long it takes to reload
    public bool chamberBehavior = false; // To enable chambering behavior (if using animations, make sure u make them!)
    public float chamberTime = 1f; // How long it takes to chamber a round


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
    [Range(0, 2)]
    public float recoilRelief = .1f; // The greater, the faster the offsets will go away
    [Range(0, 2)]
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


    // Sounds
    [Header("Audio")]
    public AudioClip gunShotAudio;
    [Range(0, 1)]
    public float gunShotVolume = .5f;


    // Optional Settings
    [Header("Optional Settings")]
    public Transform bolt; // Bolt that goes back when shooting
    public float boltBackValue; // Value the bolt will go back when shooting
    public GunAnimations gunAnims; // Set this if you have a GunAnimation script setup for this gun
    public GameObject spentShell; // Shell that will come out every time you shoot

    // ---------------------- PRIVATE VARIABLES ----------------------
    // Gun Status
    private int currentCapacity;
    private float lastShotTime;
    private bool ready;
    private bool reloading;
    private bool chambering;
    private bool mouse0Down;
    private bool recoilEnabled;
    private bool needsChambering = true;

    // Other
    private AudioSource soundEmitter; // Place to have audio
    private GameObject player; // Player Game Object
    private HandManager handManager; // Hand Manger of the Player

    // Offsets for player
    private Vector2 mainGripPosOffset = new Vector2(0, 0);
    private Vector3 mainGripRotOffset = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start() {
        // Setting values
        player = GameObject.FindGameObjectWithTag("Player");
        handManager = player.GetComponent<HandManager>();
        soundEmitter = GetComponent<AudioSource>();
        currentCapacity = maxCapacity;
        lastShotTime = fireRate;
        equipped = true;
        ready = true;
    }

    // Update is called once per frame
    void Update() {
        if (!equipped) { return; }

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
        chambering = false;
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
        if (gunAnims) {
            gunAnims.cancel();
        }
    }

    // Unequip
    public override void unequip() {
        equipped = false;
        reset();
        handManager.leftHandGrip = new Vector2(0, 0);
        handManager.leftHandGripRotOffset = new Vector3(0, 0, 0);
        handManager.yAdjust = 0;
        handManager.currToolLength = 0;
        gameObject.SetActive(false);
    }

    // Equip
    public override void equip() {
        equipped = true;
        reset();
        ready = true;
        gameObject.SetActive(true);
        handManager.leftHandGrip = leftHandGrip;
        handManager.leftHandGripRotOffset = leftHandGripRotOffset;
        handManager.yAdjust = yAdjust;
        handManager.currToolLength = toolLength;
        if (needsChambering && chamberBehavior) {
            if (ready && !chambering) {
                chamber();
                return;
            }
        }
    }
    
    // Chambers a round
    private void chamber() {
        chambering = true;
        ready = false;
        if (gunAnims) {
            gunAnims.chamber(chamberTime);
        }
        Invoke("finishChamber", chamberTime);
    }

    // Shoots one bullet
    private void shoot() {
        // Checking Bullets and Chambering
        if (currentCapacity == 0) { return; }
        if (needsChambering && chamberBehavior) {
            if (ready && !chambering) {
                chamber();
                return;
            }
        }
        currentCapacity--;
        if (currentCapacity == 0) { needsChambering = true; }

        // Setting Values
        lastShotTime = 0;
        recoilEnabled = true;
        Invoke("finishRecoil", recoilTime);

        // Effects like particles and sounds
        soundEmitter.pitch = Random.Range(.9f, 1.1f);
        soundEmitter.PlayOneShot(gunShotAudio, gunShotVolume);
        if (bolt) {
            bolt.localPosition = new Vector3(boltBackValue, 0, 0);
        }
        if (spentShell) {
            GameObject newShell = GameObject.Instantiate(spentShell, transform.position, transform.rotation);
            Destroy(newShell, 5);
        }

        // Fixing offsets
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
        if (needsChambering && chamberBehavior) {
            chamber();
        } else {
            ready = true;
        }
        reloading = false;
    }

    // Used for finishing recoil
    private void finishRecoil() {
        recoilEnabled = false;
    }

    // Used for finishing recoil
    private void finishChamber() {
        needsChambering = false;
        chambering = false;
        ready = true;
    }
}
