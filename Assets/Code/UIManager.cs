using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {//ist veralltet wird nicht mehr verwendet

    [SerializeField]
    Text Timer;

    private void OnDestroy() {
        GameManager.RegistUI(this);
    }
    
    void Start () {
        if (!Timer) {
            throw new System.Exception("Enemy text not assinght. UI");
        }
        GameManager.RegistUI(this);
	}

    void Update () {
        float time = GameManager.GetTime();
        Timer.text = ((int)(time/60)).ToString("D2") + ":" + ((int)time%60).ToString("D2") + ":" + ((int)(time*100)%100).ToString("D2");
    }
}
