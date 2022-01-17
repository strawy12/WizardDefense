using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private Transform targetPoint;
    [SerializeField] private int waveMaxCost;

    private PatternData currentPattern;
    private WaveData currentWave;

    private int currentCost = 0;
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
        int patternIndex = 0;
        while (currentCost < waveMaxCost)//currentWave.maxCost)
        {

            yield return InitPatternData(patternIndex);

            yield return new WaitForSeconds(nextPatternDelay);
        }
    }
    private IEnumerator InitPatternData(int index)
    {
        currentPattern = GameManager.Inst.Data.GetPatternData(index);

        nextPatternDelay = currentPattern.nextPatternDelay;
        currentCost += currentPattern.cost;

        yield return GenerateMonsters();
    }
    

    public IEnumerator GenerateMonsters()
    {
        MonsterMove monster = null;

        for(int i = 0; i < currentPattern.spawnMonsterCnt; i++)
        {
            currentSpawnInfo = currentPattern.monsterInfoList[i];
            currentMonsterPref = GameManager.Inst.Data.FindMonsterPrefab(currentSpawnInfo.monsterId);
            currentMonsterBase = GameManager.Inst.Data.Find_SetMonsterBase(currentSpawnInfo.monsterId, currentSpawnInfo.monsterData);

            for (int j = 0; j < currentSpawnInfo.spawnCount; j++)
            {
                monster = Instantiate(currentMonsterPref, enemySpawnPoint.position, Quaternion.identity);
                monster.Init(currentMonsterBase, targetPoint);

                yield return new WaitForSeconds(currentPattern.monsterSpawnDelay);
            }

        }
    }


}
