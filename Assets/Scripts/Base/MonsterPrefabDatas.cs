using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterPrefab
{
    public string monster_ID;
    public MonsterMove monster_Prefab;
}

[CreateAssetMenu(fileName = "MonsterPrefabDatas", menuName = "Scriptable Object/MonsterPrefabDatas")]
public class MonsterPrefabDatas : ScriptableObject
{
    public List<MonsterPrefab> monsterPrefabDatas;

    public MonsterMove FindMonsterPrefab(string id)
    {
        return monsterPrefabDatas.Find((monster) => monster.monster_ID == id).monster_Prefab;
    }
} 
