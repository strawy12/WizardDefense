using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float speed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float jumpPower;

    private float sitValue = 0.1f;

    // 카메라 변수
    [Header("감도")]
    [SerializeField] private float lookSensitivity;

    [SerializeField] private float cameraRotationLimit;
    private float currentCameraRotationX;


    // 앉기 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    [SerializeField] private Camera thisCamera;

    // 점프, 달리기 등등 상태 변수
    bool isRun = false;
    bool jDown = false;
    bool isJump = false;
    bool isCrouch = false;

    Vector3 moveVec;

    Animator anim;
    Rigidbody myrigidbody;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        myrigidbody = GetComponent<Rigidbody>();
        originPosY = thisCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    private void Update()
    {
        Player();
        PlayerAnimation();
        CameraRotation();
        CharacterRotation();
    }

    private void Player()
    {
        Move();
        Run();
        Jump();
        TryCrough();
    }
    private void Move()
    {
        float moveX = Input.GetAxisRaw(ConstantManager.KEYINPUT_HMOVE);
        float moveY = Input.GetAxisRaw(ConstantManager.KEYINPUT_VMOVE);
        isRun = Input.GetKey(KeyCode.LeftShift);
        jDown = Input.GetButtonDown(ConstantManager.KEYINPUT_JUMP);

        Vector3 _moveHo = transform.right * moveX;
        Vector3 _moveVer = transform.forward * moveY;

        moveVec = (_moveHo + _moveVer).normalized * speed;

        myrigidbody.MovePosition(transform.position + moveVec * Time.deltaTime);

        transform.position += moveVec * speed * Time.deltaTime;
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

    private void Jump()
    {
        if (jDown && !isJump)
        {
            myrigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }
    
    private void TryCrough()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;
        if(isCrouch)
        {
            speed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            speed = normalSpeed;
            applyCrouchPosY = originPosY;
        }

        thisCamera.transform.localPosition = new Vector3(thisCamera.transform.localPosition.x, applyCrouchPosY, thisCamera.transform.localPosition.z);
    }

    private void PlayerAnimation()
    {
        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", isRun);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        thisCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myrigidbody.MoveRotation(myrigidbody.rotation * Quaternion.Euler(_characterRotationY));
    }

}
