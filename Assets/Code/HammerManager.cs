using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerManager : MonoBehaviour {

    [SerializeField]
    float HammerFrequency;
    [SerializeField]
    float HammerOnBeginning;
    [SerializeField]
    float HammerOnEnd;

    GameControler GC;
    BoxCollider2D HammerCol;

    // Use this for initialization
    void Start () {
        GC = GameObject.Find("GameManager").GetComponent<GameControler>();
        if (!GC) {
            throw new System.Exception("GameManager not found. Hammer");
        }
        HammerCol = GetComponent<BoxCollider2D>();
        if (!HammerCol) {
            throw new System.Exception("Hammer Collider not found. Hammer");
        }
    }
	
	// Update is called once per frame
	void Update () {
        float time = GC.GetTime() % HammerFrequency;
        if (time > HammerOnBeginning && time < HammerOnEnd) {
            HammerCol.enabled = true;
        } else if (HammerCol.enabled == true) {
            HammerCol.enabled = false;
        }
	}
}
