using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreView : MonoBehaviour
{
    [Header("프리뷰 창")] [SerializeField] private GameObject preViewChang = null;
    [Header("타워 사정거리")] [SerializeField] private GameObject tower = null;

    private GameObject a;
    private bool isPreView = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickPreViewOut();
        }
    }

    public void OnClickpreView()
    {
        GameManager.Instance.UIManager.UiSound.PlaySound(0);
        CheckPreView();
        OnClick1();
    }

    public void OnClickPreViewOut()
    {
        GameManager.Instance.UIManager.UiSound.PlaySound(0);
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
