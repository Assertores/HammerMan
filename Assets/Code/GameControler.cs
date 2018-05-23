using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControler : MonoBehaviour {

    float LevelTimeStart = 0.0f;
    int EnemyCount = 0;
    [SerializeField]
    int MaxLevelLife = 10;
    int CurentLevelLife;

    void Start() {
        LevelTimeStart = Time.time;
        CurentLevelLife = MaxLevelLife;
    }

    public float GetTime() {
        return Time.time - LevelTimeStart;
    }

    public void ChangeEnemyCount(int count, int loseLife = 0) {
        EnemyCount += count;
        CurentLevelLife -= loseLife;
    }
}
