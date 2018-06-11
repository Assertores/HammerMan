using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    float ShakeY = 0.8f;
    float ShakeYSpeed = 0.8f;
    Vector3 initialPosition;
    bool shakeIt = false;
    float stopShaking = 0;
    bool dirY = true;

    private void Start() {
        initialPosition = transform.position;
        GameManager.RegistCamera(this);
    }
    private void OnDestroy() {
        GameManager.RegistCamera(this);
    }

    private void Update() {
        if (shakeIt) {
            Vector3 _newPosition;
            if (dirY) {
                _newPosition = new Vector3(0, ShakeY, 0);
            } else {
                _newPosition = new Vector3(ShakeY, 0, 0);
            }
            
            if (ShakeY < 0) {
                ShakeY *= ShakeYSpeed;
            }
            ShakeY = -ShakeY;
            transform.Translate(_newPosition, Space.Self);
            if (GameManager.GetTime() >= stopShaking)
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
