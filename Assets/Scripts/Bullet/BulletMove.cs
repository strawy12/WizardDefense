using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : PoolObject
{
    TowerAttack towerAttack;
    Enemy targetEenemy;
    TowerState state;
    SphereCollider col;
    float originRad;

    protected override void Awake()
    {
        col = GetComponent<SphereCollider>();
        originRad = col.radius;
        base.Awake();
    }
    public void Init(TowerAttack towerAttack)
    {
        this.towerAttack = towerAttack;
        state = towerAttack.towerState;

        if (state == TowerState.OutControl)
        {
            targetEenemy = towerAttack.targetEnemy;
            col.radius = originRad;
        }
        else
        {
            col.radius = originRad * 2.5f;
        }
    }

    private void Update()
    {
        if (state == TowerState.OutControl)
        {
            if (targetEenemy == null)
            {
                towerAttack.SetTargetEnemy();
                if (towerAttack.targetEnemy == null)
                {
                    Despawn();
                    return;
                }
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
        transform.position = Vector3.MoveTowards(transform.localPosition, targetEenemy.transform.position, Time.deltaTime * 30f);
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
            if (targetEenemy == null)
            {
                collision.gameObject.GetComponent<Enemy>().Damaged(towerAttack.towerBase.attackPower);
            }
            else if (targetEenemy?.gameObject == collision.gameObject)
            {
                towerAttack.targetEnemy?.Damaged(towerAttack.towerBase.attackPower);
            }

            Despawn();
        }
    }
}
