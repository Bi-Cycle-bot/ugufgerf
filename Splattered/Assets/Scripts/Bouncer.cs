using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{

    public float bounceForce;
    [SerializeField] private PlayerMovement player;
    // Start is called before the first frame update

    private Oscillation bounceOsc;
    private Oscillation posOsc;
    private GameObject cam;
    private CameraManager camCam;

    void Start() {
        cam = GameObject.Find("Main Camera");
        camCam = cam.GetComponent<CameraManager>();
        bounceOsc = gameObject.AddComponent<Oscillation>();
        bounceOsc.destroyOnIdle = false;
        bounceOsc.idle = true;
        bounceOsc.setUpdateTick(0);
        bounceOsc.setSineWaveValues(.5f, 1f, 0);
        bounceOsc.setDecayValues(.1f, 0f);

        posOsc = gameObject.AddComponent<Oscillation>();
        posOsc.destroyOnIdle = false;
        posOsc.idle = true;
        posOsc.setUpdateTick(1);
        posOsc.setSineWaveValues(1f, 3f, .5f);
        posOsc.setDecayValues(.1f, .08f);

        camCam.addSizeOscillation(bounceOsc);
        camCam.addPosOscillation(null, posOsc);
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.tag == "Player")
        {
           player.knockbackFromPosition(transform.position, 50f);
           bounceOsc.reset();
           posOsc.reset();
        }
    }

    void doOscillation() {
        
    }
}
