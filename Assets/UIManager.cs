using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image towerStatBar;
    private Text towerStatText;

    void Start()
    {
        towerStatText = towerStatBar.GetComponentInChildren<Text>();
    }

    public void ShowTowerStatBar(bool isShow, int attack = 0, float speed = 0)
    {
        towerStatBar.gameObject.SetActive(isShow);
        towerStatText.text = string.Format("���ݷ� {0}\n���ݼӵ� {1}", attack, speed);
    }
}
