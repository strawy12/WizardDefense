using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public GameState gameState;
    public InGameState inGameState;

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
    public GameObject player;
    public GameObject home;
    public ItemObject Itempref;

    public TowerAttack selectedTower;
    public TowerAttack censorTower;

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
        Cursor.lockState = CursorLockMode.Locked;

        mainCam = FindObjectOfType<CameraMove>();
        UIManager = GetComponent<UIManager>();
        KeyManager = GetComponent<KeyManager>();
        gameState = GameState.Playing;
        //StartCoroutine(SpawnEnemies());

    }

    void Start()
    {
        gameState = GameState.Playing;
        screenCenter = (new Vector3(mainCam.cam.pixelWidth / 2, mainCam.cam.pixelHeight / 2));
        EnterBreakTime();

    }

    private void Init()
    {

    }

    private void Update()
    {
        inputAxis = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        if (inGameState == InGameState.BreakTime && gameState != GameState.Setting)
        {
            if (Input.GetKeyUp(KeyCode.V))
            {
                SkipBreakTime();
            }

            breakTime -= Time.unscaledDeltaTime;
            UIManager.SetTimer(breakTime);

            if (breakTime < 0)
            {
                UIManager.ActiveBreakTimeUI(false);
                inGameState = InGameState.DefenseTime;
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
        inGameState = InGameState.BreakTime;
        UIManager.ActiveBreakTimeUI(true);
    }

    public void SkipBreakTime()
    {
        breakTime = 0f;
    }

    public void SpawnItem(ItemBase data, Vector3 spawnPos)
    {
        ItemObject item = Instantiate(Itempref, spawnPos, Quaternion.identity);
        item.item = data;
    }
}