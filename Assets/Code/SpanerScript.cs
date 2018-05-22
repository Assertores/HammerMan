using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpanerScript : MonoBehaviour {

    [SerializeField]
    GameObject Creap1;
    [SerializeField]
    float SpawnRate = 10.0f;
    [SerializeField]
    float SpawnActiveTime = 100f;

    float LastSpawn = 0.0f;
    GameControler GC;
	// Use this for initialization
	void Start () {
		GC = GameObject.Find("GameManager").GetComponent<GameControler>();
        if (!GC) {
            throw new System.Exception("GameManager not found. Spawner");
        }
        if (!Creap1) {
            throw new System.Exception("Creep1 not assigned. Spawner");
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (GC.GetTime() > LastSpawn + SpawnRate && GC.GetTime() < SpawnActiveTime) {
            Instantiate(Creap1);
            Creap1.transform.position = this.transform.position;
            LastSpawn = GC.GetTime();
        }
	}
}
