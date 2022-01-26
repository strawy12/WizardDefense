using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MonsterLove.StateMachine;
using DG.Tweening;

public class MonsterMove : MonoBehaviour
{
    [SerializeField] private MonsterBase monsterBase;

    private NavMeshAgent agent;
    private SkinnedMeshRenderer meshRenderer;

    public float RemainingDistance { get { return agent.remainingDistance; } }

    private float currentHp = 0;
    public float virtualHP;

    private bool finished_Init = false;

    private Vector3 currentDir = Vector3.zero;
    private Transform targetPoint = null;

    private ItemBase currentItem;

    private UnitInfo unitInfo;

    private Outline outline;

    private ParticleSystem particle;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        outline = GetComponent<Outline>();
        particle = GetComponentInChildren<ParticleSystem>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

    }

    private void Start()
    {
        virtualHP = currentHp;
        unitInfo = new UnitInfo();
    }

    private void Update()
    {
        if (!finished_Init) return;
        if (agent.velocity.magnitude > 0.2f && agent.remainingDistance <= 3f)
        {   
            AttackPointTower();
        }
    }

    public void Init(MonsterBase monsterBase, Transform target)
    {
        if(monsterBase.dropItem != null)
        {
            currentItem = monsterBase.dropItem;
        }

        this.monsterBase = monsterBase;
        currentHp = monsterBase.info.maxHp;
        finished_Init = true;
        targetPoint = target;
        agent.SetDestination(targetPoint.position);
        GameManager.Instance.enemies.Add(this);

        finished_Init = true;
    }

    private bool CheckIsMoved()
    {
        if (agent.velocity.x > 0f)
        {
            return true;
        }

        if (agent.velocity.y > 0f)
        {
            return true;
        }

        if (agent.velocity.z > 0f)
        {
            return true;
        }

        return false;
    }

    public void Damaged(int damage)
    {
        currentHp -= damage;
        particle.Play();

        if (currentHp <= 0)
        {
            if (currentItem != null && currentItem.itemData != null && currentItem.itemData.itemName != "")
            {
                if (Random.Range(0, 100) < 20)
                {
                    GameManager.Instance.SpawnItem(currentItem, transform.position);
                }
            }

            Dead();
        }
        else
        {
            EventManager<Vector3>.TriggerEvent(ConstantManager.MONSTER_ATTACKED, transform.position);
            StartCoroutine(OnDamaged());
        }
    }

    private IEnumerator OnDamaged()
    {
        meshRenderer.materials[1].color = new Color32(255, 0, 0, 143);  
        yield return new WaitForSeconds(0.1f);
        meshRenderer.materials[1].color = new Color32(0, 0, 0, 0);
    }

    public void VirtualDamaged(int power)
    {
        virtualHP -= power;
    }

    public void Dead()
    {
        GameManager.Instance.enemies.Remove(this);

        Destroy(gameObject);
    }

    public void AttackPointTower()
    {
        EventManager<int>.TriggerEvent(ConstantManager.MONSTER_ATTACK, monsterBase.info.attackPower);
        Dead();
    }

    public void GetInfo()
    {
        unitInfo.unitName = monsterBase.monsterName;
        unitInfo.attackPower = monsterBase.info.attackPower;
        unitInfo.defence = monsterBase.info.defense;
        unitInfo.maxHp = monsterBase.info.maxHp;
        unitInfo.currentHp = currentHp;

        EventManager<UnitInfo>.TriggerEvent(ConstantManager.VIEW_UNITINFO, unitInfo);
    }

    public void ShowOutLine(bool isShow)
    {
        if (isShow)
        {
            outline.OutlineColor = Color.yellow;
        }
        else
        {
            outline.OutlineColor = new Color32(255, 68, 68, 255);
        }
    }

}
public struct UnitInfo
{
    public string unitName;
    public int attackPower;
    public float defence;
    public float maxHp;
    public float currentHp;
    public Sprite unitSprite;
}

