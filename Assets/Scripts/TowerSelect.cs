using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelect : MonoBehaviour
{
    [SerializeField] private Text towerName;
    [SerializeField] private Text needText;
    [SerializeField] private Image towerImage;
    private int curRune;

    public List<Tower> towerList = new List<Tower>();

    [SerializeField] private Sprite[] towerSprite;

    private int needMax;

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

    private void SetValue(int num)
    {
        needMax = towerList[num].energy;
        towerImage.sprite = towerSprite[num];

        needText.text = $"{curRune} / {needMax}";
        towerName.text = towerList[num].name;
    }

    public void AddRune()
    {
        Debug.Log("add");
        curRune++;
    }

    public void MinusRune()
    {
        curRune--;
    }

    public void OnClickBuild()
    {
        if (curRune == needMax)
        {
            Debug.Log("설치성공");
        }
        else
        {
            Debug.Log("룬이 부족하자나;;");
        }
    }

}
