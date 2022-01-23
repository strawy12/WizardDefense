using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image infoBar;
    private Text infoText;

    [SerializeField] Image skillImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoBar.transform.DOScale(1f, 0.3f).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBar.transform.DOScale(new Vector3(0f, 1f, 0f), 0.3f).SetUpdate(true);
    }
}
