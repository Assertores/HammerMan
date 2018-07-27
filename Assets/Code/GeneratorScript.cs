using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorScript : MonoBehaviour {

    [SerializeField]
    Image LifeBar;
    [SerializeField]
    Image Shield;
    [SerializeField]
    float MaxShieldLife;
    float CurrentShieldLife;
    float LastHit;
    [SerializeField]
    float RegDelay;
    [SerializeField]
    float RegRate;
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
        CurrentShieldLife = MaxShieldLife;
        Shield.fillAmount = 1;
        CurrentLife = MaxLife;
        LifeBar.fillAmount = 1;

        GameManager.ChangeGeneratorCount(1);
    }
	
	// Update is called once per frame
	void OnDestroy () {
        GameManager.ChangeGeneratorCount(-1);
    }

    private void Update() {
        if (CurrentShieldLife < MaxShieldLife && LastHit + RegDelay < GameManager.GetTime()) {
            CurrentShieldLife += RegRate * Time.deltaTime;
            Shield.fillAmount = CurrentShieldLife / MaxShieldLife;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {//macht dass es getroffen werden kann
        if (col.transform.gameObject.tag == StringCollection.HAMMER) {
            Hit();
        }
    }

    private void Hit(int damage = 1) {//TODO: Überarbeiten
        GameObject handle;
        if (CurrentShieldLife > 0)
            CurrentShieldLife -= damage;
        else
            CurrentLife -= damage;
        LastHit = GameManager.GetTime();
        Shield.fillAmount = CurrentShieldLife / MaxShieldLife;
        LifeBar.fillAmount = CurrentLife / (float)MaxLife;

        if (CurrentShieldLife < 0)
            CurrentShieldLife = 0;

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
