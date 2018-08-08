using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour {

	public void restart() {
        GameManager.GM.StartLevel(1);
    }

    public void exit() {
        GameManager.GM.StartMainMenu();
    }
}
