using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Shockwave : MonoBehaviour {

    [SerializeField]
    float ShockDuration = 1.0f;

    private void Start() {
        ShockDuration += GameManager.GetTime();
    }

    private void Update() {
        if(ShockDuration <= GameManager.GetTime()) {
            GameManager.Destroy(this.transform.gameObject);
        }
    }
}
