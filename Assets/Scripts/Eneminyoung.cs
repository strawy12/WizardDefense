using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eneminyoung : MonoBehaviour
{
    public float speed = 50f;
    public int hp = 3;
    private MeshRenderer meshRenderer;
    Color originalColor;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
    }
    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }

    public void Damaged(int damage)
    {
        hp -= damage;

        if (hp < 0)
        {
            GameManager.Instance.eneminyoungs.Remove(this);
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
}
