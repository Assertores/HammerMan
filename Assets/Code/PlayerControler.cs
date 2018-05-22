using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour {

    Rigidbody2D rb;

    [SerializeField]
    float PlayerSpeed = 1.0f;

    float goHorizontal = 0.0f;
    bool goUp = false;
    bool isUpPossible = false;


    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        if (!rb) {
            throw new System.Exception("Rigitbody not found. Player");
        }
    }
    
    void Update () {
        Movement();
	}

    private void FixedUpdate() {
        InputManager();
    }


    private void InputManager() {
        goHorizontal = Input.GetAxis("Horizontal");

        if (Input.GetButton("Jump")) {
            goUp = true;
        } else {
            goUp = false;
        }
    }

    private void Movement() {
        rb.velocity = new Vector2(goHorizontal * PlayerSpeed * Time.deltaTime, rb.velocity.y);
        if(goUp && isUpPossible) {
            rb.velocity = new Vector2(rb.velocity.x, PlayerSpeed * Time.deltaTime);
        }
    }

    void OnTriggerStay2D(Collider2D col) {
        if (col.transform.gameObject.tag == "Ladder") {
            isUpPossible = true;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.transform.gameObject.tag == "Ladder") {
            isUpPossible = false;
        }
    }
}
