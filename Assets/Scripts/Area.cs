using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] protected Transform target;
    [SerializeField] protected float distance = 4f;

    private bool isArea;

    protected virtual void Start()    
    {
        rb = GetComponent<Rigidbody>();
        //target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        target = GameManager.Instance.player.transform;
    }

    private void Update()
    {
        if(GameManager.Instance.inGameState == InGameState.BreakTime)
        {
            FollowTarget();
        }
    }

    protected virtual void FollowTarget()
    {
        if (Vector3.Distance(transform.position, target.position) < distance + 2)
        {
            if (Vector3.Distance(transform.position, target.position) < distance)
            {
                TowerSelect.buildObj = gameObject;
                TowerSelect.buildTrn = gameObject.transform;
                GameManager.Instance.UIManager.FMarkTrue();
            }
            else
            {
                GameManager.Instance.UIManager.FMarkFalse();
            }
        }
    }
}
