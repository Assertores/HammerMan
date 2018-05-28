﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControler : MonoBehaviour {

    float LevelTimeStart = 0.0f;
    int EnemyCount = 0;
    int SpawnerCount = 0;
    [SerializeField]
    int MaxLevelLife = 10;
    private int CurentLevelLife = 0;

    UIManager UIM;

    public static GameControler GC = null;

    void Awake() {
        if (GC == null) {
            GC = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this.gameObject);
        }
        //StartMainMenu();
    }

    public void StartLvl() {
        
        SceneManager.LoadScene("Scene 01", LoadSceneMode.Single);

        LevelTimeStart = Time.time;
        CurentLevelLife = MaxLevelLife;
        print("init: " + CurentLevelLife + " ID: " + this.gameObject.GetInstanceID());

        /*try {
            UIM = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        } catch (System.Exception) {
            throw new System.Exception("UI Entity not found or UIManager not found as Component. GameControler");
        }*/
    }

    public void UIInit(UIManager temp) {
        UIM = temp;
    }

    public void GameOver() {
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

    public void StartMainMenu() {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public static float GetTime() {
        return Time.time - GC.LevelTimeStart;
    }

    public static void ChangeEnemyCount(int count, int loseLife = 0) {
        print("change: " + GC.CurentLevelLife + " ID: " + GC.gameObject.GetInstanceID());
        GC.EnemyCount += count;
        GC.CurentLevelLife -= loseLife;
        GC.UIM.UpdateLife(GC.CurentLevelLife / (float)GC.MaxLevelLife);
        if (GC.CurentLevelLife <= 0) {
            print("its all over:" + GC.CurentLevelLife);
            GC.GameOver();
        }
        GC.UIM.UpdateEnemyCount(GC.EnemyCount);
    }

    public void ChangeSpawnerCount(int count) {
        SpawnerCount += count;

    }
}
