using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    [SerializeField]
    float EnemySpeed = 10;
    [SerializeField]
    float TurningDistance = 10;

    Rigidbody2D rb;

    bool DirRight = true;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        DirRight = (Random.value > 0.5f);
        if (!rb) {
            throw new System.Exception("Rigitbody not found. Enemy");
        }

        if (!DirRight) {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (Physics2D.Raycast(this.transform.position, this.transform.forward, TurningDistance)) {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        rb.velocity = new Vector2(DirRight ? EnemySpeed : -EnemySpeed, rb.velocity.y);
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
