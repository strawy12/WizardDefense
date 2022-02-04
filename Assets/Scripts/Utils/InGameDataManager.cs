using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class InGameDataManager : MonoBehaviour
{
    [SerializeField] private MonsterDatas monsterDatas;
    [SerializeField] private Patterns patterns;
    [SerializeField] private Waves waveDatas;
    [SerializeField] private ItemDatas itemDatas;

    [SerializeField] private MonsterPrefabDatas monsterPrefabs;
    [SerializeField] private ItemSpriteDatas itemSprites;
    [SerializeField] private TowerRoots towerRoots;

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

        if(itemDatas == null)
        {
            itemDatas = Resources.Load<ItemDatas>(SAVE_PATH + "ItemDatas");
        }

        if (monsterPrefabs == null)
        {
            monsterPrefabs = Resources.Load<MonsterPrefabDatas>(SAVE_PATH + "MonsterPrefabDatas");
        }

        if(itemSprites == null)
        {
            itemSprites = Resources.Load<ItemSpriteDatas>(SAVE_PATH + "ItemSpriteDatas");
        }

        if (waveDatas == null)
        {
            waveDatas = Resources.Load<Waves>(SAVE_PATH + "Waves");
        }

        StartCoroutine(DataDownLoad());
    }

    private IEnumerator DataDownLoad()
    {
        yield return StartItemDataDownLoad();
        yield return StartMonsterDataDownLoad();
        yield return StartWavePatternDataDownLoad();
        yield return StartWaveDataDownLoad();
    }

    #region Monster Data

    private IEnumerator StartMonsterDataDownLoad()
    {
        const string UPDATEURL = "https://docs.google.com/spreadsheets/d/1RbsXVREigxOpq7ozlbjoLSYzNgFlV6enstlbATOXQ-4/export?format=tsv&gid=0&range=A1";
        UnityWebRequest www = UnityWebRequest.Get(UPDATEURL);
        yield return www.SendWebRequest();
        if (www.downloadHandler.text == monsterDatas.updateVersion && !www.downloadHandler.text.Contains("T"))
        {
            yield break;
        }

        monsterDatas.updateVersion = www.downloadHandler.text;
        yield return DownLoadMonsterDatas();
    }
    private IEnumerator DownLoadMonsterDatas()
    {
        const string URL = "https://docs.google.com/spreadsheets/d/1RbsXVREigxOpq7ozlbjoLSYzNgFlV6enstlbATOXQ-4/export?format=tsv&gid=0&range=B2:E19";

        UnityWebRequest www = UnityWebRequest.Get(URL);

        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;

        SetMonsterDatas(data);
    }
    private void SetMonsterDatas(string data)
    {
        string[] row = data.Split('\n');
        string[] column;
        int rowSize = row.Length;
        int colummSize = row[0].Split('\t').Length;
        MonsterBase monsterBase = null;

        string id = "";
        string name = "";
        string itemId = "";
        PropertyType type;
        ItemBase item = null;

        for (int i = 0; i < rowSize; i++)
        {
            column = row[i].Split('\t');
            for (int j = 0; j < colummSize; j++)
            {
                id = column[0];
                name = column[1];
                type = (PropertyType)Enum.Parse(typeof(PropertyType), column[2]);
                column[3] = Regex.Replace(column[3], "[^a-zA-Z_]", "");
                item = FindItemBase(column[3]);

                if (i >= monsterDatas.monsterDatas.Count)
                {

                    monsterDatas.monsterDatas.Add(new MonsterBase(id, name, type, item));
                }

                else
                {
                    monsterBase = monsterDatas.monsterDatas[i];
                    monsterBase.monsterId = id;
                    monsterBase.monsterName = name;
                    monsterBase.monsterType = type;
                    monsterBase.dropItem = item;
                }
            }
        }
    }

    #endregion

    #region WavePattern Data

    private IEnumerator StartWavePatternDataDownLoad()
    {
        const string UPDATEURL = "https://docs.google.com/spreadsheets/d/1RbsXVREigxOpq7ozlbjoLSYzNgFlV6enstlbATOXQ-4/export?format=tsv&gid=706645150&range=A1";
        UnityWebRequest www = UnityWebRequest.Get(UPDATEURL);
        yield return www.SendWebRequest();
        if (www.downloadHandler.text == patterns.updateVersion && !www.downloadHandler.text.Contains("T"))
        {
            yield break;
        }

        patterns.updateVersion = www.downloadHandler.text;
        yield return DownLoadWavePatternData();
    }

    private IEnumerator DownLoadWavePatternData()
    {
        const string URL = "https://docs.google.com/spreadsheets/d/1RbsXVREigxOpq7ozlbjoLSYzNgFlV6enstlbATOXQ-4/export?format=tsv&gid=706645150&range=B2:H";

        UnityWebRequest www = UnityWebRequest.Get(URL);

        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;

        SetWavePatternDatas(data);
    }

    private void SetWavePatternDatas(string data)
    {
        string[] row = data.Split('\n');
        string[] column;
        int rowSize = row.Length;
        int colummSize = row[0].Split('\t').Length;
        PatternData patternData = null;

        string ID;
        bool doReUse;
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
                doReUse = column[6].Equals("TRUE");
                nextPatternDelay = float.Parse(column[4]);
                monsterSpawnDelay = float.Parse(column[2]);
                direction = (DirectionType)Enum.Parse(typeof(DirectionType), column[5]);
                monsterInfoList = ConversionSpawnMonsterInfoList(column[1], column[3]);

                if (i >= patterns.patterns.Count)
                {
                    patterns.patterns.Add(new PatternData(ID, doReUse, nextPatternDelay, monsterSpawnDelay, direction, monsterInfoList));
                }

                else
                {
                    patternData = patterns.patterns[i];
                    patternData.ID = ID;
                    patternData.doReUse = doReUse;
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
        float hp = 0;
        float defense = 0;
        int attackPower = 0;

        for (int i = 0; i < monsterInfoArr.Length; i++)
        {
            infos = monsterInfoArr[i].Split('_');

            id = monsterIdArr[int.Parse(infos[0]) - 1];
            spawnCnt = int.Parse(infos[1]);
            hp = float.Parse(infos[2]);
            defense = float.Parse(infos[3]);
            attackPower = int.Parse(infos[4]);
            monsterInfoList.Add(new SpawnMonsterInfo(id, spawnCnt, hp, defense, attackPower));
        }

        return monsterInfoList;
    }
    #endregion

    #region WaveData

    private IEnumerator StartWaveDataDownLoad()
    {
        const string UPDATEURL = "https://docs.google.com/spreadsheets/d/1RbsXVREigxOpq7ozlbjoLSYzNgFlV6enstlbATOXQ-4/export?format=tsv&gid=1596492275&range=A1";
        UnityWebRequest www = UnityWebRequest.Get(UPDATEURL);
        yield return www.SendWebRequest();
        if (www.downloadHandler.text == waveDatas.updateVersion && !www.downloadHandler.text.Contains("T"))
        {
            yield break;
        }

        waveDatas.updateVersion = www.downloadHandler.text;

        yield return DownLoadWaveDatas();
    }
    private IEnumerator DownLoadWaveDatas()
    {
        const string URL = "https://docs.google.com/spreadsheets/d/1RbsXVREigxOpq7ozlbjoLSYzNgFlV6enstlbATOXQ-4/export?format=tsv&gid=1596492275&range=B2:F";

        UnityWebRequest www = UnityWebRequest.Get(URL);

        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;

        SetWaveDatas(data);
    }
    private void SetWaveDatas(string data)
    {
        string[] row = data.Split('\n');
        string[] column;
        int rowSize = row.Length;
        int colummSize = row[0].Split('\t').Length;

        WaveData waveData = null;

        int waveIndex = 0;
        int executionCnt = 0;
        List<string> usePatternIDs = new List<string>();
        bool existBoss = false;
        string bossID = "";


        for (int i = 0; i < rowSize; i++)
        {
            column = row[i].Split('\t');
            for (int j = 0; j < colummSize; j++)
            {
                waveIndex = int.Parse(column[0]);
                usePatternIDs = ConversionUsePatternList(column[1]);
                executionCnt = int.Parse(column[2]);
                existBoss = column[3].Equals("TRUE");

                if(existBoss)
                {
                    bossID = column[4];
                }

                if (i >= waveDatas.waves.Count)
                {
                    if (existBoss)
                    {
                        waveDatas.waves.Add(new WaveData(waveIndex, usePatternIDs, executionCnt, bossID));
                    }

                    else
                    {
                        waveDatas.waves.Add(new WaveData(waveIndex, usePatternIDs, executionCnt));
                    }
                }

                else
                {
                    waveData = waveDatas.waves[i];

                    waveData.SetDefaultData(waveIndex, usePatternIDs, executionCnt);

                    if (existBoss)
                    {
                        waveData.existBoss = true;
                        waveData.bossID = bossID;
                    }
                }
            }
        }
    }

    private List<string> ConversionUsePatternList(string patternIds)
    {
        List<string> usePatternList = new List<string>();
        string[] usePatternIDs = patternIds.Split(',');

        for (int i = 0; i < usePatternIDs.Length; i++)
        {
            usePatternList.Add(usePatternIDs[i]);
        }

        return usePatternList;
    }

    #endregion

    #region Item Data

    private IEnumerator StartItemDataDownLoad()
    {
        const string UPDATEURL = "https://docs.google.com/spreadsheets/d/1RbsXVREigxOpq7ozlbjoLSYzNgFlV6enstlbATOXQ-4/export?format=tsv&gid=1191904830&range=A1";
        UnityWebRequest www = UnityWebRequest.Get(UPDATEURL);
        yield return www.SendWebRequest();
        if (www.downloadHandler.text == itemDatas.updateVersion && !www.downloadHandler.text.Contains("T"))
        {
            yield break;
        }

        itemDatas.updateVersion = www.downloadHandler.text;
        yield return DownLoadItemDatas();
    }
    private IEnumerator DownLoadItemDatas()
    {
        const string URL = "https://docs.google.com/spreadsheets/d/1RbsXVREigxOpq7ozlbjoLSYzNgFlV6enstlbATOXQ-4/export?format=tsv&gid=1191904830&range=B2:D19";

        UnityWebRequest www = UnityWebRequest.Get(URL);

        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;

        SetItemDatas(data);
    }
    private void SetItemDatas(string data)
    {
        string[] row = data.Split('\n');
        string[] column;
        int rowSize = row.Length;
        int colummSize = row[0].Split('\t').Length;
        ItemData itemData = null;

        for (int i = 0; i < rowSize; i++)
        {
            column = row[i].Split('\t');
            for (int j = 0; j < colummSize; j++)
            {
                if (i >= itemDatas.itemDataList.Count)
                {
                    itemDatas.itemDataList.Add(new ItemData(column[0], column[1], (PropertyType)Enum.Parse(typeof(PropertyType), column[2])));
                }

                else
                {
                    itemData = itemDatas.itemDataList[i];
                    itemData.item_ID = column[0];
                    itemData.itemName = column[1];
                    itemData.itemType = (PropertyType)Enum.Parse(typeof(PropertyType), column[2]);
                }
            }
        }
    }

    #endregion
    #region GetData

    public MonsterBase Find_SetMonsterBase(string monster_ID, MonsterInfo info)
    {
        MonsterBase monsterBase = monsterDatas.monsterDatas.Find((monster) => monster.monsterId.Equals(monster_ID));

        return new MonsterBase(monsterBase, info);
    }

    public MonsterMove FindMonsterPrefab(string monster_ID)
    {
        return monsterPrefabs.FindMonsterPrefab(monster_ID);
    }

    public Sprite FindItemSprite(string item_ID)
    {
        return itemSprites.FindItemSprite(item_ID);
    }

    public ItemBase FindItemBase(string item_ID)
    {
        if (item_ID.Equals("NULL")) return null;

        ItemData data = itemDatas.itemDataList.Find((item) => item.item_ID == item_ID);
        ItemBase item = new ItemBase(data, FindItemSprite(data.item_ID));
        return item;
    }

    public ItemBase GetItemBase(int index)
    {
        if (index >= itemDatas.Length) return null;
        ItemData data = itemDatas.itemDataList[index];
        ItemBase item = new ItemBase(data, FindItemSprite(data.item_ID));
        return item;
    }

    public ItemBase ConversionToItemBase(ItemData data)
    {
        ItemBase item = new ItemBase(data, FindItemSprite(data.item_ID));
        return item;
    }

    public PatternData FindPatternData(string patternData_ID)
    {
        PatternData patternData = patterns.patterns.Find((pattern) => pattern.ID == patternData_ID);

        return patternData;
    }

    public PatternData GetPatternData(int index)
    {
        return patterns.patterns[index];
    }

    public WaveData GetWaveData(int index)
    {
        return waveDatas.waves[index];
    }

    #endregion



}
