using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    [SerializeField]
    float EnemySpeed = 10;
    [SerializeField]
    int EnemyDamageOnExit = 1;
    [SerializeField]
    float TurningDistance = 10;
    public LayerMask layer;

    Rigidbody2D rb;

    GameControler GC;

    bool DirRight = true;
    int falling = 0;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        if (!rb) {
            throw new System.Exception("Rigitbody not found. Enemy");
        }

        DirRight = (Random.value > 0.5f);
        if (!DirRight) {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        GC = GameObject.Find("GameManager").GetComponent<GameControler>();
        if (!GC) {
            throw new System.Exception("GameManager not found. Enemy");
        }

    }
	
	void Update () {
        
        if (falling == 0 && rb.velocity.y < -1.5f ) {
            falling = 1;
        }else if (falling == 1) {
            falling = 2;
            ChangeDir(Random.value > 0.5f);
        }else if (falling == 2 && rb.velocity.y >= 0.0f) {
            falling = 0;
        }
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.right * rb.velocity.x, TurningDistance, layer);
        
        if (falling == 0 && hit.collider != null && hit.collider.tag == "Level") {
            DirRight = !DirRight;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        rb.velocity = new Vector2(DirRight ? EnemySpeed : -EnemySpeed, rb.velocity.y);
	}

    private void OnTriggerEnter2D(Collider2D col) {
        switch (col.transform.gameObject.tag) {
            case "Hammer":
                DieByHammer();
                break;
            case "Exit":
                DieByExit();
                break;
            default:
                break;
        }
    }

    void DieByHammer() {
        //here nice sfx and animation
        GC.ChangeEnemyCount(-1);
        GameObject.Destroy(this.transform.gameObject);
    }

    void DieByExit() {
        //here nice sfx and animation
        GC.ChangeEnemyCount(-1, EnemyDamageOnExit);
        GameObject.Destroy(this.transform.gameObject);
    }

    void ChangeDir(bool right) {
        if (right != DirRight) {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        DirRight = right;
    }
}
