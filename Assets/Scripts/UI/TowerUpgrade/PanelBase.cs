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

    public virtual void Init() { }
    public virtual void Init(int index) { }
    public virtual void UpdateData() { }
}
