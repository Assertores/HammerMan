using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraIntroScript : MonoBehaviour {
    //----- ----- hätte auch von ausen startbar machen können, dann währe es agiler gewesen
    [SerializeField]
    CameraPathAnchor[] CameraPath;
    [SerializeField]
    UIElements[] ShowElements;
    [SerializeField]
    int PlayerAnimation = 0; //index des CameraPaths, wo die spieleranimation gestartet wird
    [SerializeField]
    bool Intro = false; //für debug. um direkt starten zu können

    int index = 0; //CameraPath
    float LerpDuration;
    float LerpStart = 0;
    int index2 = 0; //ShowElements
    bool finished = false;
    Camera cam;

    void Start() {
        if (!Intro) {
            index = CameraPath.Length; //springt direkt ans ende
        }
        if (CameraPath.Length > 0) { //setzt alles richtig auf
            cam = this.gameObject.GetComponent<Camera>();
            cam.orthographicSize = CameraPath[0].Zoom;
            this.transform.position = new Vector3(CameraPath[0].Position.transform.position.x, CameraPath[0].Position.transform.position.y, -10);
        } else { //falls der path lehr ist
            GameManager.EndOfIntro();
            finished = true;
        }
        for(int i = 1; i < ShowElements.Length; i++) {//blendet alle elemente aus
            ShowElements[i].Element.SetActive(false);
        }
        ShowElements[0].Element.SetActive(true);
        GameManager.GM.BPMUpdate += BPMUpdate;
    }

    private void OnDestroy() {
        GameManager.GM.BPMUpdate -= BPMUpdate;
    }

    void BPMUpdate(int count) {
        //kümmert sich um den camera swipe
        if (!finished) {
            if (index >= CameraPath.Length-1) {
                cam.orthographicSize = CameraPath[CameraPath.Length - 1].Zoom;
                this.transform.position = new Vector3(CameraPath[CameraPath.Length - 1].Position.transform.position.x, CameraPath[CameraPath.Length - 1].Position.transform.position.y, -10);
                GameManager.EndOfIntro();
                finished = true;
            } else if (CameraPath[index].End == count) {
                LerpDuration = (CameraPath[index + 1].Start - count) * GameManager.GetBeatSeconds();
                LerpStart = GameManager.GetTime();
                index++;
            } else if (index == PlayerAnimation && CameraPath[index].Start == count) {
                GameManager.PlayerAnimation();
            }
        }
        //kümmert sich um die schrift
        if (index2 < ShowElements.Length && ShowElements[index2].Start == count) {
            ShowElements[index2].Element.SetActive(true);
        }
        if (index2 < ShowElements.Length && ShowElements[index2].Stop == count) {
            ShowElements[index2].Element.SetActive(false);
            index2++;
        }
    }
    
    void Update() {
        if(LerpStart != 0) {
            if (GameManager.GetTime() - LerpStart >=  LerpDuration)
                LerpStart = 0;
            cam.orthographicSize = Mathf.Lerp(CameraPath[index - 1].Zoom, CameraPath[index].Zoom, (GameManager.GetTime()-LerpStart) / LerpDuration); //kümmert sich um zoom.
            Vector2 temp = Vector2.Lerp(CameraPath[index - 1].Position.transform.position, CameraPath[index].Position.transform.position, 1 / (1 + Mathf.Pow(2, -(((GameManager.GetTime() - LerpStart) / LerpDuration) - 0.5f) * 20))); // mach den swipe [1/(1+2^-((deltaTime-0.5)*20))]
            transform.position = new Vector3(temp.x, temp.y, -10);
        }
    }
}
