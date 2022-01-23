using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentButton : MonoBehaviour
{
    [SerializeField] private bool isRune;
    private Image image;
    private Button button;

    private void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        button.onClick.AddListener(() => OnButtonSelected());
    }

    public void OnItemSelect(Attribute attribute)
    {
        if(isRune)
        {
            GameManager.Instance.selectedTower.SetAttribute(attribute);
            image.sprite = attribute.sprite;
        }

        else
        {
            GameManager.Instance.selectedTower.SetAttribute(GameManager.Instance.attributes[0]);
        }
    }

    public void OnButtonSelected()
    {
        Debug.Log("인벤토리 켜짐");
        GameManager.Instance.UIManager.SetCurEquipBtn(this);
    }
}
