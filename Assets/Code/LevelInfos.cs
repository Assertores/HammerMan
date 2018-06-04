﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfos : MonoBehaviour {

    [SerializeField]
    int LevelLife;

    void Start () {
        GameManager.RegistLvlInfos(this);
	}
    private void OnDestroy() {
        GameManager.RegistLvlInfos(this);
    }

    public int GetLife() {
        return LevelLife;
    }
}
