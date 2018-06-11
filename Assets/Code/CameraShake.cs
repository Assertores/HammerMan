using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    float ShakeY = 0.8f;
    float ShakeYSpeed = 0.8f;
    Vector3 initialPosition;
    bool shakeIt = false;
    float stopShaking = 0;

    private void Start() {
        GameManager.RegistCamera(this);
    }
    private void OnDestroy() {
        GameManager.RegistCamera(this);
    }

    private void Update() {
        if (shakeIt) {
            Vector3 _newPosition = new Vector3(0, ShakeY, 0);
            if (ShakeY < 0) {
                ShakeY *= ShakeYSpeed;
            }
            ShakeY = -ShakeY;
            transform.Translate(_newPosition, Space.Self);
            if (GameManager.GetTime() >= stopShaking)
                StopShaking();
        }
    }

    public void StartShaking(float shakitime = 1.0f, float force = 0.8f) {
        ShakeY = force;
        initialPosition = transform.position;
        shakeIt = true;
        stopShaking = GameManager.GetTime() + shakitime;
    }
    void StopShaking() {
        shakeIt = false;
        transform.position = initialPosition;
        LogSystem.LogOnConsole("stop shaking. back in position.y: " + transform.position.y.ToString());
    }
}
