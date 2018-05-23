using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpanerScript : MonoBehaviour {

    [SerializeField]
    Image WaveBar;
    //[SerializeField]
    //SpanerTimeStamp[] Waves;
    [SerializeField]
    GameObject Creap1;
    [SerializeField]
    float SpawnRate;
    [SerializeField]
    float WaveLength;
    [SerializeField]
    float GabLength;


    //int NextSpawnIndex = 0;
    //float LastSpawn = 0.0f;
    float NextSpawn;
    GameControler GC;

	void Start () {
		GC = GameObject.Find("GameManager").GetComponent<GameControler>();
        if (!GC) {
            throw new System.Exception("GameManager not found. Spawner");
        }
        /*if (Waves.Length == 0) {
            throw new System.Exception("no Creaps assigned. Spawner");
        }*/
        if (!WaveBar) {
            throw new System.Exception("WaveBar not assigned. Spawner");
        }
        NextSpawn = GC.GetTime();
    }
	
	void Update () {
        SpawnBehavior();


    }

    void SpawnBehavior() {
        /*if (GC.GetTime() > LastSpawn + SpawnRate && GC.GetTime() < SpawnActiveTime) {
            Instantiate(Creap1);
            Creap1.transform.position = this.transform.position;
            GC.ChangeEnemyCount(1);
            LastSpawn = GC.GetTime();
        }
        WaveBar.fillAmount = 1 - GC.GetTime()/SpawnActiveTime;*/

        /*if (GC.GetTime() > Waves[NextSpawnIndex].timeStamp) {
            Instantiate(Waves[NextSpawnIndex].creap).transform.position = this.transform.position;
            GC.ChangeEnemyCount(1);
            NextSpawnIndex++;
        }
        WaveBar.fillAmount = 1 - GC.GetTime() / Waves[Waves.Length - 1].timeStamp;*/

        if (GC.GetTime() % (WaveLength + GabLength) <= WaveLength && GC.GetTime() >= NextSpawn) {
            Instantiate(Creap1).transform.position = this.transform.position;
            GC.ChangeEnemyCount(1);
            NextSpawn = GC.GetTime() + SpawnRate;
        }


    }
}
