using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InGameDataManager : MonoBehaviour
{
    [SerializeField] private MonsterDatas monsterDatas;
    [SerializeField] private Patterns patterns;
    [SerializeField] private MonsterPrefabDatas monsterPrefabs;

    private void Awake()
    {
        DownLoadInGameData();
    }

    public void DownLoadInGameData()
    {
        const string SAVE_PATH = "ScriptableObject/";
        if (monsterDatas == null)
        {
            monsterDatas = Resources.Load<MonsterDatas>(SAVE_PATH + "MonsterDatas");
        }

        if (patterns == null)
        {
            patterns = Resources.Load<Patterns>(SAVE_PATH + "Patterns");
        }

        if (monsterPrefabs == null)
        {
            monsterPrefabs = Resources.Load<MonsterPrefabDatas>(SAVE_PATH + "MonsterPrefabDatas");
        }

        StartCoroutine(DataDownLoad());
    }

    private IEnumerator DataDownLoad()
    {
        yield return StartMonsterDataDownLoad();
        yield return StartWavePatternDataDownLoad();

<<<<<<< HEAD
        // StartCoroutine(GameManager.Instance.Wave.StartWave());
=======
       StartCoroutine(GameManager.Instance.Wave.StartWave());
>>>>>>> OIF
    }


    private IEnumerator StartMonsterDataDownLoad()
    {
        const string UPDATEURL = "https://docs.google.com/spreadsheets/d/1RbsXVREigxOpq7ozlbjoLSYzNgFlV6enstlbATOXQ-4/export?format=tsv&gid=0&range=A1";
        UnityWebRequest www = UnityWebRequest.Get(UPDATEURL);
        yield return www.SendWebRequest();
        if (www.downloadHandler.text == monsterDatas.updateVersion/* && !www.downloadHandler.text.Contains("T")*/)
        {
            yield break;
        }

        monsterDatas.updateVersion = www.downloadHandler.text;
        yield return DownLoadMonsterDatas();
    }

    private IEnumerator StartWavePatternDataDownLoad()
    {
        const string UPDATEURL = "https://docs.google.com/spreadsheets/d/1RbsXVREigxOpq7ozlbjoLSYzNgFlV6enstlbATOXQ-4/export?format=tsv&gid=706645150&range=A1";
        UnityWebRequest www = UnityWebRequest.Get(UPDATEURL);
        yield return www.SendWebRequest();
<<<<<<< HEAD
        if (www.downloadHandler.text == patterns.updateVersion /*&& !www.downloadHandler.text.Contains("T")*/)
=======
        if (www.downloadHandler.text == patterns.updateVersion/* && !www.downloadHandler.text.Contains("T")*/)
>>>>>>> OIF
        {
            yield break;
        }

        patterns.updateVersion = www.downloadHandler.text;
        yield return DownLoadWavePatternData();
    }

    private IEnumerator DownLoadMonsterDatas()
    {
        const string URL = "https://docs.google.com/spreadsheets/d/1RbsXVREigxOpq7ozlbjoLSYzNgFlV6enstlbATOXQ-4/export?format=tsv&gid=0&range=B2:D";

        UnityWebRequest www = UnityWebRequest.Get(URL);

        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;

        SetMonsterDatas(data);
    }

    private IEnumerator DownLoadWavePatternData()
    {
        const string URL = "https://docs.google.com/spreadsheets/d/1RbsXVREigxOpq7ozlbjoLSYzNgFlV6enstlbATOXQ-4/export?format=tsv&gid=706645150&range=B2:H";

        UnityWebRequest www = UnityWebRequest.Get(URL);

        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;

        SetWavePatternDatas(data);
    }


    private void SetMonsterDatas(string data)
    {
        string[] row = data.Split('\n');
        string[] column;
        int rowSize = row.Length;
        int colummSize = row[0].Split('\t').Length;
        MonsterBase monsterBase = null;

        for (int i = 0; i < rowSize; i++)
        {
            column = row[i].Split('\t');
            for (int j = 0; j < colummSize; j++)
            {
                if (i >= monsterDatas.monsterDatas.Count)
                {
                    monsterDatas.monsterDatas.Add(new MonsterBase(column[0], column[1], (PropertyType)Enum.Parse(typeof(PropertyType), column[2])));
                }

                else
                {
                    monsterBase = monsterDatas.monsterDatas[i];
                    monsterBase.monsterId = column[0];
                    monsterBase.monsterName = column[1];
                    monsterBase.monsterType = (PropertyType)Enum.Parse(typeof(PropertyType), column[2]);
                }
            }
        }
    }

    private void SetWavePatternDatas(string data)
    {
        string[] row = data.Split('\n');
        string[] column;
        int rowSize = row.Length;
        int colummSize = row[0].Split('\t').Length;
        PatternData patternData = null;

        string ID;
        int cost;
        float nextPatternDelay;
        float monsterSpawnDelay;
        DirectionType direction;
        List<SpawnMonsterInfo> monsterInfoList;

        for (int i = 0; i < rowSize; i++)
        {
            column = row[i].Split('\t');
            for (int j = 0; j < colummSize; j++)
            {
                ID = column[0];
                cost = int.Parse(column[2]);
                nextPatternDelay = float.Parse(column[5]);
                monsterSpawnDelay = float.Parse(column[3]);
                direction = (DirectionType)Enum.Parse(typeof(DirectionType), column[6]);
                monsterInfoList = ConversionSpawnMonsterInfoList(column[1], column[4]);

                if (i >= patterns.patterns.Count)
                {
                    patterns.patterns.Add(new PatternData(ID, cost, nextPatternDelay, monsterSpawnDelay, direction, monsterInfoList));
                }

                else
                {
                    patternData = patterns.patterns[i];
                    patternData.ID = ID;
                    patternData.cost = cost;
                    patternData.direction = direction;
                    patternData.monsterSpawnDelay = monsterSpawnDelay;
                    patternData.nextPatternDelay = nextPatternDelay;
                    patternData.monsterInfoList = monsterInfoList;
                }
            }
        }
    }

    private List<SpawnMonsterInfo> ConversionSpawnMonsterInfoList(string monsterIds, string monsterInfos)
    {
        List<SpawnMonsterInfo> monsterInfoList = new List<SpawnMonsterInfo>();
        string[] monsterIdArr = monsterIds.Split(',');
        string[] monsterInfoArr = monsterInfos.Split(',');
        string[] infos;

        string id = "";
        int spawnCnt = 0;
        int hp = 0;
        int defense = 0;
        int attackPower = 0;

        for (int i = 0; i < monsterInfoArr.Length; i++)
        {
            infos = monsterInfoArr[i].Split('_');

            id = monsterIdArr[int.Parse(infos[0]) - 1];
            spawnCnt = int.Parse(infos[1]);
            hp = int.Parse(infos[2]);
            defense = int.Parse(infos[3]);
            attackPower = int.Parse(infos[4]);
            monsterInfoList.Add(new SpawnMonsterInfo(id, spawnCnt, hp, defense, attackPower));
        }

        return monsterInfoList;
    }

    public MonsterBase Find_SetMonsterBase(string monster_ID, MonsterInfo info)
    {
        MonsterBase monsterBase = monsterDatas.monsterDatas.Find((monster) => monster.monsterId.Equals(monster_ID));

        return new MonsterBase(monsterBase, info);
    }

    public MonsterMove FindMonsterPrefab(string monster_ID)
    {
        MonsterMove monsterPref = monsterPrefabs.monsterPrefabDatas.Find((prefab) => prefab.monster_ID.Equals(monster_ID)).monster_Prefab;

        return monsterPref;
    }

    public PatternData GetPatternData(int index)
    {
        return patterns.patterns[index];
    }

}
