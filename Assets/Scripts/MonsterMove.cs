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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        virtualHP = currentHp;
    }

    private void Update()
    {
        if (!finished_Init) return;

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

    public void Damaged(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            GameManager.Instance.enemies.Remove(this);
            Destroy(gameObject);
        }
        else
        {
            //StartCoroutine(OnDamagedEffect());
        }
    }

    public void VirtualDamaged(int power)
    {
        virtualHP -= power;
    }
}
