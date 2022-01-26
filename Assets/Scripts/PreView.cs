using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreView : MonoBehaviour
{
    [Header("������ â")] [SerializeField] private GameObject preViewChang = null;
    [Header("Ÿ�� �����Ÿ�")] [SerializeField] private GameObject tower = null;
    [Header("��ž ��ġ â")] [SerializeField] private GameObject towerChang = null;

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
