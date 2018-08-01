using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultyKillControler : MonoBehaviour {

    [SerializeField]
    float LiveTime = 1;
    [SerializeField]
    float Speed = 1;

	// Use this for initialization
	void Start () {
        LiveTime += GameManager.GetTime();
        Invoke("Play", GameManager.GetBeatSeconds() - (GameManager.GetTime() - GameManager.GetTimeOfLastBeat()));
    }
	
	// Update is called once per frame
	void Update () {
		if(GameManager.GetTime() < LiveTime) {
            transform.position += new Vector3(0, Speed * Time.deltaTime, 0);
        } else {
            Destroy(gameObject);
        }
	}

    void Play() {
        GetComponent<AudioSource>().Play();
    }
}
