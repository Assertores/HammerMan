using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Boxen : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D col) {
        if(col.transform.gameObject.tag == StringCollection.ENEMY && col.gameObject.GetComponent<EnemyBehavior>().HealingFlag == false)
            Destroy(this.gameObject);
    }
}
