using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpanerScript : MonoBehaviour {

    [Header("Spawner")]
    [SerializeField]
    AudioClip[] HitSound;
    [SerializeField]
    AudioClip DeathSound;
    [SerializeField]
    Image LifeBar;
    [SerializeField]
    int MaxLife = 1;
    int CurrentLife;
    [SerializeField]
    [Header("Creaps")]
    SpawnCreap[] Creaps;
    List<GameObject> Creap1;
    [SerializeField]
    [Header("Wave")]
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

    [SerializeField]
    GameObject SpawnHitParticle;

    float NextSpawn;
    int WaveCount = 0;

	void Start () {
        if (!LifeBar) {
            throw new System.Exception("WaveBar not assigned. Spawner");
        }
        CurrentLife = MaxLife;
        NextSpawn = GameManager.GetTime();
        LifeBar.fillAmount = 1;
        if (!GameManager.ChangeEnemyCount(100)) {//TODO: gegen Spawner-Generator system ersetzten
            LogSystem.LogOnConsole("Spawner konnte nicht den enemyCount erhöhen");
        }
        Creap1 = new List<GameObject>();
        Creap1.Clear();
        for(int i = 0; i < Creaps.Length; i++) {//fügt creaps in der anzahl der Ratio zu ner liste hinzu aus der ein zufälliges element genommen wird
            for(int j = 0; j < Creaps[i].Ratio; j++) {
                Creap1.Add(Creaps[i].Creap);
            }
        }
    }

    private void OnDestroy() {
        GameManager.ChangeEnemyCount(-100);
    }
	
	void Update () {
        switch (UseFunction) {
            case 0: SpawnBehavior0(); break;
            case 1: SpawnBehavior1(); break;
            case 2: SpawnBehavior2(); break;
            case 3: SpawnBehavior3(); break;
        }
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

    private void OnTriggerEnter2D(Collider2D col) {//macht dass es getroffen werden kann
        if (col.transform.gameObject.tag == StringCollection.HAMMER) {
            Hit();
        }
    }

    private void Hit(int damage = 1) {
        GameObject handle;
        CurrentLife -= damage;
        LifeBar.fillAmount = CurrentLife / (float)MaxLife;
        if (CurrentLife <= 0) {
            LogSystem.LogOnConsole("Spawner is dead");// ----- ----- LOG ----- -----
            handle = Instantiate(SpawnHitParticle, this.transform.position, this.transform.rotation);
            if(DeathSound != null)
                handle.GetComponent<AudioSource>().clip = DeathSound;
            if (handle.GetComponent<ParticleKiller>() == null) {
                print("ParticalKillerScript kann nicht gefunden werden.");
            } else {
                handle.GetComponent<ParticleKiller>().PlayStart();
            }
            GameObject.Destroy(this.transform.gameObject);
            return;
        }
        LogSystem.LogOnConsole("Spawner got hit");// ----- ----- LOG ----- -----

        handle = Instantiate(SpawnHitParticle, this.transform.position, this.transform.rotation);
        if(HitSound.Length > 0) {//kümmert sich um particel
            int temp = Random.Range(0, HitSound.Length);
            handle.GetComponent<AudioSource>().clip = HitSound[temp];
        }
        if (handle.GetComponent<ParticleKiller>() == null) {
            print("ParticalKillerScript kann nicht gefunden werden.");
        } else {
            handle.GetComponent<ParticleKiller>().PlayStart();
        }
    }
}
