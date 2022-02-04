using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerRoot
{
    public string name;
    [TextArea] public string info;
    public int price;
    public int value;
    public Sprite rootImage;
}

[System.Serializable]
public class Roots
{
    public int index;
    public List<TowerRoot> roots;
}

[CreateAssetMenu(fileName = "TowerRoots", menuName = "Scriptable Object/TowerRoots")]
public class TowerRoots : ScriptableObject
{
    public List<Roots> towerRoots;
}

