using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitControler : MonoBehaviour {

    [SerializeField]
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
    }
}
