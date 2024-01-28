using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FillShape : MonoBehaviour {
    private float totalArea = 0;
    public Collider[] Colliders { get; private set; }
    public List<PlayerController> collidingPlayers = new();

    public void Init() {
        Colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in Colliders) {
            var b = collider.bounds;
            totalArea += (b.max.x - b.min.x) * (b.max.y - b.min.y) * (b.max.z - b.min.z);
        }
    }

    public float CalculateOverlap() {
        float overlapArea = 0.0f;
        foreach (var p in collidingPlayers) {
            foreach (var c in p.colliders) {
                foreach (var mc in Colliders) {
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
        float percent = overlapArea / totalArea; // bounds kinda weird when rotating
        percent = Mathf.Clamp01(percent);
        return percent;
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
