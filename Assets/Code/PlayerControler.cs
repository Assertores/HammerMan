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
    float LastVericalAxis = 0.0f;
    float VericalAxis = 0.0f;
    bool goUp = false;
    bool isUpPossible = false;
    bool DirRight = true;
    public int goDown = 0;
    float LadderX = 0.0f;


    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        if (!rb) {
            throw new System.Exception("Rigitbody not found. Player");
        }
    }
    
    void Update () {
        if (this.transform.position.y < -0.5) {
            this.transform.position = new Vector3(this.transform.position.x, -0.1f, this.transform.position.z);
        }
        InputManager();
	}

    private void FixedUpdate() {
        Movement();
    }


    private void InputManager() {
        goHorizontal = Input.GetAxis(StringCollection.HORIZONTAL);
        VericalAxis = Input.GetAxis(StringCollection.VERTICAL);

        if (Input.GetButtonUp(StringCollection.CANCEL)) {
            GameControler.GC.FreezeGame();
        }

        if (VericalAxis > 0) {
            goUp = true;
        } else {
            goUp = false;
        }
        if ((VericalAxis < 0 && VericalAxis < LastVericalAxis) && goDown == 0) {
            goDown = 1;
        }else if(VericalAxis > LastVericalAxis && goDown == 3) {
            goDown = 0;
        }
        LastVericalAxis = VericalAxis;
    }

    private void FallThrough(bool able) {
        GetComponent<CapsuleCollider2D>().enabled = !able;
    }

    private void Movement() {
        if (goUp && isUpPossible) {
            MoveClimb3();
        } else {
            MoveHorizontal0();
        }
        MoveFall0();
    }

    void MoveHorizontal0() {
        rb.velocity = new Vector2(goHorizontal * PlayerSpeed, rb.velocity.y);

        if ((goHorizontal < 0 && DirRight) || (goHorizontal > 0 && !DirRight)) {
            ChangeDir(!DirRight);
        }
    }
    
    void MoveClimb0() {
        rb.velocity = new Vector2(rb.velocity.x, PlayerSpeed);
    }

    void MoveClimb1() {
        this.transform.position =new Vector3(this.transform.position.x, this.transform.position.y + ClimbSpeed, this.transform.position.z);
    }

    void MoveClimb2() {
        rb.velocity = new Vector2(0, PlayerSpeed);
    }

    void MoveClimb3() {
        this.transform.position = new Vector2(LadderX, this.transform.position.y);
        rb.velocity = new Vector2(0, PlayerSpeed);
    }

    void MoveFall0() {
        if (goDown == 1) {
            GetComponent<CapsuleCollider2D>().isTrigger = true;
            goDown = 2;
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);
        } else if (goDown == 2 && !Physics2D.IsTouchingLayers(GetComponent<CapsuleCollider2D>(), FallLayers)) {
            GetComponent<CapsuleCollider2D>().isTrigger = false;
            goDown = 3;
        }
    }

    void OnTriggerStay2D(Collider2D col) {
        if (col.transform.gameObject.tag == StringCollection.LADDER) {
            LadderX = col.transform.position.x;
            isUpPossible = true;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.transform.gameObject.tag == StringCollection.LADDER) {
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
