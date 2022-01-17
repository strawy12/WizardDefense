using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject targetPicker;
    [SerializeField] private InventorySettingPanal slotSettingPanal;

    private InventorySlot selectSlot;

    private bool selectItem = false;

    private void Start()
    {
        EventManager<InventorySlot>.StartListening(ConstantManager.INVENTORY_CLICK_LEFT, SelectSlot);
        EventManager<InventorySlot>.StartListening(ConstantManager.INVENTORY_CLICK_RIGHT, SettingSlot);
    }

    private void SelectSlot(InventorySlot slot)
    {
        ChangeSelectSlot(slot);
        SetTargetPickerPos(slot.transform.position);
    }

    private void SettingSlot(InventorySlot slot)
    {
        ChangeSelectSlot(slot);
        SetSlotSettingPanalPos(slot.transform.position);
    }

    private void SetTargetPickerPos(Vector2 targetPos)
    {
        targetPicker.transform.DOKill();
        targetPicker.transform.DOMove(targetPos, 0.25f);
    }

    private void SetSlotSettingPanalPos(Vector2 targetPos)
    {
        slotSettingPanal.SetPosition(targetPos);
    }

    private void ChangeSelectSlot(InventorySlot slot)
    {
        selectSlot = slot;
    }
}
