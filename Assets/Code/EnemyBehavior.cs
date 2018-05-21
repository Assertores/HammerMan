using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    Rigidbody2D rb;

    bool DirRight = true;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        if (!rb) {
            throw new System.Exception("Rigitbody not found. Enemy");
        }
        if (DirRight) {
            rb.velocity = this.transform.right;
        } else {
            rb.velocity = -this.transform.right;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (rb.velocity.x <= 0) {
            if (DirRight) {
                DirRight = false;
                rb.velocity = -this.transform.right;
            } else {
                DirRight = true;
                rb.velocity = this.transform.right;
            }
        }

	}

    private void OnTriggerEnter2D(Collider2D col) {
        switch (col.transform.gameObject.tag) {
            case "Hammer":
                Die();
                break;
            case "Finish":

            default:
                break;
        }
    }

    public void Die() {
        //here nice sfx and animation
        //comunicate to gamemanager your dad
        Destroy(this);
    }
}
