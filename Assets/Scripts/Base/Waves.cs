using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public int waveIndex;
    public int maxCost;

    public List<string> patternIDs;
}

[CreateAssetMenu(fileName = "Waves", menuName = "Scriptable Object/Waves")]
public class Waves : ScriptableObject
{
    public List<WaveData> waves;
}
