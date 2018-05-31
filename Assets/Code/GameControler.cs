using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControler : MonoBehaviour {

    float LevelTimeStart = 0.0f;
    int EnemyCount = 0;
    [SerializeField]
    int MaxLevelLife = 10;
    private int CurentLevelLife = 0;
    [SerializeField]
    bool StartingInLevel = false;

    UIManager UIM;

    public static GameControler GC = null;

    void Awake() {
        if (GC == null) {
            GC = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start() {
        if (!StartingInLevel) {
            StartMainMenu();
        } else {
            GC.LevelTimeStart = Time.time;
            GC.CurentLevelLife = GC.MaxLevelLife;
        }
    }

    public void StartLvl() {
        GC.EnemyCount = 0;
        GC.LevelTimeStart = Time.time;
        GC.CurentLevelLife = GC.MaxLevelLife;
        SceneManager.LoadScene(StringCollection.SCENE01, LoadSceneMode.Single);
    }

    public void UIInit(UIManager temp) {
        UIM = temp;
    }

    public void GameOver() {
        SceneManager.LoadScene(StringCollection.GAMEOVER, LoadSceneMode.Single);
    }

    public void StartMainMenu() {
        SceneManager.LoadScene(StringCollection.MAINMENU, LoadSceneMode.Single);
    }

    public static float GetTime() {
        return Time.time - GC.LevelTimeStart;
    }

    public static void ChangeEnemyCount(int count, int loseLife = 0) {
        GC.EnemyCount += count;
        GC.CurentLevelLife -= loseLife;
        GC.UIM.UpdateLife(GC.CurentLevelLife / (float)GC.MaxLevelLife);
        if (GC.CurentLevelLife <= 0) {
            GC.GameOver();
        }
        if (GC.EnemyCount <= 0) {
            GC.StartMainMenu();
        }
        GC.UIM.UpdateEnemyCount(GC.EnemyCount);
    }

    public void FreezeGame() {
        if (Time.timeScale != 0) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }

    public static void PlayerAnimation() {
        print("dam dam dam dam .... Player"); //----- ----- Print ----- -----
    }

    public static void EndOfIntro() {
        GameObject temp = GameObject.FindGameObjectWithTag(StringCollection.PLAYER);
        if (temp) {
            temp.GetComponent<PlayerControler>().SetPlayerControl(true);
        }
    }
}
