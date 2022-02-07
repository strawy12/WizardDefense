using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TowerAttack : MonoBehaviour
{
    public TowerBase towerBase;
    public Transform muzzlePosition;
    public SpriteRenderer boundary;

    private MonsterMove targetEnemy;
    private PoolManager pool;

    private float curFireTime = 0f;
    public float useSkillTime = 0f;
    public float selectedTime = 0f;

    public Skill skill;

    public bool isBuilding;
    public GameObject towerUnit;

    private Outline outline;
    [SerializeField] private GameObject bullet;

    void Awake()
    {
        pool = FindObjectOfType<PoolManager>();
        outline = GetComponentInChildren<Outline>();
        useSkillTime = 100f;
        towerBase.currentRoot.index = -1;
        towerBase.currentRoot.rootIndex = 0;
        TowerBuild();
        SetBoundary();
    }

    private void Update()
    {
        curFireTime += Time.deltaTime;
        selectedTime += Time.deltaTime;
        SetMuzzleRotation();
        SkillCoolTime();

        Fire();

        if (IsInBoundary())
        {
            ChangeBoundaryColor(Color.white);
            ShowBoundary(true);
        }
        else
        {
            ShowBoundary(false);
        }

        OnUseSKill();
    }

    private void SetBoundary()
    {
        Vector3 scale = transform.localScale;
        scale.y = scale.x;
        boundary.gameObject.transform.localScale = new Vector2(towerBase.distance, towerBase.distance) * (2f / scale.x);
        boundary.gameObject.SetActive(true);
    }

    private void TowerBuild()
    {
        isBuilding = true;
        float firstPosY = -8f;
        float lastPosY = 5.04f;
        transform.DOMoveY(firstPosY, 0f);
        transform.DOMoveY(lastPosY, 2f).OnComplete(() => isBuilding = false);
    }

    private void SetMuzzleRotation()
    {
        //Ray ray = GameManager.Instance.mainCam.cam.ScreenPointToRay(GameManager.Instance.screenCenter);
        CameraMove cam = GameManager.Instance.mainCam;
        RaycastHit hitInfo;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);


        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, towerBase.distance))
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
        bool isTargeting = SetTargetEnemy();
        if (isTargeting)
        {
            obj.GetComponent<BulletMove>().Init(this);
            obj.GetComponent<BulletAttack>().Init(this);

            obj.transform.position = muzzlePosition.position;
            obj.transform.rotation = muzzlePosition.rotation;

            obj.SetActive(true);
        }
    }


    #region ToFire
    public bool SetTargetEnemy()
    {
        List<MonsterMove> enemies = GameManager.Instance.enemies;
        if (enemies.Count == 0) return false;

        float minDistance = 999f;
        float distance;
        int minSpawnOrder = 999;
        targetEnemy = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            distance = Vector3.Distance(enemies[i].transform.position, transform.position);

            if (distance > towerBase.distance) continue;

            if (distance < minDistance && enemies[i].virtualHP > 0 && enemies[i].SpawnOrder < minSpawnOrder)
            {
                targetEnemy = enemies[i];
                minDistance = distance;
                minSpawnOrder = enemies[i].SpawnOrder;
            }
        }

        targetEnemy?.VirtualDamaged(towerBase.attackPower);

        return (targetEnemy != null);
    }


    public bool IsInBoundary()
    {
        List<MonsterMove> enemies = GameManager.Instance.enemies;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (Vector3.Distance(enemies[i].transform.position, transform.position) <= towerBase.distance)
                return true;
        }

        return false;
    }
    #endregion

    #region Control
    private void ShowBoundary(bool isActive)
    {
        boundary.gameObject.SetActive(isActive);
    }
    #endregion

    #region Skill
    private void OnUseSKill()
    {
        if (Input.GetKeyDown/*(KeyManager.keySettings[KeyAction.Skill])*/(KeyCode.Q))
        {
            if (!CheckSkillCoolTime()) return;

            GameObject obj = pool.GetPoolObject(skill.bulletPrefab.PoolType).gameObject;
            InstantiateOrPooling(obj);

            useSkillTime = 0f;
        }
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
    public MonsterMove GetTargetEnemy()
    {
        return targetEnemy;
    }
    #endregion

    public void ChangeBoundaryColor(Color color)
    {
        color.a = 0.4f;
        boundary.color = color;
    }

    public void ShowOutLine(bool isShow)
    {
        if (isBuilding || outline == null) return;
        if (isShow)
        {
            outline.OutlineWidth = outline.thisOutLine;
        }
        else
        {
            outline.OutlineWidth = 0f;
        }
    }

    private void InstantiateBulletEffect(Vector3 point)
    {
        GameObject bulletEffect = Instantiate(bullet);
        bulletEffect.transform.position = muzzlePosition.position;
        bulletEffect.transform.LookAt(point);
    }
}
