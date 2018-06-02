using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControler : MonoBehaviour {

    public static float Horicontal { get; private set; }
    public static float Vertical { get; private set; }
    float LastVertical = 0.0f;
    public static bool Up { get; private set; }
    public static int DownCount { get; private set; }
    bool Down = false;

    private void Awake() {
        Horicontal = 0.0f;
        Vertical = 0.0f;
        Up = false;
        DownCount = 0;
    }

    void Update () {
        Horicontal = Input.GetAxis(StringCollection.HORIZONTAL);
        Vertical = Input.GetAxis(StringCollection.VERTICAL);
        if(Vertical > 0 && LastVertical < Vertical) {
            if (!Up)
                Up = true;
        } else {
            if (Up)
                Up = false;
        }
        if(Vertical < 0 && LastVertical > Vertical) {
            if (!Down) {
                Down = true;
                DownCount++;
            }
        } else {
            if (Down)
                Down = false;
        }
        LastVertical = Vertical;
	}
    public static void ChangeDown(int i) {
        DownCount += i;
        if (DownCount < 0)
            DownCount = 0;
    }

}
