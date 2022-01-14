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

        SetMuzzleRotation();

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
        OnUseSKill();
    }

    private void OnMouseUp()
    {
        ZoomInTower();
    }

    #region Fire
    private void FireByPlayer()
    {
        if (Input.GetMouseButton(0) && curTime > towerBase.handFireRate)
        {
            InstantiateOrPooling(pool.GetPoolObject(EPoolingType.DefaultBullet).gameObject);
            curTime = 0f;
        }
    }

    private void SetMuzzleRotation()
    {
        //Ray ray = GameManager.Instance.mainCam.cam.ScreenPointToRay(GameManager.Instance.screenCenter);
        Ray ray = GameManager.Instance.mainCam.cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hitInfo;
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.blue);

        if (Physics.Raycast(ray, out hitInfo, 999f))
        {
            //muzzlePosition.localEulerAngles = GameManager.Instance.mainCam.transform.localEulerAngles;
            //Debug.DrawRay(muzzlePosition.position, hitInfo.point - muzzlePosition.position, Color.red);
            muzzlePosition.LookAt(hitInfo.point);
        }
    }

    private void Fire()
    {
        if (curTime > towerBase.fireRate)
        {
            InstantiateOrPooling(pool.GetPoolObject(EPoolingType.DefaultBullet).gameObject);
            curTime = 0;
        }
    }

    private void InstantiateOrPooling(GameObject obj)
    {
        if (towerState == TowerState.InControl || SetTargetEnemy())
        {
            obj.GetComponent<BulletMove>().Init(this);
            obj.GetComponent<BulletAttack>().Init(this);

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

        if (towerState == TowerState.OutControl)
        {
            targetEnemy?.VirtualDamaged(towerBase.attackPower);
        }

        return (targetEnemy != null);
    }
    #endregion

    #region Control
    private void ZoomInTower()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y += 2f;
        GameManager.Instance.mainCam.CameraMoveToPosition(cameraPosition, 1f);
        //이거 fireRate 다름
        GameManager.Instance.UIManager.ShowTowerStatBar(true, towerBase.attackPower, towerBase.fireRate);
        towerState = TowerState.InControl;
    }

    private void ZoomOutTower()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            towerState = TowerState.OutControl;
            GameManager.Instance.mainCam.CameraMoveToPosition(new Vector3(0, 11.5f, -10f), 1f);
            GameManager.Instance.mainCam.CameraRotate(new Vector3(31f, 0f, 0f), 1f);
            GameManager.Instance.UIManager.ShowTowerStatBar(true);
            curTime = 0f;
        }
    }

    private void ShowBoundary()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (boundary.activeSelf)
            {
                boundary.SetActive(false);
            }
            else
            {
                boundary.transform.localScale = new Vector2(towerBase.distance, towerBase.distance) * transform.localScale * 0.5f;
            }
        }
    }

    private void CameraMove()
    {
        Vector2 mouse = GameManager.Instance.inputAxis*4f;
        Quaternion rot = GameManager.Instance.mainCam.transform.rotation;
        GameManager.Instance.mainCam.ChangeLocalEulerAngle(new Vector3(Mathf.Clamp(-mouse.y + rot.eulerAngles.x, 0, 80), mouse.x + rot.eulerAngles.y, 0f));
    }
    #endregion

    #region
    private void OnUseSKill()
    {
        if (Input.GetKeyDown(KeyCode.Q) && towerState == TowerState.InControl)
        {
            Skill skill = GetSkill();

            GameObject obj = pool.GetPoolObject(skill.bulletPrefab.PoolType).gameObject;
            InstantiateOrPooling(obj);
        }
    }

    private Skill GetSkill()
    {
        return GameManager.Instance.skills.Find(skill => skill.attributeName == towerBase.attribute.attributeName);
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