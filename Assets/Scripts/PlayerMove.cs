using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float speed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpPower;
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
    }

    private void Move()
    {
        hAxis = Input.GetAxisRaw(ConstantManager.KEYINPUT_HMOVE);
        vAxis = Input.GetAxisRaw(ConstantManager.KEYINPUT_VMOVE);
        isRun = Input.GetKey(KeyCode.LeftShift);
        jDown = Input.GetButtonDown(ConstantManager.KEYINPUT_JUMP);

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * Time.deltaTime;

        transform.LookAt(transform.position + moveVec);
    }

    private void PlayerAnimation()
    {
        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", isRun);
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
}
