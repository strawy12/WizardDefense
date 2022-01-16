using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class InventorySettingPanal : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject settingBtns;

    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.SetActive(false);
    }

    public void SetPosition(Vector2 pos)
    {
        settingBtns.transform.position = pos;
        settingBtns.transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        settingBtns.transform.DOScale(Vector3.one, 0.2f);
    }
}
