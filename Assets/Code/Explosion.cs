using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    [SerializeField]
    float explosionDelay = 0.0f;
    [SerializeField]
    float explosionForce = 0.0f;

    float startTime;
	// Use this for initialization
	void Start () {
        startTime = GameManager.GetTime();
	}
	
	// Update is called once per frame
	void Update () {
		if(startTime+explosionDelay <= GameManager.GetTime()) {
            Vector2 temp = GameManager.GetPlayerPosition();
            temp = new Vector2(temp.x - this.transform.position.x, temp.y - this.transform.position.y);
            temp *= explosionForce * 1 / temp.magnitude;
            GameManager.KickPlayer(temp);
            GameObject.Destroy(this.transform.gameObject);
        }
	}
}
