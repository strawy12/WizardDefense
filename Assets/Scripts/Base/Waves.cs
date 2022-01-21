using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public int waveIndex;
    public List<string> availablePatternIDs;
    public int executionCnt;

    public bool existBoss;
    public string bossID;

    public WaveData(int waveIndex, List<string> patternIDs, int executionCnt)
    {
        SetDefaultData(waveIndex, patternIDs, executionCnt);
    }

    public WaveData(int waveIndex, List<string> patternIDs, int executionCnt, string bossID)
    {
        SetDefaultData(waveIndex, patternIDs, executionCnt);

        existBoss = true;
        this.bossID = bossID;
    }

    public void SetDefaultData(int waveIndex, List<string> patternIDs, int executionCnt)
    {
        this.waveIndex = waveIndex;
        availablePatternIDs = patternIDs;
        this.executionCnt = executionCnt;
    }
}

[CreateAssetMenu(fileName = "Waves", menuName = "Scriptable Object/Waves")]
public class Waves : ScriptableObject
{
    public string updateVersion;
    public List<WaveData> waves;
}
