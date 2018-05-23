using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControler : MonoBehaviour {

    float LevelTimeStart = 0.0f;
    int EnemyCount = 0;
    
    void Start () {
        LevelTimeStart = Time.time;
	}
	
	void Update () {
		
	}

    public float GetTime() {
        return Time.time - LevelTimeStart;
    }

    public void ChangeEnemyCount(int count) {
        EnemyCount += count;
    }
}
