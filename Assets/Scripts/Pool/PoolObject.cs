using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [SerializeField] private EPoolingType ePoolingType;

    private PoolManager poolManager = null;
    public EPoolingType PoolType { get { return ePoolingType; } }

    protected virtual void Awake()
    {
        poolManager = FindObjectOfType<PoolManager>();
    }

    virtual public void Despawn()
    {
        gameObject.SetActive(false);
        transform.SetParent(poolManager.transform);
        poolManager.EnQuquePoolObject(this);
    }
}
