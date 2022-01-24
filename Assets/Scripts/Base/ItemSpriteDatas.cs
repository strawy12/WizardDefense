using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSpriteData
{
    public string itemId;
    public Sprite itemSprite;
}

[CreateAssetMenu(fileName = "ItemSpriteDatas", menuName = "Scriptable Object/ItemSpriteDatas")]
public class ItemSpriteDatas : ScriptableObject
{
    public List<ItemSpriteData> itemSpriteDatas;

    public Sprite FindItemSprite(string id)
    {
        return itemSpriteDatas.Find((item) => item.itemId == id).itemSprite;
    }
}
