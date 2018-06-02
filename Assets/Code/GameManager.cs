using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //===== ===== Outer Variables ===== =====
    [SerializeField]
    bool StartingInLevel = false;

    //===== ===== Inner Variables ===== =====
    float LevelTimeAtStart = 0;
    int CurrentLife = 0;
    int EnemyCount = 0;
    
    //===== ===== Singelton ===== =====
    public static GameManager GM = null;

    void Awake() {
        if (GM == null) {
            GM = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this.gameObject);
        }
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

    public static void StartMainMenu() {

    }

    public static void StartLevel(int level) {
        GM.LevelTimeAtStart = Time.time;
        //init shit
        if (GM.StartingInLevel) {
            return;
        }
        //load level
    }

    public static void StartGameOver() {

    }

    public static void StartCreadits() {

    }

    //===== ===== Registration ===== =====
    UIManager UIM = null;
    public static void RegistUI(UIManager handle) {
        if (GM.UIM == handle)
            GM.UIM = null;
        else
            GM.UIM = handle;
    }
    
    public static void RegistPlayer() {

    }
    
    public static void RegistCamera() {

    }

    LevelInfos LI;
    public static void RegistLvlInfos(LevelInfos handle) {
        if (GM.LI == handle)
            GM.LI = null;
        else
            GM.LI = handle;
    }

    //===== ===== Comunicator ===== =====
    public static void ChangeEnemyCount(int count = 1, int life = 0) {
        GM.EnemyCount += count;
        GM.CurrentLife += life;

        if(GM.CurrentLife <= 0) {
            GameManager.StartGameOver();
            return;
        }
        if(GM.EnemyCount <= 0) {
            GameManager.StartMainMenu();
            return;
        }

        if(GM.UIM == null) {
            print("no UI available");// ----- ----- LOG ----- -----
            return;
        }
        GM.UIM.UpdateLife(GM.CurrentLife/(float)GM.LI.GetLife());
        GM.UIM.UpdateEnemyCount(GM.EnemyCount);
    }
    //===== ===== Library ===== =====
    public static float GetTime() {
        return Time.time - GM.LevelTimeAtStart;
    }
}
