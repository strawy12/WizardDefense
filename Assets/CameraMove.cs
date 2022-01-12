using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    public Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    public void CameraMoveToPosition(Vector3 position, float second)
    {
        transform.DOMove(position, second);
    }

    public void ChangeLocalEulerAngle(Vector3 angle)
    {
        transform.localEulerAngles = angle;
    }

    public void CameraRotate(Vector3 rotation, float second)
    {
        transform.DORotate(rotation, second);
    }
}