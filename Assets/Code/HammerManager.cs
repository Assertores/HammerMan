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
    new AudioSource audio;
    
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

        GameManager.GM.BPMUpdate += BPMUpdate;
    }
    
    void OnDestroy() {
        GameManager.GM.BPMUpdate -= BPMUpdate;
        CancelInvoke();
    }

    void BPMUpdate(int count) {
        if(count >= 0) {
            //print("Hammer: " + (GameManager.GetTime() - GameManager.GetTimeOfLastBeat()));
            if(audio)
                audio.Play();
            Invoke("DoHammerOn", HammerOnBeginning * GameManager.GetBeatSeconds());
            Invoke("DoHammerOff", HammerOnEnd * GameManager.GetBeatSeconds());
        }
    }

    void DoHammerOn() {
        if (DoHammer)
            HammerCol.enabled = true;
    }

    void DoHammerOff() {
        HammerCol.enabled = false;
    }

    public void SetHammer(bool on) { //wird vom spieler aufgerufen
        DoHammer = on;
        if (!on)
            HammerCol.enabled = false;
    }
}
