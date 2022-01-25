using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform targetPicker;
    [SerializeField] private InventorySettingPanal slotSettingPanal;

    [SerializeField] private Transform quickSlotTrs;
    [SerializeField] private Transform itemSlotTrs;

    private InventoryQuickSlot[] quickSlots;
    private InventorySlot[] itemSlots;

    private InventorySlot selectSlot;
    private ItemBase selectItem;

    public CanvasGroup canvasGroup;

    [SerializeField] private Image itemImage;


    private bool selectedItemSlot = false;

    private void Start()
    {
        quickSlots = quickSlotTrs.GetChild(0).GetComponentsInChildren<InventoryQuickSlot>();
        itemSlots = itemSlotTrs.GetChild(0).GetComponentsInChildren<InventorySlot>();
        canvasGroup = GetComponent<CanvasGroup>();

        InitItemSlot();

        EventManager<InventorySlot>.StartListening(ConstantManager.INVENTORY_CLICK_LEFT, SelectSlot);
        EventManager<InventorySlot>.StartListening(ConstantManager.INVENTORY_CLICK_RIGHT, SettingSlot);

        EventManager.StartListening(ConstantManager.INVENTORY_CLICK_MOVEBTN, SettingMoveEvent);
        EventManager.StartListening(ConstantManager.INVENTORY_CLICK_DROPBTN, DropEvent);
        EventManager.StartListening(ConstantManager.INVENTORY_CLICK_EQUIPBTN, EquipEvent);
        EventManager.StartListening(ConstantManager.INVENTORY_CLICK_BACKGROUND, SelectItemDropEvent);
        EventManager.StartListening(ConstantManager.TURNOFF_INVENTORY, TurnOffInventory);

        EventManager<ItemBase>.StartListening(ConstantManager.PICKUP_ITEM, PickUpItem);

    }

    private void Update()
    {
        if (selectSlot == null) return;

        if (selectSlot.currentState == InventorySlotState.Move)
        {
            itemImage.rectTransform.position = GameManager.Instance.MousePos;
        }
    }

    private void OnDisable()
    {
        EventManager.TriggerEvent(ConstantManager.TURNOFF_INVENTORY);
    }

    private void InitItemSlot()
    {
        List<InventoryData> inventoryDatas = DataManager.Instance.PlayerData.inventoryList;
        ItemBase item = null;
        int index = 0;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (inventoryDatas[i].item != null && inventoryDatas[i].item.item_ID != "")
            {
                item = GameManager.Instance.Data.ConversionToItemBase(inventoryDatas[i].item);
                itemSlots[i]?.Init(item, inventoryDatas[i].count);
            }
        }

        for (int i = itemSlots.Length; i < inventoryDatas.Count; i++)
        {
            if (inventoryDatas[i].item != null && inventoryDatas[i].item.item_ID != "")
            {
                index = i - itemSlots.Length;
                item = GameManager.Instance.Data.ConversionToItemBase(inventoryDatas[i].item);
                quickSlots[index]?.Init(item, inventoryDatas[i].count);
            }
        }
    }

    private void PickUpItem(ItemBase item)
    {
        InventorySlot inventorySlot = null;

        foreach (var slot in itemSlots)
        {
            if (inventorySlot == null && slot.TargetItemName == "")
            {
                inventorySlot = slot;
            }

            if (slot.TargetItemName == item.itemData.itemName)
            {
                slot.IncreaseItem();
                return;
            }
        }

        inventorySlot.AddTargetItem(item);
    }

    private void SelectSlot(InventorySlot slot)
    {
        SettingMoveEvent(slot);
        SetTargetPickerPos(slot.rectTransform.position, slot.slotType);
    }

    private void SettingSlot(InventorySlot slot)
    {
        selectSlot = slot;
        SetTargetPickerPos(slot.rectTransform.position, slot.slotType);
        SetSlotSettingPanalPos(slot.rectTransform.position);
    }

    private void SetTargetPickerPos(Vector3 targetPos, string type)
    {
        if (type.Contains("Quick"))
        {
            targetPicker.SetParent(quickSlotTrs);
        }

        else
        {
            targetPicker.SetParent(itemSlotTrs);
        }

        targetPicker.DOKill();
        targetPicker.DOMove(targetPos, 0.25f);

    }

    private void SetSlotSettingPanalPos(Vector3 targetPos)
    {

        slotSettingPanal.SetPosition(targetPos);
    }
    private void SettingMoveEvent(InventorySlot slot)
    {
        MoveEvent(slot);
    }

    private void StartMoveEvent()
    {
        selectSlot.currentState = InventorySlotState.Move;
        itemImage.sprite = selectItem.itemSprite;
        itemImage.gameObject.SetActive(true);
    }

    private void ReleaseMoveEvent()
    {
        itemImage.sprite = null;
        itemImage.gameObject.SetActive(false);
    }

    private void SettingMoveEvent()
    {
        selectSlot.currentState = InventorySlotState.Move;
        MoveEvent(selectSlot);
    }
    private void MoveEvent(InventorySlot slot)
    {
        if (selectItem != null)
        {
            if (slot.targetItem == null)
            {
                selectSlot = slot;
                selectSlot.ChangeTargetItem(selectItem);
                selectItem = null;
                selectSlot.currentState = InventorySlotState.Idle;
                ReleaseMoveEvent();
            }

            else
            {
                selectSlot.ChangeTargetItem(slot.targetItem);
                slot.ChangeTargetItem(selectItem);

                selectSlot = slot;
                selectItem = null;

                ReleaseMoveEvent();
            }
        }

        else
        {
            if (slot.targetItem == null)
            {
                selectSlot = slot;
                slot.currentState = InventorySlotState.Idle;
                ReleaseMoveEvent();
            }

            else
            {

                if (slot == selectSlot)
                {
                    selectItem = slot.targetItem;
                    slot.ResetSlot();
                    StartMoveEvent();
                }

                selectSlot = slot;

            }
        }

    }

    //private void MoveEvent(InventorySlot slot)
    //{
    //    if (selectItem != null)
    //    {
    //        if (slot.targetItem == null)
    //        {
    //            slot.ChangeTargetItem(selectItem);
    //            selectItem = null;
    //            selectSlot = slot;
    //            selectSlot.currentState = InventorySlotState.Idle;
    //            ReleaseMoveEvent();
    //        }

    //        else
    //        {
    //            ItemBase itemTemp = selectItem;
    //            selectItem = slot.targetItem;

    //            slot.ChangeTargetItem(itemTemp);
    //            selectSlot = slot;

    //            StartMoveEvent();
    //        }
    //    }

    //    else
    //    {
    //        if (slot.targetItem == null)
    //        {
    //            selectSlot = slot;
    //            slot.currentState = InventorySlotState.Idle;
    //            ReleaseMoveEvent();
    //        }

    //        else
    //        {
    //            selectSlot = slot;

    //            selectItem = selectSlot.targetItem;
    //            selectSlot.ResetSlot();

    //            StartMoveEvent();
    //        }
    //    }

    //}

    private void DropEvent()
    {
        EventManager<ItemBase>.TriggerEvent(ConstantManager.INVENTORY_DROP, selectSlot.targetItem);
        selectSlot.ResetSlot();
    }

    private void SelectItemDropEvent()
    {
        if (selectItem == null) return;

        EventManager<ItemBase>.TriggerEvent(ConstantManager.INVENTORY_DROP, selectItem);
        selectSlot.ResetSlot();
        ReleaseMoveEvent();
        selectItem = null;
    }

    private void EquipEvent()
    {
        if (selectSlot.slotType.Contains("Quick"))
        {
            UnEquipItem();
        }

        else
        {
            EquipItem();
        }
    }

    private void EquipItem()
    {
        foreach (var slot in quickSlots)
        {
            if (slot.targetItem == null)
            {
                slot.ChangeTargetItem(selectSlot.targetItem);
                selectSlot.ResetSlot();
                return;
            }
        }

        ItemBase tempItem = quickSlots[0].targetItem;

        quickSlots[0].ChangeTargetItem(selectSlot.targetItem);
        selectSlot.ChangeTargetItem(tempItem);
    }

    private void UnEquipItem()
    {
        foreach (var slot in itemSlots)
        {
            if (slot.targetItem == null)
            {
                slot.ChangeTargetItem(selectSlot.targetItem);
                selectSlot.ResetSlot();
                return;
            }
        }

        ItemBase tempItem = itemSlots[0].targetItem;

        itemSlots[0].ChangeTargetItem(selectSlot.targetItem);
        selectSlot.ChangeTargetItem(tempItem);
    }

    private void TurnOffInventory()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }
}
