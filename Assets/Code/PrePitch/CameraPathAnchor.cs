using System;
using UnityEngine;

[Serializable()]
public class CameraPathAnchor
{
    public GameObject Position;
    public float Zoom;
    public float Stay;
    public float Duration;
    public float Sharpness = 2;
}
