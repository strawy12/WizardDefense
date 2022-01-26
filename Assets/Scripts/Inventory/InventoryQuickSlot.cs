using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryQuickSlot : InventorySlot
{
    public Image linkSlotImage = null;
    public Text linkSlotText = null;
    private Text currentKeyText = null;



    protected override void Start()
    {
        base.Start();
    }

    public override void Init(ItemBase item, int count)
    {
        base.Init(item, count);

        linkSlotImage.sprite = TargetItemImage.sprite;
        if (linkSlotImage != null)
        {
            linkSlotImage.gameObject.SetActive(true);
        }
        else
        {
            linkSlotImage.gameObject.SetActive(false);
        }
    }

    public override void ChangeTargetItem(ItemBase item)
    {
        base.ChangeTargetItem(item);

        linkSlotImage.sprite = TargetItemImage.sprite;
        if (linkSlotImage != null)
        {
            linkSlotImage.gameObject.SetActive(true);
        }
        else
        {
            linkSlotImage.gameObject.SetActive(false);
        }
    }

    public override void ResetSlot()
    {
        base.ResetSlot();
        linkSlotImage.sprite = null;
        Debug.Log("¿¿æ÷");
        linkSlotImage.gameObject.SetActive(false);
        Debug.Log(linkSlotImage.gameObject.activeSelf);

    }
}
