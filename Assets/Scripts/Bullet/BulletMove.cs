using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : PoolObject
{
    TowerAttack towerAttack;
    Transform targetTransform;
    Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    public void Init(TowerAttack towerAttack)
    {
        this.towerAttack = towerAttack;
        targetTransform = towerAttack.targetEnemey.transform;
    }

    private void Update()
    {
        //transform.Translate(targetTransform.position * Time.deltaTime * 3f);
        transform.position = Vector3.MoveTowards(transform.localPosition, targetTransform.position, Time.deltaTime * 30f);
        //transform.position = Vector3.LerpUnclamped(transform.position, target.position, Time.deltaTime * 5f);
        //transform.position = Vector3.SlerpUnclamped(transform.position, target.position, Time.deltaTime * 5f);
        //rigid.velocity = 10 * (target.position - transform.position).normalized;
    }

    public override void Despawn()
    {
        base.Despawn();
    }


}
