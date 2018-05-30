using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFronScene : MonoBehaviour {

    [SerializeField]
    public CameraPathAnchor[] CameraPath;
    [SerializeField]
    int PlayerAnimation = 0;

    int index = 0;
    float time = 0;
    bool finished = false;
    bool yellAnimation = true;
    Camera cam;

    void Start() {
        time = GameControler.GetTime();
        cam = this.gameObject.GetComponent<Camera>();
        cam.orthographicSize = CameraPath[0].Zoom;
        this.transform.position = new Vector3(CameraPath[0].Position.transform.position.x, CameraPath[0].Position.transform.position.y, -10);
    }
	
	// Update is called once per frame
	void Update () {
        if (index < CameraPath.Length-1) {
            if (GameControler.GetTime() - time > CameraPath[index].Stay) {
                //print("index: " + index + " Time: " + (GameControler.GetTime() - time - CameraPath[index].Stay));
                cam.orthographicSize = Mathf.Lerp(CameraPath[index].Zoom, CameraPath[index + 1].Zoom, (GameControler.GetTime() - time - CameraPath[index].Stay) / CameraPath[index].Duration);
                Vector2 temp = Vector2.Lerp(CameraPath[index].Position.transform.position, CameraPath[index+1].Position.transform.position, 1/(1+Mathf.Pow(CameraPath[index].Sharpness, -(((GameControler.GetTime() - time - CameraPath[index].Stay) / CameraPath[index].Duration) -0.5f)*20)));
                transform.position = new Vector3(temp.x, temp.y, -10);
            }
            if (GameControler.GetTime() - time > CameraPath[index].Stay + CameraPath[index].Duration){
                index++;
                time = GameControler.GetTime();
            }
        } else if (!finished) {
            cam.orthographicSize = CameraPath[CameraPath.Length - 1].Zoom;
            this.transform.position = new Vector3(CameraPath[CameraPath.Length - 1].Position.transform.position.x, CameraPath[CameraPath.Length - 1].Position.transform.position.y, -10);
            GameControler.EndOfIntro();
            finished = true;
        }
        if(yellAnimation && index == PlayerAnimation) {
            GameControler.PlayerAnimation();
            yellAnimation = false;
        }
	}
}
