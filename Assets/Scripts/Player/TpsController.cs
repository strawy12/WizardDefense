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

    private Animator animator;
    private Rigidbody myrigid;

    private TowerAttack tower;

    private void Start()
    {
        myrigid = GetComponent<Rigidbody>();
        animator = characterBody.GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerSet();

        if (Input.GetKeyDown(KeyManager.keySettings[KeyAction.Interaction]))
        {
            if (GameManager.Instance.UIManager.isTarget)
                GameManager.Instance.UIManager.Chang();

            if (isTargetTower && GameManager.Instance.selectedTower == null)
            {
                tower.ZoomInTower();
                gameObject.SetActive(false);
            }
        }
    }

    private void PlayerSet()
    {
        LookAround();
        Move();
        Jump();
        Run();
        Hit();
    }
    private void LookAround()
    {
        if (GameManager.Instance.gameState == GameState.Setting) return;

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
    }

    private void Hit()
    {
        var cam = GameManager.Instance.tpsCamera;

        //Hit_TowerArea(cam);
        Hit_Monster(cam);
    }

    private void Hit_Monster(Camera cam)
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
                    targetMonster.GetInfo();
                    return;
                }
            }
        }

    }

    private void Hit_TowerArea(Camera cam)
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * maxDistance, Color.blue);

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitTowerAreaInfo, maxDistance))
        {
            if (hitTowerAreaInfo.transform.gameObject.CompareTag("area") && GameManager.Instance.inGameState == InGameState.BreakTime)
            {
                TowerSelect.buildTrn = hitTowerAreaInfo.transform;
                GameManager.Instance.UIManager.FMarkTrue();
                isTarget = true;
            }
            else if (hitTowerAreaInfo.transform.gameObject.CompareTag(ConstantManager.TOWER_TAG) && GameManager.Instance.inGameState == InGameState.DefenseTime)
            {
                tower = hitTowerAreaInfo.collider.gameObject.GetComponent<TowerAttack>();

                if(!tower.isBuilding)
                {
                    isTargetTower = true;
                    isTarget = false;
                    GameManager.Instance.UIManager.FMarkTrue();
                }

                else
                {
                    GameManager.Instance.UIManager.FMarkFalse();
                    isTarget = false;
                    isTargetTower = false;
                    tower = null;
                }
            }
            else
            {
                GameManager.Instance.UIManager.AreaCheack();
                GameManager.Instance.UIManager.FMarkFalse();
                isTarget = false;
                return;
            }
        }

        else
        {
            isTarget = false;
            isTargetTower = false;
            tower = null;
        }
    }
}