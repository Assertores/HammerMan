using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Shockwave : MonoBehaviour {

    [SerializeField]
    float ShockDuration = 1.0f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float KillZoneStart = 0;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float KillZoneEnd = 1;
    [SerializeField]
    GameObject MultyKillObj = null;
    [SerializeField]
    int MultyKillThreshold = 2;

    int KillCount = 0;

    BoxCollider2D col;

    private void Start() {
        col = GetComponent<BoxCollider2D>();
        if (!col)
            new System.Exception("no BoxCollider found. Shockwave");
        if (KillZoneStart != 0)
            col.enabled = false;

        KillZoneStart *= ShockDuration;
        KillZoneEnd *= ShockDuration;
        float time = GameManager.GetTime();
        ShockDuration += time;
        KillZoneStart += time;
        KillZoneEnd += time;
    }

    private void Update() {
        float time = GameManager.GetTime();
        if (ShockDuration <= time) {
            GameManager.Destroy(this.transform.gameObject);
        }
        if(KillZoneStart < time && KillZoneEnd > time) {
            col.enabled = true;
        } else {
            col.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == StringCollection.ENEMY) {
            KillCount++;
            if(KillCount == MultyKillThreshold) {
                Instantiate(MultyKillObj);
            }
        }
    }
}
