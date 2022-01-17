using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelect : MonoBehaviour
{
    [SerializeField] private Text towerName;
    [SerializeField] private Text needText;
    [SerializeField] private Image towerImage;
    [SerializeField] private Button buildBtn;
    [HideInInspector] public int curRune;

    public List<Tower> towerList = new List<Tower>();

    [SerializeField] private Sprite[] towerSprite;

    [HideInInspector] public int needMax;

    [SerializeField] private GameObject[] tower;

    public static Transform buildTrn;

    private int selectTower;

    private void Start()
    {
        OnClickTower1();
    }

    public void OnClickTower1()
    {
        SetValue(0);
    }

    public void OnClickTower2()
    {
        SetValue(1);
    }

    public void OnClickTower3()
    {
        SetValue(2);
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
            //Debug.Log("설치성공");
            UpdateUI();
            SelectTower();
        }
        else
        {
            Debug.Log("룬이 부족하자나;;");
        }
    }

    private void SelectTower()
    {
        if(selectTower==0)
        {
            Instantiate(tower[0],buildTrn);
        }
        else if(selectTower == 1)
        {
            Instantiate(tower[1],buildTrn);
        }

        else if(selectTower == 2)
        {
            Instantiate(tower[2],buildTrn);
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
