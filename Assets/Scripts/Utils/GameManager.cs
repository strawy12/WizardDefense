using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Monster monster;
    [SerializeField] private List<Item> itemList;

    [SerializeField] private MonsterDatas monsterDatas;
    [SerializeField] private Patterns patterns;

    public static GameManager Inst = null;
    private WaveManager waveManager;
    public WaveManager Wave { get { return waveManager; } }

    private void Awake()
    {
        Inst = this;
        waveManager = GetComponent<WaveManager>();
    }

    void Start()
    {
        StartCoroutine(DataDownLoad());
    }

    private void Init()
    {
        StartCoroutine(Wave.GenerateMonsters());
    }

    private void Update()
    {
        ClickObj();
        if (Input.GetKeyDown(KeyCode.A))
        {
            monster.Damaged(1);
        }
    }

    #region Data DownLoad

    private IEnumerator DataDownLoad()
    {
        yield return StartMonsterDataDownLoad();
        yield return StartWavePatternDataDownLoad();
        Debug.Log(1);
        //Init();
    }


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

        for (int i = 0; i < monsterInfoArr.Length; i++)
        {
            infos = monsterInfoArr[i].Split('_');

            id = monsterIdArr[int.Parse(infos[0]) - 1];
            spawnCnt = int.Parse(infos[1]);
            hp = int.Parse(infos[2]);
            defense = int.Parse(infos[3]);
            monsterInfoList.Add(new SpawnMonsterInfo(id, spawnCnt, hp, defense));
        }

        return monsterInfoList;
    }

    private MonsterBase FindMonster(string id)
    {
        return monsterDatas.monsterDatas.Find((monster) => monster.monsterId == id);
    }
    #endregion
    private void ClickObj()
    {
        Item item = null;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                item = hit.transform.GetComponent<Item>();
                item?.Despawn();
            }

        }
    }

    public void EqualItem(string itemName)
    {
        Item item = itemList.Find((item) => item.itemData.itemName == itemName);

        if (item != null)
        {
            Instantiate(item, monster.transform.position, Quaternion.identity);
        }
    }


}
