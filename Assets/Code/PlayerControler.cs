using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour {

    [SerializeField]
    float PlayerSpeed = 1.0f;

    float goHorizontal = 0.0f;
    bool goUp = false;
	// Update is called once per frame
	void Update () {
        Movement();
	}
    private void FixedUpdate() {
        InputManager();
    }

    private void InputManager() {
        goHorizontal = Input.GetAxis("Horizontal");
    }

    private void Movement() {
        
    }
}
