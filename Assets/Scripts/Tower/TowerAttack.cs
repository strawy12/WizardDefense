using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    public TowerBase towerBase;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private GameObject bulletPrefab;
    public Eneminyoung targetEnemey;

    void Start()
    {
        StartCoroutine(Fire());
    }

    private IEnumerator Fire()
    {
        while (true)
        {
            yield return new WaitForSeconds(towerBase.fireRate);
            InstantiateOrPooling();
        }
    }

    private void InstantiateOrPooling()
    {
        SetTargetEnemy();

        if (targetEnemey != null)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.GetComponent<BulletMove>().Init(this);
            obj.transform.position = bulletPosition.position;
            obj.SetActive(true);
        }
    }

    private bool SetTargetEnemy()
    {
        List<Eneminyoung> enemies = GameManager.Instance.eneminyoungs;
        float minDistance = Vector3.Distance(enemies[0].transform.position, bulletPosition.position);

        for (int i = 0; i < enemies.Count; i++)
        {
            float distance = Vector3.Distance(enemies[i].transform.position, bulletPosition.position);

            if (distance <= minDistance && distance <= towerBase.distance)
            {
                minDistance = distance;
                targetEnemey = enemies[i];
            }
        }

        return (!targetEnemey);
    }
}
