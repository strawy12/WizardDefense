using DG.Tweening;
using UnityEngine;

public class TpsController : MonoBehaviour
{
    [SerializeField] private Transform characterBody;
    public Vector3 towerPos;

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
    private bool isTargetItem = false;
    private bool isTargetTower = false;
    private bool isTargetTowerArea = false;
    private bool isTowerRun;

    private MonsterMove targetMonster;
    private ItemObject targetItem;

    private Animator animator;
    private Rigidbody myrigid;

    private TowerAttack tower;
    private Area area;

    private float fieldOfView;


    private void Awake()
    {
        EventManager<float>.StartListening(ConstantManager.CHANGE_SENSITVITY, SetSentivity);
    }
    private void Start()
    {
        myrigid = GetComponent<Rigidbody>();
        animator = characterBody.GetComponent<Animator>();
        EventManager<ItemBase>.StartListening(ConstantManager.INVENTORY_DROP, DropItem);
        EventManager.StartListening(ConstantManager.TURNON_INVENTORY, StopPlayer);
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameState.Setting) return;

        PlayerSet();
        if (Input.GetKeyDown(KeyManager.keySettings[KeyAction.Interaction]) && GameManager.Instance.UIManager.IsFMarkActive())
        {
            if (GameManager.Instance.censorTower == null/* && isTargetTowerArea*/)
            {
<<<<<<< HEAD
                GameManager.Instance.UIManager.Chang();
=======
                if (GameManager.Instance.censorTower == null && isTargetTowerArea)
                {

                    GameManager.Instance.UIManager.Chang();
                }

                else if (isTargetItem && targetItem != null)
                {
                    targetItem.PickUpItem();
                    targetItem = null;
                }
                //else
                //{
                //    TowerBase tower = GameManager.Instance.censorTower.towerBase;
                //    GameManager.Instance.UIManager.ShowSkillUI(GameManager.Instance.censorTower, true);
                //    GameManager.Instance.UIManager.ShowTowerStatBar(true, tower.attackPower, tower.fireRate);
                //}
>>>>>>> OIF
            }
            //else
            //{
            //    TowerBase tower = GameManager.Instance.censorTower.towerBase;
            //    GameManager.Instance.UIManager.ShowSkillUI(GameManager.Instance.censorTower, true);
            //    GameManager.Instance.UIManager.ShowTowerStatBar(true, tower.attackPower, tower.fireRate);
            //}

            //tower.ZoomInTower();
            //gameObject.SetActive(false);
            if (isTargetTower && GameManager.Instance.selectedTower == null)
            {
<<<<<<< HEAD
                GameManager.Instance.censorTower?.ZoomInTower();
                GameManager.Instance.UIManager.ShowSkillUI(GameManager.Instance.censorTower);
            }
=======
                //tower.ZoomInTower();
                //gameObject.SetActive(false);
                if (isTargetTower && GameManager.Instance.selectedTower == null)
                {
                    GameManager.Instance.censorTower?.ZoomInTower();
                    GameManager.Instance.UIManager.ShowSkillUI(GameManager.Instance.censorTower);
                }

                else if (isTargetItem && targetItem != null)
                {
                    targetItem.PickUpItem();
                    targetItem = null;
                }
>>>>>>> OIF

            else if (isTargetItem && targetPropertyEnegy != null)
            {
                targetPropertyEnegy.AddPropertyEnergy();
                targetPropertyEnegy = null;
            }

        }

        if (Input.GetMouseButtonDown(0) && GameManager.Instance.censorTower != null && !isTowerRun)
        {
            towerPos = GameManager.Instance.censorTower.transform.position - new Vector3(0f, 0f, 5f);
            if (Vector3.Distance(towerPos, transform.position) < 10f || GameManager.Instance.UIManager.IsFMarkActive()) return;
            fieldOfView = 3f * Vector3.Distance(towerPos, transform.position);
            fieldOfView = Mathf.Clamp(fieldOfView, 60f, 120f);

            GameManager.Instance.tpsCamera.fieldOfView = fieldOfView;
            isTowerRun = true;

            transform.DOMove(towerPos, 1f).OnComplete(() =>
            {
                isTowerRun = false;
            });
        }

