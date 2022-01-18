using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpsController : MonoBehaviour
{
    [SerializeField] private Transform characterBody;

    [SerializeField] private Transform cameraArm;

    [Header("�̵��ӵ�")] [SerializeField] private float normalSpeed = 5f;
    [Header("�޸���ӵ�")] [SerializeField] private float runSpeed = 8f;
    [Header("����")] [SerializeField] private float sensivity;
    [Header("���� ��")] [SerializeField] private float jumpPower = 2f;
    [Header("������ ����")] [SerializeField] private float maxDistance = 10f;

<<<<<<< HEAD
    private float speed;
=======
    [Header("��ž��ġ����ǥ��")] [SerializeField] private GameObject FMark;
    [Header("��ž��ġâ")] [SerializeField] private GameObject buildChang;

    private RaycastHit hitTowerAreaInfo;

    private bool isArea;
>>>>>>> OIF

    private bool jDown;
    private bool isRun;
    private bool isJump;
    private bool isTarget = false;
    private bool isTargetTower = false;

    private RaycastHit hitInfo;

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
            if (isTarget)
                GameManager.Instance.UIManager.Chang();

            if (isTargetTower && GameManager.Instance.selectedTower == null)
                tower.ZoomInTower();
        }
    }

    private void PlayerSet()
    {
        LookAround();
        Move();
        Jump();
        Run();
        Hit();
<<<<<<< HEAD
=======
        if (isTarget)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Chang();
            }
        }
>>>>>>> OIF
    }
    private void LookAround()
    {
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
        Vector2 moveInput = new Vector2(Input.GetAxis(ConstantManager.KEYINPUT_HMOVE), Input.GetAxis(ConstantManager.KEYINPUT_VMOVE));
        bool isMove = moveInput.magnitude != 0;
        speed = normalSpeed;
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
        speed = runSpeed;
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
<<<<<<< HEAD
        var cam = GameManager.Instance.tpsCamera;
        Debug.DrawRay(cam.transform.position, cam.transform.forward * maxDistance, Color.blue);
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, maxDistance))
=======
        Hit_TowerArea();
        Hit_Monster();
    }

    private void Hit_Monster()
    {
        Debug.DrawRay(theCam.transform.position, theCam.transform.forward * maxDistance * 2, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(theCam.transform.position, theCam.transform.forward, maxDistance * 2);
        Debug.Log(hits.Length);

        foreach (var hit in hits)
        {
            Debug.Log(hit.transform.name);

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

    private void Hit_TowerArea()
    {
        Debug.DrawRay(theCam.transform.position, theCam.transform.forward * maxDistance, Color.blue);

        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward, out hitTowerAreaInfo, maxDistance))
>>>>>>> OIF
        {
            if (hitTowerAreaInfo.transform.gameObject.CompareTag("area"))
            {
<<<<<<< HEAD
                TowerSelect.buildTrn = hitInfo.transform;
                GameManager.Instance.UIManager.FMarkTrue();
=======
                TowerSelect.buildTrn = hitTowerAreaInfo.transform;
                FMark.SetActive(true);
>>>>>>> OIF
                isTarget = true;
            }
            else if (hitInfo.transform.gameObject.CompareTag(ConstantManager.TOWER_TAG))
            {
                TowerSelect.buildTrn = hitInfo.transform;
                GameManager.Instance.UIManager.FMarkTrue();
                isTargetTower = true;
                tower = hitInfo.collider.gameObject.GetComponent<TowerAttack>();
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