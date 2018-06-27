using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSync : MonoBehaviour {

    [SerializeField]
    float delayToFirstBeatInFile = 0.0f;
    [SerializeField]
    float delayToFirstHammerHitInGame = 0.0f;

    bool StartSuccessful = false;

    public void Start() {
        Play(delayToFirstHammerHitInGame - delayToFirstBeatInFile);
        GameManager.SetHammerDelay(delayToFirstHammerHitInGame);
    }

    private void Play(float skip = 0.0f) {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio) {
            if (skip >= 0) {
                audio.PlayDelayed(skip);
            } else {
                audio.Play();
                audio.time = -skip;
            }
        }
    }
}
