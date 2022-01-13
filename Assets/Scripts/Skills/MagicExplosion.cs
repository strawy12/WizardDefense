using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicExplosion : Skill
{
    public override void OnTowerZoomIn()
    {
        base.OnTowerZoomIn();
    }

    public override void OnUseSkill()
    {
        Instantiate(bulletPrefab);
    }
}
