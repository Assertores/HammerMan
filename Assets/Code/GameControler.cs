using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControler : MonoBehaviour {

    float LevelTimeStart = 0.0f;

	// Use this for initialization
	void Start () {
        LevelTimeStart = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetTime() {
        return Time.time - LevelTimeStart;
    }
}
