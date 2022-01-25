using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : Button, IPointerClickHandler
{
    public RectTransform rectTransform { get; private set; }

    public ItemBase targetItem;

    public string TargetItemName 
    {
        get 
        { 
            if(targetItem == null)
            {
                return "";
            }

            if(targetItem.itemData == null)
            {
                return "";
            }

            return targetItem.itemData.itemName; 
        } 
    }

    public Image TargetItemImage { get; private set; }

    public Text targetItemCountText = null;

    public InventorySlotState currentState;

    private int currentIndex;

    protected Image currentImage;

    private ButtonClickedEvent onClick_Right;

    public string slotType = "";

    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        currentImage = GetComponent<Image>();
        TargetItemImage = transform.GetChild(0).GetComponent<Image>();
        targetItemCountText = transform.GetChild(1).GetComponent<Text>();
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

    public void Init(ItemBase item, int count)
    {
        targetItem = item;
        targetItem.count = count;
        UpdateItemCountText(targetItem.count);
        TargetItemImage.sprite = targetItem.itemSprite;
        TargetItemImage.gameObject.SetActive(true);
    }

    public void AddTargetItem(ItemBase item)
    {
        targetItem = item;
        TargetItemImage.sprite = targetItem?.itemSprite;
        targetItem.count = 1;
        UpdateItemCountText(targetItem.count);
        TargetItemImage.gameObject.SetActive(true);
        DataManager.Instance.SetInventoryData(currentIndex, targetItem.itemData, targetItem.count, slotType.Contains("Quick"));

    }

    private void SelectSlot()
    {
        EventManager<InventorySlot>.TriggerEvent(ConstantManager.INVENTORY_CLICK_LEFT, this);

        
    }

    private void SettingSlot()
    {
        EventManager<InventorySlot>.TriggerEvent(ConstantManager.INVENTORY_CLICK_RIGHT, this);
    }

    public void IncreaseItem()
    {
        targetItem.count++;
        UpdateItemCountText(targetItem.count);
        DataManager.Instance.SetInventoryData(currentIndex, targetItem.itemData, targetItem.count, slotType.Contains("Quick"));
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


        UpdateItemCountText(targetItem.count);

    }

    private void UpdateItemCountText(int count)
    {
        targetItemCountText.text = count.ToString();

    }

    public virtual void ResetSlot()
    {
        targetItem = null;
        UpdateItemCountText(0);
        TargetItemImage.gameObject.SetActive(false);
        DataManager.Instance.SetInventoryData(currentIndex, null, 0, slotType.Contains("Quick"));
    }


}