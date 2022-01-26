using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Runes : MonoBehaviour
{
    private EnergyData currentEnergtData;

    private int leftNum { get { return currentEnergtData.count - indexNum; } }    // 사용 후 남은
    private int currentNum { get { return currentEnergtData.count; } }   // 현재 룬

    [SerializeField] private PropertyType property;

    [SerializeField] private Image runeImg;
    [SerializeField] private Text runeText;

    private int indexNum = 0;   // 제작대에 넣은 룬 갯수

    private void Start()
    {
        currentEnergtData = DataManager.Instance.GetEnergyData(property);
        EventManager.StartListening(ConstantManager.OPEN_BUILDPANEL, UpdateText);
        EventManager.StartListening(ConstantManager.RETURN_RUNEVALUE, ReturnRune);
        UpdateText();
    }

    public void OnClickPlus()
    {
        if (leftNum <= 0)
        {
            return;
        }
        else
        {
            indexNum++;
            UpdateText();
            EventManager.TriggerEvent(ConstantManager.BUILDUI_ADDRUNE);
        }
    }

    public void OnClickMinus()
    {
        if (indexNum <= 0)
        {
            return;
        }
        else
        {
            indexNum--;
            UpdateText();
            EventManager.TriggerEvent(ConstantManager.BUILDUI_SUBRUNE);
        }
    }

    private void UpdateText()
    {
        runeText.text = $"{leftNum} / {currentNum}";
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

    private void ReturnRune()
    {
        if (indexNum == 0) return;

        currentEnergtData.count -= indexNum;
        indexNum = 0;
        DataManager.Instance.SaveToJson();
    }
}
