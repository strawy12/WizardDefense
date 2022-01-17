using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnMonsterInfo
{
    public string monsterId;
    public int spawnCount;
    public MonsterInfo monsterData;

    public SpawnMonsterInfo(string id, int spawnCount, int maxHp, int defense)
    {
        monsterId = id;
        this.spawnCount = spawnCount;
        monsterData = new MonsterInfo(maxHp, defense);
    }
}

[System.Serializable]
public class PatternData
{
    [Header("Pattern Setting")]
    public string ID;
    public int cost;
    public float nextPatternDelay;

    [Header("Monster Setting")]
    public DirectionType direction;
    public float monsterSpawnDelay;

    public List<SpawnMonsterInfo> monsterInfoList;

    public int spawnMonsterCnt { get { return monsterInfoList.Count; } }

    public PatternData(string ID, int cost, float nextPatternDelay, float monsterSpawnDelay, DirectionType direction, List<SpawnMonsterInfo> monsterInfoList)
    {
        this.cost = cost;
        this.direction = direction;
        this.monsterInfoList = monsterInfoList;
        this.nextPatternDelay = nextPatternDelay;
        this.monsterSpawnDelay = monsterSpawnDelay;
        this.monsterInfoList = monsterInfoList;

    }

}

[CreateAssetMenu(fileName = "Patterns", menuName = "Scriptable Object/Patterns")]
public class Patterns : ScriptableObject
{
    public string updateVersion;
    public List<PatternData> patterns;
}
