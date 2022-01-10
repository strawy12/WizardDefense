using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : PoolObject
{
    TowerAttack towerAttack;
    Transform targetTransform;

    public void Init(TowerAttack towerAttack)
    {
        this.towerAttack = towerAttack;
        targetTransform = towerAttack.targetEnemey.transform;
    }

    private void Update()
    {
        if (targetTransform == null)
        {
            towerAttack.SetTargetEnemy();
            if (towerAttack.targetEnemey == null)
            {
                Despawn();
                return;
            }
            targetTransform = towerAttack.targetEnemey.transform;
        }


        if (towerAttack.towerState == TowerState.OutControl)
        {
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

        //transform.Translate(targetTransform.position * Time.deltaTime * 3f);
        //transform.position = Vector3.LerpUnclamped(transform.position, target.position, Time.deltaTime * 5f);
        //transform.position = Vector3.SlerpUnclamped(transform.position, target.position, Time.deltaTime * 5f);
        //rigid.velocity = 10 * (target.position - transform.position).normalized;
    }
    private void Move_InControl()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        //transform.position = Vector3.MoveTowards(ray.origin, ray.direction * 100f, Time.deltaTime * 30f);
        transform.Translate(ray.direction * Time.deltaTime * 10f);
    }

    public override void Despawn()
    {
        base.Despawn();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (targetTransform.gameObject == collision.gameObject)
            {
                towerAttack.targetEnemey.Damaged(towerAttack.towerBase.attackPower);
            }

            Despawn();
        }
    }
}
