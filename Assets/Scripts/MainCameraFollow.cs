using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    [Header("Ä«¸Þ¶ó")]
    [SerializeField] private SubCamera mainCamera;
    [SerializeField] private SubCamera subCamera;

    private bool isChange = false;
    private void Update()
    {
        transform.position = target.position + offset;
        if(Input.GetKeyDown(KeyCode.T))
        {
            ChangeView();
        }
    }

    private void ChangeView()
    {
        isChange = !isChange;
        if(isChange)
        {
            SubCameraOn();
        }
        else
        {
            MainCameraOn();
        }
    }

    private void MainCameraOn()
    {
        mainCamera.enabled = true;
        subCamera.enabled = false;
    }

    private void SubCameraOn()
    {
        subCamera.enabled = true;
        mainCamera.enabled = false;
    }
}
