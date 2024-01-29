
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    public float speed = 5.0f;
    public float jumpSpeed = 10f;

    public static bool InvertedControls = false;

    //PlayerInput input;
    public Rigidbody body { get; private set; }
    public Collider[] colliders { get; private set; }
    RaycastHit[] hits = new RaycastHit[32];

    public List<GruntCollection> grunts = new List<GruntCollection>();
    GruntCollection myGrunts;
    AudioSource source;

    // Start is called before the first frame update
    void Awake() {
        body = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }

    public bool rotating { get; private set; } = false;

    PlayerInput input;
    int playerIndex = -1;
    int groundLayers = 1 << Layers.Ground | 1 << Layers.Player1 | 1 << Layers.Player2 | 1 << Layers.Player3 | 1 << Layers.Player4;
    public void Init(int playerIndex, PlayerInput input, int layer) {
        this.playerIndex = playerIndex;
        myGrunts = grunts[playerIndex];
        this.input = input;
        groundLayers &= ~(1 << layer); // remove your layer from the ground layers

        input.actions["Jump"].performed += Jump;
        input.actions["RotateRight"].performed += RotateRight;
        input.actions["RotateLeft"].performed += RotateLeft;

        colliders = GetComponentsInChildren<Collider>();

        // not needed anymore with manually set player models
        //// center player colliders around their center
        //Vector3 averageOffset = Vector3.zero;
        //int count = 0;
        //foreach(var c in colliders) {
        //    averageOffset += c.transform.localPosition;
        //    count++;
        //}
        //if (count > 0) {
        //    averageOffset /= count;
        //    foreach (var c in colliders) {
        //        c.transform.localPosition = c.transform.localPosition - averageOffset;
        //    }
        //}
    }

    Vector3 spawnPosition = Vector3.zero;
    public void SetSpawn(Vector3 pos) {
        spawnPosition = pos;
        transform.position = spawnPosition;
    }

    public void Respawn() {
        transform.position = spawnPosition;
        targetRot = Quaternion.identity;
    }

    void PlayRandomSound() {
        source.clip = myGrunts.grunts[Random.Range(0, myGrunts.grunts.Length)];
        source.pitch = Random.Range(0.9f, 1.1f);
        source.Play();
    }

    Quaternion startRot = Quaternion.identity;
    Quaternion targetRot = Quaternion.identity;

    private void RotateRight(InputAction.CallbackContext obj) {
        rotating = true;
        startRot = body.rotation;
        float rot = InvertedControls ? 90 : -90;
        targetRot *= Quaternion.Euler(0, 0, rot);
        time = 0.0f;
        PlayRandomSound();
    }
    private void RotateLeft(InputAction.CallbackContext obj) {
        rotating = true;
        startRot = body.rotation;
        float rot = InvertedControls ? -90 : 90;
        targetRot *= Quaternion.Euler(0, 0, rot);
        time = 0.0f;
        PlayRandomSound();
    }

    float groundedLockout = 0.0f;
    void Jump(InputAction.CallbackContext obj) {
        if (grounded && !body.isKinematic) {
            Vector3 vel = body.velocity;
            vel.y = jumpSpeed;
            body.velocity = vel;
            PlayRandomSound();
            grounded = false;
            groundedLockout = 0.1f;
        }
    }

    float time = 0.0f;
    void Update() {
        if (!input) {
            return;
        }
        time += Time.deltaTime * 3.0f;
        Quaternion r = Quaternion.Lerp(startRot, targetRot, time);
        if (time >= 1.0f) {
            rotating = false;
        }
        body.MoveRotation(r);
        //body.rotation = Quaternion.Euler(rot);
        var move = input.actions["Move"].ReadValue<Vector2>();
        move *= speed;
        if (InvertedControls) {
            move *= -1.0f;
        }
        if (!body.isKinematic) {
            Vector3 vel = body.velocity;
            vel.x = move.x;
            vel.z = move.y;
            body.velocity = vel;
        }
        groundedLockout -= Time.deltaTime;
        grounded = false;
        if (groundedLockout < 0) {
            foreach (var c in colliders) {
                int count = Physics.BoxCastNonAlloc(c.bounds.center, c.bounds.extents * .95f, Vector3.down, hits, Quaternion.identity, .1f, groundLayers);
                //Debug.DrawLine(c.bounds.center, c.bounds.center + Vector3.down);
                if (count > 0) {
                    grounded = true;
                    break;
                }
            }
        }

        // failsafe
        if(transform.position.y < -2.0f) {
            Respawn();
        }
    }

    bool grounded = false;

    private void OnTriggerExit(Collider other) {
        var shape = other.transform.parent.GetComponent<FillShape>();
        if (shape) {
            shape.RemovePlayer(this);
        }
    }

    private void OnTriggerStay(Collider other) {
        var shape = other.transform.parent.GetComponent<FillShape>();
        if (shape) {
            shape.AddPlayer(this);
        }
    }

}

[System.Serializable]
public struct GruntCollection {
    public AudioClip[] grunts;
}
