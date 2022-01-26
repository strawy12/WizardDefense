using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMove : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 2f);
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * 2f);
    }
}
