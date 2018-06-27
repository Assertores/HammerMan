using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerManager : MonoBehaviour {

    [SerializeField]
    [Range(0.0f, 1.0f)]
    [Tooltip("Anfang des Hammerschlags als prozentzahl der frequenzzeit")]
    float HammerOnBeginning;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    [Tooltip("Ende des Hammerschlags als prozentzahl der frequenzzeit")]
    float HammerOnEnd;

    bool DoHammer = true;

    BoxCollider2D HammerCol;
    AudioSource audio;
    
    void Start () {
        HammerCol = GetComponent<BoxCollider2D>();
        if (!HammerCol) {
            throw new System.Exception("Hammer Collider not found. Hammer");
        }
        HammerCol.enabled = false;
        audio = GetComponent<AudioSource>();
        if (!audio) {
            throw new System.Exception("Audio Sorce not found. Hammer");
        }
    }
	
	void Update () {
        if (DoHammer && GameManager.GetHammerTime() >= 0) {
            float time = GameManager.GetHammerTime() % 1;//mach zeit zwischen den beats
            if (time > HammerOnBeginning && time < HammerOnEnd) {
                if (!HammerCol.enabled) { //so it get set and played only ones
                    HammerCol.enabled = true;
                    audio.Play();
                }
            } else if (HammerCol.enabled == true) {
                HammerCol.enabled = false;
            }
        }
	}

    public void SetHammer(bool on) { //wird vom spieler aufgerufen
        DoHammer = on;
        if (!on)
            HammerCol.enabled = false;
    }
}
