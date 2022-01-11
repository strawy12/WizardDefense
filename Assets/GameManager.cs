using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public GameObject enemy;
    public GameObject home;

    public Vector3 screenCenter;

    public List<Enemy> enemies = new List<Enemy>();
    public List<Attribute> attributes = new List<Attribute>();

    void Start()
    {
        screenCenter = (new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2));
        StartCoroutine(SpawnEnemies());
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