using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eneminyoung : MonoBehaviour
{
    public float speed = 50f;

    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }
}
