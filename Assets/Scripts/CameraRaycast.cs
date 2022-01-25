using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    MonsterMove targetMonster;
    Camera cam;
    float maxDistance;
    private void Start()
    {
        cam = GetComponent<Camera>();
        maxDistance = GameManager.Instance.player.GetMaxDistance();
    }

    private void Update()
    {
        Hit_Unit(cam);
    }

    private void Hit_Unit(Camera cam)
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * maxDistance * 2, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(cam.transform.position, cam.transform.forward, maxDistance * 2);

        foreach (var hit in hits)
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                targetMonster = hit.transform.GetComponent<MonsterMove>();

                if (targetMonster != null)
                {
                    GameManager.Instance.selectedMonster?.ShowOutLine(false);
                    targetMonster.GetInfo();
                    targetMonster.ShowOutLine(true);
                    GameManager.Instance.selectedMonster = targetMonster;
                    return;
                }
            }
        }
    }
}
