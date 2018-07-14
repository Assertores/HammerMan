using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [SerializeField]
    GameObject Position = null; //standardposition
    float ShakeY = 0.8f; //max magnitude am anfang
    float ShakeYSpeed = 0.8f; // wie schnell die magnitude abnehmen soll
    bool dirY = true; //richtung des camera shakes
    float stopShaking = 0; //zu welcher zeit er aufhöhren soll zu shaken

    //globale varialben
    Vector3 initialPosition;
    bool shakeIt = false;

    private void Start() {
        GameManager.RegistCamera(this);
        //setzt die position
        if (Position == null)
            initialPosition = transform.position;
        else
            initialPosition = new Vector3(Position.transform.position.x, Position.transform.position.y, -10);
    }
    private void OnDestroy() {
        GameManager.RegistCamera(this);
    }

    private void Update() {
        if (shakeIt) {
            Vector3 _newPosition;
            if (dirY) { //entscheidet die richtung
                _newPosition = new Vector3(0, ShakeY, 0);
            } else {
                _newPosition = new Vector3(ShakeY, 0, 0);
            }
            
            if (ShakeY < 0) { //rechnet wert für nächstes Update
                ShakeY *= ShakeYSpeed;
            }
            ShakeY = -ShakeY;
            transform.Translate(_newPosition, Space.Self);
            if (GameManager.GetTime() >= stopShaking) //wenn es am ende angekommen ist.
                StopShaking();
        }
    }

    public void StartShaking(bool inYDirekton = true, float amplitude = 0.8f, float falloff = 0.8f, float shakitime = 1.0f) {
        dirY = inYDirekton;
        ShakeY = amplitude;
        ShakeYSpeed = falloff;
        shakeIt = true;
        stopShaking = GameManager.GetTime() + shakitime;
    }
    void StopShaking() {
        shakeIt = false;
        transform.position = initialPosition;
        LogSystem.LogOnConsole("stop shaking. back in position.y: " + transform.position.y.ToString());
    }
}
