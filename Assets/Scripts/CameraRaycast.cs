using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    MonsterMove targetMonster;
    Camera cam;
    float maxDistance = 25f;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        Hit_Unit(cam);
    }

    private void Hit_Unit(Camera cam)
    {
        //���̾� ����ũ ���ֱ�
        Debug.DrawRay(cam.transform.position, cam.transform.forward * (maxDistance + 12f), Color.green);
        RaycastHit[] hits = Physics.RaycastAll(cam.transform.position, cam.transform.forward, maxDistance + 12f);
        MonsterMove curMonster;
        foreach (var hit in hits)
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                curMonster = hit.transform.GetComponent<MonsterMove>();

                if (curMonster != null)
                {
                    targetMonster?.ShowOutLine(false);
                    GameManager.Instance.selectedMonster?.ShowOutLine(false);

                    targetMonster = curMonster;
                    targetMonster.ShowOutLine(true);
                    targetMonster.GetInfo();
                    GameManager.Instance.selectedMonster = targetMonster;
                    return;
                }
            }
        }
    }
}
