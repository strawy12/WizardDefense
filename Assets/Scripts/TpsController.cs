using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpsController : MonoBehaviour
{
    [SerializeField] private Transform characterBody;

    [SerializeField] private Transform cameraArm;

    [Header("이동속도")] [SerializeField] private float speed = 5f;
    [Header("감도")] [SerializeField] private float sensivity;
    [Header("점프 힘")] [SerializeField] private float jumpPower = 2f;

    bool jDown;
    bool isJump;

    Animator animator;
    Rigidbody myrigid;

    private void Start()
    {
        myrigid = GetComponent<Rigidbody>();
        animator = characterBody.GetComponent<Animator>();
    }

    private void Update()
    {
        LookAround();
        Move();
        Jump();
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
        if(jDown && !isJump)
        {
            myrigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJump = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            isJump = false;
        }
    }
}
