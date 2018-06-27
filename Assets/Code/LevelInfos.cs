using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfos : MonoBehaviour {

    [SerializeField]
    int LevelLife;
    [SerializeField]
    float HammerBPM;
    
    //meldet sich beim gamemanager an und ab
    void Start () {
        GameManager.RegistLvlInfos(this);
        HammerBPM = 1/(HammerBPM / 60); //macht aus Beats per minute die zeit zwischen zwei hammerschlägen in secunden
	}
    private void OnDestroy() {
        GameManager.RegistLvlInfos(this);
    }

    public int GetLife() {
        return LevelLife;
    }

    public float GetHammerFrequenz() {
        return HammerBPM;
    }
}
