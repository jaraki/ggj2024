using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillShape : MonoBehaviour {

    public float totalArea = 0;
    Collider[] colliders;

    // Start is called before the first frame update
    void Start() {
        colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders) {
            var b = collider.bounds;

            totalArea += (b.max.x - b.min.x) * (b.max.y - b.min.y);

        }

    }

    // Update is called once per frame
    void Update() {

    }
}
