using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float speed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpPower;

    [Header("°¨µµ")]
    [SerializeField] private float lookSensitivity;

    [SerializeField] private float cameraRotationLimit;
    private float currentCameraRotationX;

    [SerializeField] private Camera thisCamera;
    private float hAxis;
    private float vAxis;

    bool isRun = false;
    bool jDown = false;
    bool isJump = false;

    Vector3 moveVec;

    Animator anim;
    Rigidbody myrigidbody;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        myrigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Run();
        Jump();
        PlayerAnimation();
        CameraRotation();
        CharacterRotation();
    }

    private void Move()
    {
        hAxis = Input.GetAxisRaw(ConstantManager.KEYINPUT_HMOVE);
        vAxis = Input.GetAxisRaw(ConstantManager.KEYINPUT_VMOVE);
        isRun = Input.GetKey(KeyCode.LeftShift);
        jDown = Input.GetButtonDown(ConstantManager.KEYINPUT_JUMP);

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * Time.deltaTime;
    }

    private void PlayerAnimation()
    {
        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", isRun);
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
