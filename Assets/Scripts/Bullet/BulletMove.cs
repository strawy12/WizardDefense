using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : PoolObject
{
    private TowerAttack towerAttack;
    private MonsterMove targetEnemy;
    private TowerState state;

    private SphereCollider col;
    private float originRad;

    [SerializeField] private float speed;

    protected override void Awake()
    {
        col = GetComponent<SphereCollider>();
        originRad = col.radius;
        base.Awake();
    }

    private void Update()
    {
        if (state == TowerState.OutControl)
        {
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
    }
    #endregion

    #region Fire
    private void Move_OutControl()
    {
        if(targetEnemy != null)
        {
            transform.LookAt(targetEnemy.transform);
        }
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
    #endregion

    public override void Despawn()
    {
        base.Despawn();
    }
}
