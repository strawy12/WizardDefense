using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerRoot
{
    [SerializeField] private string name;
    [SerializeField] [TextArea] private string info;
    [SerializeField] private int price;
    [SerializeField] private int value;
    [SerializeField] private Sprite rootImage;
}

[System.Serializable]
public class Roots
{
    [SerializeField] private int index;
    [SerializeField] private List<TowerRoot> roots;
}

[CreateAssetMenu(fileName = "TowerRoots", menuName = "Scriptable Object/TowerRoots")]
public class TowerRoots : ScriptableObject
{
    public List<Roots> towerRoots;
}

