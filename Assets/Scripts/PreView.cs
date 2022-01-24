using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreView : MonoBehaviour
{
    [Header("프리뷰 창")] [SerializeField] private GameObject preViewChang = null;
    [Header("타워 배열")] [SerializeField] private GameObject[] tower = null;

    public Transform mother;

    private GameObject _tower;
    private bool isPreView = false;

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
        Destroy(_tower);
        Towerr(0);
    }

    public void OnClick2()
    {
        Destroy(_tower);
        Towerr(1);
    }

    public void OnClick3()
    {
        Destroy(_tower);
        Towerr(2);
    }

    private void Towerr(int t)
    {
        _tower = Instantiate(tower[t], TowerSelect.buildTrn);

        _tower.transform.SetParent(mother);
    }

    private void DeleteChild()
    {
        for (int i = 0; i < mother.transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void TowerBuild(int num)
    {
        Instantiate(tower[num]);
    }
}
