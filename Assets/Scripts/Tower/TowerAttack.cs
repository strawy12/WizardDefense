using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TowerAttack : MonoBehaviour
{


    public TowerBase towerBase;
    public Transform bulletPosition;
    [SerializeField] private GameObject bulletPrefab;
    public Eneminyoung targetEnemey;
    PoolManager pool;
    public TowerState towerState;

    private float curTime = 0f;
    void Start()
    {
        pool = FindObjectOfType<PoolManager>();
    }

    private void Update()
    {
        Fire();

        if (Input.GetKeyDown(KeyCode.Escape) && towerState == TowerState.InControl)
        {
            towerState = TowerState.OutControl;
            Camera.main.transform.DOMove(new Vector3(0, 11.5f, -10f), 1f);
            Camera.main.transform.DORotate(new Vector3(31f, 0f, 0f), 1f);
        }

        if (towerState == TowerState.InControl)
        {
            CameraMove();
        }
    }

    private void CameraMove()
    {
        float yRot = Input.GetAxisRaw("Mouse X") * 4f;
        float xRot = -Input.GetAxisRaw("Mouse Y") * 4f;

        Quaternion rot = Camera.main.transform.rotation;

        Camera.main.transform.localEulerAngles =
            new Vector3(Mathf.Clamp(xRot + rot.eulerAngles.x, 10, 80), rot.eulerAngles.y + yRot, 0f);

    }
    private void Fire()
    {
        curTime += Time.deltaTime;

        if (curTime > towerBase.fireRate)
        {
            InstantiateOrPooling();
            curTime = 0;
        }
    }

    private void InstantiateOrPooling()
    {
        if (SetTargetEnemy())
        {
            GameObject obj = pool.GetPoolObject(EPoolingType.BulletMove).gameObject;
            obj.GetComponent<BulletMove>().Init(this);
            obj.transform.position = bulletPosition.position;
            obj.SetActive(true);
        }
    }

    public bool SetTargetEnemy()
    {
        List<Eneminyoung> enemies = GameManager.Instance.eneminyoungs;
        if (enemies.Count == 0) return false;

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

        if (targetEnemey == null)
        {
            targetEnemey = enemies[0];
            return false;
        }

        return true;
    }

    private void OnMouseUp()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y += 2f;

        Camera.main.transform.DOMove(cameraPosition, 1f);
        towerState = TowerState.InControl;
    }
}