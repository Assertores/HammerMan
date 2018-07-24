using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FurnitureBehavior : MonoBehaviour {

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
            GameManager.GM.BPMUpdate += Throw;
        }
    }

    void Throw(int i) {
        LogSystem.LogOnConsole("furiture got hit");// ----- ----- LOG ----- -----
        rb.velocity = new Vector2(Random.Range(-2f, 2f), Random.Range(7.0f, 10.0f));
        rb.angularVelocity = Random.Range(-500f, 500f);
        destroyed = true;
        GameManager.GM.BPMUpdate -= Throw;
    }
}
