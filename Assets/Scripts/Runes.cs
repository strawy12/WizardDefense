using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Runes : MonoBehaviour
{
    [SerializeField] private int maxNum;    // ·é ÃÖ´ë°¹¼ö
    [SerializeField] private int currentNum;    // ÇöÀç ·é °¹¼ö
    [SerializeField] private Text runeText;
    private TowerSelect towerSelect;

    private int indexNum = 0;   // Á¦ÀÛ´ë¿¡ ³ÖÀº ·é °¹¼ö

    private void Start()
    {
        towerSelect = GetComponent<TowerSelect>();
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
            indexNum++;
            currentNum--;
            towerSelect.AddRune();
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
            indexNum--;
            currentNum++;
            towerSelect.MinusRune();
            UpdateText();
        }
    }

    private void UpdateText()
    {
        runeText.text = $"{currentNum} / {maxNum}";
    }
}
