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

    private bool finished_Init = false;

    private Vector3 currentDir = Vector3.zero;
    private Transform targetPoint = null;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
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
        agent.SetAreaCost(0, 1);
    }

    
}
