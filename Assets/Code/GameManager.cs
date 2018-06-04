using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //===== ===== Outer Variables ===== =====
    [SerializeField]
    bool StartingInLevel = false;

    //===== ===== Inner Variables ===== =====
    float LevelTimeAtStart = 0;
    int CurrentLife = 0;
    int EnemyCount = 0;

    int Scene = 0;
    
    //===== ===== Singelton ===== =====
    public static GameManager GM = null;

    void Awake() {
        //print("GameManager awake bevor: " + GM.name);// ----- ----- LOG ----- -----
        if (GM == null) {
            GM = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this.gameObject);
        }
        print("GameManager awake after: " + GM.name);// ----- ----- LOG ----- -----
    }

    //===== ===== Starting of ===== =====
    void Start () {
        if (!StartingInLevel) {
            StartMainMenu();
        } else {
            StartLevel(0);
            StartingInLevel = false;
        }
	}

    private void Update() {
        if(InputControler.ExitCount > 0) {
            InputControler.PopExit();

            if(Scene == 0) {
                Application.Quit();
            }else if(Scene <= 2) {
                StartMainMenu();
            }else if(Time.timeScale != 0) {
                GM.FreezeGame();
            } else {
                StartMainMenu();
            }
        }

    }

    public static void StartMainMenu() {
        SceneManager.LoadScene(StringCollection.MAINMENU, LoadSceneMode.Single);
        GM.Scene = 0;
    }

    public static void StartLevel(int level) {
        GM.LevelTimeAtStart = Time.time;
        GM.EnemyCount = 0;
        if (!GM.StartingInLevel) {
            switch (level) {
                case 1: SceneManager.LoadScene(StringCollection.SCENE01, LoadSceneMode.Single); break;
                default: print("Level not found"); break;// ----- ----- LOG ----- -----
            }
        }
        GM.Scene = level + 2;
    }

    public static void StartGameOver() {
        SceneManager.LoadScene(StringCollection.GAMEOVER, LoadSceneMode.Single);
        GM.Scene = 1;
    }

    public static void StartCreadits() {
        SceneManager.LoadScene(StringCollection.CREADITS, LoadSceneMode.Single);
        GM.Scene = 2;
    }

    //===== ===== Registration ===== =====
    UIManager UIM = null;
    public static void RegistUI(UIManager handle) {
        if (GM.UIM == handle)
            GM.UIM = null;
        else
            GM.UIM = handle;
    }

    PlayerMovment PM = null;
    public static void RegistPlayer(PlayerMovment handle) {
        print("GameManager got: " + handle);// ----- ----- LOG ----- -----
        print("GameManager is: " + GM.name);// ----- ----- LOG ----- -----
        if (GM.PM == handle)
            GM.PM = null;
        else
            GM.PM = handle;
    }
    
    public static void RegistCamera() {

    }

    LevelInfos LI = null;
    public static void RegistLvlInfos(LevelInfos handle) {
        if (GM.LI == handle)
            GM.LI = null;
        else {
            GM.LI = handle;
            GM.CurrentLife = handle.GetLife();
            print("live is: " + handle.GetLife());
        }
    }

    //===== ===== Comunicator ===== =====
    public static bool ChangeEnemyCount(int count = 1, int life = 0) {
        if(GM.LI == null) {
            print("Level infos not loaded yet");// ----- ----- LOG ----- -----
            return false;
        }
        GM.EnemyCount += count;
        GM.CurrentLife += life;

        if(GM.CurrentLife <= 0) {
            print("i changed to game over: " + GM.CurrentLife);
            GameManager.StartGameOver();
            return true;
        }
        if(GM.EnemyCount <= 0) {
            GameManager.StartMainMenu();
            return true;
        }

        if(GM.UIM == null) {
            print("no UI available");// ----- ----- LOG ----- -----
            return false;
        }
        GM.UIM.UpdateLife(GM.CurrentLife/(float)GM.LI.GetLife());
        GM.UIM.UpdateEnemyCount(GM.EnemyCount);
        return true;
    }

    public static bool PlayerAnimation() {
        if(GM.PM == null) {
            print("no Player available");// ----- ----- LOG ----- -----
            return false;
        }
        print("start Player Animation");// ----- ----- LOG ----- -----
        return true;
    }

    public static bool EndOfIntro() {
        if(GM.PM == null) {
            print("no Player available");// ----- ----- LOG ----- -----
            return false;
        }
        GM.PM.SetPlayerControl(true);
        return true;
    }

    //===== ===== Library ===== =====
    public static float GetTime() {
        return Time.time - GM.LevelTimeAtStart;
    }

    public void FreezeGame() {
        if (Time.timeScale != 0) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }
}
