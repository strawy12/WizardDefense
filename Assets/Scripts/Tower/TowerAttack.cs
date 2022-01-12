using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TowerAttack : MonoBehaviour
{
    public TowerBase towerBase;
    public Transform muzzlePosition;
    public GameObject boundary;

    private Enemy targetEnemy;
    private PoolManager pool;

    private TowerState towerState;
    private float curTime = 0f;

    void Start()
    {
        pool = FindObjectOfType<PoolManager>();
    }

    private void Update()
    {
        curTime += Time.deltaTime;

        if (towerState == TowerState.OutControl)
        {
            Fire();
        }

        else
        {
            CameraMove();
            ZoomOutTower();
            FireByPlayer();
        }

        ShowBoundary();
    }

    private void OnMouseUp()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ZoomInTower();
        }
    }

    #region Fire
    private void FireByPlayer()
    {
        if (Input.GetMouseButton(0) && curTime > towerBase.handFireRate)
        {
            InstantiateOrPooling();
            curTime = 0f;
        }
    }

    private void SetMuzzleRotation()
    {
        Ray ray = GameManager.Instance.mainCam.cam.ScreenPointToRay(GameManager.Instance.screenCenter);
        muzzlePosition.LookAt(ray.GetPoint(towerBase.distance));
    }

    private void Fire()
    {
        if (curTime > towerBase.fireRate)
        {
            SetMuzzleRotation();
            InstantiateOrPooling();
            curTime = 0;
        }
    }

    private void InstantiateOrPooling()
    {
        if (towerState == TowerState.InControl || SetTargetEnemy())
        {
            GameObject obj = pool.GetPoolObject(EPoolingType.BulletMove).gameObject;
            obj.GetComponent<BulletMove>().Init(this);

            obj.transform.position = muzzlePosition.position;
            obj.transform.rotation = muzzlePosition.rotation;

            obj.SetActive(true);
        }
    }

    #endregion

    #region ToFire
    public bool SetTargetEnemy()
    {
        List<Enemy> enemies = GameManager.Instance.enemies;
        if (enemies.Count == 0) return false;

        float minDistance = 100f;
        float distance;
        targetEnemy = null;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (Vector3.Distance(enemies[i].transform.position, transform.position) > towerBase.distance) continue;

            distance = Vector3.Distance(enemies[i].transform.position, GameManager.Instance.home.transform.position);

            if (distance < minDistance && enemies[i].virtualHP > 0)
            {
                targetEnemy = enemies[i];
                minDistance = distance;
            }
        }

        if(towerState == TowerState.OutControl)
        {
            targetEnemy?.VirtualDamaged(towerBase.attackPower);
        }

        return (targetEnemy != null);
    }
    #endregion]

    #region Control
    private void ZoomInTower()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y += 2f;
        GameManager.Instance.mainCam.CameraMoveToPosition(cameraPosition, 1f);
        towerState = TowerState.InControl;
    }

    private void ZoomOutTower()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            towerState = TowerState.OutControl;
            GameManager.Instance.mainCam.CameraMoveToPosition(new Vector3(0, 11.5f, -10f), 1f);
            GameManager.Instance.mainCam.CameraRotate(new Vector3(31f, 0f, 0f), 1f);
            curTime = 0f;
        }
    }

    private void ShowBoundary()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            boundary.transform.localScale = new Vector2(towerBase.distance, towerBase.distance) * transform.localScale * 0.5f;
        }
    }

    private void CameraMove()
    {
        Vector2 mouse = GameManager.Instance.inputAxis;
        Quaternion rot = GameManager.Instance.mainCam.transform.rotation;
        GameManager.Instance.mainCam.ChangeLocalEulerAngle(new Vector3(Mathf.Clamp(mouse.x + rot.eulerAngles.x, 0, 80), rot.eulerAngles.y + mouse.y, 0f));
    }
    #endregion

    #region GetSet
    public void GetAttribute(Attribute attribute)
    {
        towerBase.attribute = attribute;
    }

    public Enemy GetTargetEnemy()
    {
        return targetEnemy;
    }

    public TowerState GetTowerState()
    {
        return towerState;
    }
    #endregion
}