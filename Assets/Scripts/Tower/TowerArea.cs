using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerArea : Area
{
    private TowerAttack tower;

    protected override void Start()
    {
        base.Start();
        tower = GetComponent<TowerAttack>();
    }

    private void Update()
    {
        FollowTarget();
    }

    protected override void FollowTarget()
    {
        if (Mathf.Abs(transform.position.x - target.position.x) < distance + 2)
        {
            if (Mathf.Abs(transform.position.x - target.position.x) < distance)
            {
                GameManager.Instance.UIManager.FMarkTrue();
                GameManager.Instance.censorTower = tower;
                tower.ShowOutLine(true);
            }
            else
            {
                GameManager.Instance.UIManager.FMarkFalse();
                GameManager.Instance.censorTower = null;
                tower.ShowOutLine(false);
            }
        }
    }

}
