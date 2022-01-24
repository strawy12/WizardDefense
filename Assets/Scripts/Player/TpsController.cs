using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpsController : MonoBehaviour
{
    [SerializeField] private Transform characterBody;

    [SerializeField] private Transform cameraArm;

    [Header("이동속도")] [SerializeField] private float normalSpeed = 5f;
    [Header("달리기속도")] [SerializeField] private float runSpeed = 8f;
    [Header("감도")] [SerializeField] private float sensivity;
    [Header("점프 힘")] [SerializeField] private float jumpPower = 2f;
    [Header("밀리는 힘")] [SerializeField] private float backPower = 2f;
    [Header("레이저 길이")] [SerializeField] private float maxDistance = 10f;

    private float speed;
    private RaycastHit hitTowerAreaInfo;

    private bool isArea;

    private bool jDown;
    private bool isRun;
    private bool isJump;
    private bool isTarget = false;
    private bool isTargetTower = false;

    private MonsterMove targetMonster;
    private ItemObject targetItem;

    private Animator animator;
    private Rigidbody myrigid;

    private TowerAttack tower;

    private void Start()
    {
        myrigid = GetComponent<Rigidbody>();
        animator = characterBody.GetComponent<Animator>();
        EventManager<ItemBase>.StartListening(ConstantManager.INVENTORY_DROP, DropItem);
        EventManager.StartListening(ConstantManager.TURNON_INVENTORY, StopPlayer);
        EventManager<float>.StartListening(ConstantManager.CHANGE_SENSITVITY, SetSentivity);
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameState.Setting) return;

        PlayerSet();

        if (Input.GetKeyDown(KeyManager.keySettings[KeyAction.Interaction]) && GameManager.Instance.UIManager.IsFMarkActive())
        {
            if (GameManager.Instance.inGameState == InGameState.BreakTime)
            {
                if (GameManager.Instance.censorTower == null)
                {
                    GameManager.Instance.UIManager.Chang();
                }
                else
                {
                    TowerBase tower = GameManager.Instance.censorTower.towerBase;
                    GameManager.Instance.UIManager.ShowSkillUI(GameManager.Instance.censorTower, true);
                    GameManager.Instance.UIManager.ShowTowerStatBar(true, tower.attackPower, tower.fireRate);
                }
            }

            else if (GameManager.Instance.inGameState == InGameState.DefenseTime && GameManager.Instance.selectedTower == null)
            {
                //tower.ZoomInTower();
                //gameObject.SetActive(false);
                GameManager.Instance.censorTower?.ZoomInTower();
                GameManager.Instance.UIManager.ShowSkillUI(GameManager.Instance.censorTower, true);
            }
        }
    }

    private void PlayerSet()
    {
        Move();

        if (GameManager.Instance.gameState == GameState.InGameSetting) return;

        LookAround();
        Jump();
        Run();
        Hit();
    }
    private void LookAround()
    {
        if (GameManager.Instance.gameState == GameState.Setting || GameManager.Instance.gameState == GameState.InGameSetting) return;
        
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X") * sensivity, Input.GetAxis("Mouse Y") * sensivity);
        Vector3 cameraAngle = cameraArm.rotation.eulerAngles;
        float x = cameraAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, cameraAngle.y + mouseDelta.x, cameraAngle.z);
    }

    private void Move()
    {
        if (GameManager.Instance.gameState != GameState.Playing)
        {
            animator.SetBool("isMove", false);
            return;
        }

        isRun = Input.GetKey(KeyCode.LeftShift);
        Vector2 moveInput = new Vector2(Input.GetAxis(ConstantManager.KEYINPUT_HMOVE), Input.GetAxis(ConstantManager.KEYINPUT_VMOVE));
        bool isMove = moveInput.magnitude != 0;
        animator.SetBool("isMove", isMove);

        if (isMove)
        {
            Vector3 lookForWard = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForWard * moveInput.y + lookRight * moveInput.x;

            //characterBody.forward = lookForWard;
            characterBody.forward = moveDir;

            transform.position = GameManager.Instance.ConversionBoundPosition(transform.position);

            transform.position += moveDir * speed * Time.deltaTime;
        }
    }

    private void Jump()
    {
        jDown = Input.GetButtonDown(ConstantManager.KEYINPUT_JUMP);
        if (jDown && !isJump)
        {
            myrigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJump = true;
        }
    }

    private void Run()
    {
        if (isRun)
        {
            speed = runSpeed;
        }
        else
        {
            speed = normalSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            isJump = false;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("j");
            myrigid.AddForce(Vector3.back * backPower);
        }
    }

    private void Hit()
    {
        var cam = GameManager.Instance.tpsCamera;

        Hit_TowerArea(cam);
        Hit_Tower(cam);
        Hit_Unit(cam);
    }

    private void Hit_Unit(Camera cam)
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * maxDistance * 2, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(cam.transform.position, cam.transform.forward, maxDistance * 2);

        foreach (var hit in hits)
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                targetMonster = hit.transform.GetComponent<MonsterMove>();

                if (targetMonster != null)
                {
                    GameManager.Instance.selectedMonster?.ShowOutLine(false);
                    targetMonster.GetInfo();
                    targetMonster.ShowOutLine(true);
                    GameManager.Instance.selectedMonster = targetMonster;
                    return;
                }
            }

            if(hit.transform.CompareTag("Item"))
            {
                Debug.Log("dd");
                targetItem = hit.transform.GetComponent<ItemObject>();

                if(targetItem != null)
                {
                    targetItem.GetInfo();
                    return;
                }
            }
        }
    }

    private void Hit_TowerArea(Camera cam)
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * maxDistance, Color.red);

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitTowerAreaInfo, maxDistance))
        {
            if (hitTowerAreaInfo.transform.gameObject.CompareTag("area") && GameManager.Instance.inGameState == InGameState.BreakTime)
            {
                TowerSelect.buildTrn = hitTowerAreaInfo.transform;
                GameManager.Instance.UIManager.FMarkTrue();
                hitTowerAreaInfo.transform.GetComponent<Area>()?.ShowOutline(true);
                isTarget = true;
            }

            else
            {
                GameManager.Instance.UIManager.AreaCheack();
                GameManager.Instance.UIManager.FMarkFalse();
                hitTowerAreaInfo.transform.GetComponent<Area>()?.ShowOutline(false);
                isTarget = false;
            }
        }

        else
        {
            isTarget = false;
        }
    }

    private void Hit_Tower(Camera cam)
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * maxDistance * 3, Color.white);

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitTowerAreaInfo, maxDistance * 3))
        {
            if (hitTowerAreaInfo.transform.gameObject.CompareTag(ConstantManager.TOWER_TAG))
            {
                tower = hitTowerAreaInfo.collider.gameObject.GetComponent<TowerAttack>();
                GameManager.Instance.censorTower = tower;

                if (!tower.isBuilding)
                {
                    isTargetTower = true;
                    GameManager.Instance.UIManager.FMarkTrue();
                    tower.ShowOutLine(true);
                }

                else
                {
                    GameManager.Instance.UIManager.FMarkFalse();
                    tower.ShowOutLine(false);
                    isTargetTower = false;
                }
            }
            else
            {
                tower?.ShowOutLine(false);
            }
        }
        else
        {
            tower?.ShowOutLine(false);
        }
    }

    private void DropItem(ItemBase item)
    {
        GameManager.Instance.SpawnItem(item, transform.position);
    }
    
    private void StopPlayer()
    {
        animator.Play("Stand");
        animator.SetBool("isMove", false);
    }

    private void SetSentivity(float value)
    {
        sensivity = value;
    }
}