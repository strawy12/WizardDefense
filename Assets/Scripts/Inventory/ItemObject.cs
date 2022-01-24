using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemBase item;
    private UnitInfo unitInfo;

    public void GetInfo()
    {
        unitInfo = new UnitInfo();
        unitInfo.unitName = item.itemData.itemName;
        unitInfo.attackPower = 0;
        unitInfo.defence = 0;
        unitInfo.maxHp = 0;
        unitInfo.currentHp = 0;

        EventManager<UnitInfo>.TriggerEvent(ConstantManager.VIEW_UNITINFO, unitInfo);
    }
}
