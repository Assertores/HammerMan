using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleKiller : MonoBehaviour {

    float time = 0.0f;

    void Start () {
        time = GameManager.GetTime();
    }

    public void PlayStart() {
        ParticleSystem temp = GetComponent<ParticleSystem>();
        if (temp) {
            temp.Play();
        }
        AudioSource audio = GetComponent<AudioSource>();
        if (audio) {
            audio.Play();
        }
    }

	void FixedUpdate () {//macht dass es wieder weck geht
		if (GameManager.GetTime() - time > 2.0f) {
            GameObject.Destroy(this.transform.gameObject);
        }
	}
}
