using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 4f;

    private void Start()    
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {

        if (Vector3.Distance(transform.position, target.position) < distance)
        {
            TowerSelect.buildObj = gameObject;
            TowerSelect.buildTrn = gameObject.transform;
            GameManager.Instance.UIManager.FMarkTrue();
        }
    }
}
