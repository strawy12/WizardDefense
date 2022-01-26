using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DataManager : MonoSingleton<DataManager>
{
    [SerializeField] private float defaultSound = 0.5f;
    [SerializeField] private PlayerData playerData;

    public PlayerData PlayerData { get { return playerData; } }

    string SAVE_PATH = "";
    string SAVE_FILE = "/SaveFile.Json";
    private void Awake()
    {

        DataManager[] dmanagers = FindObjectsOfType<DataManager>();
        if (dmanagers.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        SAVE_PATH = Application.dataPath + "/Save";

        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        LoadFromJson();

        AddHavedEnergyList();
    }

    private void LoadFromJson()
    {
        if (File.Exists(SAVE_PATH + SAVE_FILE))
        {
            string stringJson = File.ReadAllText(SAVE_PATH + SAVE_FILE);
            playerData = JsonUtility.FromJson<PlayerData>(stringJson);
        }
        else
        {
            InitData();
        }

        SaveToJson();
    }

    public void SaveToJson()
    {
        string stringJson = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILE, stringJson, System.Text.Encoding.UTF8);
    }

    public void InitData()
    {
        playerData = new PlayerData();

        playerData.bgmSoundVolume = defaultSound;
        playerData.effectSoundVolume = defaultSound;
        playerData.sensitivityValue = 15f;


        AddHavedEnergyList();
        AddkeyInputDataList();
        AddInventoryList();
        SaveToJson();
    }

    private void AddInventoryList()
    {
        playerData.inventoryList = new List<InventoryData>();

        for (int i = 0; i < 36; i++)
        {
            playerData.inventoryList.Add(new InventoryData(i, false));
        }

        for (int i = 0; i < 4; i++)
        {
            playerData.inventoryList.Add(new InventoryData(i, true));
        }
    }

    private void AddkeyInputDataList()
    {
        playerData.keyInputDataList = new List<KeyInputData>();

        playerData.keyInputDataList.Add(new KeyInputData(KeyAction.Jump, KeyCode.Space));
        playerData.keyInputDataList.Add(new KeyInputData(KeyAction.Run, KeyCode.LeftShift));
        playerData.keyInputDataList.Add(new KeyInputData(KeyAction.Skill, KeyCode.Q));
        playerData.keyInputDataList.Add(new KeyInputData(KeyAction.Inventory, KeyCode.Tab));
        playerData.keyInputDataList.Add(new KeyInputData(KeyAction.Interaction, KeyCode.F));
        playerData.keyInputDataList.Add(new KeyInputData(KeyAction.ChangeView, KeyCode.F5));
        playerData.keyInputDataList.Add(new KeyInputData(KeyAction.Boundary, KeyCode.R));
        playerData.keyInputDataList.Add(new KeyInputData(KeyAction.Skip, KeyCode.V));
        playerData.keyInputDataList.Add(new KeyInputData(KeyAction.QuitSlot1, KeyCode.Alpha1));
        playerData.keyInputDataList.Add(new KeyInputData(KeyAction.QuitSlot2, KeyCode.Alpha2));
        playerData.keyInputDataList.Add(new KeyInputData(KeyAction.QuitSlot3, KeyCode.Alpha3));
        playerData.keyInputDataList.Add(new KeyInputData(KeyAction.QuitSlot4, KeyCode.Alpha4));
    }


    private void AddHavedEnergyList()
    {
        playerData.havedEnergyList = new List<EnergyData>();

        playerData.havedEnergyList.Add(new EnergyData(PropertyType.Fire, 0));
        playerData.havedEnergyList.Add(new EnergyData(PropertyType.Water, 0));
        playerData.havedEnergyList.Add(new EnergyData(PropertyType.Terra, 0));
        playerData.havedEnergyList.Add(new EnergyData(PropertyType.Air, 0));
        playerData.havedEnergyList.Add(new EnergyData(PropertyType.Holy, 0));
        playerData.havedEnergyList.Add(new EnergyData(PropertyType.Void, 0));

        RandomPickEnergy();

        SaveToJson();
    }

    private void RandomPickEnergy()
    {
        int randomIndex = 0;
        for(int i = 0; i < 25; i++)
        {
            randomIndex = Random.Range(0, 6);
            playerData.havedEnergyList[randomIndex].count++;
        }
    }

    public void SetKeyInput(KeyAction keyAction, KeyCode keyCode)
    {
        playerData.keyInputDataList.Find((keyInput)=> keyInput.keyAction == keyAction).keyCode = keyCode;
        SaveToJson();

    }

    public void SetInventoryData(int index, ItemData itemData, int count, bool isQuitSlot)
    {
        InventoryData slot = playerData.inventoryList.Find((slot) => slot.index == index && slot.isQuitSlot == isQuitSlot);

        slot.item = itemData;
        slot.count = count;
        SaveToJson();

    }

    public EnergyData GetEnergyData(PropertyType type)
    {
        return playerData.havedEnergyList.Find((data) => data.type == type);
    }

    private void OnApplicationQuit()
    {
       SaveToJson();
    }

    private void OnApplicationPause(bool pause)
    {
        SaveToJson();
    }


}