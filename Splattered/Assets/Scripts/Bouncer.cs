using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{

    public float bounceForce;
    [SerializeField] private PlayerMovement player;
    // Start is called before the first frame update

    private Oscillation bounceOsc;
    private GameObject cam;
    private CameraManager camCam;

    void Start() {
        cam = GameObject.Find("Main Camera");
        camCam = cam.GetComponent<CameraManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
           player.knockbackFromPosition(transform.position, 50f);
           doOscillation();
        }
    }

    void doOscillation() {
        bounceOsc = gameObject.AddComponent<Oscillation>();
        bounceOsc.setUpdateTick(0);
        bounceOsc.setSineWaveValues(.8f, .08f, 0);
        bounceOsc.setDecayValues(.0013f, .001f);

        camCam.addSizeOscillation(bounceOsc);
    }
}
