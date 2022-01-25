using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreView : MonoBehaviour
{
    [Header("프리뷰 창")] [SerializeField] private GameObject preViewChang = null;
    [Header("타워 사정거리")] [SerializeField] private GameObject tower = null;
    [Header("포탑 설치 창")] [SerializeField] private GameObject towerChang = null;

    private GameObject a;
    private bool isPreView = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickPreViewOut();
        }
    }

    public void OnClickpreView()
    {
        towerChang.SetActive(false);
        CheckPreView();
        OnClick1();
    }

    public void OnClickPreViewOut()
    {
        Destroy(a);
        CheckPreView();
        towerChang.SetActive(true);
    }

    private void CheckPreView()
    {
        isPreView = !isPreView;

        if (isPreView)
        {
            preViewChang.SetActive(true);
            GameManager.Instance.UIManager.AddCurrentPanels(preViewChang);
        }
        else
        {
            preViewChang.SetActive(false);
            GameManager.Instance.UIManager.RemoveCurrentPanels(preViewChang);
        }
    }

    public void OnClick1()
    {
        a = Instantiate(tower, TowerSelect.buildTrn);
        a.transform.SetParent(null);
    }
}