        if (GameManager.Instance.tpsCamera.fieldOfView > 60f)
        {
            GameManager.Instance.tpsCamera.fieldOfView -= Time.deltaTime * (fieldOfView - 60f);
        }
    }

    private void PlayerSet()
    {

        if (GameManager.Instance.gameState == GameState.InGameSetting) return;
        Move();
        LookAround();
        //Jump();
        Fly();
        Run();
        Hit();

        transform.position = GameManager.Instance.ConversionBoundPosition(transform.position);
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

            characterBody.forward += moveDir;

            //transform.position = GameManager.Instance.ConversionBoundPosition(transform.position);
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

    private void Fly()
    {
        float speed = 8f;
        if (isRun) speed *= 2f;

        if (Input.GetKey(KeyManager.keySettings[KeyAction.Jump]) && !isJump)
        {
            Vector3 flyPos = transform.position;
            flyPos.y += 0.1f;
            transform.position += Vector3.up * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Vector3 downPos = transform.position;
            downPos.y -= 0.1f;
            transform.position += Vector3.down * Time.deltaTime * speed;
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
                    isTargetItem = false;
                    return;
                }
            }

            if (hit.transform.CompareTag("Item"))
            {
                targetItem = hit.transform.GetComponent<ItemObject>();

                if (targetItem != null)
                {
                    GameManager.Instance.selectedItem?.ShowOutLine(false);
                    targetItem.GetInfo();
                    targetItem.ShowOutLine(true);
                    GameManager.Instance.selectedItem = targetItem;
                    GameManager.Instance.UIManager.FMarkTrue();
                    isTargetItem = true;
                    return;
                }
            }

            else
            {
                isTargetItem = false;
            }
        }

        isTargetItem = false;
    }

    private void Hit_TowerArea(Camera cam)
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * maxDistance, Color.red);

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitTowerAreaInfo, maxDistance))
        {
            if (hitTowerAreaInfo.transform.gameObject.CompareTag("area"))
            {
                TowerSelect.buildTrn = hitTowerAreaInfo.transform;
                GameManager.Instance.UIManager.FMarkTrue();
                area = hitTowerAreaInfo.transform.GetComponent<Area>();
                area.ShowOutline(true);
                isTargetItem = false;
                isTargetTowerArea = true;
            }

            else
            {
                GameManager.Instance.UIManager.AreaCheack();
                GameManager.Instance.UIManager.FMarkFalse();
                area?.ShowOutline(false);
                //isTargetTowerArea = false;
            }
        }
    }

    private void Hit_Tower(Camera cam)
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 999f, Color.white);
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitTowerAreaInfo, 999f))
        {
            if (hitTowerAreaInfo.transform.gameObject.CompareTag(ConstantManager.TOWER_TAG))
            {
                tower = hitTowerAreaInfo.collider.gameObject.GetComponent<TowerAttack>();
                GameManager.Instance.censorTower = tower;


                if (!tower.isBuilding)
                {
                    if (GameManager.Instance.inGameState == InGameState.DefenseTime)
                    {
                        if (Vector3.Distance(transform.position, tower.transform.position) > 10f)
                        {
                            isTargetTower = false;
                            GameManager.Instance.UIManager.FMarkFalse();
                        }
                        else
                        {
                            GameManager.Instance.UIManager.FMarkTrue();
                            isTargetTower = true;
                            isTargetItem = false;
                        }
                    }
                    isTargetTower = true;
                    tower.ShowOutLine(true);
                }

                else
                {
                    tower.ShowOutLine(false);
                    isTargetTower = false;
                }
            }
            else
            {
                tower?.ShowOutLine(false);
                GameManager.Instance.censorTower = null;
                isTargetTower = false;
            }
        }
        else
        {
            tower?.ShowOutLine(false);
            isTargetTower = false;
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

    public void TargetingTowerArea(bool isTarget)
    {
        isTargetTowerArea = isTarget;
    }

    private void SetSentivity(float value)
    {
        sensivity = value;
        DataManager.Instance.PlayerData.sensitivityValue = value;
        DataManager.Instance.SaveToJson();
    }

<<<<<<< HEAD
    public float GetMaxDistance()
    {
        return maxDistance;
    }
=======

>>>>>>> OIF
}