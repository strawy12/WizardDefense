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
    private int targetItemCnt;

    protected Image currentImage;

    private ButtonClickedEvent onClick_Right;
    private bool isItemAdd = false;

    public string slotType = "";

    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        currentImage = GetComponent<Image>();
        TargetItemImage = transform.GetChild(0).GetComponent<Image>();
        onClick_Right = new ButtonClickedEvent();
        currentState = InventorySlotState.Idle;
        currentIndex = transform.GetSiblingIndex();
    }

    protected override void Start()
    {
        base.Start();

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

    public void Init(ItemBase item)
    {
        targetItem = item;
        TargetItemImage.sprite = targetItem.itemSprite;
        TargetItemImage.gameObject.SetActive(true);
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
        DataManager.Instance.SetInventoryData(currentIndex, targetItem.itemData, targetItemCnt, slotType.Contains("Quick"));

    }

    private void SelectSlot()
    {
        EventManager<InventorySlot>.TriggerEvent(ConstantManager.INVENTORY_CLICK_LEFT, this);

        
    }

    private void SettingSlot()
    {
        EventManager<InventorySlot>.TriggerEvent(ConstantManager.INVENTORY_CLICK_RIGHT, this);
    }

    public virtual void ChangeTargetItem(ItemBase item)
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

        DataManager.Instance.SetInventoryData(currentIndex, item.itemData, targetItemCnt, slotType.Contains("Quick"));

    }

    public virtual void ResetSlot()
    {
        targetItem = null;
        currentIndex = 0;
        TargetItemImage.gameObject.SetActive(false);
        DataManager.Instance.SetInventoryData(currentIndex, null, targetItemCnt, slotType.Contains("Quick"));
    }


}