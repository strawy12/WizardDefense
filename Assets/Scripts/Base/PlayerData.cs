using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData 
{
    public float bgmSoundVolume;
    public float effectSoundVolume;

    public float sensitivityValue;

    public List<InventoryData> inventoryList;
    public List<EnergyData> havedEnergyList;
    public List<KeyInputData> keyInputDataList;
}

[System.Serializable]
public class InventoryData
{
    public int index;
    public ItemData item;
    public int count;
    public bool isQuitSlot;

    public InventoryData(int index, bool isQuitSlot)
    {
        this.index = index;
        this.isQuitSlot = isQuitSlot;
        count = 0;
        item = null;
    }
}

[System.Serializable]
public class EnergyData
{
    public PropertyType type;
    public int count;

    public EnergyData(PropertyType type, int count)
    {
        this.type = type;
        this.count = count;
    }
}

[System.Serializable]
public class KeyInputData
{
    public KeyAction keyAction;
    public KeyCode keyCode;

    public KeyInputData(KeyAction keyAction, KeyCode keyCode)
    {
        this.keyAction = keyAction;
        this.keyCode = keyCode;
    }
}

