using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WayPoints[] wayPoints;
    [SerializeField] private MainWayPoints mainWayPoints;
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private List<Monster> enemyPrefabs;

    public IEnumerator GenerateMonsters()
    {
        yield return new WaitForSeconds(2f);

        for (int j = 0; j < 5; j++)
        {
            int generateCount = Random.Range(3, 10);

            for (int i = 0; i < generateCount; i++)
            {
                SpawnMonster();
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(5f);
        }

    }

    private void SpawnMonster()
    {
        int randIndex = Random.Range(0, enemyPrefabs.Count);
        int randPosIndex = Random.Range(0, enemySpawnPoints.Length);

        Monster monster = Instantiate(enemyPrefabs[randIndex], enemySpawnPoints[randPosIndex].localPosition, Quaternion.identity);
        monster.SetDirType((DirectionType)randPosIndex);
        monster.Init();
    }


    public Transform GetWayPoint(int index, DirectionType dirType)
    {
        if(index < wayPoints[(int)dirType].Length)
        {
            return wayPoints[(int)dirType].GetWayPoint(index);
        }

        else
        {
            int reLoadIndex = index - wayPoints[(int)dirType].Length;

            switch (dirType)
            {
                case DirectionType.Left:
                    Debug.Log("Left");
                    return mainWayPoints.GetLeftWayPoint(reLoadIndex);

                case DirectionType.Center:
                    Debug.Log("Top");
                    return mainWayPoints.GetTopWayPoint(reLoadIndex);

                case DirectionType.Right:
                    Debug.Log("Right");
                    return mainWayPoints.GetRightWayPoint(reLoadIndex);
            }
        }

        return null;
    }
}
