using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTower : MonoBehaviour
{
    [SerializeField] private int maxHp;
    private int currentHp;

    private void Awake()
    {
        EventManager<int>.StartListening(ConstantManager.MONSTER_ATTACK, Damaged);
    }

    private void Start()
    {
        currentHp = maxHp;
        EventManager<float>.TriggerEvent(ConstantManager.POINTTOWER_INIT, maxHp);
    }

    public void Damaged(int attackDamage)
    {
        currentHp -= attackDamage;
        EventManager<float>.TriggerEvent(ConstantManager.POINTTOWER_DAMAGED, currentHp);
    }
}
