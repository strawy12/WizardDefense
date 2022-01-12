using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera theCam;
    [Header("레이저 길이")] [SerializeField] private float maxDistance = 10f;

    [Header("포탑설치가능표시")] [SerializeField] private GameObject FMark;
    [Header("포탑설치창")] [SerializeField] private GameObject buildChang;

    private RaycastHit hitInfo;

    private bool isArea;
    private bool[] towers;

    private void Update()
    {
        Hit();
    }

    private void Hit()
    {
        Debug.DrawRay(theCam.transform.position, theCam.transform.forward * maxDistance, Color.blue);
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward, out hitInfo, maxDistance))
        {
            if (hitInfo.transform.gameObject.name == "area")
            {
                FMark.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Chang();
                    Debug.Log("나와");
                }
            }
            else
            {
                isArea = false;
                FMark.SetActive(false);
                return;
            }
        }
    }

    private void Chang()
    {
        isArea = !isArea;
        if(isArea)
        {
            FMark.SetActive(false);
            buildChang.SetActive(true);
        }
        else
        {
            FMark.SetActive(false);
            buildChang.SetActive(false);
        }
    }

    public void OnClickTo1()
    {
        towers[0] = true;

        towers[1] = false;
        towers[2] = false;
    }

    public void OnClickTo2()
    {
        towers[1] = true;

        towers[0] = false;
        towers[2] = false;
    }

    public void OnClickTo3()
    {
        towers[2] = true;

        towers[0] = false;
        towers[1] = false;
    }

}
