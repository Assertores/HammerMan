using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour {

    Rigidbody2D rb;

    [SerializeField]
    float PlayerSpeed = 1.0f;
    [SerializeField]
    float PlayerClimbSpeed = 1.0f;

    GameControler GC;

    float goHorizontal = 0.0f;
    bool goUp = false;
    float IPDown = 0.0f;
    bool isUpPossible = false;
    bool DirRight = true;


    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        if (!rb) {
            throw new System.Exception("Rigitbody not found. Player");
        }

        GC = GameObject.Find("GameManager").GetComponent<GameControler>();
        if (!GC) {
            throw new System.Exception("GameManager not found. Spawner");
        }
    }
    
    void Update () {
        Movement();

        if(IPDown != 0.0f && GC.GetTime() - IPDown > 0.5) {
            FallThrough(false);
            IPDown = 0.0f;
        }
        //Hammer.transform.Rotate(transform.forward, Mathf.Sin(GC.GetTime()));
	}

    private void FixedUpdate() {
        InputManager();
    }


    private void InputManager() {
        goHorizontal = Input.GetAxis("Horizontal");

        if (Input.GetAxis("Vertical") > 0) {
            goUp = true;
        } else {
            goUp = false;
        }
        if (Input.GetAxis("Vertical") < 0 && IPDown == 0.0f) {
            FallThrough(true);
            IPDown = GC.GetTime();
        }/* else if(Input.GetAxis("Vertical") >= 0) {
            IPDown = 0.0f;
        }*/
    }

    private void FallThrough(bool able) {
        GetComponent<CapsuleCollider2D>().enabled = !able;
    }

    private void Movement() {
        if(this.transform.position.y < 0) {
            this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        }
        rb.velocity = new Vector2(goHorizontal * PlayerSpeed * Time.deltaTime, rb.velocity.y);

        if((goHorizontal < 0 && DirRight) || (goHorizontal > 0 && !DirRight)) {
            ChangeDir(!DirRight);
        }

        if(goUp && isUpPossible) {
            rb.velocity = new Vector2(/*rb.velocity.x*/ 0, PlayerClimbSpeed * Time.deltaTime);
            //this.transform.position += new Vector3(this.transform.position.x, PlayerClimbSpeed * Time.deltaTime, this.transform.position.z);
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

    void ChangeDir(bool right) {
        if (right != DirRight) {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        DirRight = right;
    }
}
