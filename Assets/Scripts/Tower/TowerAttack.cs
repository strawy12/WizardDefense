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

    private TowerState towerState;
    private float curFireTime = 0f;
    public float useSkillTime = 0f;
    public float selectedTime = 0f;

    public Skill skill;

    public bool isBuilding;

    private Outline outline;

    void Awake()
    {
        pool = FindObjectOfType<PoolManager>();
        outline = GetComponentInChildren<Outline>();
        useSkillTime = 100f;

        TowerBuild();

        Vector3 scale = transform.localScale;
        scale.y = scale.x;
        boundary.gameObject.transform.localScale = new Vector2(towerBase.distance, towerBase.distance) * 2f * (1 / scale.x);
        boundary.gameObject.SetActive(true);
    }

    private void Update()
    {
        curFireTime += Time.deltaTime;
        selectedTime += Time.deltaTime;
        SetMuzzleRotation();
        SkillCoolTime();

        if (towerState == TowerState.OutControl)
        {
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
        }

        else
        {
            CameraMove();
            ZoomOutTower();
            FireByPlayer();
        }

        OnUseSKill();
    }

    private void TowerBuild()
    {
        isBuilding = true;
        float firstPosY = -40f;
        transform.DOMoveY(firstPosY, 0f);
        //transform.DOMoveY(transform.localScale.y * 0.5f/* + 5f*/, 2f).OnComplete(() => isBuilding = false);
        transform.DOMoveY(-25f, 2f).OnComplete(() => isBuilding = false);
    }

    public void EquipItems()
    {
        //GameManager.Instance.UIManager.
    }

    #region Fire
    private void FireByPlayer()
    {
        if (Input.GetKey(KeyCode.Mouse0) && curFireTime > towerBase.handFireRate)
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
        bool isTargeting = SetTargetEnemy();

        if (towerState == TowerState.InControl || isTargeting)
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

    public bool IsInBoundary()
    {
        List<MonsterMove> enemies = GameManager.Instance.enemies;
        Vector2 pos;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (Vector3.Distance(enemies[i].transform.position, transform.position) <= towerBase.distance)
                return true;
        }

        return false;
    }
    #endregion

    #region Control
    public void ZoomInTower()
    {
        GameManager.Instance.mainCam.cam.enabled = true;

        GameManager.Instance.tpsCamera.enabled = false;

        Vector3 cameraPosition = transform.position;
        cameraPosition.y += 2f;
        muzzlePosition.transform.position = cameraPosition;
        GameManager.Instance.mainCam.CameraMoveToPosition(cameraPosition, 1f);
        //이거 fireRate 다름
        GameManager.Instance.UIManager.ShowTowerStatBar(true, towerBase.attackPower, towerBase.fireRate);

        GameManager.Instance.selectedTower = this;
        towerState = TowerState.InControl;
        selectedTime = 0f;

        ShowBoundary(true);
        ChangeBoundaryColor(Color.red);
    }

    private void ZoomOutTower()
    {
        if (Input.GetKeyDown(KeyManager.keySettings[KeyAction.Interaction]) && selectedTime > 1f)
        {
            //고정값이니 바꾸어도 됨
            GameManager.Instance.player.SetActive(true);

            Vector3 pos = GameManager.Instance.tpsCamera.transform.position;
            Vector3 rot = GameManager.Instance.tpsCamera.transform.parent.eulerAngles;
            GameManager.Instance.mainCam.ZoomOutCamera(pos, rot, 1f);
            //GameManager.Instance.UIManager.ShowTowerStatBar(true);    
            GameManager.Instance.selectedTower = null;

            curFireTime = 0f;

            towerState = TowerState.OutControl;
        }
    }

    private void ShowBoundary(bool isActive)
    {
        boundary.gameObject.SetActive(isActive);
    }

    private void CameraMove()
    {
        if (GameManager.Instance.gameState == GameState.Setting) return;

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
    public void SetAttribute(Attribute attribute)
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

    public void ChangeBoundaryColor(Color color)
    {
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
}