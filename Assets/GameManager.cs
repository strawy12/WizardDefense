using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public GameObject enemy;
    public GameObject home;

    public Vector3 screenCenter;

    public List<Enemy> enemies { get; private set; } = new List<Enemy>();
    public List<Attribute> attributes = new List<Attribute>();

    public CameraMove mainCam;

    public Vector2 inputAxis;

    public UIManager UIManager { get; private set; }

    void Start()
    {
        mainCam = Camera.main.GetComponent<CameraMove>();
        screenCenter = (new Vector3(mainCam.cam.pixelWidth / 2, mainCam.cam.pixelHeight / 2));
        UIManager = GetComponent<UIManager>();
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        inputAxis = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    }

    private void SpawnEnemy()
    {
        GameObject obj = Instantiate(enemy);

        obj.transform.position = new Vector3(20, 1, 8);
        obj.SetActive(true);

        enemies.Add(obj.GetComponent<Enemy>());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(1f);
        }
    }
}