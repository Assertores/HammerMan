using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Trap : MonoBehaviour {

    [SerializeField]
    int TrapEnemyCount = 3;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.transform.tag == StringCollection.ENEMY) {
            TrapEnemyCount--;
            if(TrapEnemyCount <= 0) {
                GameObject.Destroy(this.transform.gameObject);
            }
        }
    }

}
