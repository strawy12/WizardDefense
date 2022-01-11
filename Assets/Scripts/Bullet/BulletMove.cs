using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : PoolObject
{
    TowerAttack towerAttack;
    Transform targetTransform;
    Rigidbody rigid;
    TowerState state;

    protected override void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        base.Awake();
    }
    public void Init(TowerAttack towerAttack)
    {
        this.towerAttack = towerAttack;
        targetTransform = towerAttack.targetEnemy?.transform;
        state = towerAttack.towerState;

        rigid.velocity = Vector3.zero;
    }

    private void Update()
    {
        if (state == TowerState.OutControl)
        {
            if (targetTransform == null)
            {
                towerAttack.SetTargetEnemy();
                if (towerAttack.targetEnemy == null)
                {
                    Despawn();
                    return;
                }
                targetTransform = towerAttack.targetEnemy.transform;
            }

            Move_OutControl();
        }

        else
        {
            Move_InControl();
        }
    }

    private void Move_OutControl()
    {
        transform.position = Vector3.MoveTowards(transform.localPosition, targetTransform.position, Time.deltaTime * 30f);
    }
    private void Move_InControl()
    {
        transform.Translate(Vector3.forward * 0.3f, Space.Self);

        if (Vector3.Distance(transform.position, towerAttack.bulletPosition.position) > towerAttack.towerBase.distance)
        {
            Despawn();
        }
    }

    public override void Despawn()
    {
        base.Despawn();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (targetTransform == null)
            {
                collision.gameObject.GetComponent<Eneminyoung>().Damaged(towerAttack.towerBase.attackPower);
            }
            else if (targetTransform?.gameObject == collision.gameObject)
            {
                towerAttack.targetEnemy.Damaged(towerAttack.towerBase.attackPower);
            }

            Despawn();
        }
    }
}
