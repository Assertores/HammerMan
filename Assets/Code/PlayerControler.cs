using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour {

    Rigidbody2D rb;

    [SerializeField]
    float PlayerSpeed = 1.0f;
    [SerializeField]
    float ClimbSpeed = 1.0f;
    [Header("Debug Informations")]
    [SerializeField]
    LayerMask FallLayers;

    GameControler GC;

    float goHorizontal = 0.0f;
    bool goUp = false;
    bool isUpPossible = false;
    bool DirRight = true;
    public int goDown = 0;
    int GroundInColider = 0;


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
        if (this.transform.position.y < 0) {
            this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        }
        InputManager();
        

        /*if (GetComponent<CapsuleCollider2D>().isTrigger == true) {

        }*/
        //Hammer.transform.Rotate(transform.forward, Mathf.Sin(GC.GetTime()));
	}

    private void FixedUpdate() {
        Movement();
    }


    private void InputManager() {
        goHorizontal = Input.GetAxis("Horizontal");

        if (Input.GetAxis("Vertical") > 0) {
            goUp = true;
        } else {
            goUp = false;
        }
        if (Input.GetAxis("Vertical") < 0 && goDown == 0) {
            goDown = 1;
            //GetComponent<CapsuleCollider2D>().isTrigger = true;
        }else if(Input.GetAxis("Vertical") >= -0.5 && goDown == 3) {
            goDown = 0;
        }
        print(Input.GetAxis("Vertical"));
    }

    private void FallThrough(bool able) {
        GetComponent<CapsuleCollider2D>().enabled = !able;
    }

    private void Movement() {
        
        MoveHorizontal0();
        MoveClimb2();
        MoveFall0();
    }

    void MoveHorizontal0() {
        rb.velocity = new Vector2(goHorizontal * PlayerSpeed, rb.velocity.y);

        if ((goHorizontal < 0 && DirRight) || (goHorizontal > 0 && !DirRight)) {
            ChangeDir(!DirRight);
        }
    }
    
    void MoveClimb0() {
        if (goUp && isUpPossible) {
            rb.velocity = new Vector2(rb.velocity.x, PlayerSpeed);
        }
    }

    void MoveClimb1() {
        if (goUp && isUpPossible) {
            this.transform.position =new Vector3(this.transform.position.x, this.transform.position.y + ClimbSpeed * Time.deltaTime, this.transform.position.z);
        }
    }

    void MoveClimb2() {
        if (goUp && isUpPossible) {
            rb.velocity = new Vector2(0, PlayerSpeed);
        }
    }

    void MoveFall0() {
        if (goDown == 1) {
            GetComponent<CapsuleCollider2D>().isTrigger = true;
            goDown = 2;
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
            //print("lets go");
        } else if (goDown == 2 && !Physics2D.IsTouchingLayers(GetComponent<CapsuleCollider2D>(), FallLayers)) {
            //print("thats enuth.");
            GetComponent<CapsuleCollider2D>().isTrigger = false;
            goDown = 3;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.transform.gameObject.tag == "Level") {
            GroundInColider++;
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
        }else if (col.transform.gameObject.tag == "Level") {
            GroundInColider--;
        }
    }

    void ChangeDir(bool right) {
        if (right != DirRight) {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        DirRight = right;
    }
}
