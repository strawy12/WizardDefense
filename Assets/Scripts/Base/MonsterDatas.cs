using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterInfo
{
    public float maxHp;
    public float defense;
    public int attackPower;

    public MonsterInfo(float maxHp, float defense, int attackPower)
    {
        this.maxHp = maxHp;
        this.defense = defense;
        this.attackPower = attackPower;
    }
}

[System.Serializable]
public class MonsterBase
{
    public string monsterName;
    public string monsterId;
    public PropertyType monsterType;
    public MonsterInfo  info;
    public ItemBase dropItem;


    public MonsterBase(string id,string name, PropertyType type, ItemBase item)
    {
        monsterName = name;
        monsterId = id;
        monsterType = type;
        dropItem = item;
        info = null;
    }

    public MonsterBase(MonsterBase baseData, MonsterInfo info)
    {
        monsterName = baseData.monsterName;
        monsterId = baseData.monsterId;
        monsterType = baseData.monsterType;
        dropItem = baseData.dropItem;
        this.info = info;
    }
}

[CreateAssetMenu(fileName = "MonsterDatas", menuName = "Scriptable Object/MonsterDatas")]
public class MonsterDatas : ScriptableObject
{
    public string updateVersion;
    public List<MonsterBase> monsterDatas;
}
