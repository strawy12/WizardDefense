using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public GameObject enemy;
    public GameObject home;

    public RectTransform center;

    public List<Eneminyoung> enemies = new List<Eneminyoung>();
    public List<Attribute> attributes = new List<Attribute>();

    void Start()
    {
        //StartCoroutine(SpawnEnemies());
    }

    private void SpawnEnemy()
    {
        GameObject obj = Instantiate(enemy);

        obj.transform.position = new Vector3(20, 1, 8);
        obj.SetActive(true);

        enemies.Add(obj.GetComponent<Eneminyoung>());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(2f);
        }
    }
}