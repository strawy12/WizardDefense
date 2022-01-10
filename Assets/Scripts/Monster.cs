using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterBase monsterData;
    private int currentHp = 0;
    private float currentTime = 0f;
    private float waitingTime = 0f;

    private Vector3 currentDir = Vector3.zero;
    private void Awake()
    {

    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= waitingTime)
        {
            ChangeDir();
            currentTime = 0f;
        }

        DoMove();
    }

    private void Init()
    {
        currentHp = monsterData.maxHp;
        waitingTime = Random.Range(3f, 7f);
        currentTime = waitingTime;
        ChangeDir();
    }

    private void DoMove()
    {
        transform.Translate(currentDir * monsterData.speed * Time.deltaTime);
    }

    private void ChangeDir()
    {
        currentDir = Utils.GetRandomDir();
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
