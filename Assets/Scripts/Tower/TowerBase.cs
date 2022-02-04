using System.Collections.Generic;

[System.Serializable]
public class TowerBase
{
    public string name;
    public int attackPower;
    public float fireRate;
    public float distance;

    public List<int> availableRootIndexes;
    public CurrentTowerRoot currentRoot;
}

[System.Serializable]
public class CurrentTowerRoot
{
    public int rootIndex;
    public int index;
}