using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Tower UI Various
    [Header("타워 UI")]
    [SerializeField] private GameObject towerUI;

    [SerializeField] private Image towerStatBar;
    private Text towerStatText;

    [SerializeField] private Image skillImage;
    [SerializeField] private Image skillCoolTimeImage;

    #endregion

    #region Panels Various
    [Header("패널 UI")]

    [SerializeField] private GameObject keyPanelTemplate;
    private List<KeyPanel> keyPanels = new List<KeyPanel>();
    #endregion

    [Header("설정창")]
    [SerializeField] private Transform settingPanelsParent;
    [SerializeField] private Transform settingButtonsParent;

    [Header("포탑설치가능표시")] [SerializeField] private GameObject FMark;
    [Header("포탑설치창")] [SerializeField] private GameObject buildChang;

    [Header("시간 텍스트")]
    [SerializeField] private GameObject timeBar;
    [SerializeField] private Text timeText;

    private List<GameObject> currentUIPanels = new List<GameObject>();

    private bool isArea;

    void Start()
    {
        towerStatText = towerStatBar.GetComponentInChildren<Text>();

        InstantiatePanel();

        for (int i = 0; i < settingButtonsParent.childCount; i++)
        {
            Button button = settingButtonsParent.GetChild(i).GetComponent<Button>();
            button.onClick.AddListener(() => OnClickSettingButton(button.transform.GetSiblingIndex()));
        }
    }

    private void Update()
    {
        ShowSkillUI(GameManager.Instance.selectedTower);

        if (Input.GetKeyDown(KeyCode.Escape)) SetCurrentPanels();
    }

    public void SetTimer(float time)
    {
        timeText.text = string.Format("{0} : {1}", (int)time / 60, (time % 60).ToString("F1"));
    }

    public void ActiveTimer(bool isActive)
    {
        timeBar.SetActive(isActive);
    }

    private void SetCurrentPanels()
    {
        if (currentUIPanels.Count > 0)
        {
            currentUIPanels[currentUIPanels.Count - 1].gameObject.SetActive(false);
            currentUIPanels.RemoveAt(currentUIPanels.Count - 1);

            if (currentUIPanels.Count == 0)
            {
                ActiveUIPanalState(false);
                CursorLocked(true);
            }
        }
        else
        {
            ActiveSettingPanel();
        }
    }

    #region Setting Panel
    private void OnClickSettingButton(int index)
    {
        for (int i = 0; i < settingPanelsParent.childCount; i++)
        {
            settingPanelsParent.GetChild(i).gameObject.SetActive(i == index);
        }
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
        foreach (KeyPanel panel in keyPanels)
        {
            panel.ResetData();
        }
    }

    private void ActiveSettingPanel()
    {
        GameObject panel = settingPanelsParent.parent.gameObject;
        CursorLocked(panel.activeSelf);
        if (panel.activeSelf)
        {
            ActiveUIPanalState(false);
        }
        else
        {
            ActiveUIPanalState(true);
        }

        panel.SetActive(!panel.activeSelf);
    }
    #endregion

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

    #region Tower Build UI
    public void Chang()
    {
        CursorLocked(false);

        isArea = !isArea;
        if (isArea)
        {
            FMark.SetActive(false);
            buildChang.SetActive(true);
            currentUIPanels.Add(buildChang);

            ActiveUIPanalState(true);
        }
        else
        {
            FMark.SetActive(false);
            buildChang.SetActive(false);
            currentUIPanels.Remove(buildChang);

            ActiveUIPanalState(false);
        }

    }

    public void OnClickOutChang()
    {
        isArea = !isArea;
        FMark.SetActive(false);
        buildChang.SetActive(false);
        ActiveUIPanalState(false);
        CursorLocked(true);
    }
    #endregion

    public void ActiveUIPanalState(bool isActive)
    {
        if (isActive)
        {
            Time.timeScale = 0f;
            GameManager.Instance.gameState = GameState.Setting;
        }

        else
        {
            Time.timeScale = 1f;
            GameManager.Instance.gameState = GameState.Playing;
        }
    }

    public void CursorLocked(bool isLocked)
    {
        if (isLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    #region Set
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

    public void RemoveCurrentPanels(GameObject panel)
    {
        currentUIPanels.Remove(panel);
    }
    #endregion
}
