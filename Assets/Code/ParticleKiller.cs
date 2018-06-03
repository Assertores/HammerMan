﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleKiller : MonoBehaviour {

    float time = 0.0f;
	// Use this for initialization
	void Start () {
        time = GameManager.GetTime();
        ParticleSystem temp = GetComponent<ParticleSystem>();
        if (temp) {
            temp.Play();
            return;
        }
        AudioSource audio = GetComponent<AudioSource>();
        if (audio) {
            audio.Play();
            return;
        }

    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if (GameManager.GetTime() - time > 2.0f) {
            GameObject.Destroy(this.transform.gameObject);
        }
	}
}
