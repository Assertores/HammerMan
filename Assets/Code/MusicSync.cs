using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSync : MonoBehaviour {

    [SerializeField]
    float delayToFirstBeat = 0.0f;
    [SerializeField]
    float BPM = 0.0f;

    bool StartSuccessful = false;

    public void Update() { //started das lied vom code aus gesynct mit dem hammer
        if (!StartSuccessful) {
            float nextBeat = GameManager.StartMusic();
            if(nextBeat >= 0) {
                StartSuccessful = true;
                Invoke("Play", nextBeat + (BPM / 60 - delayToFirstBeat));
            } 
        }
    }
    private void Play() {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio) {
            audio.Play();
        }
    }
}
