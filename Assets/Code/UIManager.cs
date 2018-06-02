﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField]
    Image Life;
    [SerializeField]
    Text Enemy;

    void Awake() {
        GameManager.RegistUI(this);
    }
    private void OnDestroy() {
        GameManager.RegistUI(this);
    }

    // Use this for initialization
    void Start () {
        if (!Life) {
            throw new System.Exception("Life Image not assinght. UI");
        }
        if (!Enemy) {
            throw new System.Exception("Enemy text not assinght. UI");
        }
        Life.fillAmount = 1.0f;
	}
	
	// Update is called once per frame
	public void UpdateLife (float LifeRatio) {
        Life.fillAmount = LifeRatio;
	}

    public void UpdateEnemyCount(int EnemyCount) {
        Enemy.text = EnemyCount.ToString();
    }

}
