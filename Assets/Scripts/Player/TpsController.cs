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

    [Header("플레이어 오브젝트")] [SerializeField] private GameObject player = null;

    private float speed;

    private bool isDonMove = false;

    private bool jDown;
    private bool isJump;

    private bool isRun;

    private bool isTarget = false;
    private bool isDonCamera = false;
    
    private RaycastHit hitInfo;

    private Animator animator;
    private Rigidbody myrigid;

    private void Start()
    {
        myrigid = GetComponent<Rigidbody>();
        animator = characterBody.GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerSet();
        if (isTarget)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GameManager.Instance.UIManager.Chang();
                DonMove();
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

        if(isDonCamera)
        {
            cameraArm.rotation = Quaternion.Euler(cameraArm.rotation.x, cameraArm.rotation.y, cameraArm.rotation.z);
        }
        else
        {
            cameraArm.rotation = Quaternion.Euler(x, cameraAngle.y + mouseDelta.x, cameraAngle.z);
        }
    }
    private void Move()
    {
        isRun = Input.GetKey(KeyCode.LeftShift);
        Vector2 moveInput = new Vector2(Input.GetAxis(ConstantManager.KEYINPUT_HMOVE), Input.GetAxis(ConstantManager.KEYINPUT_VMOVE));
        
        bool isMove = moveInput.magnitude != 0;

        if (isMove)
        {
            if(isDonMove)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                player.gameObject.SetActive(false);
            }
            else
            {
                player.gameObject.SetActive(true);
                animator.SetBool("isMove", isMove);
                Vector3 lookForWard = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
                Vector3 moveDir = lookForWard * moveInput.y + lookRight * moveInput.x;

                //characterBody.forward = lookForWard;
                characterBody.forward = moveDir;

                transform.position += moveDir * speed * Time.deltaTime;
            }
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
        if(isRun)
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
        Debug.DrawRay(cam.transform.position, cam.transform.forward * maxDistance, Color.blue);
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, maxDistance))
        {
            if (hitInfo.transform.gameObject.CompareTag("area"))
            {
                TowerSelect.buildTrn = hitInfo.transform;
                GameManager.Instance.UIManager.FMarkTrue();
                isTarget = true;
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
        }
    }

    public void DonMove()
    {
        isDonCamera = true;
        isDonMove = true;
    }

    public void CanMove()
    {
        isDonCamera = false;
        isDonMove = false;
    }
}
