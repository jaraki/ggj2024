using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FillShape : MonoBehaviour {

    public float totalArea = 0;
    public Collider[] colliders { get; private set; }
    List<PlayerController> collidingPlayers = new List<PlayerController>();
    public TMP_Text text;

    // Start is called before the first frame update
    void Start() {
        colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders) {
            var b = collider.bounds;

            totalArea += (b.max.x - b.min.x) * (b.max.y - b.min.y) * (b.max.z - b.min.z);

        }

    }

    void CalculateOverlap() {
        float overlapArea = 0.0f;
        foreach (var p in collidingPlayers) {
            foreach (var c in p.colliders) {
                foreach (var mc in colliders) {
                    var a = c.bounds;
                    var b = mc.bounds;
                    float area = 
                        Mathf.Max(Mathf.Min(a.max.x, b.max.x) - Mathf.Max(a.min.x, b.min.x), 0) *
                        Mathf.Max(Mathf.Min(a.max.y, b.max.y) - Mathf.Max(a.min.y, b.min.y), 0) *
                        Mathf.Max(Mathf.Min(a.max.z, b.max.z) - Mathf.Max(a.min.z, b.min.z), 0);
                    //Debug.Log(area);
                    overlapArea += area;
                }
            }
        }
        text.text = $"{overlapArea / totalArea * 100.0f:0.0}%";
    }

    // Update is called once per frame
    void Update() {
        CalculateOverlap();
    }

    public void AddPlayer(PlayerController player) {
        if (!collidingPlayers.Contains(player)) {
            collidingPlayers.Add(player);
        }
    }

    public void RemovePlayer(PlayerController player) {
        collidingPlayers.Remove(player);
    }
}
