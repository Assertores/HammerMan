using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class FurnitureBehavior : MonoBehaviour {

    [SerializeField]
    AudioClip[] HitSound;
    Rigidbody2D rb = null;

    bool destroyed = false;
    bool resolfed = false;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        if (!rb) {
            throw new System.Exception("no rigetbody on Furniture");
        }
	}
	
	void Update () {//macht dass der stuff nicht mehr berechnet wird wenn er auf dem boden liegt
		if(!resolfed && destroyed && rb.velocity.magnitude <= 0) {
            rb.bodyType = RigidbodyType2D.Static;
            resolfed = true;
        }
	}

    private void OnTriggerEnter2D(Collider2D col) { //started die animation
        LogSystem.LogOnConsole("some kollision has acurded");// ----- ----- LOG ----- -----
        if (!destroyed && col.transform.gameObject.tag == StringCollection.HAMMER) {
            //GameManager.GM.BPMUpdate += Throw;
            Invoke("Throw", GameManager.GetBeatSeconds() - (GameManager.GetTime() - GameManager.GetTimeOfLastBeat()));
        }
    }

    void Throw() {
        //print("furiture got hit " + gameObject.name + " " + (GameManager.GetTime() - GameManager.GetTimeOfLastBeat()));// ----- ----- LOG ----- -----

        int temp = Random.Range(0, HitSound.Length); //welcher sound soll für den tod verwendet werden
        if (HitSound.Length != 0) {//fügt audioclip hinzu
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = HitSound[temp];
            GetComponent<AudioSource>().Play();
        }

        rb.velocity = new Vector2(Random.Range(-2f, 2f), Random.Range(7.0f, 10.0f));
        rb.angularVelocity = Random.Range(-500f, 500f);
        destroyed = true;
        //GameManager.GM.BPMUpdate -= Throw;
    }
}
