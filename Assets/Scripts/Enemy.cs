using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 50f;
    public int hp = 3;
    private MeshRenderer meshRenderer;
    Color originalColor;
    public int virtualHP;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
        virtualHP = hp;
    }

    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }

    public void Damaged(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            GameManager.Instance.enemies.Remove(this);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(OnDamagedEffect());
        }
    }

    private IEnumerator OnDamagedEffect()
    {
        meshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        meshRenderer.material.color = originalColor;

    }

    public void VirtualDamaged(int power)
    {
        virtualHP -= power;
    }
}
