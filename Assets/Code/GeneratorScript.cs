using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorScript : MonoBehaviour {

    [SerializeField]
    Image LifeBar;
    [SerializeField]
    int MaxLife = 1;
    int CurrentLife;
    [SerializeField]
    GameObject SpawnHitParticle;
    [SerializeField]
    AudioClip[] HitSound;
    [SerializeField]
    AudioClip DeathSound;

    // Use this for initialization
    void Start () {
        if (!LifeBar) {
            throw new System.Exception("WaveBar not assigned. Spawner");
        }
        CurrentLife = MaxLife;
        LifeBar.fillAmount = 1;

        if (!GameManager.ChangeEnemyCount(100)) {
            LogSystem.LogOnConsole("Spawner konnte nicht den enemyCount erhöhen");
        }
    }
	
	// Update is called once per frame
	void OnDestroy () {
        GameManager.ChangeEnemyCount(-100);
    }

    private void OnTriggerEnter2D(Collider2D col) {//macht dass es getroffen werden kann
        if (col.transform.gameObject.tag == StringCollection.HAMMER) {
            Hit();
        }
    }

    private void Hit(int damage = 1) {//TODO: Überarbeiten
        GameObject handle;
        CurrentLife -= damage;
        LifeBar.fillAmount = CurrentLife / (float)MaxLife;
        if (CurrentLife <= 0) {
            LogSystem.LogOnConsole("Spawner is dead");// ----- ----- LOG ----- -----
            handle = Instantiate(SpawnHitParticle, this.transform.position, this.transform.rotation);
            if (DeathSound != null)
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
        if (HitSound.Length > 0) {//kümmert sich um particel
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
