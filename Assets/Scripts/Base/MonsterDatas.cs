using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterInfo
{
    public int maxHp;
    public int defense;

    public MonsterInfo(int maxHp, int defense)
    {
        this.maxHp = maxHp;
        this.defense = defense;
    }
}

[System.Serializable]
public class MonsterBase
{
    public string monsterName;
    public string monsterId;
    public PropertyType monsterType;
    public MonsterInfo data;
    public List<ItemBase> dropItemList;


    public MonsterBase(string id,string name, PropertyType type)
    {
        monsterName = name;
        monsterId = id;
        this.monsterType = type;
        data = null;
    }
}

[CreateAssetMenu(fileName = "MonsterDatas", menuName = "Scriptable Object/MonsterDatas")]
public class MonsterDatas : ScriptableObject
{
    public string updateVersion;
    public List<MonsterBase> monsterDatas;
}
