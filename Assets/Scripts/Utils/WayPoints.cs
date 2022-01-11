using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    private Transform[] wayPoints;

    private void Awake()
    {
        wayPoints = GetComponentsInChildren<Transform>(transform);
    }

    public T[] GetComponentsInChildren<T>(Transform transform)
    {

        List<T> list = new List<T>();
        T[] array = transform.GetComponentsInChildren<T>();

        list.AddRange(array);
        list.RemoveAt(0);

        return list.ToArray();
    }

    public Transform GetWayPoint(int index)
    {
        if(index < wayPoints.Length)
        {
            return wayPoints[index];
        }

        else
        {
            return null;
        }
    }
}
