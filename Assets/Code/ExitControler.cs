﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class ExitControler : MonoBehaviour {

    /*[SerializeField]
    int Life = 3;
    int maxLife;
    [SerializeField]
    Image Door;

    private void Start() {
        maxLife = Life;
    }

    public void Hit(int hit) { //wird von gegnern aufgerufen, wenn sie die tür berühren
        Life -= hit;
        if (Life > maxLife)
            Life = maxLife;
        if(Life <= 0) {
            GameManager.EndGame();
        }
        Door.fillAmount = (Life-1)/((float)maxLife-1);
    }*/

    void OnCollisionEnter2D(Collision2D col) {
        if (col.transform.gameObject.tag == StringCollection.ENEMY && col.gameObject.GetComponent<EnemyBehavior>().HealingFlag == false)
            GameManager.EndGame();
    }
}
