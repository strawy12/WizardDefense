using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoyBackground : Button
{
    protected override void Start()
    {
        base.Start();
        onClick.AddListener(() => OnClickBackground());
    }

    private void OnClickBackground()
    {
        EventManager.TriggerEvent(ConstantManager.INVENTORY_CLICK_BACKGROUND);
    }
}
