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

    public PlayerState State = PlayerState.Idle;
    float DistToGround = 0;

    Rigidbody2D rb;
    HammerManager Hammer;
    bool isUpPossible = false;
    bool DirRight = true;
    Vector2 Ladder;//position der leiter an der du hoch gehst
    bool InControle = false;
    bool PlayerIsInGround = false;
    float oldGravityScale;

    private void OnDestroy() {
        GameManager.RegistPlayer(this);
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        if (!rb) {
            throw new System.Exception("Rigitbody not found. Player");
        }

        Hammer = GetComponentInChildren<HammerManager>();
        if (!Hammer) {
            throw new System.Exception("Hammer not found. Player");
        }

        oldGravityScale = rb.gravityScale;
        LogSystem.LogOnConsole("i'm here. Player: " + this);// ----- ----- LOG ----- -----
        GameManager.RegistPlayer(this);

        Animator anim = GetComponentInChildren<Animator>();
        if (!anim) {
            throw new System.Exception("No Animation vor hammerman. Player");
        }
        anim.speed = anim.GetCurrentAnimatorStateInfo(0).length / GameManager.GetBeatSeconds();
        GameManager.GM.BPMUpdate += StartAnim;
    }

    void StartAnim(int i) {
        Animator anim = GetComponentInChildren<Animator>();
        if (!anim) {
            throw new System.Exception("No Animation vor hammerman. Player");
        }
        anim.Play(anim.GetCurrentAnimatorStateInfo(0).fullPathHash);
        GameManager.GM.BPMUpdate -= StartAnim;
    }

    void Update() {
        if (this.transform.position.y < -0.1) {//macht das man nicht durch den boden durchfallen kann
            this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
            InputControler.SetDown(0);
            ChangeState(PlayerState.Idle);
        }
        if (InControle) {
            DistToGround = Physics2D.Raycast(this.transform.position, -this.transform.up, 1000, FallLayers).distance;
            switch (State) { //finite state machine: übergänge von states
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
                else if (this.transform.position.y - Ladder.y > 2.7 && InputControler.Horizontal != 0) {
                    this.transform.position = new Vector3(this.transform.position.x, Ladder.y + 3.1f, this.transform.position.z);
                    ChangeState(PlayerState.Moving);
                } else if (InputControler.Vertical <= 0)
                    ChangeState(PlayerState.Falling);
                break;
            case PlayerState.Jumping:
                if (rb.velocity.y <= 0)
                    ChangeState(PlayerState.Falling);
                break;
            case PlayerState.Falling:
                //print("================================" + InputControler.DownCount);
                if (DistToGround <= HoverHight && InputControler.DownCount <= 0)
                    ChangeState(PlayerState.Landing);
                break;
            case PlayerState.Landing:
                //if (GameManager.GetHammerTime() % 1 > 0.9)
                ChangeState(PlayerState.Idle);
                break;
            default:
                StateMachine_Transition01();
                break;
            }
        } else
            ChangeState(PlayerState.Idle);
    }

    void FixedUpdate() {
        if (InControle) {

            if (Physics2D.IsTouchingLayers(GetComponent<CapsuleCollider2D>(), FallLayers) && !PlayerIsInGround) {//macht dass man durch die richtige anzahl an ebenen durchfällt
                PlayerIsInGround = true;
            } else if (!Physics2D.IsTouchingLayers(GetComponent<CapsuleCollider2D>(), FallLayers) && PlayerIsInGround) {
                PlayerIsInGround = false;
                InputControler.ChangeDown(-1);
            }

            switch (State) { //finite state machine: während diesem state
            case PlayerState.Idle:
                break;
            case PlayerState.Moving:
                rb.velocity = new Vector2(InputControler.Horizontal * PlayerSpeed, rb.velocity.y);
                if ((InputControler.Horizontal < 0 && DirRight) || (InputControler.Horizontal > 0 && !DirRight)) {
                    ChangeDir(!DirRight);
                }
                break;
            case PlayerState.Climbing:
                rb.velocity = new Vector2(0, InputControler.Vertical * ClimbSpeed);
                this.transform.position = new Vector3(Ladder.x, this.transform.position.y, this.transform.position.z);
                break;
            case PlayerState.Jumping:
                break;
            case PlayerState.Falling:
                goto case PlayerState.Moving;
            /*case PlayerState.Landing:
                break;*/
            default:
                StateMachine_StayInState01();
                break;
            }
        }
    }

    void ChangeState(PlayerState newState) {
        if (State == newState) //stellt sicher, dass es einen übergang giebt
            return;

        switch (State) { //finite state machine: bei verlassen des states
        case PlayerState.Idle:
            break;
        case PlayerState.Moving:
            break;
        case PlayerState.Climbing:
            rb.gravityScale = oldGravityScale;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            InputControler.SetDown(0);
            break;
        case PlayerState.Jumping:
            break;
        case PlayerState.Falling:
            GetComponent<CapsuleCollider2D>().isTrigger = false;
            break;
        /*case PlayerState.Landing:
            rb.gravityScale = oldGravityScale;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - DistToGround, this.transform.position.z);
            break;*/
        default:
            StateMachine_LeaveState01();
            break;
        }

        State = newState;

        switch (State) { //finite state machine: bei betreten des states
        case PlayerState.Idle:
            rb.velocity = new Vector2(0, 0);
            Hammer.SetHammer(true);
            break;
        case PlayerState.Moving:
            Hammer.SetHammer(true);
            break;
        case PlayerState.Climbing:
            Hammer.SetHammer(false);
            LogSystem.LogOnConsole("i'm klimping");// ----- ----- LOG ----- -----
            rb.gravityScale = 0;
            this.transform.position = new Vector3(Ladder.x, this.transform.position.y, this.transform.position.z);
            break;
        case PlayerState.Jumping:
            Hammer.SetHammer(false);
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + JumpStrength);
            break;
        case PlayerState.Falling:
            Hammer.SetHammer(false);
            GetComponent<CapsuleCollider2D>().isTrigger = true;
            rb.velocity = new Vector2(rb.velocity.x, -FallThroughBoost);
            break;
        /*case PlayerState.Landing:
            LogSystem.LogOnConsole("i'm landing");// ----- ----- LOG ----- -----
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0, 0);
            break;*/
        default:
            StateMachine_EnterState01();
            break;
        }


    }

    //----- ----- für vererbung ----- -----
    void StateMachine_Transition01() {
        ChangeState(PlayerState.Idle);
    }

    void StateMachine_StayInState01() {

    }

    void StateMachine_LeaveState01() {

    }

    void StateMachine_EnterState01() {

    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.transform.gameObject.tag == StringCollection.LADDER) {
            LogSystem.LogOnConsole("i'm on ladder");// ----- ----- LOG ----- -----
            Ladder = col.transform.position;
            isUpPossible = true;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.transform.gameObject.tag == StringCollection.LADDER) {
            if (InputControler.Vertical < 0) {
                LogSystem.LogOnConsole("no ladder anymore");// ----- ----- LOG ----- -----
                rb.velocity = new Vector2(rb.velocity.x, 0.1f);
            }
            rb.gravityScale = oldGravityScale;
            isUpPossible = false;
        }
    }

    void ChangeDir(bool right) { //kümmert sich ums umdrehen
        if (right != DirRight) {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        DirRight = right;
    }

    public void SetPlayerControl(bool controle) { //wird von GameManager aufgerufen
        InControle = controle;
    }

    public void KickPlayer(Vector2 force) { //wird von GameManager aufgerufen
        rb.velocity = rb.velocity + force;
    }
}
