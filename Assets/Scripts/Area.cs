using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] protected Transform target;
    [SerializeField] protected float distance = 4f;

    private bool isArea;

    Outline outline;

    protected virtual void Start()    
    {
        rb = GetComponent<Rigidbody>();
        //target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        target = GameManager.Instance.player.transform;
        outline = GetComponent<Outline>();
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
                ShowOutline(true);
            }
            else
            {
                GameManager.Instance.UIManager.FMarkFalse();
                ShowOutline(false);
            }
        }
    }

    public void ShowOutline(bool isShow)
    {
        if (outline == null) return;

        if(isShow)
        {
            outline.OutlineWidth = outline.thisOutLine;
        }
        else
        {
            outline.OutlineWidth = 0f;
        }
    }
}
