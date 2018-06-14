using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIntroScript : MonoBehaviour {

    [SerializeField]
    CameraPathAnchor[] CameraPath;
    [SerializeField]
    UIElements[] ShowElements;
    [SerializeField]
    int PlayerAnimation = 0;
    [SerializeField]
    bool Intro = false;

    int index = 0;
    int index2 = 0;
    float time = 0;
    bool finished = false;
    bool yellAnimation = true;
    Camera cam;

    void Start() {
        if (!Intro) {
            index = CameraPath.Length;
        }
        if (CameraPath.Length > 0) {
            time = GameManager.GetTime();
            cam = this.gameObject.GetComponent<Camera>();
            cam.orthographicSize = CameraPath[0].Zoom;
            this.transform.position = new Vector3(CameraPath[0].Position.transform.position.x, CameraPath[0].Position.transform.position.y, -10);
        } else {
            GameManager.EndOfIntro();
        }
        for(int i = 0; i < ShowElements.Length; i++) {
            ShowElements[i].Element.SetActive(false);
        }
    }
    
    void Update() {
        if (CameraPath.Length > 0) {
            if (index < CameraPath.Length - 1) {
                if (GameManager.GetTime() - time > CameraPath[index].Stay) {
                    cam.orthographicSize = Mathf.Lerp(CameraPath[index].Zoom, CameraPath[index + 1].Zoom, (GameManager.GetTime() - time - CameraPath[index].Stay) / CameraPath[index].Duration);
                    Vector2 temp = Vector2.Lerp(CameraPath[index].Position.transform.position, CameraPath[index + 1].Position.transform.position, 1 / (1 + Mathf.Pow(CameraPath[index].Sharpness, -(((GameManager.GetTime() - time - CameraPath[index].Stay) / CameraPath[index].Duration) - 0.5f) * 20)));
                    transform.position = new Vector3(temp.x, temp.y, -10);
                }
                if (GameManager.GetTime() - time > CameraPath[index].Stay + CameraPath[index].Duration) {
                    index++;
                    time = GameManager.GetTime();
                }
            } else if (!finished) {
                cam.orthographicSize = CameraPath[CameraPath.Length - 1].Zoom;
                this.transform.position = new Vector3(CameraPath[CameraPath.Length - 1].Position.transform.position.x, CameraPath[CameraPath.Length - 1].Position.transform.position.y, -10);
                GameManager.EndOfIntro();
                finished = true;
            }
            if (yellAnimation && index == PlayerAnimation) {
                GameManager.PlayerAnimation();
                yellAnimation = false;
            }
        }

        if(index2 < ShowElements.Length && ShowElements[index2].Start <= GameManager.GetTime()) {
            ShowElements[index2].Element.SetActive(true);
        }
        if (index2 < ShowElements.Length && ShowElements[index2].Stop <= GameManager.GetTime()) {
            ShowElements[index2].Element.SetActive(false);
            index2++;
        }
    }
}
