using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TowerAttack : MonoBehaviour
{
    public TowerBase towerBase;
    public Transform bulletPosition;
    public Enemy targetEnemy;
    private PoolManager pool;
    public TowerState towerState;
    private float curTime = 0f;
    public GameObject boundary;

    void Start()
    {
        pool = FindObjectOfType<PoolManager>();


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            boundary.transform.localScale = new Vector2(towerBase.distance, towerBase.distance) * transform.localScale * 0.5f;
        }

        curTime += Time.deltaTime;

        if (towerState == TowerState.OutControl)
        {
            Fire();
            return;
        }
        else
        {
            CameraMove();
        }

        if (Input.GetMouseButton(0) && towerState == TowerState.InControl && curTime > towerBase.handFireRate)
        {
            InstantiateOrPooling();
            curTime = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && towerState == TowerState.InControl)
        {
            towerState = TowerState.OutControl;
            Camera.main.transform.DOMove(new Vector3(0, 11.5f, -10f), 1f);
            Camera.main.transform.DORotate(new Vector3(31f, 0f, 0f), 1f);
            curTime = 0f;
        }

    }

    private void CameraMove()
    {
        float yRot = Input.GetAxisRaw("Mouse X") * 4f;
        float xRot = -Input.GetAxisRaw("Mouse Y") * 4f;

        Quaternion rot = Camera.main.transform.rotation;

        Camera.main.transform.localEulerAngles =
            new Vector3(Mathf.Clamp(xRot + rot.eulerAngles.x, 0, 80), rot.eulerAngles.y + yRot, 0f);

        Ray ray = Camera.main.ScreenPointToRay(GameManager.Instance.screenCenter);
        bulletPosition.LookAt(ray.GetPoint(towerBase.distance));


    }
    private void Fire()
    {
        if (curTime > towerBase.fireRate)
        {
            InstantiateOrPooling();
            curTime = 0;
        }
    }

    private void InstantiateOrPooling()
    {
        Camera mainCam = Camera.main;
        if (towerState == TowerState.InControl || SetTargetEnemy())
        {
            GameObject obj = pool.GetPoolObject(EPoolingType.BulletMove).gameObject;
            obj.transform.position = bulletPosition.position;

            if (towerState == TowerState.InControl)
            {

                obj.transform.rotation = bulletPosition.rotation;
            }

            else
            {
                obj.transform.rotation = Quaternion.identity;
            }

            obj.SetActive(true);
            obj.GetComponent<BulletMove>().Init(this);
        }
    }
    public bool SetTargetEnemy()
    {
        List<Enemy> enemies = GameManager.Instance.enemies;
        if (enemies.Count == 0) return false;

        float minDistance = 100f;
        float distance = 0f;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (Vector3.Distance(enemies[i].transform.position, transform.position) > towerBase.distance)
                continue;

            distance = Vector3.Distance(enemies[i].transform.position, GameManager.Instance.home.transform.position);

            if (distance < minDistance)
            {
                targetEnemy = enemies[i];
                minDistance = distance;
            }
        }

        if (targetEnemy == null) return false;
        return true;
    }

    private void OnMouseUp()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y += 2f;

        Camera.main.transform.DOMove(cameraPosition, 1f);
        towerState = TowerState.InControl;
    }

    public void GetAttribute(Attribute attribute)
    {
        towerBase.attribute = attribute;
    }
}