using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpanerScript : MonoBehaviour {

    [SerializeField]
    Image WaveBar;
    [SerializeField]
    int MaxLife = 1;
    int CurrentLife;
    [SerializeField]
    GameObject Creap1;
    [SerializeField]
    float SpawnRate;
    [SerializeField]
    float WaveLength;
    [SerializeField]
    float GapLength;
    [SerializeField]
    float Factor;
    [SerializeField]
    float MaximumGapLength;
    [SerializeField]
    [Tooltip("0 = no change, 1 = liniar growth, 2 = growing growth, 3 = stagnating growth")]
    int UseFunction;
    
    float NextSpawn;
    int WaveCount = 0;

	void Start () {
        if (!WaveBar) {
            throw new System.Exception("WaveBar not assigned. Spawner");
        }
        CurrentLife = MaxLife;
        NextSpawn = GameControler.GetTime();
        GameControler.ChangeEnemyCount(100);
    }
	
	void Update () {
        switch (UseFunction) {
            case 0: SpawnBehavior0(); break;
            case 1: SpawnBehavior1(); break;
            case 2: SpawnBehavior2(); break;
            case 3: SpawnBehavior3(); break;
        }
    }

    void SpawnBehavior0() {
        if (GameControler.GetTime() % (WaveLength + GapLength) <= WaveLength && GameControler.GetTime() >= NextSpawn) {
            Instantiate(Creap1).transform.position = this.transform.position;
            GameControler.ChangeEnemyCount(1);
            NextSpawn = GameControler.GetTime() + SpawnRate;
        }
        WaveBar.fillAmount = CurrentLife / (float)MaxLife;
    }

    void SpawnBehavior1() {
        SpawnBehavior0();
        if (GameControler.GetTime() % (WaveLength + GapLength) > WaveLength && GameControler.GetTime() > WaveCount * (WaveLength + GapLength) && GapLength < MaximumGapLength) {
            WaveCount++;
            GapLength += Factor;
        }
        if(GapLength > MaximumGapLength) {
            GapLength = MaximumGapLength;
        }
    }

    void SpawnBehavior2() {
        SpawnBehavior0();
        if (GameControler.GetTime() % (WaveLength + GapLength) > WaveLength && GameControler.GetTime() > WaveCount * (WaveLength + GapLength) && GapLength < MaximumGapLength) {
            WaveCount++;
            GapLength += Factor * WaveCount;
        }
        if (GapLength > MaximumGapLength) {
            GapLength = MaximumGapLength;
        }
    }

    void SpawnBehavior3() {
        SpawnBehavior0();
        if (GameControler.GetTime() % (WaveLength + GapLength) > WaveLength && GameControler.GetTime() > WaveCount * (WaveLength + GapLength) && GapLength < MaximumGapLength) {
            WaveCount++;
            GapLength += Factor * 1/(float)WaveCount;
        }
        if (GapLength > MaximumGapLength) {
            GapLength = MaximumGapLength;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.transform.gameObject.tag == StringCollection.HAMMER) {
            Hit();
        }
    }

    private void Hit(int damage = 1) {
        CurrentLife -= damage;
        if(CurrentLife <= 0) {
            GameControler.ChangeEnemyCount(-100);
            GameObject.Destroy(this.transform.gameObject);
        }
    }
}
