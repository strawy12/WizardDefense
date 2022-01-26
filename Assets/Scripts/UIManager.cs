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

    private EquipmentButton currentEquipButton;
    #endregion

    #region Panels Various
    [Header("패널 UI")]

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

    public ObjectSound UiSound;

    private List<GameObject> currentUIPanels = new List<GameObject>();

    public TowerSelect towerSelect;

    private bool isArea;
    [HideInInspector] public bool isClosePreView;
    private bool turnOnInventory;
    [HideInInspector] public bool isTarget;

    public GameObject quickSlot;

    void Start()
    {
        towerStatText = towerStatBar.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        ShowSkillUI(GameManager.Instance.selectedTower);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCurrentPanels();
        }

        if (Input.GetKeyDown(KeyManager.keySettings[KeyAction.Inventory]))
        {
            if (settingPanel.activeSelf || buildChang.activeSelf) return;

            turnOnInventory = !turnOnInventory;
            TurnOnInventory(turnOnInventory);
        }
    }

    public void SetTimer(float time)
    {
        timeText.text = string.Format("{0} : {1}", (int)time / 60, (time % 60).ToString("F1"));
    }

    public void ActiveBreakTimeUI(bool isActive)
    {

        breakTimeUI.SetActive(isActive);
        skipKeyText.text = KeyManager.keySettings[KeyAction.Skip].ToString();
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

    #region Tower Build UI
    public void ActivePanal(GameObject panal)
    {
        UiSound.PlaySound(0);
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
        FMark.SetActive(false);
        buildChang.SetActive(true);
        currentUIPanels.Add(buildChang);

        SetGameState(GameState.InGameSetting);
    }

    public void OnClickOutChang()
    {
        FMark.SetActive(false);
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

    public void SetCurEquipBtn(EquipmentButton button)
    {
        currentEquipButton = button;
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
}
