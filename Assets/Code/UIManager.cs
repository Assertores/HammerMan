using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {//ist veralltet wird nicht mehr verwendet

    [SerializeField]
    Image Life;
    [SerializeField]
    Text Enemy;

    private void OnDestroy() {
        GameManager.RegistUI(this);
    }
    
    void Start () {
        if (!Life) {
            throw new System.Exception("Life Image not assinght. UI");
        }
        if (!Enemy) {
            throw new System.Exception("Enemy text not assinght. UI");
        }
        GameManager.RegistUI(this);
        Life.fillAmount = 1.0f;
	}
	
	public void UpdateLife (float LifeRatio) {
        Life.fillAmount = LifeRatio;
	}

    public void UpdateEnemyCount(int EnemyCount) {
        Enemy.text = EnemyCount.ToString();
    }

}
