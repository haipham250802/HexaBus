using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeCamera : MonoBehaviour
{
    [SerializeField] Camera _cam;
    public void OnResize()
    {
        float sizeStart = _cam.fieldOfView;
        float size = _cam.fieldOfView * 9 / (16 * _cam.aspect);
        if (size > sizeStart)
            _cam.fieldOfView = size;
    }
}
