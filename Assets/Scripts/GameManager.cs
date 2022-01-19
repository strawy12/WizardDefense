using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public GameState gameState;
    public GameObject home;

    public Vector3 screenCenter;
    public List<MonsterMove> enemies { get; private set; } = new List<MonsterMove>();
    public List<Attribute> attributes = new List<Attribute>();
    public List<Skill> skills = new List<Skill>();
    public CameraMove mainCam { get; private set; }
    public Camera tpsCamera;

    public Vector2 inputAxis;
    public GameObject boundary;
    public UIManager UIManager { get; private set; }
    public KeyManager KeyManager { get; private set; }

    public TowerAttack selectedTower;

    public GameObject player;

    private WaveManager waveManager;
    private InGameDataManager dataManager;
    private UIManager uiManager;

    public WaveManager Wave { get { return waveManager; } }
    public InGameDataManager Data { get { return dataManager; } }
    public UIManager UI { get { return uiManager; } }

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
    }

    private void Init()
    {
    }

    private void Update()
    {
        inputAxis = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            //SpawnEnemy();
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator ShowBoundary(Vector3 position,Vector3 scale)
    {
        boundary.transform.position = new Vector3(position.x, 0.13f, position.z);
        boundary.transform.localScale = scale;
        boundary.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        boundary.SetActive(false);
    }
}