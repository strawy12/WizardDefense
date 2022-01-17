using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst = null;
    private WaveManager waveManager;
    private InGameDataManager dataManager;

    public WaveManager Wave { get { return waveManager; } }
    public InGameDataManager Data {  get { return dataManager; } }


    private void Awake()
    {
        Inst = this;
        waveManager = GetComponent<WaveManager>();
        dataManager = GetComponent<InGameDataManager>();
    }

    void Start()
    {
        Init();
    }

    private void Init()
    {
        StartCoroutine(Wave.StartWave());
    }

    private void Update()
    {
    }


}
