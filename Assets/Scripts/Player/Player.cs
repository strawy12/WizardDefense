using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera theCam;
    [Header("������ ����")] [SerializeField] private float maxDistance = 10f;

    [Header("��ž��ġ����ǥ��")] [SerializeField] private GameObject FMark;
    [Header("��ž��ġâ")] [SerializeField] private GameObject buildChang;

    private RaycastHit hitInfo;

    private bool isArea;

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

    public void OnClickOutChang()
    {
        isArea = !isArea;
        FMark.SetActive(false);
        buildChang.SetActive(false);
    }
}
