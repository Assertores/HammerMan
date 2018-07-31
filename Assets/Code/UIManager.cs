using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {//ist veralltet wird nicht mehr verwendet

    [SerializeField]
    GameObject WinScreen;
    [SerializeField]
    GameObject LossScreen;
    [SerializeField]
    GameObject GrayOut;

    private void OnDestroy() {
        GameManager.RegistUI(this);
    }
    
    void Start () {
        if (!WinScreen || !LossScreen || !GrayOut)
            new System.Exception("Win/Loss or Grayout Screen not found. UI");

        WinScreen.SetActive(false);
        LossScreen.SetActive(false);
        GrayOut.SetActive(false);
        GameManager.RegistUI(this);
	}

    public void GameEnds(bool Won) {
        if (Won)
            WinScreen.SetActive(true);
        else
            LossScreen.SetActive(true);
        GrayOut.SetActive(true);
    }
}
