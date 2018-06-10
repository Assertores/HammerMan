using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
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
	
	void Update () {
		if(!resolfed && destroyed && rb.velocity.magnitude <= 0) {
            rb.bodyType = RigidbodyType2D.Static;
            resolfed = true;
        }
	}

    private void OnTriggerEnter2D(Collider2D col) {
        LogSystem.LogOnConsole("some kollision has acurded");// ----- ----- LOG ----- -----
        if (!destroyed && col.transform.gameObject.tag == StringCollection.HAMMER) {
            LogSystem.LogOnConsole("furiture got hit");// ----- ----- LOG ----- -----
            rb.velocity = new Vector2(Random.Range(-2f, 2f), Random.Range(7.0f, 10.0f));
            destroyed = true;
        }
    }
}
