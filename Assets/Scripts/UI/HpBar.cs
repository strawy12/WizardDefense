using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HpBar : MonoBehaviour
{
    private Image frontHpPanal;
    private Image backHpPanal;
    private Text hpInfoText;

    private float maxHp;

    protected virtual void Awake()
    {
        backHpPanal = transform.GetChild(0).GetComponent<Image>();
        frontHpPanal = transform.GetChild(1).GetComponent<Image>();
        hpInfoText = transform.GetChild(2).GetComponent<Text>();
    }

    public void InitHpBar(float maxHp)
    {
        this.maxHp = maxHp;
        backHpPanal.transform.localScale = Vector3.one;
        frontHpPanal.transform.localScale = Vector3.one;
        UpdateHpText(maxHp);
    }

    public void UpdateHpBar(float hp)
    {
        float value = (1f / maxHp) * hp;

        frontHpPanal.transform.DOScaleX(value, 0.2f);

        UpdateHpText(hp);
    }

    private void UpdateHpText(float hp)
    {
        hpInfoText.text = string.Format("{0:0.00} / {1:0.00}", hp, maxHp);
    }



}
