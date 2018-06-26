using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeadEnemy : MonoBehaviour {

    [SerializeField]
    GameObject Enemy;

    private void Start() {
        GameManager.ChangeEnemyCount();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        switch (col.gameObject.transform.tag) {
        case StringCollection.ENEMY:
            Instantiate(Enemy);//resereckt
            goto case StringCollection.HAMMER;
        case StringCollection.HAMMER:
            GameManager.ChangeEnemyCount(-1);//kill kompletly
            GameObject.Destroy(this.transform.gameObject);
            break;
        default:
            break;
        }
    }
}
