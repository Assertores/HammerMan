using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControler : MonoBehaviour {//liest input aus und macht ihn global verfügbar

    public static float Horizontal { get; private set; }
    public static float Vertical { get; private set; }
    float LastVertical = 0.0f;
    public static bool Up { get; private set; }
    public static int DownCount { get; private set; }
    bool DownCountOn = false;
    public bool Down { get; private set; }
    public static int ExitCount { get; private set; }
    bool ExitCountOn = false;
    public static bool Jump { get; private set; }

    private void Awake() {
        Horizontal = 0.0f;
        Vertical = 0.0f;
        Up = false;
        DownCount = 0;
        Down = false;
        ExitCount = 0;
        Jump = false;
    }

    void Update () {
        //code
        Horizontal = Input.GetAxis(StringCollection.HORIZONTAL);
        Vertical = Input.GetAxis(StringCollection.VERTICAL);
        if((Vertical > 0 && LastVertical < Vertical) || Vertical == 1) {
            if (!Up)
                Up = true;
        } else {
            if (Up)
                Up = false;
        }
        if((Vertical < 0 && LastVertical > Vertical) || Vertical == -1) {
            if (!Down) {
                Down = true;
                DownCount++;
            }
        } else {
            if (Down) {
                Down = false;
            }
        }
        LastVertical = Vertical;

        Jump = Input.GetButton(StringCollection.JUMP);

        if (Input.GetButtonDown(StringCollection.CROUCH)) {
            DownCount++;
        }

        if (Input.GetButtonDown(StringCollection.CANCEL)) {
            ExitCount++;
        }
    }

    public static void ChangeDown(int i) {
        DownCount += i;
        if (DownCount < 0)
            DownCount = 0;
    }

    public static void SetDown(int i) {
        DownCount = i;
        if (DownCount < 0)
            DownCount = 0;
    }
    
    public static void PopExit() {
        ExitCount--;
        if (ExitCount < 0)
            ExitCount = 0;
    }
}
