using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuild : MonoBehaviour
{
    private RaycastHit hitTowerAreaInfo;
    [SerializeField] private float maxDistance;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject towerObj;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Material[] towerMaterial;

    private bool isClickF = false;
    private bool isBuild;

    private void Update()
    {
        BuildTower();
        CanBuild(cam);
    }

    private void CanBuild(Camera _cam)
    {
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hitTowerAreaInfo, maxDistance))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isClickF)
                {
                    isClickF = false;
                    towerObj.SetActive(false);
                }
            }

            if (hitTowerAreaInfo.transform.gameObject.CompareTag("floor"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    towerObj.transform.position = new Vector3(hitTowerAreaInfo.point.x, hitTowerAreaInfo.point.y, hitTowerAreaInfo.point.z);
                    Debug.Log("f 누름");
                    isClickF = true;
                    towerObj.SetActive(true);
                }

                if (isClickF && isBuild == false)
                {
                    towerObj.transform.position = hitTowerAreaInfo.point;
                }

            }
        }
    }

    private void BuildTower()
    {
        if (isClickF)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isBuild) return;
                isClickF = false;
                isBuild = true;
                Debug.Log("타워설치요");
                towerObj.SetActive(false);
                Instantiate(towerPrefab, new Vector3(towerObj.transform.position.x, towerObj.transform.position.y, towerObj.transform.position.z), Quaternion.identity);
                Debug.Log(towerObj.transform.position);
                isBuild = false;
            }
        }
    }

}
 