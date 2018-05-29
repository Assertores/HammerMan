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
    float GabLength;
    
    float NextSpawn;

	void Start () {
        if (!WaveBar) {
            throw new System.Exception("WaveBar not assigned. Spawner");
        }
        CurrentLife = MaxLife;
        NextSpawn = GameControler.GetTime();
        GameControler.ChangeEnemyCount(100);
    }
	
	void Update () {
        SpawnBehavior();
    }

    void SpawnBehavior() {
        if (GameControler.GetTime() % (WaveLength + GabLength) <= WaveLength && GameControler.GetTime() >= NextSpawn) {
            Instantiate(Creap1).transform.position = this.transform.position;
            GameControler.ChangeEnemyCount(1);
            NextSpawn = GameControler.GetTime() + SpawnRate;
        }
        WaveBar.fillAmount = CurrentLife / (float)MaxLife;
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
