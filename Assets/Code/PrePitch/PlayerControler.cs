using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour {

    Rigidbody2D rb;

    [SerializeField]
    float PlayerSpeed = 1.0f;
    [SerializeField]
    float ClimbSpeed = 1.0f;
    [SerializeField]
    float FallThroughBoost = 1.0f;

    [Header("Debug Informations")]
    [SerializeField]
    LayerMask FallLayers;

    GameControler GC;

    float goHorizontal = 0.0f;
    float LastVerticalAxis = 0.0f;
    float VerticalAxis = 0.0f;
    bool goUp = false;
    bool isUpPossible = false;
    bool DirRight = true;
    public int goDown = 0;
    bool DownKeyIsPressed = false;
    bool PlayerIsInGround = false;
    Vector2 Ladder;
    bool InControle = false;


    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        if (!rb) {
            throw new System.Exception("Rigitbody not found. Player");
        }
    }
    
    void Update () {
        if (this.transform.position.y < -0.1) {
            this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
            goDown = 0;
        }
        InputManager();
	}

    private void FixedUpdate() {
        if (InControle) {
            Movement();
        }
    }


    private void InputManager() {
        goHorizontal = Input.GetAxis(StringCollection.HORIZONTAL);
        VerticalAxis = Input.GetAxis(StringCollection.VERTICAL);
        if (goHorizontal == 0) {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetButtonUp(StringCollection.CANCEL)) {
            GameControler.GC.FreezeGame();
        }

        if (VerticalAxis > 0) {
            goUp = true;
        } else if(goUp) {
            goUp = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        InputFall1();
        LastVerticalAxis = VerticalAxis;
    }

    private void InputFall0() {
        if ((VerticalAxis < 0 && VerticalAxis < LastVerticalAxis) && goDown == 0) {
            goDown = 1;
        } else if (VerticalAxis > LastVerticalAxis && goDown == 3) {
            goDown = 0;
        }
    }

    private void InputFall1() {
        if(VerticalAxis < 0 && VerticalAxis < LastVerticalAxis && !DownKeyIsPressed) {
            DownKeyIsPressed = true;
            goDown++;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - FallThroughBoost);
        }else if (VerticalAxis > LastVerticalAxis && DownKeyIsPressed) {
            DownKeyIsPressed = false;
        }
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
        MoveFall1();
    }

    void MoveHorizontal0() {
        int tempHorizontal = 0;
        if(goHorizontal < 0) {
            tempHorizontal = -1;
        }else if (goHorizontal > 0) {
            tempHorizontal = 1;
        }
        rb.velocity = new Vector2(tempHorizontal * PlayerSpeed, rb.velocity.y);

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
        this.transform.position = new Vector2(Ladder.x, this.transform.position.y);
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

    void MoveFall1() {
        if (Physics2D.IsTouchingLayers(GetComponent<CapsuleCollider2D>(), FallLayers) && !PlayerIsInGround) {
            PlayerIsInGround = true;
        }else if(!Physics2D.IsTouchingLayers(GetComponent<CapsuleCollider2D>(), FallLayers) && PlayerIsInGround) {
            PlayerIsInGround = false;
            goDown--;
        }
        if (goDown <= 0) {
            goDown = 0;
            GetComponent<CapsuleCollider2D>().isTrigger = false;
        } else {
            GetComponent<CapsuleCollider2D>().isTrigger = true;
        }
    }

    void OnTriggerStay2D(Collider2D col) {
        if (col.transform.gameObject.tag == StringCollection.LADDER) {
            Ladder = col.transform.position;
            isUpPossible = true;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.transform.gameObject.tag == StringCollection.LADDER) {
            if (goUp) {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                this.transform.position = new Vector2(this.transform.position.x, Ladder.y + 4.1f);
            }
            isUpPossible = false;
        }
    }

    void ChangeDir(bool right) {
        if (right != DirRight) {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        DirRight = right;
    }

    public void SetPlayerControl(bool controle) {
        InControle = controle;
    }
}
