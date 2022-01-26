using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelect : MonoBehaviour
{
    [Header("타워 이름 텍스트")] [SerializeField] private Text towerName;
    [Header("설치에 필요한 룬 텍스트")] [SerializeField] private Text needText;
    [Header("타워 이미지")] [SerializeField] private Image towerImage;
    [Header("설치 버튼")] [SerializeField] private Button buildBtn;

    [HideInInspector] public int curRune;

    [Header("타워 정보 리스트")] public List<Tower> towerList = new List<Tower>();

    [Header("타워들 스프라이트")] [SerializeField] private Sprite[] towerSprite;
    [Header("타워들 오브젝트 배열")] [SerializeField] private GameObject[] tower;

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
