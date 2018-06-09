using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfos : MonoBehaviour {

    [SerializeField]
    int LevelLife;
    [SerializeField]
    float HammerFrequenz;

    void Start () {
        GameManager.RegistLvlInfos(this);
	}
    private void OnDestroy() {
        GameManager.RegistLvlInfos(this);
    }

    public int GetLife() {
        return LevelLife;
    }

    public float GetHammerFrequenz() {
        return HammerFrequenz;
    }
}
