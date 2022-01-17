using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector2 targetDir = Vector2.zero;
    [SerializeField] private float speed = 3f;
    void Update()
    {
        targetDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        transform.Translate(targetDir * speed * Time.deltaTime);


    }
}
