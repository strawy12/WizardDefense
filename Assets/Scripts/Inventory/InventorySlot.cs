using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : Button, IPointerClickHandler
{
    public RectTransform rectTransform { get; private set; }

    public ItemBase targetItem;

    public Image TargetItemImage { get; private set; }

    public InventorySlotState currentState;

    private int currentIndex;

    private Image currentImage;

    private ButtonClickedEvent onClick_Right;
    private bool isItemAdd = false;


    protected override void Start()
    {
        base.Start();
        rectTransform = GetComponent<RectTransform>();
        currentImage = GetComponent<Image>();
        TargetItemImage = transform.GetChild(0).GetComponent<Image>();
        onClick_Right = new ButtonClickedEvent();
        
        currentState = InventorySlotState.Idle;

        currentIndex = transform.GetSiblingIndex();

        onClick.AddListener(AddTargetItem);
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

    private void AddTargetItem()
    {
        if (isItemAdd) return;
        isItemAdd = true;
        onClick.AddListener(SelectSlot);
        onClick.RemoveListener(AddTargetItem);
        targetItem = GameManager.Instance.Data.GetItemBase(currentIndex);


        if (targetItem == null)
        {
            return;
        }

        TargetItemImage.sprite = targetItem.itemSprite;
        TargetItemImage.gameObject.SetActive(true);
    }

    private void SelectSlot()
    {
        EventManager<InventorySlot>.TriggerEvent(ConstantManager.INVENTORY_CLICK_LEFT, this);

        
    }

    private void SettingSlot()
    {
        EventManager<InventorySlot>.TriggerEvent(ConstantManager.INVENTORY_CLICK_RIGHT, this);
    }

    public void ChangeTargetItem(ItemBase item)
    {
        targetItem = item;
        TargetItemImage.sprite = targetItem?.itemSprite;

        if(TargetItemImage.sprite == null)
        {
            TargetItemImage.gameObject.SetActive(false);
        }

        else
        {
            TargetItemImage.gameObject.SetActive(true);
        }
    }

    public void ResetSlot()
    {
        targetItem = null;
        TargetItemImage.gameObject.SetActive(false);
    }


}