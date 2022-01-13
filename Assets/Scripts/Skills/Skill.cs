using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float coolTime;
    protected Attribute attribute;

    public virtual void OnTowerZoomIn()
    {
        GameManager.Instance.UIManager.ShowSkillUI(this);
    }

    public virtual void OnUseSkill()
    {
        Instantiate(bulletPrefab);
    }
}
