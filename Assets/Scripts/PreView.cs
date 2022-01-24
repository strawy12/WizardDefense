using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreView : MonoBehaviour
{
    [Header("������ â")] [SerializeField] private GameObject preViewChang = null;
    [Header("Ÿ�� �����Ÿ�")] [SerializeField] private GameObject tower = null;

    private GameObject a;
    private bool isPreView = false;

    public void OnClickpreView()
    {
        CheckPreView();
        OnClick1();
    }

    public void OnClickPreViewOut()
    {
        Destroy(a);
        CheckPreView();
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
