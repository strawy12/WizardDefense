using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelect : MonoBehaviour
{
    [Header("Ÿ�� �̸� �ؽ�Ʈ")] [SerializeField] private Text towerName;
    [Header("��ġ�� �ʿ��� �� �ؽ�Ʈ")] [SerializeField] private Text needText;
    [Header("Ÿ�� �̹���")] [SerializeField] private Image towerImage;
    [Header("��ġ ��ư")] [SerializeField] private Button buildBtn;

    [HideInInspector] public int curRune;

    [Header("Ÿ�� ���� ����Ʈ")] public List<Tower> towerList = new List<Tower>();

    [Header("Ÿ���� ��������Ʈ")] [SerializeField] private Sprite[] towerSprite;
    [Header("Ÿ���� ������Ʈ �迭")] [SerializeField] private GameObject[] tower;

    [HideInInspector] public int needMax;

    public static Transform buildTrn;
    public static GameObject buildObj;

    private int selectTower;

    public TpsController tpsController;

    private void Start()
    {
        OnClickTower1();
    }

    public void OnClickTower1()
    {
        SetValue(0);
    }

    private void UpdateUI()
    {
        needText.text = $"{curRune} / {needMax}";
        CheckCanBuild();
    }

    private void SetValue(int num)
    {
        selectTower = num;
        needMax = towerList[num].energy;
        towerImage.sprite = towerSprite[num];

        UpdateUI();
        towerName.text = towerList[num].name;
    }

    public void AddRune()
    {
        curRune++;
        UpdateUI();
    }

    public void MinusRune()
    {
        if (needMax > 0)
        {
            curRune--;
            UpdateUI();
        }
        else
        {
            return;
        }
    }

    public void OnClickBuild()
    {
        if (curRune >= needMax)
        {
            curRune -= needMax;
            GameManager.Instance.UIManager.OnClickOutChang();
            GameManager.Instance.UIManager.FMarkFalse();
            Debug.Log("��ġ����");
            UpdateUI();
            SelectTower();
        }
        else
        {
            Debug.Log("���� �������ڳ�;;");
        }
    }

    private void SelectTower()
    {
        GameObject a = null;

        a = Instantiate(tower[0], buildTrn);
        a.transform.SetParent(null);
        Destroy(buildObj);
    }

    private void CheckCanBuild()
    {
        if (curRune >= needMax)
        {
            buildBtn.interactable = true;
        }
        else
        {
            buildBtn.interactable = false;
        }
    }
}
