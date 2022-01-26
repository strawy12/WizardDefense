using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Transform enemyLeftSpawnPoint = null;
    [SerializeField] private Transform enemyRightSpawnPoint = null;
    [SerializeField] private Transform targetPoint = null;

    private List<PatternData> patternDataList;

    private PatternData currentPattern;
    private WaveData currentWave;

    private Transform currentSpawnPoint;

    private int currentSpawnCnt = 0;
    private int waveIndex = 0;
    private int currentPatternCnt = 0;
    private float nextPatternDelay = 0f;

    private SpawnMonsterInfo currentSpawnInfo = null;
    private MonsterMove currentMonsterPref = null;
    private MonsterBase currentMonsterBase = null;

    public void Init()
    {
        //Wave 데이터 가져오는 코드
    }

    public IEnumerator StartWave()
    {
        currentWave = GameManager.Instance.Data.GetWaveData(waveIndex);

        SettingWaveData();

        while (currentPatternCnt < currentWave.executionCnt)//currentWave.maxCost)
        {
            yield return InitPatternData(currentPatternCnt);
            yield return new WaitForSeconds(nextPatternDelay);
        }
    }

    private void SettingWaveData()
    {
        int randIndex = 0;
        patternDataList = new List<PatternData>();

        List<string> patternIDList = currentWave.availablePatternIDs.ToList();

        string pattern_ID = "";
        PatternData pattern = null;

        for (int i = 0; i < currentWave.executionCnt; i++)
        {
            randIndex = Random.Range(0, patternIDList.Count);

            pattern_ID = patternIDList[randIndex];
            pattern = GameManager.Instance.Data.FindPatternData(pattern_ID);
            patternDataList.Add(pattern);

            if(!pattern.doReUse)
            {
                patternIDList.RemoveAt(randIndex);
            }
        }
    }

    private IEnumerator InitPatternData(int index)
    {
        currentPattern = patternDataList[index];

        nextPatternDelay = currentPattern.nextPatternDelay;

        SetSpawnDirTrn();

        yield return GenerateMonsters();

        currentPatternCnt++;
    }

    public IEnumerator GenerateMonsters()
    {
        MonsterMove monster = null;

        for (int i = 0; i < currentPattern.spawnMonsterCnt; i++)
        {
            currentSpawnInfo = currentPattern.monsterInfoList[i];
            currentMonsterPref = GameManager.Instance.Data.FindMonsterPrefab(currentSpawnInfo.monsterId);
            currentMonsterBase = GameManager.Instance.Data.Find_SetMonsterBase(currentSpawnInfo.monsterId, currentSpawnInfo.monsterData);

            for (int j = 0; j < currentSpawnInfo.spawnCount; j++)
            {
                monster = Instantiate(currentMonsterPref, currentSpawnPoint.position, Quaternion.identity);
                monster.Init(currentMonsterBase, targetPoint, currentSpawnCnt++);

                yield return new WaitForSeconds(currentPattern.monsterSpawnDelay);
            }

        }

    }

    private void SetSpawnDirTrn()
    {
        switch (currentPattern.direction)
        {
            case DirectionType.Left:
                currentSpawnPoint = enemyLeftSpawnPoint;
                break;

            case DirectionType.Right:
                currentSpawnPoint = enemyRightSpawnPoint;
                break;

            case DirectionType.Random:
                currentSpawnPoint = Random.Range(0, 2) == 0 ? enemyLeftSpawnPoint : enemyRightSpawnPoint;
                break;
        }
    }
}
