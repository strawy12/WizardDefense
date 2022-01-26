using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UnitInfoPanal : MonoBehaviour
{
    private CanvasGroup hudGroup;
    
    private Text unitNameText;
    private Text unitStatText;
    private Image unitImage;
    private HpBar unitHp;

    private bool isActive;
    private float timer;

    private void Awake()
    {
        hudGroup = GetComponent<CanvasGroup>();

        unitNameText = transform.GetChild(1).GetChild(0).GetComponent<Text>();
        unitImage = transform.GetChild(2).GetComponent<Image>();
        unitHp = transform.GetChild(3).GetComponent<HpBar>();
        unitStatText = transform.GetChild(4).GetComponent<Text>();

        EventManager<UnitInfo>.StartListening(ConstantManager.VIEW_UNITINFO, UpdateInfo);
        ClearPanal();
    }

    private void Update()
    {
        if(isActive)
        {
            timer += Time.deltaTime;

            if(timer >= 3f)
            {
                ClearPanal();
                GameManager.Instance.selectedMonster?.ShowOutLine(false);
                GameManager.Instance.selectedItem?.ShowOutLine(false);
            }
        }
    }


    private void UpdateInfo(UnitInfo info)
    {
        unitNameText.text = info.unitName;
        unitImage.sprite = info.unitSprite;
        unitStatText.text = string.Format("공격력 : {0:0.00}\n방어력 : {1:0.00}", info.attackPower, info.defence);
        //unitImage.sprite = info.unitSprite;
        unitHp.InitHpBar(info.maxHp);
        hudGroup.DOKill();
        hudGroup.DOFade(1f, 0.3f);
        unitHp.UpdateHpBar(info.currentHp);
        isActive = true;
        timer = 0f;
    }

    private void ClearPanal()
    {
        hudGroup.DOKill();
        hudGroup.DOFade(0f, 0.2f);
        isActive = false;
    }
    
}
