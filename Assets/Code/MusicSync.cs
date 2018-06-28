using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSync : MonoBehaviour {

    [SerializeField]
    float delayToFirstBeatInFile = 0.0f;
    [SerializeField]
    float delayToFirstHammerHitInGame = 0.0f;
    [SerializeField]
    float BPM = 0;

    public void Start() {
        Play(delayToFirstHammerHitInGame - delayToFirstBeatInFile - GameManager.GetTime());
        GameManager.StartBeats(1 / (BPM / 60), delayToFirstHammerHitInGame);
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
