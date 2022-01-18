using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTowerHpBar : HpBar
{
    protected override void Awake()
    {
        base.Awake();

        EventManager<int>.StartListening(ConstantManager.POINTTOWER_INIT, InitHpBar);
        EventManager<int>.StartListening(ConstantManager.POINTTOWER_DAMAGED, UpdateHpBar);
    }
}
