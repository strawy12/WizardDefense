using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : PoolObject
{
    private TowerAttack towerAttack;
    private Enemy targetEnemy;
    private TowerState state;

    private SphereCollider col;
    private float originRad;
    private TrailRenderer trail;

    [SerializeField] private float speed;

    protected override void Awake()
    {
        col = GetComponent<SphereCollider>();
        trail = GetComponent<TrailRenderer>();
        originRad = col.radius;
        base.Awake();
    }

    private void Update()
    {
        if (state == TowerState.OutControl)
        {
            //if (targetEenemy == null)
            //{
            //    towerAttack.SetTargetEnemy();
            //    if (targetEenemy == null) return;
            //}

            Move_OutControl();
        }

        else
        {
            Move_InControl();
        }

        transform.Translate(Vector3.forward * speed, Space.Self);
    }

    #region SetData
    public void Init(TowerAttack towerAttack)
    {
        this.towerAttack = towerAttack;
        state = towerAttack.GetTowerState();

        if (state == TowerState.OutControl)
        {
            targetEnemy = towerAttack.GetTargetEnemy();
            col.radius = originRad;
        }

        else
        {
            col.radius = originRad * 2.5f;
        }

        trail.enabled = true;
    }
    #endregion

    #region Fire
    private void Move_OutControl()
    {
        transform.LookAt(targetEnemy.transform);
    }

    private void Move_InControl()
    {
        if (Vector3.Distance(transform.position, towerAttack.muzzlePosition.position) > towerAttack.towerBase.distance)
        {
            Despawn();
        }
    }
    #endregion

    #region Collide
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            CollideEnemy(collision);
        }
    }

    private void CollideEnemy(Collision collision)
    {
        if (targetEnemy == null)
        {
            collision.gameObject.GetComponent<Enemy>().Damaged(towerAttack.towerBase.attackPower);
        }

        else if (targetEnemy?.gameObject == collision.gameObject)
        {
            targetEnemy?.Damaged(towerAttack.towerBase.attackPower);
        }

        Despawn();
    }
    #endregion

    public override void Despawn()
    {
        trail.enabled = false;
        base.Despawn();
    }
}
