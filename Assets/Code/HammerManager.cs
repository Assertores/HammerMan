using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerManager : MonoBehaviour {

    [SerializeField]
    float HammerFrequency;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    [Tooltip("Anfang des Hammerschlags als prozentzahl der frequenzzeit")]
    float HammerOnBeginning;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    [Tooltip("Ende des Hammerschlags als prozentzahl der frequenzzeit")]
    float HammerOnEnd;

    BoxCollider2D HammerCol;
    
    void Start () {
        HammerCol = GetComponent<BoxCollider2D>();
        if (!HammerCol) {
            throw new System.Exception("Hammer Collider not found. Hammer");
        }
        HammerOnBeginning = HammerFrequency * HammerOnBeginning;
        HammerOnEnd = HammerFrequency * HammerOnEnd;
    }
	
	void Update () {
        float time = GameControler.GetTime() % HammerFrequency;
        if (time > HammerOnBeginning && time < HammerOnEnd) {
            HammerCol.enabled = true;
        } else if (HammerCol.enabled == true) {
            HammerCol.enabled = false;
        }
	}
}
