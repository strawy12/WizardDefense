using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraFollow : MonoBehaviour
{
    [Header("Ä«¸Þ¶ó")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera subCamera;

    private bool isChange = false;

    private void Start()
    {
        MainCameraOn();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyManager.keySettings[KeyAction.ChangeView]))
        {
            ChangeView();
        }

        if(Input.GetKeyUp(KeyManager.keySettings[KeyAction.ChangeView]))
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
