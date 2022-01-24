using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase
{
    public ItemData itemData;
    public Sprite itemSprite;

    public ItemBase(ItemData data, Sprite sprite)
    {
        itemData = new ItemData(data.item_ID, data.itemName, data.itemType);
        itemSprite = sprite;
    }
}

[System.Serializable]
public class ItemData
{
    public string item_ID;
    public string itemName;
    public PropertyType itemType;

    public ItemData(string id, string name, PropertyType type)
    {
        item_ID = id;
        itemName = name;
        itemType = type;
    }
}



[CreateAssetMenu(fileName = "ItemDatas", menuName = "Scriptable Object/ItemDatas")]
public class ItemDatas : ScriptableObject
{
    public string updateVersion;
    public List<ItemData> itemDataList;

    public int Length {  get { return itemDataList.Count; } }
}
