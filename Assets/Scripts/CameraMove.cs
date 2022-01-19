using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    public Camera cam;

    private Vector3 earlyPos;
    private Vector3 earlyEngle;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        earlyPos = transform.position;
        earlyEngle = transform.rotation.eulerAngles;
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

    public void ZoomOutCamera(Vector3 position, Vector3 rotation, float second)
    {
        transform.DOMove(position, second);
        transform.DORotate(rotation, second).OnComplete(() => 
        { 
            cam.enabled = false;
            transform.position = earlyPos;
            transform.rotation = Quaternion.Euler(earlyEngle);
            GameManager.Instance.tpsCamera.enabled = true; 
        }
        );
    }
}
