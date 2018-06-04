using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Animator State;

    Rigidbody2D rb;
    bool isUpPossible = false;
    bool DirRight = true;
    Vector2 Ladder;
    bool InControle = false;
    bool PlayerIsInGround = false;

    private void OnDestroy() {
        GameManager.RegistPlayer(this);
    }

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        if (!rb) {
            throw new System.Exception("Rigitbody not found. Player");
        }
        print("i'm here. Player: " + this);// ----- ----- LOG ----- -----
        GameManager.RegistPlayer(this);
    }
	
	void Update () {
        if (this.transform.position.y < -0.1) {
            this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
            InputControler.SetDown(0);
        }
    }

    void FixedUpdate() {
        if (InControle) {
            if (isUpPossible && this.transform.position.y > Ladder.y + 1) {
                if (InputControler.Up) {
                    print("klimp");// ----- ----- LOG ----- -----
                    this.transform.position = new Vector2(Ladder.x, this.transform.position.y);
                    rb.velocity = new Vector2(0, ClimbSpeed);
                } else {
                    print("dont klimp annymore");// ----- ----- LOG ----- -----
                    rb.velocity = new Vector2(0, 0);
                }
            } else {
                rb.velocity = new Vector2(InputControler.Horizontal * PlayerSpeed, rb.velocity.y);

                if ((InputControler.Horizontal < 0 && DirRight) || (InputControler.Horizontal > 0 && !DirRight)) {
                    ChangeDir(!DirRight);
                }
            }
            if (Physics2D.IsTouchingLayers(GetComponent<CapsuleCollider2D>(), FallLayers) && !PlayerIsInGround) {
                PlayerIsInGround = true;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - FallThroughBoost);
            } else if (!Physics2D.IsTouchingLayers(GetComponent<CapsuleCollider2D>(), FallLayers) && PlayerIsInGround) {
                PlayerIsInGround = false;
                InputControler.ChangeDown(-1);
            }
            if (InputControler.DownCount <= 0) {
                GetComponent<CapsuleCollider2D>().isTrigger = false;
            } else {
                GetComponent<CapsuleCollider2D>().isTrigger = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.transform.gameObject.tag == StringCollection.LADDER) {
            print("i'm on ladder");// ----- ----- LOG ----- -----
            Ladder = col.transform.position;
            isUpPossible = true;
            rb.gravityScale = 0;
            //rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.transform.gameObject.tag == StringCollection.LADDER) {
            if (InputControler.Vertical < 0) {
                print("no ladder anymore");// ----- ----- LOG ----- -----
                rb.velocity = new Vector2(rb.velocity.x, 0.1f);
                this.transform.position = new Vector2(this.transform.position.x, Ladder.y + 4.0f);
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
