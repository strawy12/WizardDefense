using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemBase itemData;

    public void Init()
    {
    }

    public void Despawn()
    {
        Debug.Log(itemData.itemName);
        Destroy(gameObject);
    }
}
