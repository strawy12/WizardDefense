using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class InventorySettingPanal : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject settingBtns;

    private void Start()
    {
        EventManager.StartListening(ConstantManager.INVENTORY_TURNOFF_SETTING, TurnOffSettingPanal);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.TriggerEvent(ConstantManager.INVENTORY_TURNOFF_SETTING);
    }

    public void SetPosition(Vector3 pos)
    {
        settingBtns.transform.position = pos;
        settingBtns.transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        settingBtns.transform.DOScale(Vector3.one, 0.2f);
    }

    public void OnClickMoveBtn()
    {
        EventManager.TriggerEvent(ConstantManager.INVENTORY_CLICK_MOVEBTN);
        TurnOffSettingPanal();
    }

    public void OnClickClosedBtn()
    {
        EventManager.TriggerEvent(ConstantManager.INVENTORY_TURNOFF_SETTING);
    }
    
    public void OnClickDropBtn()
    {
        EventManager.TriggerEvent(ConstantManager.INVENTORY_CLICK_DROPBTN);
        TurnOffSettingPanal();
    }

    private void TurnOffSettingPanal()
    {
        gameObject.SetActive(false);
    }
}
