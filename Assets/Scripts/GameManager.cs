using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public GameState gameState;
    public InGameState inGameState { get; private set; }
    public PlayerState playerState { get; private set; }

    #region About Camera
    public CameraMove mainCam { get; private set; }
    public Camera tpsCamera;
    public Camera uiCamara;
    public Vector3 screenCenter;
    #endregion

    #region Controller
    public UIManager UIManager { get; private set; }
    public KeyManager KeyManager { get; private set; }

    private WaveManager waveManager;
    public WaveManager Wave { get { return waveManager; } }

    private InGameDataManager dataManager;
    public InGameDataManager Data { get { return dataManager; } }
    #endregion

    #region InGame
    public GameObject boundary;
    public TpsController player;
    public GameObject pointTower;
    public Transform mapBorder;
    public ItemObject Itempref;

    public TowerAttack selectedTower;
    public TowerAttack censorTower;
    public MonsterMove selectedMonster;
    public ItemObject selectedItem;

    public List<MonsterMove> enemies { get; private set; } = new List<MonsterMove>();
    public List<Attribute> attributes = new List<Attribute>();
    public List<Skill> skills = new List<Skill>();

    public Vector2 inputAxis;

    private float breakTime = ConstantManager.BREAK_TIME;
    #endregion

    #region Utils

    public Vector3 MousePos
    {
        get
        {
            Vector3 screenPoint = Input.mousePosition;
            screenPoint.z = 3.0f; 
            
            return uiCamara.ScreenToWorldPoint(screenPoint);
        }
    }

    #endregion

    private void Awake()
    {
        gameState = GameState.Setting;
        waveManager = GetComponent<WaveManager>();
        dataManager = GetComponent<InGameDataManager>();

        mainCam = FindObjectOfType<CameraMove>();
        UIManager = GetComponent<UIManager>();
        KeyManager = GetComponent<KeyManager>();
        gameState = GameState.Playing;
    }

    void Start()
    {
        gameState = GameState.Playing;
        playerState = PlayerState.Idle;
        screenCenter = (new Vector3(mainCam.cam.pixelWidth / 2, mainCam.cam.pixelHeight / 2));
        EnterBreakTime();
        SetPlayerSentivity();
    }

    private void Init()
    {

    }

    private void Update()
    {
        inputAxis = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        if (inGameState == InGameState.BreakTime && gameState != GameState.Setting)
        {
            if (Input.GetKeyUp(KeyManager.keySettings[KeyAction.Skip]) && UIManager.isStarted)
            {
                SkipBreakTime();
            }

            breakTime -= Time.unscaledDeltaTime;
            UIManager.SetTimer(breakTime);

            if (breakTime < 0)
            {
                UIManager.ActiveBreakTimeUI(false);
                SetInGameState(InGameState.DefenseTime);
                StartCoroutine(Wave.StartWave());
            }
        }
    }

    public IEnumerator ShowBoundary(Vector3 position, Vector3 scale)
    {
        boundary.transform.position = new Vector3(position.x, 4f, position.z);
        boundary.transform.localScale = scale;
        boundary.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        boundary.SetActive(false);
    }

    private void EnterBreakTime()
    {
        breakTime = ConstantManager.BREAK_TIME;
        SetInGameState(InGameState.BreakTime);
        UIManager.ActiveBreakTimeUI(true);
    }

    public void SkipBreakTime()
    {
        breakTime = 0f;
    }

    public void SpawnItem(ItemBase item, Vector3 spawnPos)
    {
        if (item == null) return;

        ItemObject itemObj = Instantiate(Itempref, spawnPos, Quaternion.identity);
        itemObj.item = item;
    }

    public Vector3 ConversionBoundPosition(Vector3 pos)
    {
        return pos;
        float x = mapBorder.localScale.x / 2;
        float additionX = mapBorder.position.x;
        float y = mapBorder.localScale.y - 5f;
        float additionY = mapBorder.position.y;
        float z = mapBorder.localScale.z / 2;
        float additionZ = mapBorder.position.z;

        pos.x = Mathf.Clamp(pos.x, -x + additionX, x + additionX);
        pos.y = Mathf.Clamp(pos.y, -28f, y + additionY);
        pos.z = Mathf.Clamp(pos.z, -z + additionZ, z + additionZ);

        return pos;
    }
    private void SetPlayerSentivity()
    {
        EventManager<float>.TriggerEvent(ConstantManager.CHANGE_SENSITVITY, DataManager.Instance.PlayerData.sensitivityValue);
    }

    public void SetInGameState(InGameState state)
    {
        inGameState = state;

        if (inGameState == InGameState.BreakTime)
        {
            EventManager.TriggerEvent(ConstantManager.START_BREAKTIME);
        }

        else if(inGameState == InGameState.DefenseTime)
        {
            EventManager.TriggerEvent(ConstantManager.START_DEFENSETIME);
        }
    }
    public void SetPlayerState(PlayerState state)
    {
        playerState = state;
    }
}