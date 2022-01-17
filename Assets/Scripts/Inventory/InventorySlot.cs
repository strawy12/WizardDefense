using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : Button, IPointerClickHandler
{
    #region Target Item
    private Item targetItem;
    public Image TargetItemImage { get; private set; }
    #endregion

    private Image currentImage;

    private ButtonClickedEvent onClick_Right;


    protected override void Start()
    {
        base.Start();
        currentImage = GetComponent<Image>();
        onClick_Right = new ButtonClickedEvent();
        onClick.AddListener(SelectSlot);
        onClick_Right.AddListener(SettingSlot);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            onClick.Invoke();
        }

        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            onClick_Right.Invoke();
        }
    }

    private void SelectSlot()
    {
        EventManager<InventorySlot>.TriggerEvent(ConstantManager.INVENTORY_CLICK_LEFT, this);
    }

    private void SettingSlot()
    {
        EventManager<InventorySlot>.TriggerEvent(ConstantManager.INVENTORY_CLICK_RIGHT, this);
    }


}