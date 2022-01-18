using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    public TowerBase towerBase;
    public Transform muzzlePosition;
    public GameObject boundary;

    private MonsterMove targetEnemy;
    private PoolManager pool;

    private TowerState towerState;
    private float curFireTime = 0f;
    public float useSkillTime = 0f;
    public float selectedTime = 0f;

    public Skill skill;

    void Start()
    {
        pool = FindObjectOfType<PoolManager>();
        useSkillTime = 100f;

        boundary.transform.localScale = new Vector2(towerBase.distance, towerBase.distance) * transform.localScale * 0.5f;
        boundary.gameObject.SetActive(true);
    }

    private void Update()
    {
        curFireTime += Time.deltaTime;
        selectedTime += Time.deltaTime;
        Mathf.Clamp(selectedTime, 0f, 2f);

        SetMuzzleRotation();
        SkillCoolTime();

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

    #region Fire
    private void FireByPlayer()
    {
        if (Input.GetKey(KeyManager.keySettings[KeyAction.Fire]) && curFireTime > towerBase.handFireRate)
        {
            InstantiateOrPooling(pool.GetPoolObject(EPoolingType.DefaultBullet).gameObject);
            curFireTime = 0f;
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
        if (curFireTime > towerBase.fireRate)
        {
            InstantiateOrPooling(pool.GetPoolObject(EPoolingType.DefaultBullet).gameObject);
            curFireTime = 0;
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
        List<MonsterMove> enemies = GameManager.Instance.enemies;
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
    public void ZoomInTower()
    {
        Debug.Log("ZZ");
        GameManager.Instance.tpsCamera.gameObject.SetActive(false);
        Vector3 cameraPosition = transform.position;
        cameraPosition.y += 2f;
        GameManager.Instance.mainCam.CameraMoveToPosition(cameraPosition, 1f);
        //이거 fireRate 다름
        GameManager.Instance.UIManager.ShowTowerStatBar(true, towerBase.attackPower, towerBase.fireRate);
        
        GameManager.Instance.selectedTower = this;
        towerState = TowerState.InControl;
        selectedTime = 0f;
    }

    private void ZoomOutTower()
    {
        if (Input.GetKeyDown(KeyManager.keySettings[KeyAction.Interaction]) && selectedTime > 1f)
        {
            //고정값이니 바꾸어도 됨
            GameManager.Instance.mainCam.CameraMoveToPosition(new Vector3(0, 11.5f, -10f), 1f);
            GameManager.Instance.mainCam.CameraRotate(new Vector3(31f, 0f, 0f), 1f);
            //GameManager.Instance.UIManager.ShowTowerStatBar(true);
            GameManager.Instance.selectedTower = null;

            curFireTime = 0f;

            towerState = TowerState.OutControl;
        }
    }

    private void ShowBoundary()
    {
        if (Input.GetKeyDown(KeyManager.keySettings[KeyAction.Boundary]))
        {
            if (boundary.activeSelf)
            {
                boundary.SetActive(false);
            }
            else
            {
                boundary.transform.localScale = new Vector2(towerBase.distance, towerBase.distance) * (1 / transform.localScale.x);
            }
        }
    }

    private void CameraMove()
    {
        Vector2 mouse = GameManager.Instance.inputAxis * 4f;
        Quaternion rot = GameManager.Instance.mainCam.transform.rotation;
        GameManager.Instance.mainCam.ChangeLocalEulerAngle(new Vector3(Mathf.Clamp(-mouse.y + rot.eulerAngles.x, 0, 80), mouse.x + rot.eulerAngles.y, 0f));
    }
    #endregion

    #region Skill
    private void OnUseSKill()
    {
        if (Input.GetKeyDown(KeyManager.keySettings[KeyAction.Skill]) && towerState == TowerState.InControl)
        {
            skill = GetSkill();

            if (!CheckSkillCoolTime()) return;

            GameObject obj = pool.GetPoolObject(skill.bulletPrefab.PoolType).gameObject;
            InstantiateOrPooling(obj);

            useSkillTime = 0f;
        }
    }

    private Skill GetSkill()
    {
        return GameManager.Instance.skills.Find(skill => skill.attributeName == towerBase.attribute.attributeName);
    }

    private void SkillCoolTime()
    {
        useSkillTime += Time.deltaTime;
    }

    public bool CheckSkillCoolTime()
    {
        return (useSkillTime > skill.coolTime);
    }
    #endregion

    #region GetSet
    public void GetAttribute(Attribute attribute)
    {
        towerBase.attribute = attribute;
    }

    public MonsterMove GetTargetEnemy()
    {
        return targetEnemy;
    }

    public TowerState GetTowerState()
    {
        return towerState;
    }
    #endregion
}