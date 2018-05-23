using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControler : MonoBehaviour {

    float LevelTimeStart = 0.0f;
    int EnemyCount = 0;
    [SerializeField]
    int MaxLevelLife = 10;
    int CurentLevelLife;

    UIManager UIM;

    void Start() {
        LevelTimeStart = Time.time;
        CurentLevelLife = MaxLevelLife;

        try {
            UIM = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        } catch (System.Exception) {
            throw new System.Exception("UI Entity not found or UIManager not found as Component. GameControler");
        }
        
    }

    public float GetTime() {
        return Time.time - LevelTimeStart;
    }

    public void ChangeEnemyCount(int count, int loseLife = 0) {
        EnemyCount += count;
        CurentLevelLife -= loseLife;
        UIM.UpdateLife(CurentLevelLife / (float)MaxLevelLife);
        if (CurentLevelLife <= 0) {
            GameOver();
        }
        UIM.UpdateEnemyCount(EnemyCount);
    }

    void GameOver() {
        //Make GameOver Stuff
    }
}
