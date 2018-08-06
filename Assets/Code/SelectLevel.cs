using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour {

    [SerializeField]
    Text text;
    [SerializeField]
    Button button;
	int Level = 1;

    public void ChangeLevel (int delta = 1) {
        Level += delta;
        if (Level < 1) {
            Level = 1;
        }
        if(Level > GameManager.GetNextLevel()) {
            Level = GameManager.GetNextLevel();
        }
        text.text = "Level " + Level;
    }
    
    public void StartLevel() {
        GameManager.GM.StartLevel(Level);
    }
}
