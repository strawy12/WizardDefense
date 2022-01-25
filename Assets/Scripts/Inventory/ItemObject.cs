using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemBase item;
    private UnitInfo unitInfo;
    private Outline outline;
    private float timer = 0f;
    private bool isActive = false;

    private void Start()
    {
        outline = GetComponent<Outline>();
    }
    private void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;

            if (timer >= 3f)
            {
                ShowOutLine(false);
            }
        }
    }

    public void GetInfo()
    {
        unitInfo = new UnitInfo();
        unitInfo.unitName = item.itemData.itemName;
        unitInfo.attackPower = 0;
        unitInfo.defence = 0;
        unitInfo.maxHp = 0;
        unitInfo.currentHp = 0;
        unitInfo.unitSprite = item.itemSprite;

        EventManager<UnitInfo>.TriggerEvent(ConstantManager.VIEW_UNITINFO, unitInfo);
    }

    public void PickUpItem()
    {
        EventManager<ItemBase>.TriggerEvent(ConstantManager.PICKUP_ITEM, item);
        Destroy(gameObject);
    }

    public void ShowOutLine(bool isShow)
    {
        if (isShow)
        {
            outline.OutlineWidth = outline.thisOutLine;
            isActive = true;
            timer = 0f;
        }
        else
        {
            outline.OutlineWidth = 0f;
            isActive = false;
            timer = 0f;
        }
    }
}
