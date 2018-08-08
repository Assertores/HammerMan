using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
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
    AudioClip[] ShieldHitSounds;
    [SerializeField]
    AudioClip[] HitSound;
    [SerializeField]
    AudioClip DeathSound;

    BoxCollider2D GenCol;
    AudioSource ShieldAudio;

    // Use this for initialization
    void Start () {
        if (!LifeBar) {
            throw new System.Exception("WaveBar not assigned. Spawner");
        }
        CurrentShieldLife = MaxShieldLife;
        Shield.fillAmount = 1;
        CurrentLife = MaxLife;
        LifeBar.fillAmount = 1;

        GenCol = GetComponent<BoxCollider2D>();
        ShieldAudio = GetComponent<AudioSource>();

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

    private void FixedUpdate() {
        BetterTriggerEnter();
    }

    bool IsEntert = false;
    void BetterTriggerEnter() {

        Collider2D col = null;
        foreach(Collider2D it in Physics2D.OverlapBoxAll((Vector2)transform.position + GenCol.offset, GenCol.size, 0)) {
            if (it.tag == StringCollection.HAMMER) {
                col = it;
                break;
            }
        }

        if (col != null) {
            if (!IsEntert) {
                IsEntert = true;
                LastHit = GameManager.GetTime();
                if (CurrentShieldLife > 0)
                    HitShield();
                else
                    Hit();
            }
        } else {
            IsEntert = false;
        }
    }

    /*private void OnTriggerEnter2D(Collider2D col) {//macht dass es getroffen werden kann
        if (col.transform.gameObject.tag == StringCollection.HAMMER) {
            LastHit = GameManager.GetTime();
            if (CurrentShieldLife > 0)
                HitShield();
            else
                Hit();
        }
    }*/

    private void HitShield (int damage = 1) {
        CurrentShieldLife -= damage;
        Shield.fillAmount = CurrentShieldLife / MaxShieldLife;
        if(ShieldHitSounds.Length > 0) {
            ShieldAudio.clip = ShieldHitSounds[Random.Range(0,ShieldHitSounds.Length-1)];
            Invoke("Play", GameManager.GetBeatSeconds() - (GameManager.GetTime() - GameManager.GetTimeOfLastBeat()));
        }
    }

    private void Hit(int damage = 1) {//TODO: Überarbeiten
        CurrentLife -= damage;
        LifeBar.fillAmount = CurrentLife / (float)MaxLife;

        if (CurrentShieldLife < 0)
            CurrentShieldLife = 0;

        GameObject handle;
        if (CurrentLife <= 0) {
            LogSystem.LogOnConsole("Spawner is dead");// ----- ----- LOG ----- -----
            handle = Instantiate(SpawnHitParticle, this.transform.position, this.transform.rotation);
            if (DeathSound != null) {
                handle.GetComponent<AudioSource>().clip = DeathSound;
                handle.GetComponent<ParticleKiller>().PlayStart();
            }
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
            ShieldAudio.clip = HitSound[Random.Range(0, HitSound.Length - 1)];
            Invoke("Play", GameManager.GetBeatSeconds() - (GameManager.GetTime() - GameManager.GetTimeOfLastBeat()));
        }/*
            int temp = Random.Range(0, HitSound.Length-1);
            handle.GetComponent<AudioSource>().clip = HitSound[temp];
        }
        if (handle.GetComponent<ParticleKiller>() == null) {
            print("ParticalKillerScript kann nicht gefunden werden.");
        } else {
            handle.GetComponent<ParticleKiller>().PlayStart();
        }*/
    }

    void Play() {
        ShieldAudio.Play();
    }
}
