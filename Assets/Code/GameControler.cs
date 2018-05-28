using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControler : MonoBehaviour {

    float LevelTimeStart = 0.0f;
    int EnemyCount = 0;
    int SpawnerCount = 0;
    [SerializeField]
    int MaxLevelLife = 10;
    int CurentLevelLife;

    UIManager UIM;

    void Start() {
        StartLvl();
    }

    public void StartLvl() {
        SceneManager.LoadScene("Scene 01");

        LevelTimeStart = Time.time;
        CurentLevelLife = MaxLevelLife;

        try {
            UIM = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        } catch (System.Exception) {
            throw new System.Exception("UI Entity not found or UIManager not found as Component. GameControler");
        }
    }

    public void GameOver() {
        SceneManager.LoadScene("GameOver");
    }

    public void StartMainMenu() {
        SceneManager.LoadScene("MainMenu");
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

    public void ChangeSpawnerCount(int count) {
        SpawnerCount += count;

    }
}
