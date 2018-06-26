using System;
using UnityEngine;


[Serializable()]
public class CameraPathAnchor { //ist wichtig für den camerapath
    public GameObject Position;
    public float Zoom;
    public float Stay;
    public float Duration;
    public float Sharpness = 2;
}
