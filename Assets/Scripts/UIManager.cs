using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    #region Tower UI Various
    [Header("타워 UI")]
    [SerializeField] private Image towerUI;

    [SerializeField] private Image towerStatBar;
    [SerializeField] private Text towerStatText;

    [SerializeField] private Transform towerButtons;

    [SerializeField] private Image skillImage;
    [SerializeField] private Image skillCoolTimeImage;
    #endregion

    #region Panels Various
    [Header("패널 UI")]
    [SerializeField] private GameObject towerUpgradeUI;
    [SerializeField] private GameObject availablePanelTemplate;
    private List<PanelBase> availablePanels = new List<PanelBase>();
    public TowerRootController RootView;
    #endregion

    [Header("설정창")]
    [SerializeField] private GameObject settingPanel;

    [Header("포탑설치가능표시")] [SerializeField] private GameObject FMark;
    [Header("포탑설치창")] [SerializeField] private GameObject buildChang;

    [Header("시간 텍스트")]
    [SerializeField] private GameObject breakTimeUI;
    [SerializeField] private Text timeText;
    [SerializeField] private Text skipKeyText;

    [Header("사용자 지정 키 전용")]
    [SerializeField] private GameObject keySettingPanal;
    [SerializeField] private InventoryUIManager inventoryUIManager;
    [Header("시작 화면")]
    [SerializeField] private GameObject StartScene;
    [Header("시작 도움말")]
    [SerializeField] private GameObject Help;
    public ObjectSound UiSound;

    public List<GameObject> currentUIPanels = new List<GameObject>();

    public TowerSelect towerSelect;

    private Text fMarkText;

    private bool isArea;
    [HideInInspector] public bool isClosePreView;
    private bool turnOnInventory;
    [HideInInspector] public bool isTarget;
    public GameObject quickSlot;
    public bool isStarted = false;
    void Start()
    {
        // Time.timeScale = 0;
        towerStatText = towerStatBar.GetComponentInChildren<Text>();
        fMarkText = FMark.GetComponentInChildren<Text>();
        StartGame();

        InstantiatePanels(availablePanelTemplate, availablePanels, 3);
    }

    private void Update()
    {
        if (isStarted)
        {
            ShowSkillUI(GameManager.Instance.selectedTower);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetCurrentPanels();
            }

            if (Input.GetKeyDown(KeyManager.keySettings[KeyAction.Inventory]))
            {
                Debug.Log("ㅇㅇ");
                if (settingPanel.activeSelf || buildChang.activeSelf) return;

                turnOnInventory = !turnOnInventory;
                TurnOnInventory(turnOnInventory);
            }
        }
    }


    public void SetTimer(float time)
    {
        timeText.text = string.Format("{0} : {1}", (int)time / 60, (time % 60).ToString("F1"));
    }

    public void ActiveBreakTimeUI(bool isActive)
    {

        breakTimeUI.SetActive(isActive);
        string interactionKey = KeyManager.keySettings[KeyAction.Skip].ToString();
        if (interactionKey.Length > 1)
        {
            interactionKey = interactionKey[0].ToString();
        }

        skipKeyText.text = interactionKey;
    }

    private void SetCurrentPanels()
    {
        UiSound.PlaySound(0);
        if (currentUIPanels.Count > 0)
        {
            currentUIPanels[currentUIPanels.Count - 1].gameObject.SetActive(false);
            currentUIPanels.RemoveAt(currentUIPanels.Count - 1);

            if (currentUIPanels.Count == 0 && settingPanel.activeSelf == false)
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

    public void ResetKeyPanel()
    {
        EventManager.TriggerEvent(ConstantManager.CLICK_KEYSETTINGBTN);
    }

    public void ActiveSettingPanel()
    {
        CursorLocked(settingPanel.activeSelf);

        if (settingPanel.activeSelf)
        {
            ActiveUIPanalState(false);
            Time.timeScale = 1f;
        }
        else
        {
            ActiveUIPanalState(true);
            Time.timeScale = 0f;
        }
        settingPanel.SetActive(!settingPanel.activeSelf);
    }
    #endregion

    #region TowerUI
    public void ShowSkillUI(TowerAttack tower)
    {
        if (tower == null)
        {
            towerUI.gameObject.SetActive(false);
            skillCoolTimeImage.fillAmount = 0f;
            return;
        }
        else
        {
            towerUI.gameObject.SetActive(true);
            towerButtons.gameObject.SetActive(GameManager.Instance.inGameState == InGameState.BreakTime);

            CursorLocked(false);
            SetGameState(GameState.InGameSetting);

            if (!tower.CheckSkillCoolTime())
            {
                skillCoolTimeImage.fillAmount = (tower.skill.coolTime - tower.useSkillTime) / tower.skill.coolTime;
            }
        }
    }

    public void ShowTowerStatBar(bool isShow, int attack = 0, float speed = 0)
    {
        UiSound.PlaySound(0);
        towerStatBar.gameObject.SetActive(isShow);
        towerStatText.text = string.Format("공격력 {0}\n공격속도 {1}", attack, speed);
    }
    #endregion

    #region Tower Upgrade UI
    private void InstantiatePanels(GameObject panel, List<PanelBase> panels, int count, Transform position = null)
    {
        position ??= panel.transform.parent;

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(panel, position);
            PanelBase panelBase = obj.GetComponent<PanelBase>();
            panelBase.Init(i);
            panels.Add(panelBase);
        }

        panel.gameObject.SetActive(false);
    }

    public void UpdateAvailablePanels()
    {
        foreach (PanelBase panel in availablePanels)
        {
            panel.UpdateData();
        }
    }

    public void ShowTowerUpgradeUI()
    {
        towerUpgradeUI.SetActive(true);
        currentUIPanels.Add(towerUpgradeUI);
        UpdateAvailablePanels();
        SetGameState(GameState.InGameSetting);
        CursorLocked(false);
    }

    public void DeselectAvailablePanels()
    {
        foreach (PanelBase panel in availablePanels)
        {
            panel.Deselect();
        }
    }

    public TowerRoot GetCurrentSelectedRoot()
    {
        foreach (PanelBase panel in availablePanels)
        {
            if (panel.GetIsSelected())
                return panel.GetRoot();
        }
        return null;
    }
    #endregion

    #region Tower Build UI
    public void ActivePanal(GameObject panal)
    {
        UiSound.PlaySound(0);
        StartScene.SetActive(false);
        panal.SetActive(true);
        currentUIPanels.Add(panal);
        panal.transform.DOKill();
        panal.transform.DOScaleY(1f, 0.3f).SetUpdate(true);
    }

    public void ActiveKeySettingPanal(bool isActive)
    {
        UiSound.PlaySound(0);
        if (isActive)
        {
            ActivePanal(keySettingPanal);
        }

        else
        {
            UnActivePanal(keySettingPanal);
        }
    }

    public void UnActivePanal(GameObject panal)
    {
        currentUIPanels.Remove(panal);
        panal.transform.DOKill();
        panal.transform.DOScaleY(0f, 0.2f).SetUpdate(true).OnComplete(() => panal.SetActive(false));
    }

    public void Chang()
    {
        if (settingPanel.activeSelf) return;

        EventManager.TriggerEvent(ConstantManager.OPEN_BUILDPANEL);

        CursorLocked(false);
        FMarkFalse();
        buildChang.SetActive(true);
        currentUIPanels.Add(buildChang);

        SetGameState(GameState.InGameSetting);
    }

    public void OnClickOutChang()
    {
        FMarkFalse();
        buildChang.SetActive(false);
        ActiveUIPanalState(false);
        CursorLocked(true);
    }
    #endregion

    public void ActiveUIPanalState(bool isActive)
    {
        UiSound.PlaySound(0);

        if (isActive)
        {
            SetGameState(GameState.InGameSetting);
            GameManager.Instance.gameState = GameState.Setting;
        }

        else
        {
            SetGameState(GameState.Playing);
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
        string interactionKey = KeyManager.keySettings[KeyAction.Interaction].ToString();
        if (interactionKey.Length > 1)
        {
            interactionKey = interactionKey[0].ToString();
        }

        fMarkText.text = interactionKey;
        FMark.SetActive(true);
        isTarget = true;
    }

    public void FMarkFalse()
    {
        FMark.SetActive(false);
        isTarget = false;
    }

    public void RemoveCurrentPanels(GameObject panel)
    {
        currentUIPanels.Remove(panel);
    }

    public void AddCurrentPanels(GameObject panel)
    {
        currentUIPanels.Add(panel);
    }
    #endregion

    public bool IsFMarkActive()
    {
        return FMark.activeSelf;
    }

    private void SetGameState(GameState gameState)
    {
        GameManager.Instance.gameState = gameState;
    }

    private void TurnOnInventory(bool turnOn)
    {
        UiSound.PlaySound(0);
        if (turnOn)
        {
            GameManager.Instance.gameState = GameState.Setting;
            CursorLocked(false);
            inventoryUIManager.gameObject.SetActive(true);
            inventoryUIManager.canvasGroup.blocksRaycasts = true;
            inventoryUIManager.canvasGroup.DOKill();
            inventoryUIManager.canvasGroup.DOFade(1f, 0.25f).SetUpdate(true);
            currentUIPanels.Add(inventoryUIManager.gameObject);
            EventManager.TriggerEvent(ConstantManager.TURNON_INVENTORY);
        }

        else
        {
            GameManager.Instance.gameState = GameState.Playing;
            CursorLocked(true);
            inventoryUIManager.canvasGroup.blocksRaycasts = false;
            inventoryUIManager.canvasGroup.DOKill();
            inventoryUIManager.canvasGroup.DOFade(0f, 0.25f).SetUpdate(true).OnComplete(() => EventManager.TriggerEvent(ConstantManager.TURNOFF_INVENTORY));
            currentUIPanels.Remove(inventoryUIManager.gameObject);
        }
    }

    public void OnclickHelp(GameObject panal)
    {
        UiSound.PlaySound(0);
        panal.SetActive(true);
        currentUIPanels.Add(panal);
        panal.transform.DOKill();
        panal.transform.DOScaleY(1f, 0.3f).SetUpdate(true);
    }

    public void OnClickOutGame()
    {
        UiSound.PlaySound(0);
        Application.Quit();
    }

    public void StartGame()
    {
        UiSound.PlaySound(0);
        //StartScene.SetActive(false);
        //Help.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        isStarted = true;
    }
    public void StartButton()
    {
        UiSound.PlaySound(0);
        StartScene.SetActive(false);
    }
}
