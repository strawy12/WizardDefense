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
    public TowerAttack selectedTower;
    public GameObject player;
    public GameObject home;

    public List<MonsterMove> enemies { get; private set; } = new List<MonsterMove>();
    public List<Attribute> attributes = new List<Attribute>();
    public List<Skill> skills = new List<Skill>();

    public Vector2 inputAxis;

    private float breakTime = ConstantManager.BREAK_TIME;
    #endregion

    private void Awake()
    {
        waveManager = GetComponent<WaveManager>();
        dataManager = GetComponent<InGameDataManager>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        mainCam = FindObjectOfType<CameraMove>();
        screenCenter = (new Vector3(mainCam.cam.pixelWidth / 2, mainCam.cam.pixelHeight / 2));
        UIManager = GetComponent<UIManager>();
        KeyManager = GetComponent<KeyManager>();
        gameState = GameState.Playing;
        //StartCoroutine(SpawnEnemies());

        EnterBreakTime();
    }

    private void Init()
    {
    }

    private void Update()
    {
        inputAxis = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        if(inGameState == InGameState.BreakTime)
        {
            breakTime -= Time.deltaTime;
            UIManager.SetTimer(breakTime);

            if(breakTime < 0)
            {
                UIManager.ActiveTimer(false);
                inGameState = InGameState.DefenseTime;
                waveManager.StartCoroutine(waveManager.StartWave());
            }
        }
    }

    public IEnumerator ShowBoundary(Vector3 position, Vector3 scale)
    {
        boundary.transform.position = new Vector3(position.x, 0.13f, position.z);
        boundary.transform.localScale = scale;
        boundary.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        boundary.SetActive(false);
    }

    private void EnterBreakTime()
    {
        breakTime = ConstantManager.BREAK_TIME;
        inGameState = InGameState.BreakTime;
        UIManager.ActiveTimer(true);
    }
}