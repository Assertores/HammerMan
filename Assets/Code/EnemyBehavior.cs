﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState {
    Moving,
    Falling
}

public class EnemyBehavior : MonoBehaviour {

    [Header("Death")]
    [SerializeField]
    GameObject EnemyDieParticle;
    [SerializeField]
    AudioClip[] DeathSound;
    [SerializeField]
    GameObject[] InvoceOnEnemyDeath;
    [Header("Generel")]
    [SerializeField]
    float EnemySpeed = 10;
    [SerializeField]
    int EnemyDamageOnExit = 1;
    [SerializeField]
    float TurningDistance = 10;
    public LayerMask layer;
    public LayerMask FallLayers;

    Rigidbody2D rb;

    EnemyState State = EnemyState.Moving;
    bool DirRight = true;
    float DistToGround = 0.0f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        if (!rb) {
            throw new System.Exception("Rigitbody not found. Enemy");
        }

        DirRight = (Random.value > 0.5f);
        if (!DirRight) {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    void Update() {

        //DistToGround = Physics2D.Raycast(this.transform.position, -this.transform.up, 1000, FallLayers).distance;
        //LogSystem.LogOnConsole(State.ToString());// ----- ----- LOG ----- -----
        switch (State) {
        case EnemyState.Moving:
            /*if (DistToGround > 0.5f)//triggert zu früh
                ChangeState(EnemyState.Falling);*/
            if (rb.velocity.y < -1.5f)
                ChangeState(EnemyState.Falling);
            break;
        case EnemyState.Falling:
            if (rb.velocity.y >= -0.5f)
                ChangeState(EnemyState.Moving);
            break;
        default:
            StateMachine_Transition01();
            break;
        }
    }

    private void FixedUpdate() {
        switch (State) {
        case EnemyState.Moving:
            RaycastHit2D hit = Physics2D.Raycast(new Vector3(this.transform.position.x + (DirRight ? 0.6f: -0.6f), this.transform.position.y, this.transform.position.z),
                                                this.transform.right * rb.velocity.x, TurningDistance - 0.6f, layer);

            if (hit.collider != null) {
                ChangeDir(!DirRight);
            }
            if (rb.velocity.x <= 0.1f && rb.velocity.x >= -0.1f) {
                rb.velocity = new Vector2(DirRight ? -EnemySpeed : EnemySpeed, rb.velocity.y);
            } else {
                rb.velocity = new Vector2(DirRight ? EnemySpeed : -EnemySpeed, rb.velocity.y);
            }
            break;
        case EnemyState.Falling:
            break;
        default:
            StateMachine_StayInState01();
            break;
        }
    }

    void ChangeState(EnemyState newState) {
        if (State == newState)
            return;

        switch (State) {
        case EnemyState.Moving:
            break;
        case EnemyState.Falling:
            break;
        default:
            StateMachine_LeaveState01();
            break;
        }

        State = newState;

        switch (State) {
        case EnemyState.Moving:
            break;
        case EnemyState.Falling:
            ChangeDir(Random.value > 0.5f);
            break;
        default:
            StateMachine_EnterState01();
            break;
        }
    }

    void StateMachine_Transition01() {

    }

    void StateMachine_StayInState01() {

    }

    void StateMachine_LeaveState01() {

    }

    void StateMachine_EnterState01() {

    }

    private void OnTriggerEnter2D(Collider2D col) {
        switch (col.transform.gameObject.tag) {
        case StringCollection.HAMMER:
            DieByHammer();
            break;
        case StringCollection.EXIT:
            DieByExit();
            break;
        default:
            break;
        }
    }

    void DieByHammer() {
        int temp = Random.Range(0, DeathSound.Length);
        GameObject Die = Instantiate(EnemyDieParticle, this.transform.position, this.transform.rotation);
        Die.GetComponent<AudioSource>().clip = DeathSound[temp];
        Die.GetComponent<ParticleKiller>().PlayStart();
        if (InvoceOnEnemyDeath.Length != 0) {
            for(int i = 0; i < InvoceOnEnemyDeath.Length; i++) {
                Instantiate(InvoceOnEnemyDeath[i]).transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            }
        }
        GameManager.ChangeEnemyCount(-1);
        GameObject.Destroy(this.transform.gameObject);
    }

    void DieByExit() {
        GameManager.ChangeEnemyCount(-1, -EnemyDamageOnExit);
        GameObject.Destroy(this.transform.gameObject);
    }

    void ChangeDir(bool right) {
        if (right != DirRight) {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        DirRight = right;
    }
}
