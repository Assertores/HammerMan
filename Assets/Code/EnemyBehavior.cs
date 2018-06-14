using System.Collections;
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
    [SerializeField]
    [Tooltip("0 = boath direktion, 1 only front, 2 = only back")]
    int AbleToHitFrom = 0;
    [Header("Generel")]
    [SerializeField]
    float EnemySpeed = 10;
    [SerializeField]
    int EnemyDamageOnExit = 1;
    [SerializeField]
    float TurningDistance = 10;
    [SerializeField]
    float Invulnerable = 1;
    public LayerMask ChangeDirectionAt;
    public LayerMask FallLayers;

    Rigidbody2D rb;

    public EnemyState State = EnemyState.Moving;
    public bool DirRight = true;
    float DistToGround = 0.0f;

    void Start() {
        GameManager.ChangeEnemyCount(1);
        Invulnerable += GameManager.GetTime();
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
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(this.transform.position.x + (DirRight ? 0.6f : -0.6f), this.transform.position.y, this.transform.position.z),
                                                this.transform.right * rb.velocity.x, TurningDistance - 0.6f, ChangeDirectionAt);

        if (hit.collider != null)
        {
            ChangeDir(!DirRight);
        }
        switch (State) {
        case EnemyState.Moving:
                rb.velocity = new Vector2(DirRight ? EnemySpeed : -EnemySpeed, rb.velocity.y);
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
            if(Invulnerable < GameManager.GetTime()) {
                float dist = GameManager.GetPlayerPosition().x - this.transform.position.x;
                if (!DirRight)
                    dist *= -1;
                if (AbleToHitFrom == 0 || (AbleToHitFrom == 1 && dist > 0) || (AbleToHitFrom == 2 && dist < 0)) {
                    DieByHammer();
                }
            }
            break;
        case StringCollection.EXIT:
            col.GetComponent<ExitControler>().Hit(EnemyDamageOnExit);
            DieByExit();
            break;
        default:
            break;
        }
    }

    void DieByHammer() {
        int temp = Random.Range(0, DeathSound.Length);
        GameManager.CameraEffectOnEnemyDeath();
        GameObject Die = Instantiate(EnemyDieParticle, this.transform.position, this.transform.rotation);
        if(DeathSound.Length != 0)
            Die.GetComponent<AudioSource>().clip = DeathSound[temp];
        if(Die.GetComponent<ParticleKiller>() == null) {
            print("ParticalKillerScript kann nicht gefunden werden.");
        } else {
            Die.GetComponent<ParticleKiller>().PlayStart();
        }
        if (InvoceOnEnemyDeath.Length != 0) {
            for(int i = 0; i < InvoceOnEnemyDeath.Length; i++) {
                Instantiate(InvoceOnEnemyDeath[i]).transform.position = this.transform.position;
            }
        }
        GameManager.ChangeEnemyCount(-1);
        GameObject.Destroy(this.transform.gameObject);
    }

    void DieByExit() {
        GameManager.CameraEffectOnEnemyExit();
        //GameManager.ChangeEnemyCount(-1, -EnemyDamageOnExit);
        GameObject.Destroy(this.transform.gameObject);
    }

    void ChangeDir(bool right) {
        if (right != DirRight) {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        DirRight = right;
    }
}
