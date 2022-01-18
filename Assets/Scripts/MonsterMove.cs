using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MonsterLove.StateMachine;
using DG.Tweening;

public class MonsterMove : MonoBehaviour
{
    [SerializeField] private MonsterBase monsterBase;

    private NavMeshAgent agent;

    private int currentHp = 0;
    public int virtualHP;

    private bool finished_Init = false;

    private Vector3 currentDir = Vector3.zero;
    private Transform targetPoint = null;

    private UnitInfo unitInfo;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    private void Start()
    {
        virtualHP = currentHp;
        unitInfo = new UnitInfo();
    }

    private void Update()
    {
        if (agent.remainingDistance <= 3f)
        {
            AttackPointTower();
        }
    }

    public void Init(MonsterBase monsterBase, Transform target)
    {
        this.monsterBase = monsterBase;
        currentHp = monsterBase.info.maxHp;
        finished_Init = true;
        targetPoint = target;
        agent.SetDestination(targetPoint.position);
        GameManager.Instance.enemies.Add(this);
    }

    private bool CheckIsMoved()
    {
        if (agent.velocity.x > 0f)
        {
            return true;
        }

        if (agent.velocity.y > 0f)
        {
            return true;
        }

        if (agent.velocity.z > 0f)
        {
            return true;
        }

        return false;
    }

    public void Damaged(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Dead();
        }
    }

    public void VirtualDamaged(int power)
    {
        virtualHP -= power;
    }

    public void Dead()
    {
        GameManager.Instance.enemies.Remove(this);
        Destroy(gameObject);
    }

    public void AttackPointTower()
    {
        EventManager<int>.TriggerEvent(ConstantManager.MONSTER_ATTACK, monsterBase.info.attackPower);
        Dead();
    }

    public void GetInfo()
    {
        unitInfo.unitName = monsterBase.monsterName;
        unitInfo.attackPower = monsterBase.info.attackPower;
        unitInfo.defence = monsterBase.info.defense;
        unitInfo.maxHp = monsterBase.info.maxHp;
        unitInfo.currentHp = currentHp;

        EventManager<UnitInfo>.TriggerEvent(ConstantManager.MONSTER_GETINFO, unitInfo);
    }
}
public struct UnitInfo
{
    public string unitName;
    public int attackPower;
    public int defence;
    public int maxHp;
    public int currentHp;
    public Sprite unitSprite;
}

