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
    int index2 = 0; //ShowElements
    float time = 0;
    bool finished = false;
    bool yellAnimation = true; //es muss noch geyelled werden
    Camera cam;

    void Start() {
        if (!Intro) {
            index = CameraPath.Length; //springt direkt ans ende
        }
        if (CameraPath.Length > 0) { //setzt alles richtig auf
            time = GameManager.GetTime();
            cam = this.gameObject.GetComponent<Camera>();
            cam.orthographicSize = CameraPath[0].Zoom;
            this.transform.position = new Vector3(CameraPath[0].Position.transform.position.x, CameraPath[0].Position.transform.position.y, -10);
        } else { //falls der path lehr ist
            GameManager.EndOfIntro();
        }
        for(int i = 0; i < ShowElements.Length; i++) {//blendet alle elemente aus
            ShowElements[i].Element.SetActive(false);
        }
    }
    
    void Update() {
        if (CameraPath.Length > 0) {
            if (index < CameraPath.Length - 1) {
                if (GameManager.GetTime() - time > CameraPath[index].Stay) { //wird nach stay zeit ausgeführt, macht den swipe
                    cam.orthographicSize = Mathf.Lerp(CameraPath[index].Zoom, CameraPath[index + 1].Zoom, (GameManager.GetTime() - time - CameraPath[index].Stay) / CameraPath[index].Duration); //kümmert sich um zoom.
                    Vector2 temp = Vector2.Lerp(CameraPath[index].Position.transform.position, CameraPath[index + 1].Position.transform.position, 1 / (1 + Mathf.Pow(CameraPath[index].Sharpness, -(((GameManager.GetTime() - time - CameraPath[index].Stay) / CameraPath[index].Duration) - 0.5f) * 20))); // mach den swipe [1/1+2^-((deltaTime-0.5)*20)]
                    transform.position = new Vector3(temp.x, temp.y, -10);
                }
                if (GameManager.GetTime() - time > CameraPath[index].Stay + CameraPath[index].Duration) {//wen swipe zuende
                    index++;
                    time = GameManager.GetTime();
                }
            } else if (!finished) {//wird am ende einmal ausgeführt
                cam.orthographicSize = CameraPath[CameraPath.Length - 1].Zoom;
                this.transform.position = new Vector3(CameraPath[CameraPath.Length - 1].Position.transform.position.x, CameraPath[CameraPath.Length - 1].Position.transform.position.y, -10);
                GameManager.EndOfIntro();
                finished = true;
            }
            if (yellAnimation && index == PlayerAnimation) {//macht das der spieler sich bewegt
                GameManager.PlayerAnimation();
                yellAnimation = false;
            }
        }
        //kümmert sich um die schrift
        if(index2 < ShowElements.Length && ShowElements[index2].Start <= GameManager.GetTime()) {
            ShowElements[index2].Element.SetActive(true);
        }
        if (index2 < ShowElements.Length && ShowElements[index2].Stop <= GameManager.GetTime()) {
            ShowElements[index2].Element.SetActive(false);
            index2++;
        }
    }
}
