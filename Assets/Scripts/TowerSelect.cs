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

    public bool isMax { get { return curRune >= needMax; } }

    private int selectTower;


    private void Start()
    {
        OnClickTower1();
        EventManager.StartListening(ConstantManager.BUILDUI_ADDRUNE, AddRune);
        EventManager.StartListening(ConstantManager.BUILDUI_SUBRUNE, SubRune);
    }

    public void OnClickTower1()
    {
        GameManager.Instance.UIManager.UiSound.PlaySound(0);
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
        GameManager.Instance.UIManager.UiSound.PlaySound(1);
        curRune++;
        UpdateUI();
    }

    public void SubRune()
    {
        GameManager.Instance.UIManager.UiSound.PlaySound(2);
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
            GameManager.Instance.UIManager.UiSound.PlaySound(3);
            EventManager.TriggerEvent(ConstantManager.RETURN_RUNEVALUE);
            curRune -= needMax;
            GameManager.Instance.UIManager.OnClickOutChang();
            GameManager.Instance.UIManager.FMarkFalse();
            UpdateUI();
            SelectTower();
        }
        else
        {
            return;
        }
    }

    private void SelectTower()
    {
        GameObject a = null;

        a = Instantiate(tower[0], buildTrn);
        a.transform.SetParent(null);
        Destroy(buildObj);
        if(GameManager.Instance.UIManager.currentUIPanels.Count > 0)
        {
            GameManager.Instance.UIManager.currentUIPanels.RemoveAt(GameManager.Instance.UIManager.currentUIPanels.Count - 1);
        }
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
