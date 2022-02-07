using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelBase : MonoBehaviour
{
    [SerializeField] protected Text nameText;
    [SerializeField] protected Text infoText;
    [SerializeField] protected Text priceText;
    [SerializeField] protected Image productImage;
    protected private bool isSelected;

    public virtual void Init() { }
    public virtual void Init(int index) { }
    public virtual void UpdateData() { }
    public virtual void Deselect() { }
    public virtual void OnSelect() { }
    public bool GetIsSelected()
    {
        return isSelected;
    }

    public virtual TowerRoot GetRoot() { return null; }
}
