using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Tower UI Various
    [SerializeField] private GameObject towerUI;

    [SerializeField] private Image towerStatBar;
    private Text towerStatText;

    [SerializeField] private Image skillImage;
    [SerializeField] private Image skillCoolTimeImage;
    #endregion

    #region Panels Various
    [SerializeField] private GameObject keyPanelTemplate;
    private List<KeyPanel> keyPanels = new List<KeyPanel>();
    #endregion

    [Header("포탑설치가능표시")] [SerializeField] private GameObject FMark;
    [Header("포탑설치창")] [SerializeField] private GameObject buildChang;

    private bool isArea;

    void Start()
    {
        towerStatText = towerStatBar.GetComponentInChildren<Text>();

        InstantiatePanel();
    }

    private void Update()
    {
        ShowSkillUI(GameManager.Instance.selectedTower);
    }

    private void InstantiatePanel()
    {
        for (int i = 0; i < (int)KeyAction.Count; i++)
        {
            GameObject panel = Instantiate(keyPanelTemplate, keyPanelTemplate.transform.parent);
            KeyPanel keyPanel = panel.GetComponent<KeyPanel>();
            keyPanel.Initialize(i);
            keyPanels.Add(keyPanel);
        }

        keyPanelTemplate.SetActive(false);
    }

    public void ResetKeyPanel()
    {
        foreach(KeyPanel panel in keyPanels)
        {
            panel.ResetData();
        }
    }


    #region TowerUI
    public void ShowSkillUI(TowerAttack tower)
    {
        if (tower == null)
        {
            if (towerUI.gameObject.activeSelf)
                towerUI.gameObject.SetActive(false);

            skillCoolTimeImage.fillAmount = 0f;
            return;
        }

        else
        {
            if (!towerUI.gameObject.activeSelf)
                towerUI.gameObject.SetActive(true);

            if (!tower.CheckSkillCoolTime())
            {
                Debug.Log("sdf");
                skillCoolTimeImage.fillAmount = (tower.skill.coolTime - tower.useSkillTime) / tower.skill.coolTime;
            }
        }
    }

    public void ShowTowerStatBar(bool isShow, int attack = 0, float speed = 0)
    {
        towerStatBar.gameObject.SetActive(isShow);
        towerStatText.text = string.Format("공격력 {0}\n공격속도 {1}", attack, speed);
    }
    #endregion

    

    public void Chang()
    {
        isArea = !isArea;
        if (isArea)
        {
            FMark.SetActive(false);
            buildChang.SetActive(true);
        }
        else
        {
            FMark.SetActive(false);
            buildChang.SetActive(false);
        }
    }

    public void OnClickOutChang()
    {
        isArea = !isArea;
        FMark.SetActive(false);
        buildChang.SetActive(false);
    }

    public void AreaCheack()
    {
        isArea = false;
    }

    public void FMarkTrue()
    {
        FMark.SetActive(true);
    }

    public void FMarkFalse()
    {
        FMark.SetActive(false);
    }
}
