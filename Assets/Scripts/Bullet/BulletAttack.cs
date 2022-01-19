using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class BulletAttack : PoolObject
{
    private StateMachine<AttackType, StateDriverUnity> fsm;

    public enum AttackType
    {
        None,
        Targeting,
        Range
    }

    public AttackType state;
    private TowerAttack towerAttack;
    private MonsterMove targetEnemy;
    private Collider enemy;
    public float rangeDistance;

    protected override void Awake()
    {
        base.Awake();
        fsm = new StateMachine<AttackType, StateDriverUnity>(this);
    }

    public void Init(TowerAttack towerAttack)
    {
        this.towerAttack = towerAttack;
        targetEnemy = towerAttack.GetTargetEnemy();
        fsm.ChangeState(AttackType.None);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ConstantManager.ENEMY_TAG)) return;
        enemy = other;
        //hitPosiiton = collision.gameObject.transform.position;
        fsm.ChangeState(state);
        base.Despawn();
    }

    private void Targeting_Enter()
    {
        Attack(targetEnemy);
        EndAttack();
    }

    private void Range_Enter()
    {
        for (int i = 0; i < GameManager.Instance.enemies.Count; i++)
        {
            if (Vector3.Distance(GameManager.Instance.enemies[i].transform.position, enemy.transform.position) <= rangeDistance)
            {
                if (GameManager.Instance.enemies[i] != null)
                    Attack(GameManager.Instance.enemies[i]);
            }
        }
        
        GameManager.Instance.StartCoroutine(GameManager.Instance.ShowBoundary(enemy.transform.position, new Vector2(rangeDistance, rangeDistance) * transform.localScale));
        EndAttack();
    }

    private void Attack(MonsterMove enemy)
    {
        if (enemy == null)
        {
            this.enemy.gameObject.GetComponent<MonsterMove>().Damaged(towerAttack.towerBase.attackPower);
        }

        else
        {
            enemy.Damaged(towerAttack.towerBase.attackPower);
        }
    }

    private void EndAttack()
    {
        enemy = null;
        targetEnemy = null;
        fsm.ChangeState(AttackType.None);
    }
}
