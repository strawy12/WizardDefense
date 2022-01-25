using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Runes : MonoBehaviour
{
    [SerializeField] private int maxNum;    // ·é ÃÖ´ë°¹¼ö
    [SerializeField] private int currentNum;    // ÇöÀç ·é °¹¼ö
    [SerializeField] private Image runeImg;
    [SerializeField] private Text runeText;
    public TowerSelect towerSelect;

    private int indexNum = 0;   // Á¦ÀÛ´ë¿¡ ³ÖÀº ·é °¹¼ö

    private void Start()
    {
        UpdateText();
    }

    public void OnClickPlus()
    {
        if (currentNum == 0)
        {
            currentNum = 0;
            UpdateText();
        }
        else
        {
            towerSelect.AddRune();
            indexNum++;
            currentNum--;
            UpdateText();
        }
    }

    public void OnClickMinus()
    {
        if (indexNum == 0)
        {
            indexNum = 0;
            UpdateText();
        }
        else
        {
            if (towerSelect.curRune != 0)
            {
                indexNum--;
                currentNum++;
                towerSelect.MinusRune();
                UpdateText();
            }
        }
    }

    private void UpdateText()
    {
        runeText.text = $"{currentNum} / {maxNum}";
        RuneNumCheck();
    }

    private void RuneNumCheck()
    {
        if (currentNum == 0)
        {
            runeImg.color = new Color(0.6f, 0.6f, 0.6f, 1f);
        }
        else
        {
            runeImg.color = new Color(1, 1f, 1f, 1f);
        }
    }

    private void SetMaxNum()
    {
        maxNum = currentNum;
    }
}
