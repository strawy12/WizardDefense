using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private GameObject tower = null;

    private bool isPlant = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Build();
        }
    }

    private void Build()
    {
        isPlant = !isPlant;

        if (isPlant)
        {
            Debug.Log("설치");
            BuidTower();
        }

        else
        {
            Debug.Log("못함");
        }
    }

    private void BuidTower()
    {
        GameObject a;
        a = Instantiate(tower);
        a.transform.SetParent(null);
    }

}
