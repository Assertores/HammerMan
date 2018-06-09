using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    Idle,
    Moving,
    Climbing,
    Jumping,
    Falling,
    Landing
}

public class PlayerMovment : MonoBehaviour {

    [SerializeField]
    float PlayerSpeed = 1.0f;
    [SerializeField]
    float ClimbSpeed = 1.0f;
    [SerializeField]
    float FallThroughBoost = 1.0f;
    [SerializeField]
    LayerMask FallLayers;
    [SerializeField]
    float HoverHight = 1.0f;
    [SerializeField]
    float JumpStrength = 1.0f;

    PlayerState State = PlayerState.Idle;
    float DistToGround = 0;

    Rigidbody2D rb;
    bool isUpPossible = false;
    bool DirRight = true;
    Vector2 Ladder;
    bool InControle = false;
    bool PlayerIsInGround = false;
    float oldGravityScale;

    private void OnDestroy() {
        GameManager.RegistPlayer(this);
    }

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        if (!rb) {
            throw new System.Exception("Rigitbody not found. Player");
        }
        oldGravityScale = rb.gravityScale;
        print("i'm here. Player: " + this);// ----- ----- LOG ----- -----
        GameManager.RegistPlayer(this);
    }
	
	void Update () {
        if (this.transform.position.y < -0.1) {
            this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
            InputControler.SetDown(0);
        }
        if (InControle) {
            DistToGround = Physics2D.Raycast(this.transform.position, -this.transform.up, 1000, FallLayers).distance;
            switch (State) {
                case PlayerState.Idle:
                    goto case PlayerState.Moving;
                case PlayerState.Moving:
                    if (DistToGround > HoverHight || InputControler.DownCount > 0)
                        ChangeState(PlayerState.Falling);
                    else if (InputControler.Vertical > 0 && isUpPossible)
                        ChangeState(PlayerState.Climbing);
                    else if (InputControler.Jump)
                        ChangeState(PlayerState.Jumping);
                    else if (InputControler.Horizontal == 0)
                        ChangeState(PlayerState.Idle);
                    else
                        ChangeState(PlayerState.Moving);
                    break;
                case PlayerState.Climbing:
                    if (!isUpPossible)
                        ChangeState(PlayerState.Idle);
                    break;
                case PlayerState.Jumping:
                    if (rb.velocity.y <= 0)
                        ChangeState(PlayerState.Falling);
                    break;
                case PlayerState.Falling:
                    if (DistToGround <= HoverHight && InputControler.DownCount <= 0)
                        ChangeState(PlayerState.Landing);
                    break;
                case PlayerState.Landing:
                    if (true)//if next time beat;
                        ChangeState(PlayerState.Idle);
                    break;
                default:
                    ChangeState(PlayerState.Idle);
                    break;
            }
        } else
            ChangeState(PlayerState.Idle);
    }

    void FixedUpdate() {
        if (InControle) {

            if (Physics2D.IsTouchingLayers(GetComponent<CapsuleCollider2D>(), FallLayers) && !PlayerIsInGround) {
                PlayerIsInGround = true;
            } else if (!Physics2D.IsTouchingLayers(GetComponent<CapsuleCollider2D>(), FallLayers) && PlayerIsInGround) {
                PlayerIsInGround = false;
                InputControler.ChangeDown(-1);
            }

            switch (State) {
                case PlayerState.Idle:
                    break;
                case PlayerState.Moving:
                    rb.velocity = new Vector2(InputControler.Horizontal * PlayerSpeed, rb.velocity.y);
                    if ((InputControler.Horizontal < 0 && DirRight) || (InputControler.Horizontal > 0 && !DirRight)) {
                        ChangeDir(!DirRight);
                    }
                    break;
                case PlayerState.Climbing:
                    if (InputControler.Up) {
                        rb.velocity = new Vector2(0, ClimbSpeed);
                    } else {
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                    }
                    break;
                case PlayerState.Jumping:
                    break;
                case PlayerState.Falling:
                    break;
                case PlayerState.Landing:
                    break;
                default:
                    break;
            }
        }
    }

    void ChangeState(PlayerState newState) {
        if (State == newState)
            return;

        switch (State) {
            case PlayerState.Idle:
                break;
            case PlayerState.Moving:
                break;
            case PlayerState.Climbing:
                rb.gravityScale = oldGravityScale;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                break;
            case PlayerState.Jumping:
                break;
            case PlayerState.Falling:
                GetComponent<CapsuleCollider2D>().isTrigger = false;
                break;
            case PlayerState.Landing:
                rb.gravityScale = oldGravityScale;
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - DistToGround, this.transform.position.z);
                break;
            default:
                break;
        }

        switch (newState) {
            case PlayerState.Idle:
                rb.velocity = new Vector2(0, 0);
                break;
            case PlayerState.Moving:
                break;
            case PlayerState.Climbing:
                print("i'm klimping");// ----- ----- LOG ----- -----
                rb.gravityScale = 0;
                this.transform.position = new Vector3(Ladder.x, this.transform.position.y, this.transform.position.z);
                break;
            case PlayerState.Jumping:
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + JumpStrength);
                break;
            case PlayerState.Falling:
                GetComponent<CapsuleCollider2D>().isTrigger = true;
                rb.AddForce(new Vector2(0, -FallThroughBoost));
                break;
            case PlayerState.Landing:
                print("i'm landing");// ----- ----- LOG ----- -----
                rb.gravityScale = 0;
                rb.velocity = new Vector2(0, 0);
                break;
            default:
                break;
        }

        State = newState;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.transform.gameObject.tag == StringCollection.LADDER) {
            print("i'm on ladder");// ----- ----- LOG ----- -----
            Ladder = col.transform.position;
            isUpPossible = true;
            //rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.transform.gameObject.tag == StringCollection.LADDER) {
            if (InputControler.Vertical < 0) {
                print("no ladder anymore");// ----- ----- LOG ----- -----
                rb.velocity = new Vector2(rb.velocity.x, 0.1f);
                //this.transform.position = new Vector2(this.transform.position.x, Ladder.y + 4.0f);
                //rb.bodyType = RigidbodyType2D.Dynamic;
            }
            rb.gravityScale = 5;
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
