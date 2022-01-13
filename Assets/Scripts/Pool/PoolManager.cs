using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ObjectPoolData
{
    public PoolObject prefab = null;

    public int prefabCreateCount = 0;
}

public class PoolManager : MonoBehaviour
{
    private Dictionary<EPoolingType, Queue<PoolObject>> dictPoolList;

    [SerializeField] List<ObjectPoolData> objectPoolData = null;

    private void Awake()
    {
        dictPoolList = new Dictionary<EPoolingType, Queue<PoolObject>>();
        Init();
    }


    private void Init()
    {
        int dictCount = objectPoolData.Count;
        int objectCount = 0;
        EPoolingType type;
        PoolObject poolObject = null;

        for (int i = 0; i < dictCount; i++)
        {
            type = (EPoolingType)i;
            objectCount = objectPoolData[i].prefabCreateCount;

            dictPoolList.Add(type, new Queue<PoolObject>());

            for (int j = 0; j < objectCount; j++)
            {
                poolObject = GerenationPoolObject(type);
                dictPoolList[type].Enqueue(poolObject);
            }
        }
    }

    private PoolObject GerenationPoolObject(EPoolingType type)
    {
        int index = (int)type;
        PoolObject poolObject = Instantiate(objectPoolData[index].prefab, transform);
        poolObject.gameObject.SetActive(false);

        return poolObject;
    }

    public PoolObject GetPoolObject(EPoolingType type)
    {
        if (dictPoolList[type].Count != 0)
        {
            return dictPoolList[type].Dequeue();
        }

        else
        {
            return GerenationPoolObject(type);
        }
    }
    public void EnQuquePoolObject(PoolObject poolObject)
    {
        EPoolingType type = poolObject.PoolType;
        dictPoolList[type].Enqueue(poolObject);
    }
}
