using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterBase monsterData;
    
    private int currentHp = 0;
    private int wayPointCnt = 0;
    private float rotateSpeed = 0f;
    private float currentTime = 0f;
    private float waitingTime = 0f;

    private Vector3 currentDir = Vector3.zero;
    private Transform targetWayPoint;

    private void Awake()
    {

    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        DoMove();
    }

    private void Init()
    {
        wayPointCnt = 0;
        rotateSpeed = 10f;
        waitingTime = 0f;
        currentTime = 0f;
        currentHp = monsterData.maxHp;
        currentTime = waitingTime;
        targetWayPoint = transform;
        ChangeDir();
    }

    private void ChangeDir()
    {
        Transform beforeTarget = targetWayPoint;
        targetWayPoint = GameManager.Inst.WayPoints.GetWayPoint(wayPointCnt++);
        currentDir = (targetWayPoint.position - beforeTarget.position).normalized;
        currentDir.y = 0f;

        transform.DORotateQuaternion(Quaternion.LookRotation(currentDir), 0.5f);
    }

    private bool CheckWayPointDistance()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        float distance = Vector3.Distance(targetWayPoint.position, position);

        if(distance <= 0.35f)
        {
            return true;
        }

        return false;
    }

    private void DoMove()
    {
        if(CheckWayPointDistance())
        {
            ChangeDir();
            return;
        }

        transform.Translate(Vector3.forward * monsterData.speed * Time.deltaTime);
    }

    public void Damaged(int damage)
    {
        currentHp -= damage;
        if(currentHp <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        DropItem();
    }

    private void Attack()
    {
        
    }

    private void DropItem()
    {
        ItemBase itemData = PickItem();

        GameManager.Inst.EqualItem(itemData.itemName);
    }

    private ItemBase PickItem()
    {
        int ranNum = Random.Range(0, 100);
        int currentNum = 0;

         
        foreach (ItemBase item in monsterData.dropItemList)
        {
            currentNum += item.percent;
            
            if (ranNum < currentNum)
            {
                return item;
            }
        }

        return null;
    }
}
