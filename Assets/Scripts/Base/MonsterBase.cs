using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterBase
{
    public string monsterName;
    public PropertyType monsterType;
    public int maxHp;
    public int damage;
    public float speed;
    public List<ItemBase> dropItemList;

    public MonsterBase(string name, PropertyType type, int maxHp, float speed)
    {
        monsterName = name;
        monsterType = type;
        this.maxHp = maxHp;
        this.speed = speed;
    }
}
