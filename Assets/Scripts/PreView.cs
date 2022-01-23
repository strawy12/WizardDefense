using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreView : MonoBehaviour
{
    [Header("프리뷰 창")] [SerializeField] private GameObject preViewChang = null;
    [Header("타워 배열")] [SerializeField] private GameObject[] tower = null;

    private bool isPreView = false;

    private int selectTower;

    private void Update()
    {
        
    }

    public void OnClickpreView()
    {
        CheckPreView();
    }

    public void OnClickPreViewOut()
    {
        CheckPreView();
    }

    private void CheckPreView()
    {
        isPreView = !isPreView;

        if (isPreView)
        {
            preViewChang.SetActive(true);
        }
        else
        {
            preViewChang.SetActive(false);
        }
    }

    public void OnClick1()
    {
        Instantiate(tower[0]);
    }

    public void OnClick2()
    {
        Instantiate(tower[1]);
    }

    public void OnClick3()
    {
        Instantiate(tower[2]);
    }

    private void TowerBuild(int num)
    {
        Instantiate(tower[num]);
    }
}
