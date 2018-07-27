using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class GameManager : MonoBehaviour {

    //===== ===== Outer Variables ===== =====
    [SerializeField]
    bool StartingInLevel = false;
    [SerializeField]
    bool DebugMode = false;

    public System.Action<int> BPMUpdate;

    [Header("Debug Infos")]

    //===== ===== Inner Variables ===== =====
    public float LevelTimeAtStart = 0;
    Stopwatch LevelTime = null;
    public float BeatTimeAtStart = 0;
    public double BeatSeconds = 0;
    public int BeatCount = 0;
    public int CurrentLife = 0;
    public int EnemyCount = 0;
    double beatTime = 0;

    int GeneratorCount = 0;
    public bool GeneratorAlive { get; private set; }

    public int Scene = 0;

    int NextLevel = 0;

    //===== ===== Singelton ===== =====
    public static GameManager GM = null;

    void Awake() {
        //LogSystem.LogOnConsole("GameManager awake bevor: " + GM.name);// ----- ----- LOG ----- -----
        if (GM == null) {
            GM = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this.gameObject);
        }
        LogSystem.LogOnConsole("GameManager awake after: " + GM.name);// ----- ----- LOG ----- -----
    }

    //===== ===== Starting of ===== =====
    void Start() {
        LevelTime = new Stopwatch();
        if (!StartingInLevel) {
            StartMainMenu();
        } else {
            StartLevel(0);
            StartingInLevel = false;
        }
    }

    private void Update() {
        if (InputControler.ExitCount > 0) {//kümmert sich um exit behavior
            InputControler.PopExit();

            if (GM.Scene == 0) {
                StopExe();
            } else if (GM.Scene >= 2) {
                EndGame(false);
            } else if (Time.timeScale != 0) {
                GM.FreezeGame();
            } else {
                StartMainMenu();
            }
        }
        //beatTime += Time.deltaTime;
        //if (beatTime >= BeatSeconds)
        if(GM.BPMUpdate != null && BeatSeconds != 0 && GameManager.GetTime() >= BeatTimeAtStart + BeatCount * BeatSeconds) {
            print("test");
            beatTime -= BeatSeconds;
            GM.BPMUpdate(GM.BeatCount);
            GM.BeatCount++;
        }
    }

    public void StartMainMenu() {
        SceneManager.LoadScene(StringCollection.MAINMENU, LoadSceneMode.Single);
        GM.Scene = 0;
    }

    public void StartLevel(int level) {
        //GM.LevelTimeAtStart = Time.time;
        GM.GeneratorAlive = true;
        GM.LevelTime.Reset();
        GM.LevelTime.Start();
        LogSystem.LogOnFile("===== LevelStart =====");// ----- ----- LOG ----- -----
        GM.EnemyCount = 0;
        if (!GM.StartingInLevel) {
            switch (level) {//wählt level aus
            case 1:
                SceneManager.LoadScene(StringCollection.SCENE01);
                break;
            default:
                LogSystem.LogOnConsole("Level not found");// ----- ----- LOG ----- -----
                break;
            }
        } else {
            level = 1;
        }
        GM.Scene = level + 2;
    }

    public void StartGameOver() {
        SceneManager.LoadScene(StringCollection.GAMEOVER, LoadSceneMode.Single);
        GM.Scene = 1;
    }

    public void StartCreadits() {
        SceneManager.LoadScene(StringCollection.CREADITS, LoadSceneMode.Single);
        GM.Scene = 2;
    }

    public void StopExe() {
        Application.Quit();
    }

    //===== ===== Registration ===== =====
    public UIManager UIM = null;
    public static void RegistUI(UIManager handle) {
        if (GM.UIM == handle)
            GM.UIM = null;
        else
            GM.UIM = handle;
    }

    public PlayerMovment PM = null;
    public static void RegistPlayer(PlayerMovment handle) {
        LogSystem.LogOnConsole("GameManager got: " + handle);// ----- ----- LOG ----- -----
        if (GM.PM == handle)
            GM.PM = null;
        else
            GM.PM = handle;
    }

    public CameraShake CS = null;
    public static void RegistCamera(CameraShake handle) {
        if (GM.CS == handle)
            GM.CS = null;
        else
            GM.CS = handle;
    }

    public LevelInfos LI = null;
    public static void RegistLvlInfos(LevelInfos handle) {
        if (GM.LI == handle)
            GM.LI = null;
        else {
            GM.LI = handle;
            GM.CurrentLife = handle.GetLife();
            LogSystem.LogOnConsole("live is: " + handle.GetLife());// ----- ----- LOG ----- -----
        }
    }

    //===== ===== Comunicator ===== =====
    public static bool ChangeEnemyCount(int count = 1, int life = 0) {
        if(GM == null) {
            LogSystem.LogOnConsole("Game Manager not available");// ----- ----- LOG ----- -----
            return false;
        }
        GM.EnemyCount += count;
        LogSystem.LogOnFile("now the enemycount is " + GM.EnemyCount.ToString());// ----- ----- LOG ----- -----
        if (GM.LI == null) {
            LogSystem.LogOnConsole("no Level Infos available");// ----- ----- LOG ----- -----
            return false;
        }
        //LogSystem.LogOnConsole("Current Life was: " + GM.CurrentLife);// ----- ----- LOG ----- -----
        GM.CurrentLife += life;
        //LogSystem.LogOnConsole("Current Life is: " + GM.CurrentLife);// ----- ----- LOG ----- -----

        if (GM.CurrentLife <= 0) {
            GM.StartGameOver();
            return true;
        }
        if (GM.EnemyCount <= 0) {
            EndGame(true);
            return true;
        }
        return true;
    }

    public static void ChangeGeneratorCount(int count = 1) {
        GM.GeneratorCount += count;
        if (GM.GeneratorCount <= 0)
            GM.GeneratorAlive = false;
    }

    public static bool CameraEffectOnEnemyDeath() {
        if (GM.CS == null) {
            LogSystem.LogOnConsole("no Camera available");// ----- ----- LOG ----- -----
            return false;
        }
        GM.CS.StartShaking(true, 0.2f);
        return true;
    }

    public static bool CameraEffectOnEnemyExit() {
        if (GM.CS == null) {
            LogSystem.LogOnConsole("no Camera available");// ----- ----- LOG ----- -----
            return false;
        }
        GM.CS.StartShaking(false);
        return true;
    }

    public static bool PlayerAnimation() {
        if (GM.PM == null) {
            LogSystem.LogOnConsole("no Player available");// ----- ----- LOG ----- -----
            return false;
        }
        print("start Player Animation");// ----- ----- LOG ----- -----
        return true;
    }

    public static bool EndOfIntro() {
        if (GM.PM == null) {
            LogSystem.LogOnConsole("no Player available");// ----- ----- LOG ----- -----
            return false;
        }
        GM.PM.SetPlayerControl(true);
        return true;
    }

    public static Vector2 GetPlayerPosition() {
        Vector2 ret = new Vector2(0, 0);
        if (GM.PM == null) {
            LogSystem.LogOnConsole("no Player available");// ----- ----- LOG ----- -----
            return ret;
        }
        ret = new Vector2(GM.PM.transform.position.x, GM.PM.transform.position.y);
        return ret;
    }

    public static bool KickPlayer(Vector2 force) {
        if(GM.PM == null) {
            LogSystem.LogOnConsole("no Player available");// ----- ----- LOG ----- -----
            return false;
        }
        GM.PM.KickPlayer(force);
        return true;
    }

    public static void StartBeats(float beats, float nullTime) { //beats = abstand zweiter beats in secunden, nullTime = zeitpunkt des nullten beats abLeveltime
        GM.BeatSeconds = beats;
        GM.BeatCount = -(int)(nullTime/beats);
        GM.BeatTimeAtStart = GameManager.GetTime() + nullTime;
        print("lets go:" + (GM.BeatTimeAtStart + GM.BeatCount * GM.BeatSeconds));
        GM.beatTime = GM.BeatSeconds - (GM.BeatTimeAtStart + GM.BeatCount * GM.BeatSeconds);
        //GM.StartCoroutine(GM.TriggerBPMUpdate());
        //GM.Invoke("TriggerBPMUpdate", (GM.BeatTimeAtStart + GM.BeatCount * GM.BeatSeconds) - Time.time);
    }

    //===== ===== Library ===== =====

    public static void EndGame(bool won = false) {
        GM.LevelTime.Stop();
        GM.BPMUpdate = null;
        GM.BeatTimeAtStart = 0;
        GM.BeatSeconds = 0;
        GM.BeatCount = 0;
        if (won) {
            LogSystem.LogOnFile("===== Game Won =====");// ----- ----- LOG ----- -----
            GM.NextLevel = GM.Scene - 1;
            GM.StartMainMenu();
        } else {
            LogSystem.LogOnFile("===== Game failed =====");// ----- ----- LOG ----- -----
            GM.NextLevel = GM.Scene - 2;
            GM.StartGameOver();
        }
        GM.CurrentLife = 0;
        GM.EnemyCount = 0;
        //GM.CancelInvoke("TriggerBPMUpdate");
    }
    public static float GetTime() {
        //print(GM.LevelTime.ElapsedMilliseconds);
        return (float)GM.LevelTime.ElapsedMilliseconds/1000;
        //return Time.time - GM.LevelTimeAtStart;
    }

    public static float GetBeatSeconds() {
        return (float)GM.BeatSeconds;
    }

    public static float GetTimeOfLastBeat() {
        return (float)(GM.BeatTimeAtStart + (GM.BeatCount - 1) * GM.BeatSeconds);
    }

    public static bool GetDebugMode() {
        return GM.DebugMode;
    }

    public static int GetNextLevel() {
        return GM.NextLevel;
    }

    public static int GetBeatCount() {
        return GM.BeatCount;
    }

    public void FreezeGame() {
        if (Time.timeScale != 0) {
            Time.timeScale = 0;
            //TODO: find alle audiosorces and pause them
        } else {
            Time.timeScale = 1;
            //TODO: find all audiosorces and unpause them
        }
    }
}
