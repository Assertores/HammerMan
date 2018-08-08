using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState {
    Moving,
    Falling,
    Dead
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class EnemyBehavior : MonoBehaviour {

    public bool HealingFlag { get; private set; }

    [Header("Death")]
    [SerializeField]
    GameObject EnemyDieParticle;
    [SerializeField]
    AudioClip[] DeathSound;
    [SerializeField]
    GameObject[] InvoceOnEnemyDeath;
    [Header("Generel")]
    [SerializeField]
    string Name;
    [SerializeField]
    [Tooltip("0 = boath direktion, 1 only front, 2 = only back")]
    int AbleToHitFrom = 0;
    [SerializeField]
    float EnemySpeed = 10;
    [SerializeField]
    int EnemyDamageOnExit = 1;
    [SerializeField]
    float TurningDistance = 10;
    [SerializeField]
    float FleeDistance = 0;
    [SerializeField]
    float Invulnerable = 1; //time
    [SerializeField]
    [Tooltip("the travel distance per animation loop in unity units")]
    float DistancePerLoop = 1;
    public LayerMask ChangeDirectionAt;
    public LayerMask PlayerLayer;
    public LayerMask EnemyLayer;

    Rigidbody2D rb;

    public EnemyState State = EnemyState.Moving;
    public bool DirRight { get; private set; }

    void Start() {
        LogSystem.LogOnFile(Name + " Enemy got spawned");// ----- ----- LOG ----- -----
        GameManager.ChangeEnemyCount(1);
        Invulnerable += GameManager.GetTime();
        rb = GetComponent<Rigidbody2D>();
        if (!rb) {
            throw new System.Exception("Rigitbody not found. Enemy");
        }

        if (EnemyDamageOnExit < 0)
            HealingFlag = true;
        else
            HealingFlag = false;

        DirRight = true;
        //randam direction
        /*DirRight = (Random.value > 0.5f);
        if (!DirRight) {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }*/

        Animator anim = GetComponentInChildren<Animator>();
        if (!anim) {
            throw new System.Exception("No Animation. Enemy");
        }
        anim.speed = anim.GetCurrentAnimatorStateInfo(0).length / (DistancePerLoop / EnemySpeed);
    }
    private void OnDestroy() {
        LogSystem.LogOnFile(Name + " Enemy died");// ----- ----- LOG ----- -----
        GameManager.ChangeEnemyCount(-1);
    }

    void Update() { //finite state machine: übergänge von states
        switch (State) {
        case EnemyState.Moving:
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
        if (Physics2D.Raycast(new Vector3(this.transform.position.x + (DirRight ? 0.6f : -0.6f), this.transform.position.y, this.transform.position.z),
                                               this.transform.right * rb.velocity.x, FleeDistance - 0.6f, PlayerLayer).collider != null)
            ChangeDir(!DirRight);
        Collider2D[] hitDown = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y - 1),new Vector2(1f, 0.5f),0,EnemyLayer);

        
        
        int hitDownFlag = -1;
        for(int i = 0; hitDownFlag == -1 && i < hitDown.Length; i++) {
            if(hitDown[i] != GetComponent<Collider2D>()) {
                hitDownFlag = i;
            }
        }

        switch (State) { //finite state machine: während diesem state
        case EnemyState.Moving:

            if(hitDownFlag != -1) {
                ChangeDir(!hitDown[hitDownFlag].GetComponent<EnemyBehavior>().DirRight);
            }else if (hit.collider != null) {
                ChangeDir(!DirRight);
            }
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
        if (State == newState) //stellt sicher, dass es einen übergang giebt
            return;

        switch (State) { //finite state machine: bei verlassen des states
        case EnemyState.Moving:
            break;
        case EnemyState.Falling:
            break;
        default:
            StateMachine_LeaveState01();
            break;
        }

        State = newState;

        switch (State) { //finite state machine: bei betreten des states
        case EnemyState.Moving:
            break;
        case EnemyState.Falling:
            ChangeDir(Random.value > 0.5f);
            break;
        case EnemyState.Dead:
            rb.bodyType = RigidbodyType2D.Static;
            GetComponent<BoxCollider2D>().enabled = false;

            //animator auf death animation wechseln
            Animator anim = GetComponentInChildren<Animator>();
            if (anim) {
                anim.SetBool("Dead", true);
            }

            int temp = Random.Range(0, DeathSound.Length); //welcher sound soll für den tod verwendet werden
            if (DeathSound.Length != 0) {//fügt audioclip hinzu
                AudioSource audio = GetComponent<AudioSource>();
                audio.clip = DeathSound[temp];
                Invoke("Play", GameManager.GetBeatSeconds() - (GameManager.GetTime() - GameManager.GetTimeOfLastBeat()));
            }
            GameObject.Destroy(this.transform.gameObject, 1.0f);
            break;
        default:
            StateMachine_EnterState01();
            break;
        }
    }

    void Play() {
        //print("Enemy: " + gameObject.name + " " + (GameManager.GetTime() - GameManager.GetTimeOfLastBeat()));
        GetComponent<AudioSource>().Play();
    }

    //----- ----- für vererbung ----- -----
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
        case StringCollection.HAMMER://wenn mit hammer kolidiert
            if(Invulnerable < GameManager.GetTime()) {//wenn nichtmehr unverwundbar
                float dist = GameManager.GetPlayerPosition().x - this.transform.position.x; //für nur von einer seite aus tötbar
                if (!DirRight)
                    dist *= -1;
                if (AbleToHitFrom == 0 || (AbleToHitFrom == 1 && dist > 0) || (AbleToHitFrom == 2 && dist < 0)) {
                    DieByHammer();
                }
            }
            break;
        case StringCollection.EXIT://wen nach drausen leuft
            //col.GetComponent<ExitControler>().Hit(EnemyDamageOnExit); //macht ausgang kaputt
            DieByExit();
            break;
        case StringCollection.TRAP:
            if (Invulnerable < GameManager.GetTime()) {//wenn nichtmehr unverwundbar
                DieByTrap();
            }
            break;
        default:
            break;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(col.transform.gameObject.tag == StringCollection.EXIT) {
            DieByExit();
        }
    }

    void DieByTrap() {
        ChangeState(EnemyState.Dead);
        /*
        int temp = Random.Range(0, DeathSound.Length); //welcher sound soll für den tod verwendet werden
        GameObject Die = Instantiate(EnemyDieParticle, this.transform.position, this.transform.rotation); //macht partikel
        if (DeathSound.Length != 0)//fügt audioclip hinzu
            Die.GetComponent<AudioSource>().clip = DeathSound[temp];
        if (Die.GetComponent<ParticleKiller>() == null) {
            print("ParticalKillerScript kann nicht gefunden werden.");
        } else {
            Die.GetComponent<ParticleKiller>().PlayStart(); //started partikel und audio
        }*/
        if (!HealingFlag && InvoceOnEnemyDeath.Length != 0) {//erzeugt gameobjekts bei tot
            for (int i = 0; i < InvoceOnEnemyDeath.Length; i++) {
                Instantiate(InvoceOnEnemyDeath[i]).transform.position = this.transform.position;
            }
        }
    }

    void DieByHammer() {
        ChangeState(EnemyState.Dead);
        GameManager.CameraEffectOnEnemyDeath();
        /*
        
        
        GameObject Die = Instantiate(EnemyDieParticle, this.transform.position, this.transform.rotation); //macht partikel
        
        if(Die.GetComponent<ParticleKiller>() == null) {
            print("ParticalKillerScript kann nicht gefunden werden.");
        } else {
            Die.GetComponent<ParticleKiller>().PlayStart(); //started partikel und audio
        }*/
        if (!HealingFlag && InvoceOnEnemyDeath.Length != 0) {//erzeugt gameobjekts bei tot durch hammer
            for(int i = 0; i < InvoceOnEnemyDeath.Length; i++) {
                Instantiate(InvoceOnEnemyDeath[i]).transform.position = this.transform.position;
            }
        }
    }

    void DieByExit() {
        ChangeState(EnemyState.Dead);
        if (HealingFlag) {
            if (InvoceOnEnemyDeath.Length != 0) {//erzeugt gameobjekts bei tot durch hammer
                for (int i = 0; i < InvoceOnEnemyDeath.Length; i++) {
                    Instantiate(InvoceOnEnemyDeath[i]).transform.position = this.transform.position;
                }
            }
        }
        GameManager.CameraEffectOnEnemyExit();
    }

    void ChangeDir(bool right) { //kümmert sich ums umdrehen
        if (right != DirRight) {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        DirRight = right;
    }
}
