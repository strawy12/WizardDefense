using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainWayPoints : WayPoints
{
    [SerializeField] private Transform mainWayPointObject;
    private Transform[] mainWayPoints;
    [SerializeField] private Transform[] topWayPointOrder;
    [SerializeField] private Transform[] leftWayPointOrder;
    [SerializeField] private Transform[] rightWayPointOrder;

    private void Awake()
    {
        mainWayPoints = GetComponentsInChildren<Transform>(mainWayPointObject);
    }

    public override Transform GetWayPoint(int index)
    {
        if (index < mainWayPoints.Length)
        {
            return mainWayPoints[index];
        }

        else
        {
            return null;
        }
    }

    public Transform GetTopWayPoint(int index)
    {
        if(index < topWayPointOrder.Length)
        {
            return topWayPointOrder[index];
        }

        else
        {
            int reLoadIndex = index - topWayPointOrder.Length;
            return GetWayPoint(reLoadIndex);
        }
    }

    public Transform GetLeftWayPoint(int index)
    {
        if (index < leftWayPointOrder.Length)
        {
            return leftWayPointOrder[index];
        }

        else
        {
            int reLoadIndex = index - leftWayPointOrder.Length;
            return GetWayPoint(reLoadIndex);
        }
    }

    public Transform GetRightWayPoint(int index)
    {
        if (index < rightWayPointOrder.Length)
        {
            return rightWayPointOrder[index];
        }

        else
        {
            int reLoadIndex = index - rightWayPointOrder.Length;
            return GetWayPoint(reLoadIndex);
        }
    }
}
