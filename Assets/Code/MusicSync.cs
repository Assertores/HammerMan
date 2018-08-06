using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicSync : MonoBehaviour {

    [SerializeField]
    float delayToFirstBeatInFile = 0.0f;
    [SerializeField]
    float delayToFirstHammerHitInGame = 0.0f;
    [SerializeField]
    float BPM = 0;
    AudioSource audio = null;

    public void Start() {
        audio = GetComponent<AudioSource>();
        GameManager.StartBeats(60/ BPM, delayToFirstHammerHitInGame);
        Play(delayToFirstHammerHitInGame - delayToFirstBeatInFile - GameManager.GetTime());
    }

    private void Play(float skip = 0.0f) {

        if (audio) {
            if (delayToFirstHammerHitInGame - delayToFirstBeatInFile - GameManager.GetTime() >= 0) {
                audio.PlayDelayed(delayToFirstHammerHitInGame - delayToFirstBeatInFile - GameManager.GetTime());
            } else {
                audio.Play();
                audio.time = -delayToFirstHammerHitInGame - delayToFirstBeatInFile - GameManager.GetTime();
            }
        }
    }
}
