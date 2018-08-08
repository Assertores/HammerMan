using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpanerScript : MonoBehaviour {
    
    [Header("Creaps")]
    [SerializeField]
    SpawnCreap[] Creaps;
    List<GameObject> Creap1;
    [Header("Wave")]
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
    Animator anim;

	void Start () {
        anim = GetComponentInChildren<Animator>();
        if (!anim) {
            new System.Exception("animator not found. Spawner");
        }
        NextSpawn = GameManager.GetTime();
        Creap1 = new List<GameObject>();
        Creap1.Clear();
        for(int i = 0; i < Creaps.Length; i++) {//fügt creaps in der anzahl der Ratio zu ner liste hinzu aus der ein zufälliges element genommen wird
            for(int j = 0; j < Creaps[i].Ratio; j++) {
                Creap1.Add(Creaps[i].Creap);
            }
        }
    }
	
	void Update () {
        if (GameManager.GM.GeneratorAlive) {
            switch (UseFunction) {
            case 0: SpawnBehavior0(); break;
            case 1: SpawnBehavior1(); break;
            case 2: SpawnBehavior2(); break;
            case 3: SpawnBehavior3(); break;
            }
        }
    }

    void FixedUpdate() {
        //anim.SetBool("Alive", GameManager.GM.GeneratorAlive);
    }

    void SpawnBehavior0() {//spawnt gegner wenn es zeit dafür ist
        if (GameManager.GetTime() % (WaveLength + GapLength) <= WaveLength && GameManager.GetTime() >= NextSpawn) {
            Instantiate(Creap1[Random.Range(0,Creap1.Count)]).transform.position = this.transform.position;
            NextSpawn = GameManager.GetTime() + SpawnRate;
        }
    }

    void SpawnBehavior1() {//fügt jedesmal am ende den factor an die gaplength hinzu
        SpawnBehavior0();
        if (GameManager.GetTime() % (WaveLength + GapLength) > WaveLength && GameManager.GetTime() > WaveCount * (WaveLength + GapLength) && GapLength < MaximumGapLength) {
            WaveCount++;
            GapLength += Factor;
        }
        if(GapLength > MaximumGapLength) {
            GapLength = MaximumGapLength;
        }
    }

    void SpawnBehavior2() {//fügt jedesmal am ende den factor * waveAnzahl an die gaplength hinzu
        SpawnBehavior0();
        if (GameManager.GetTime() % (WaveLength + GapLength) > WaveLength && GameManager.GetTime() > WaveCount * (WaveLength + GapLength) && GapLength < MaximumGapLength) {
            WaveCount++;
            GapLength += Factor * WaveCount;
        }
        if (GapLength > MaximumGapLength) {
            GapLength = MaximumGapLength;
        }
    }

    void SpawnBehavior3() {//fügt jedesmal am ende den factor * 1/waveAnzahl an die gaplength hinzu
        SpawnBehavior0();
        if (GameManager.GetTime() % (WaveLength + GapLength) > WaveLength && GameManager.GetTime() > WaveCount * (WaveLength + GapLength) && GapLength < MaximumGapLength) {
            WaveCount++;
            GapLength += Factor * 1/(float)WaveCount;
        }
        if (GapLength > MaximumGapLength) {
            GapLength = MaximumGapLength;
        }
    }
}
